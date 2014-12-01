using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumFixture
{
    public class FormWrapper
    {
        private Fixture _fixture;
        private IWebElement _formElement;

        public FormWrapper(Fixture fixture, IWebElement formElement)
        {
            _fixture = fixture;
            _formElement = formElement;
        }

        public FormWrapper FillWith(object fillObject, bool throwIfElementMissing = true)
        {
            FillMethodHandler(_formElement, fillObject, throwIfElementMissing);

            return this;
        }

        public FormWrapper AutoFillWith(object fillObject)
        {
            return this;
        }

        public FormWrapper Submit()
        {
            _formElement.Submit();

            return this;
        }

        public FormWrapper AjaxSubmit(int timeOut = 60)
        {
            Submit();

            _fixture.WaitForAjax(timeOut);

            return this;
        }

        public T SubmitYields<T>()
        {
            Submit();

            return _fixture.Locate<T>();
        }

        public T AjaxSubmitYields<T>(int timeOut = 60)
        {
            Submit();

            _fixture.WaitForAjax(timeOut);

            return _fixture.Locate<T>();
        }

        public FormData GetFormData()
        {
            return GetFormValues(_formElement);
        }

        public T GetFormDataAs<T>()
        {
            T returnValue = _fixture.Locate<T>();

            MapFormValuesToObject(returnValue, GetFormValues(_formElement));

            return default(T);
        }

        #region Map Value Methods

        private void FillMethodHandler(IWebElement webElement, object valuesObject, bool throwIfMissingElement)
        {
            foreach (KeyValuePair<string, object> keyValuePair in GetValuesFromObject(valuesObject))
            {
                string elementName = keyValuePair.Key;
                IReadOnlyCollection<IWebElement> setElements;

                switch (elementName[0])
                {
                    case '#':
                        setElements = webElement.FindElements(By.Id(elementName.Substring(1)));
                        break;
                    case '/':
                        setElements = webElement.FindElements(By.XPath(elementName));
                        break;
                    default:
                        setElements = webElement.FindElements(By.Id(elementName));

                        if (setElements.Count == 0)
                        {
                            setElements = webElement.FindElements(By.Name(elementName));
                        }

                        if (setElements.Count == 0)
                        {
                            setElements = webElement.FindElements(By.CssSelector(elementName));
                        }
                        break;
                }

                if (!SetValueIntoElements(setElements, keyValuePair.Value) &&
                    throwIfMissingElement)
                {
                    throw new Exception("Could not set element by name: " + elementName);
                }
            }
        }

        private bool SetValueIntoElements(IReadOnlyCollection<IWebElement> setElements, object value)
        {
            bool setValue = false;

            foreach (IWebElement webElement in setElements)
            {
                if (webElement.TagName == "input")
                {
                    var type = webElement.GetAttribute("type");

                    switch (type)
                    {
                        case "checkbox":
                            setValue = SetValueIntoCheckBox(value, webElement);
                            break;
                        case "radio":
                            setValue = SetValueIntoRadioButton(value, webElement);
                            break;
                        default:
                            setValue = SetValueIntoTextInput(value, webElement);
                            break;

                    }
                }
                else if (webElement.TagName == "select")
                {
                    setValue = SetValueIntoSelect(value, webElement);
                }

                if (setValue)
                {
                    break;
                }
            }

            return setValue;
        }

        private bool SetValueIntoRadioButton(object value, IWebElement webElement)
        {
            string radioButtonValue = webElement.GetAttribute("value");

            if (value != null)
            {
                if (value is Enum)
                {
                    value = Convert.ChangeType(value, typeof(int));
                }

                if (value.ToString() == radioButtonValue)
                {
                    webElement.Click();

                    return true;
                }
            }

            return false;
        }

        private bool SetValueIntoSelect(object value, IWebElement element)
        {
            if (value == null)
            {
                SelectElement selectElement = new SelectElement(element);

                selectElement.DeselectAll();

                return true;
            }

            var option = element.FindElements(By.CssSelector(string.Format("option[value='{0}']", value)));

            if (option.Count != 0)
            {
                SelectElement selectElement = new SelectElement(element);

                selectElement.SelectByValue(value.ToString());

                return true;
            }
            {
                SelectElement selectElement = new SelectElement(element);

                string valueString = value.ToString();

                if (selectElement.Options.Any(e => e.Text == value.ToString()))
                {
                    selectElement.SelectByText(valueString);

                    return true;
                }

            }
            return false;
        }

        private bool SetValueIntoCheckBox(object value, IWebElement webElement)
        {
            bool setValue = false;

            if (value is bool)
            {
                setValue = (bool)value;
            }
            else
            {
                setValue = (bool)Convert.ChangeType(value, typeof(bool));
            }

            if (setValue != webElement.Selected)
            {
                webElement.Click();
            }

            return true;
        }

        private bool SetValueIntoTextInput(object value, IWebElement webElement)
        {
            webElement.Clear();
            webElement.SendKeys(value.ToString());

            return true;
        }

        private void MapFormValuesToObject(object returnValue, IDictionary<string, object> getFormValues)
        {
            foreach (PropertyInfo propertyInfo in returnValue.GetType().GetRuntimeProperties().
                                                                        Where(p => p.CanWrite &&
                                                                                   p.SetMethod.IsPublic &&
                                                                                  !p.SetMethod.IsStatic &&
                                                                                   p.SetMethod.GetParameters().Count() == 1))
            {
                object value;

                if (getFormValues.TryGetValue(propertyInfo.Name, out value))
                {
                    if (!value.GetType().IsInstanceOfType(propertyInfo.PropertyType))
                    {
                        value = Convert.ChangeType(value, propertyInfo.PropertyType);
                    }

                    propertyInfo.SetValue(returnValue, value);
                }
            }
        }

        #endregion

        #region Get Values From Form

        private FormData GetFormValues(IWebElement formElement)
        {
            var returnValue = new FormData();

            foreach (IWebElement findElement in formElement.FindElements(By.TagName("input")))
            {
                string key = findElement.GetAttribute("id");

                if (string.IsNullOrEmpty(key))
                {
                    key = findElement.GetAttribute("name");
                }

                if (string.IsNullOrEmpty(key))
                {
                    continue;
                }

                object value = null;

                switch (findElement.TagName)
                {
                    case "input":
                        value = GetInputElementValue(findElement);
                        break;
                    case "select":
                        value = GetSelectElementValue(findElement);
                        break;
                }

                if (!returnValue.ContainsKey(key))
                {
                    returnValue[key] = value;
                }
            }

            return returnValue;
        }
        private object GetSelectElementValue(IWebElement findElement)
        {
            return null;
        }

        private object GetInputElementValue(IWebElement findElement)
        {
            string typeStr = findElement.GetAttribute("type");

            switch (typeStr)
            {
                case "password":
                case "hidden":
                case "text":
                    return findElement.GetAttribute("value");

                case "checkbox":
                    return findElement.Selected;

            }

            return null;
        }
        #endregion

        #region Get Values From Objects

        private IEnumerable<KeyValuePair<string, object>> GetValuesFromObject(object valuesObject)
        {
            Func<object> valueFunc = valuesObject as Func<object>;

            if (valueFunc != null)
            {
                object value = valueFunc();

                if (value == null)
                {
                    throw new Exception("Func must return value");
                }

                return GetValuesFromObject(value);
            }

            if (valuesObject is IEnumerable<KeyValuePair<string, object>>)
            {
                return valuesObject as IEnumerable<KeyValuePair<string, object>>;
            }

            XDocument xDocument = valuesObject as XDocument;

            if (xDocument != null)
            {
                return GetValuesFromXDocument(xDocument);
            }

            return DefaultPropertiesFinder(valuesObject);
        }

        private IEnumerable<KeyValuePair<string, object>> GetValuesFromXDocument(XDocument xDocument)
        {
            yield break;
        }

        private IEnumerable<KeyValuePair<string, object>> DefaultPropertiesFinder(object valuesObject)
        {
            List<KeyValuePair<string, object>> returnList = new List<KeyValuePair<string, object>>();

            foreach (PropertyInfo runtimeProperty in valuesObject.GetType().GetRuntimeProperties())
            {
                if (runtimeProperty.CanRead &&
                    runtimeProperty.GetMethod.IsPublic &&
                    !runtimeProperty.GetMethod.IsStatic &&
                    !runtimeProperty.GetMethod.GetParameters().Any())
                {
                    returnList.Add(
                        new KeyValuePair<string, object>(runtimeProperty.Name, runtimeProperty.GetValue(valuesObject)));
                }
            }

            return returnList;
        }
        #endregion
    }
}
