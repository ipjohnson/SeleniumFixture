using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

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
        IForAction<string> Value { get; }
            
        /// <summary>
        /// Get the text value for a web element
        /// </summary>
        IForAction<string> Text { get; }

        /// <summary>
        /// Tag for an element
        /// </summary>
        IForAction<string> Tag { get; }

        /// <summary>
        /// Class for an element
        /// </summary>
        IForAction<string> Class { get; }
        
        /// <summary>
        /// Get an attribute for a web element
        /// </summary>
        /// <param name="attr">attribute name</param>
        /// <returns></returns>
        IForAction<string> Attribute(string attr);

        /// <summary>
        /// Get a typed attribute for a web element
        /// </summary>
        /// <typeparam name="T">type to return attribute as</typeparam>
        /// <param name="attr">attribute name</param>
        /// <returns></returns>
        IForAction<T> Attribute<T>(string attr);

        /// <summary>
        /// Get css value for provided
        /// </summary>
        /// <param name="propertyName">property name</param>
        /// <returns></returns>
        IForAction<string> CssValue(string propertyName);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IForAction<T>
    {
        /// <summary>
        /// For a specified element
        /// </summary>
        /// <param name="selector">element selector</param>
        /// <returns>return type of T</returns>
        T For(string selector);

        /// <summary>
        /// For a specified element
        /// </summary>
        /// <param name="selector">element selector</param>
        /// <returns>return type of T</returns>
        T For(By selector);
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
        public IForAction<string> Value
        {
            get { return new ForAction<string>(_actionProvider,e => e.GetAttribute("value"));}
        }

        /// <summary>
        /// Get the text for the specified element
        /// </summary>
        public IForAction<string> Text
        {
            get { return new ForAction<string>(_actionProvider, e => e.Text); }
        }

        /// <summary>
        /// Get the element Tag for the specified element
        /// </summary>
        public IForAction<string> Tag
        {
            get { return new ForAction<string>(_actionProvider, e => e.TagName); }
        }

        /// <summary>
        /// Get the css class for a specified element
        /// </summary>
        public IForAction<string> Class
        {
            get { return new ForAction<string>(_actionProvider, e => e.GetAttribute("class")); }
        }

        /// <summary>
        /// Get an attribute for a specified element
        /// </summary>
        /// <param name="attr">attribute name</param>
        /// <returns></returns>
        public IForAction<string> Attribute(string attr)
        {
            return new ForAction<string>(_actionProvider, e => e.GetAttribute(attr));
        }

        /// <summary>
        /// Get an attribute as certain type for a specified element
        /// </summary>
        /// <typeparam name="T">attribute type</typeparam>
        /// <param name="attr">attribute name</param>
        /// <returns></returns>
        public IForAction<T> Attribute<T>(string attr)
        {
            return new ForAction<T>(_actionProvider, e => (T)Convert.ChangeType(e.GetAttribute(attr),typeof(T)));
        }

        /// <summary>
        /// Get a css property value for a specified element
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public IForAction<string> CssValue(string propertyName)
        {
            return new ForAction<string>(_actionProvider, e => e.GetCssValue(propertyName));
        }
    }

    /// <summary>
    /// Provides fluent syntax for fetching a value for a specified element
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ForAction<T> : IForAction<T>
    {
        private readonly IActionProvider _actionProvider;
        private readonly Func<IWebElement, T> _getFunc;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="actionProvider">action provider</param>
        /// <param name="getFunc">function to get value</param>
        public ForAction(IActionProvider actionProvider, Func<IWebElement, T> getFunc)
        {
            _actionProvider = actionProvider;
            _getFunc = getFunc;
        }

        /// <summary>
        /// For a specified element
        /// </summary>
        /// <param name="selector">element selector</param>
        /// <returns>return type of T</returns>
        public T For(string selector)
        {
            var element = _actionProvider.FindElement(selector);

            return _getFunc(element);
        }

        /// <summary>
        /// For a specified element
        /// </summary>
        /// <param name="selector">element selector</param>
        /// <returns>return type of T</returns>
        public T For(By selector)
        {
            var element = _actionProvider.FindElement(selector);

            return _getFunc(element);
        }
    }
}
