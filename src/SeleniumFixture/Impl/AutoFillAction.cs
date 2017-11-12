using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SimpleFixture.Impl;

namespace SeleniumFixture.Impl
{
    public interface IAutoFillAction
    {
        IThenSubmitAction PerformFill();
    }

    public class AutoFillAction : IAutoFillAction
    {
        protected readonly IActionProvider _actionProvider;
        protected readonly IEnumerable<IWebElement> _elements;
        protected readonly object _seedWith;
        protected readonly bool _isSimpleSeed;
        protected readonly IConstraintHelper _constraintHelper;

        public AutoFillAction(IActionProvider actionProvider, IEnumerable<IWebElement> elements, object seedWith)
        {
            _actionProvider = actionProvider;
            _elements = elements;
            _seedWith = seedWith;
            _isSimpleSeed = IsSeedSimple();
            _constraintHelper = _actionProvider.UsingFixture.Data.Locate<IConstraintHelper>();
        }

        public virtual IThenSubmitAction PerformFill()
        {
            AutoFillElement();

            return new ThenSubmitAction(_actionProvider.UsingFixture, _elements.First());
        }

        protected virtual void AutoFillElement()
        {
            var radioButtons = new Dictionary<string, List<IWebElement>>();

            foreach (var element in _elements)
            {
                if (!ProcessFormElement(element, radioButtons))
                {
                    foreach (var findElement in element.FindElements(By.CssSelector("input, select, textarea, datalist")))
                    {
                        ProcessFormElement(findElement, radioButtons);
                    }
                }
            }

            foreach (var keyValuePair in radioButtons)
            {
                AutoFillRadioButtonGroup(keyValuePair.Key, keyValuePair.Value);
            }
        }

        protected virtual bool ProcessFormElement(IWebElement findElement, Dictionary<string, List<IWebElement>> radioButtons)
        {
            var returnValue = false;

            if (findElement.TagName == "select")
            {
                AutoFillSelectElement(findElement);
                returnValue = true;
            }
            else if (findElement.TagName == "input")
            {
                AutoFillInputElement(findElement, radioButtons);
                returnValue = true;
            }
            else if (findElement.TagName == "datalist")
            {
                AutoFillInputElement(findElement, radioButtons);
                returnValue = true;
            }
            else if (findElement.TagName == "textarea")
            {
                AutoFillInputElement(findElement, radioButtons);
                returnValue = true;
            }

            return returnValue;
        }

        protected virtual void AutoFillRadioButtonGroup(string key, List<IWebElement> values)
        {
            string stringConstraintValue = null;

            if (_isSimpleSeed)
            {
                stringConstraintValue = GetStringFromValue(_seedWith);
            }
            else
            {
                var setValue = _constraintHelper.GetValue<object>(_seedWith, null, key);

                if (setValue != null)
                {
                    stringConstraintValue = GetStringFromValue(setValue);
                }
            }

            if (!string.IsNullOrEmpty(stringConstraintValue))
            {
                foreach (var webElement in values)
                {
                    if (webElement.GetAttribute(ElementContants.ValueAttribute) == stringConstraintValue)
                    {
                        webElement.Click();
                        return;
                    }
                }
            }

            var radioElement = _actionProvider.Generate<IRandomDataGeneratorService>().NextInSet(values);

            if (radioElement != null)
            {
                radioElement.Click();
            }
        }

        protected virtual void AutoFillInputElement(IWebElement element, Dictionary<string, List<IWebElement>> radioButtons)
        {
            var type = element.GetAttribute(ElementContants.TypeAttribute);

            if (type == ElementContants.RadioButtonType)
            {
                AddRadioButtonToGroup(element, radioButtons);

                return;
            }

            if (type == ElementContants.HiddenType || type == ElementContants.SubmitType)
            {
                return;
            }

            if (type == ElementContants.CheckBoxType)
            {
                AutoFillCheckBox(element);

                return;
            }

            if (_isSimpleSeed)
            {
                element.Clear();
                element.SendKeys(GetStringFromValue(_seedWith));

                return;
            }

            var elementId = element.GetAttribute("id") ?? element.GetAttribute("name");
            object setValue = null;

            if (!string.IsNullOrEmpty(elementId))
            {
                setValue = _constraintHelper.GetValue<object>(_seedWith, null, elementId);
            }

            if (setValue == null)
            {
                setValue = _actionProvider.Generate<string>(elementId, new { stringType = StringType.AlphaNumeric });
            }

            element.Clear();
            element.SendKeys(GetStringFromValue(setValue));
        }

