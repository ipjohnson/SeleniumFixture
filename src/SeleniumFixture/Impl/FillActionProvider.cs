using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace SeleniumFixture.Impl
{
    public interface IFillActionProvider
    {
        /// <summary>
        /// Data to fill input elements with.
        /// </summary>
        /// <param name="fillValues"></param>
        /// <returns></returns>
        IThenSubmitActionProvider With(object fillValues);
    }

    public class FillActionProvider : IFillActionProvider
    {
        protected readonly Fixture _fixture;
        protected readonly ReadOnlyCollection<IWebElement> _elements;

        public FillActionProvider(ReadOnlyCollection<IWebElement> elements, Fixture fixture)
        {
            _fixture = fixture;
            _elements = elements;
        }

        public virtual IThenSubmitActionProvider With(object fillValues)
        {
            FillElementsWithValues(fillValues);

            return new ThenSubmitActionProvider(_fixture);
        }

        private void FillElementsWithValues(object fillValues)
        {
            foreach (var webElement in _elements)
            {
                FillElementWithValues(webElement, fillValues);
            }
        }

        private void FillElementWithValues(IWebElement element, object fillValues)
        {
            if (fillValues == null || 
                fillValues.GetType().IsPrimitive ||
                fillValues is string ||
                fillValues is DateTime ||
                fillValues is Enum)
            {
                SetPrimitiveValueIntoElement(element, fillValues);
            }
            else
            {
                SetComplexValuesIntoElement(element, fillValues);
            }
        }

        private void SetComplexValuesIntoElement(IWebElement webElement,object fillValues)
        {
            switch (webElement.TagName)
            {
                case "input":
                case "select":
                    var values = GetSetValues(fillValues);
                    var elementName = webElement.GetAttribute("id") ?? webElement.GetAttribute("name");
                    var findValue =
                        values.First(
                            kvp => string.Compare(kvp.Key, elementName, StringComparison.CurrentCultureIgnoreCase) == 0);

                    SetPrimitiveValueIntoElement(webElement, findValue);
                    break;

                default:
                    SetComplexValuesIntoForm(webElement, fillValues);
                    break;
            }
        }

        private void SetComplexValuesIntoForm(IWebElement webElement, object fillValues)
        {
            if (fillValues is IEnumerable<KeyValuePair<string, object>>)
            {
                SetDictionaryValuesIntoForm(webElement, fillValues as IEnumerable<KeyValuePair<string, object>>);
            }
            else
            {
                SetObjectValuesIntoForm(webElement, fillValues);
            }
        }

        private void SetObjectValuesIntoForm(IWebElement webElement, object fillValues)
        {
            foreach (PropertyInfo runtimeProperty in fillValues.GetType().GetRuntimeProperties())
            {
                if (runtimeProperty.CanRead && 
                   !runtimeProperty.GetMethod.GetParameters().Any() &&
                    runtimeProperty.GetMethod.IsPublic && 
                   !runtimeProperty.GetMethod.IsStatic)
                {
                    var elements = webElement.FindElements(Using.Auto("#" + runtimeProperty.Name));

                    if (elements.Count == 0)
                    {
                        string searchString = char.IsUpper(runtimeProperty.Name[0]) ? 
                                                "" + char.ToLower(runtimeProperty.Name[0]) : 
                                                "" + char.ToUpper(runtimeProperty.Name[0]);

                        if (runtimeProperty.Name.Length > 1)
                        {
                            searchString += runtimeProperty.Name.Substring(1);
                        }

                        elements = webElement.FindElements(Using.Auto("#" + searchString));
                    }

                    if (elements.Count == 0)
                    {
                        throw new Exception("Could not locate any element using: " + runtimeProperty.Name);
                    }
                    var value = runtimeProperty.GetValue(fillValues,
                                                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic,
                                                        null,
                                                        null,
                                                        CultureInfo.CurrentCulture);

                    foreach (IWebElement element in elements)
                    {
                        FillElementWithValues(element, value);
                    }
                }
            }
        }

        private void SetDictionaryValuesIntoForm(IWebElement webElement, IEnumerable<KeyValuePair<string, object>> keyValuePairs)
        {
            foreach (KeyValuePair<string, object> keyValuePair in keyValuePairs)
            {
                var elements = webElement.FindElements(Using.Auto(keyValuePair.Key));

                if (elements.Count == 0)
                {
                    throw new Exception("Could not locate any element using: " + keyValuePair.Key);
                }

                foreach (IWebElement element in elements)
                {
                    FillElementWithValues(element, keyValuePair.Value);
                }
            }
        }

        private IEnumerable<KeyValuePair<string, object>> GetSetValues(object fillValues)
        {
            if (fillValues is IEnumerable<KeyValuePair<string, object>>)
            {
                return (IEnumerable<KeyValuePair<string, object>>)fillValues;
            }

            List<KeyValuePair<string, object>> returnValue = new List<KeyValuePair<string, object>>();

            foreach (var runtimeProperty in fillValues.GetType().GetRuntimeProperties().Where(p => p.CanRead &&
                                                                                                   p.GetMethod.IsPublic &&
                                                                                                  !p.GetMethod.IsStatic))
            {
                returnValue.Add(new KeyValuePair<string, object>("#" + runtimeProperty.Name, runtimeProperty.GetValue(fillValues)));
            }

            return returnValue;
        }

        private void SetPrimitiveValueIntoElement(IWebElement webElement, object setValue)
        {
            if (webElement.TagName == "input")
            {
                var type = webElement.GetAttribute("type");

                switch (type)
                {
                    case "radio":
                        var value = webElement.GetAttribute("value");

                        if (setValue.ToString() == value)
                        {
                            webElement.Click();
                        }
                        break;

                    case "checkbox":
                        if (setValue is bool)
                        {
                            if ((bool)setValue != webElement.Selected)
                            {
                                webElement.Click();
                            }
                        }
                        else if (setValue is string)
                        {
                            bool setBool;
                            if (bool.TryParse(setValue.ToString(), out setBool))
                            {
                                if (setBool != webElement.Selected)
                                {
                                    webElement.Click();
                                }
                            }
                        }
                        break;

                    default:
                        webElement.Clear();
                        webElement.SendKeys(setValue.ToString());
                        break;
                }
            }
            else if (webElement.TagName == "select")
            {

            }
        }
    }
}
