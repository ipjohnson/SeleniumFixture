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
        T For(string selector);

        T For(By selector);
    }

    public class GetActionProvider : IGetActionProvider
    {
        private readonly IActionProvider _actionProvider;

        public GetActionProvider(IActionProvider actionProvider)
        {
            _actionProvider = actionProvider;
        }

        public string PageTitle
        {
            get { return _actionProvider.UsingFixture.Driver.Title; }
        }

        public string PageUrl
        {
            get { return _actionProvider.UsingFixture.Driver.Url; }
        }

        public string PageSource
        {
            get { return _actionProvider.UsingFixture.Driver.PageSource; }
        }

        public IForAction<string> Value
        {
            get { return new ForAction<string>(_actionProvider,e => e.GetAttribute("value"));}
        }

        public IForAction<string> Text
        {
            get { return new ForAction<string>(_actionProvider, e => e.Text); }
        }

        public IForAction<string> Tag
        {
            get { return new ForAction<string>(_actionProvider, e => e.TagName); }
        }

        public IForAction<string> Class
        {
            get { return new ForAction<string>(_actionProvider, e => e.GetAttribute("class")); }
        }

        public IForAction<string> Attribute(string attr)
        {
            return new ForAction<string>(_actionProvider, e => e.GetAttribute(attr));
        }

        public IForAction<T> Attribute<T>(string attr)
        {
            return new ForAction<T>(_actionProvider, e => (T)Convert.ChangeType(e.GetAttribute(attr),typeof(T)));
        }

        public IForAction<string> CssValue(string propertyName)
        {
            return new ForAction<string>(_actionProvider, e => e.GetCssValue(propertyName));
        }
    }

    public class ForAction<T> : IForAction<T>
    {
        private readonly IActionProvider _actionProvider;
        private readonly Func<IWebElement, T> _getFunc;

        public ForAction(IActionProvider actionProvider, Func<IWebElement, T> getFunc)
        {
            _actionProvider = actionProvider;
            _getFunc = getFunc;
        }

        public T For(string selector)
        {
            var element = _actionProvider.FindElement(selector);

            return _getFunc(element);
        }

        public T For(By selector)
        {
            var element = _actionProvider.FindElement(selector);

            return _getFunc(element);
        }
    }
}