        protected virtual void AutoFillCheckBox(IWebElement element)
        {
            var elementId = element.GetAttribute("id") ?? element.GetAttribute("name");
            bool? checkedValue = null;

            if (!string.IsNullOrEmpty(elementId))
            {
                var checkValue = _constraintHelper.GetValue<object>(_seedWith, null, elementId);

                if (checkValue != null)
                {
                    if (string.Compare(checkValue.ToString(), "true", StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        checkedValue = true;
                    }
                    else if (string.Compare(checkValue.ToString(), "false", StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        checkedValue = false;
                    }
                }
            }

            if (!checkedValue.HasValue)
            {
                checkedValue = _actionProvider.Generate<bool>();
            }

            if (element.Selected != checkedValue.Value)
            {
                element.Click();
            }
        }

        protected virtual void AddRadioButtonToGroup(IWebElement element, Dictionary<string, List<IWebElement>> radioButtons)
        {
            var name = element.GetAttribute("name");

            if (!string.IsNullOrEmpty(name))
            {
                if (!radioButtons.TryGetValue(name, out var radioButtonGroup))
                {
                    radioButtonGroup = new List<IWebElement>();

                    radioButtons[name] = radioButtonGroup;
                }

                radioButtonGroup.Add(element);
            }
        }

        protected virtual string GetStringFromValue(object stringObject)
        {
            if (stringObject is string s)
            {
                return s;
            }
            if (stringObject is DateTime date)
            {
                return date.ToString("MM/dd/yyyy");
            }

            if (stringObject is Enum)
            {
                return Convert.ChangeType(stringObject, Enum.GetUnderlyingType(stringObject.GetType())).ToString();
            }

            return stringObject.ToString();
        }

        protected virtual void AutoFillSelectElement(IWebElement element)
        {
            var selectElement = new SelectElement(element);
            var setValue = false;

            if (_isSimpleSeed)
            {
                setValue = true;
            }
            else
            {
                var elementId = element.GetAttribute("id") ?? element.GetAttribute("name");

                var value = _constraintHelper.GetValue<object>(_seedWith, null, elementId);

                if (value != null)
                {
                    var stringValue = GetStringFromValue(value);

                    var option = selectElement.Options.FirstOrDefault(e => e.GetAttribute(ElementContants.ValueAttribute) == stringValue);

                    if (option != null)
                    {
                        selectElement.SelectByValue(stringValue);
                        setValue = true;
                    }
                    else
                    {
                        option = selectElement.Options.FirstOrDefault(e => e.GetAttribute("text") == stringValue);

                        if (option != null)
                        {
                            selectElement.SelectByText(stringValue);
                            setValue = true;
                        }
                    }
                }
            }

            if (!setValue)
            {
                var selectedOption =
                    _actionProvider.Generate<IRandomDataGeneratorService>().NextInSet(
                        selectElement.Options.Where(e => !string.IsNullOrEmpty(e.GetAttribute(ElementContants.ValueAttribute))));

                if (selectedOption != null)
                {
                    var value = selectedOption.GetAttribute(ElementContants.ValueAttribute);

                    selectElement.SelectByValue(value);
                }
            }
        }

        protected virtual bool IsSeedSimple()
        {
            if (_seedWith == null)
                return false;

            return _seedWith.GetType().IsPrimitive ||
                   _seedWith is string ||
                   _seedWith is DateTime ||
                   _seedWith is Enum;
        }
    }
}
