using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumFixture.Impl
{
    /// <summary>
    /// Interface to get values from element
    /// </summary>
    public interface IGetActionProvider
    {
        /// <summary>
        /// Title property for this page
        /// </summary>
        string PageTitle { get; }

        /// <summary>
        /// Url for page
        /// </summary>
        string PageUrl { get; }

        /// <summary>
        /// Page Source
        /// </summary>
        string PageSource { get; }

        /// <summary>
        /// Get the value for a web element 
        /// </summary>
        IFromAction<string> Value { get; }

        /// <summary>
        /// Get the text value for a web element
        /// </summary>
        IFromAction<string> Text { get; }

        /// <summary>
        /// Tag for an element
        /// </summary>
        IFromAction<string> Tag { get; }

        /// <summary>
        /// Class for an element
        /// </summary>
        IFromAction<string> Class { get; }

        /// <summary>
        /// Get an attribute for a web element
        /// </summary>
        /// <param name="attr">attribute name</param>
        /// <returns></returns>
        IFromAction<string> Attribute(string attr);

        /// <summary>
        /// Get a typed attribute for a web element
        /// </summary>
        /// <typeparam name="T">type to return attribute as</typeparam>
        /// <param name="attr">attribute name</param>
        /// <returns></returns>
        IFromAction<T> Attribute<T>(string attr);

        /// <summary>
        /// Get css value for provided
        /// </summary>
        /// <param name="propertyName">property name</param>
        /// <returns></returns>
        IFromAction<string> CssValue(string propertyName);

        /// <summary>
        /// Convert data from form into specified Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IFromAction<T> DataAs<T>();

        /// <summary>
        /// Get key value pair from all form elements
        /// </summary>
        IFromAction<FormData> Data { get; }
    }

    /// <summary>
    /// For fluent syntax
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFromAction<T>
    {
        /// <summary>
        /// For a specified element
        /// </summary>
        /// <param name="selector">element selector</param>
        /// <returns>return type of T</returns>
        T From(string selector);

        /// <summary>
        /// For a specified element
        /// </summary>
        /// <param name="selector">element selector</param>
        /// <returns>return type of T</returns>
        T From(By selector);
    }

    /// <summary>
    /// Provides fluent syntax for getting page information
    /// </summary>
    public class GetActionProvider : IGetActionProvider
    {
        private readonly IActionProvider _actionProvider;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="actionProvider">action provider</param>
        public GetActionProvider(IActionProvider actionProvider)
        {
            _actionProvider = actionProvider;
        }

        /// <summary>
        /// Title on the page
        /// </summary>
        public string PageTitle
        {
            get { return _actionProvider.UsingFixture.Driver.Title; }
        }

        /// <summary>
        /// Url for the page
        /// </summary>
        public string PageUrl
        {
            get { return _actionProvider.UsingFixture.Driver.Url; }
        }

        /// <summary>
        /// Source for the page
        /// </summary>
        public string PageSource
        {
            get { return _actionProvider.UsingFixture.Driver.PageSource; }
        }

        /// <summary>
        /// Get the value for a specified element
        /// </summary>
        public IFromAction<string> Value
        {
            get { return new FromAction<string>(_actionProvider, e => e.GetAttribute(ElementContants.ValueAttribute)); }
        }

        /// <summary>
        /// Get the text for the specified element
        /// </summary>
        public IFromAction<string> Text
        {
            get { return new FromAction<string>(_actionProvider, e => e.Text); }
        }

        /// <summary>
        /// Get the element Tag for the specified element
        /// </summary>
        public IFromAction<string> Tag
        {
            get { return new FromAction<string>(_actionProvider, e => e.TagName); }
        }

        /// <summary>
        /// Get the css class for a specified element
        /// </summary>
        public IFromAction<string> Class
        {
            get { return new FromAction<string>(_actionProvider, e => e.GetAttribute("class")); }
        }

        /// <summary>
        /// Get an attribute for a specified element
        /// </summary>
        /// <param name="attr">attribute name</param>
        /// <returns></returns>
        public IFromAction<string> Attribute(string attr)
        {
            return new FromAction<string>(_actionProvider, e => e.GetAttribute(attr));
        }

        /// <summary>
        /// Get an attribute as certain type for a specified element
        /// </summary>
        /// <typeparam name="T">attribute type</typeparam>
        /// <param name="attr">attribute name</param>
        /// <returns></returns>
        public IFromAction<T> Attribute<T>(string attr)
        {
            return new FromAction<T>(_actionProvider, e => (T)Convert.ChangeType(e.GetAttribute(attr), typeof(T)));
        }

        /// <summary>
        /// Get a css property value for a specified element
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public IFromAction<string> CssValue(string propertyName)
        {
            return new FromAction<string>(_actionProvider, e => e.GetCssValue(propertyName));
        }

        /// <summary>
        /// Convert data from form into specified Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IFromAction<T> DataAs<T>()
        {
            return new FromAction<T>(_actionProvider, MatchFormValuesToData<T>);
        }

        /// <summary>
        /// Get key value pair from all form elements
        /// </summary>
        public IFromAction<FormData> Data
        {
            get { return new FromAction<FormData>(_actionProvider, GetFormData); }
        }

        private FormData GetFormData(IWebElement element)
        {
            var returnValue = new FormData();

            foreach (IWebElement findElement in element.FindElements(By.CssSelector("input, select, textarea, datalist")))
            {
                var name = findElement.GetAttribute(ElementContants.IdAttribute) ??
                           findElement.GetAttribute(ElementContants.NameAttribute);

                var type = findElement.GetAttribute(ElementContants.TypeAttribute);

                if (!string.IsNullOrEmpty(name))
                {
                    switch (findElement.TagName)
                    {
                        case "input":
                            if (type == ElementContants.SubmitType)
                            {
                                continue;
                            }

                            if ((type != ElementContants.CheckBoxType && type != ElementContants.RadioButtonType) ||
                                element.Selected)
                            {
                                returnValue[name] = findElement.GetAttribute(ElementContants.ValueAttribute);
                            }
                            break;
                        case "select":
                        case "textarea":
                        case "datalist":
                            returnValue[name] = findElement.GetAttribute(ElementContants.ValueAttribute);
                            break;
                    }
                }
            }

            return returnValue;
        }

        private T MatchFormValuesToData<T>(IWebElement element)
        {
            var compareType = typeof(T);
            var nullableType = Nullable.GetUnderlyingType(typeof(T));

            if (nullableType != null)
            {
                compareType = nullableType;
            }

            if (compareType.IsPrimitive ||
                compareType == typeof(decimal) ||
                compareType == typeof(DateTime) ||
                compareType == typeof(string))
            {
                return ReturnElementAsPrimitive<T>(element);
            }

            return ReturnElementsAsComplex<T>(element);
        }

        private T ReturnElementAsPrimitive<T>(IWebElement element)
        {
            string returnString = null;

            if (element.TagName == "select")
            {
                SelectElement selectElement = new SelectElement(element);

                if (selectElement.SelectedOption != null)
                {
                    returnString = selectElement.SelectedOption.GetAttribute(ElementContants.ValueAttribute);
                }
            }
            else if (element.TagName == "input" || 
                     element.TagName == "textarea" || 
                     element.TagName == "datalist")
            {
                string type = element.GetAttribute(ElementContants.TypeAttribute);

                switch (type)
                {
                    case ElementContants.RadioButtonType:
                    case ElementContants.CheckBoxType:
                        returnString = element.Selected.ToString();
                        break;

                    default:
                        returnString = element.GetAttribute(ElementContants.ValueAttribute);
                        break;
                }
            }

            return (T)Convert.ChangeType(returnString,typeof(T));
        }

        private T ReturnElementsAsComplex<T>(IWebElement element)
        {
            T returnValue = _actionProvider.UsingFixture.Data.Locate<T>();

            foreach (PropertyInfo propertyInfo in returnValue.GetType().GetProperties())
            {
                if (propertyInfo.SetMethod != null &&
                    propertyInfo.SetMethod.IsPublic &&
                    !propertyInfo.SetMethod.IsStatic &&
                    propertyInfo.SetMethod.GetParameters().Count() == 1)
                {
                    string propertyName = propertyInfo.Name;

                    var elements = FindMatchingElements(element, propertyName);

                    if (elements.Count > 0)
                    {
                        foreach (IWebElement webElement in elements)
                        {
                            if (FillPropertyUsingElement(propertyInfo, webElement, returnValue))
                            {
                                break;
                            }
                        }
                    }
                }
            }
            return returnValue;
        }

        private bool FillPropertyUsingElement(PropertyInfo propertyInfo, IWebElement webElement, object fillObject)
        {
            bool returnValue = false;

            switch (webElement.TagName)
            {
                case "input":
                    returnValue = FillPropertyFromInput(propertyInfo, webElement, fillObject);
                    break;
                case "select":
                    returnValue = FillPropertyFromSelect(propertyInfo, webElement, fillObject);
                    break;
                case "datalist":
                    returnValue = FillPropertyFromInput(propertyInfo, webElement, fillObject);
                    break;
                case "textarea":
                    returnValue = FillPropertyFromInput(propertyInfo, webElement, fillObject);
                    break;
            }

            return returnValue;
        }

        private bool FillPropertyFromSelect(PropertyInfo propertyInfo, IWebElement webElement, object fillObject)
        {
            SelectElement selectElement = new SelectElement(webElement);

            if (selectElement.SelectedOption != null)
            {
                string value = selectElement.SelectedOption.GetAttribute(ElementContants.ValueAttribute);

                if (value != null)
                {
                    object finalValue = null;

                    try
                    {
                        finalValue = Convert.ChangeType(value, propertyInfo.PropertyType);
                    }
                    catch (Exception exp)
                    {
                        Console.WriteLine("Could not convert {0} to type {1} {2}", value, propertyInfo.PropertyType.FullName, exp.Message);
                        finalValue = null;
                    }

                    if (finalValue != null)
                    {
                        propertyInfo.SetValue(fillObject, finalValue);
                    }
                }

                return true;
            }

            return false;
        }

        private bool FillPropertyFromInput(PropertyInfo propertyInfo, IWebElement webElement, object fillObject)
        {
            bool returnValue = false;
            object setValue = null;
            var type = webElement.GetAttribute(ElementContants.TypeAttribute);

            if (type == ElementContants.CheckBoxType)
            {
                if (webElement.Selected)
                {
                    setValue = true;
                }
            }
            else if (type == ElementContants.RadioButtonType)
            {
                if (webElement.Selected)
                {
                    setValue = webElement.GetAttribute(ElementContants.ValueAttribute);
                }
            }
            else
            {
                setValue = webElement.GetAttribute(ElementContants.ValueAttribute);
            }

            if (setValue != null)
            {
                if (!propertyInfo.PropertyType.IsInstanceOfType(setValue))
                {
                    try
                    {
                        setValue = Convert.ChangeType(setValue, propertyInfo.PropertyType);
                    }
                    catch (Exception exp)
                    {
                        Console.WriteLine("Could not convert: {0} to {1} {2}", setValue, propertyInfo.PropertyType.FullName, exp.Message);
                        setValue = null;
                    }
                }
            }

            if (setValue != null)
            {
                propertyInfo.SetValue(fillObject, setValue);

                returnValue = true;
            }

            return returnValue;
        }

        private static ReadOnlyCollection<IWebElement> FindMatchingElements(ISearchContext element, string propertyName)
        {
            var elements = element.FindElements(By.Id(propertyName));

            if (elements.Count == 0)
            {
                elements = element.FindElements(By.Name(propertyName));
            }

            if (elements.Count == 0)
            {
                bool keepSearching = false;

                if (char.IsUpper(propertyName[0]))
                {
                    propertyName = "" + char.ToLower(propertyName[0]);

                    if (propertyName.Length > 1)
                    {
                        propertyName += propertyName.Substring(1);
                    }

                    keepSearching = true;
                }
                else if (char.IsLower(propertyName[0]))
                {
                    propertyName = "" + char.ToUpper(propertyName[0]);

                    if (propertyName.Length > 1)
                    {
                        propertyName += propertyName.Substring(1);
                    }

                    keepSearching = true;
                }

                if (keepSearching)
                {
                    elements = element.FindElements(By.Id(propertyName));

                    if (elements.Count == 0)
                    {
                        elements = element.FindElements(By.Name(propertyName));
                    }
                }
            }
            return elements;
        }
    }

    /// <summary>
    /// Provides fluent syntax for fetching a value for a specified element
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FromAction<T> : IFromAction<T>
    {
        private readonly IActionProvider _actionProvider;
        private readonly Func<IWebElement, T> _getFunc;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="actionProvider">action provider</param>
        /// <param name="getFunc">function to get value</param>
        public FromAction(IActionProvider actionProvider, Func<IWebElement, T> getFunc)
        {
            _actionProvider = actionProvider;
            _getFunc = getFunc;
        }

        /// <summary>
        /// For a specified element
        /// </summary>
        /// <param name="selector">element selector</param>
        /// <returns>return type of T</returns>
        public T From(string selector)
        {
            var element = _actionProvider.FindElement(selector);

            return _getFunc(element);
        }

        /// <summary>
        /// For a specified element
        /// </summary>
        /// <param name="selector">element selector</param>
        /// <returns>return type of T</returns>
        public T From(By selector)
        {
            var element = _actionProvider.FindElement(selector);

            return _getFunc(element);
        }
    }
}
