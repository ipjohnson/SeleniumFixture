using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumFixture.Impl;

namespace SeleniumFixture
{
    public interface IAttributeElementWrapper
    {
        string Get(string attr);

        T GetAs<T>(string attr);

        void Set(string attr, string value);

        void Set<T>(string attr, T value);
    }

    public interface IWaitElementWrapper
    {
        IElementWrapper For(string element, double? timeout = null);

        IElementWrapper ForAjax(double? timeout = null);

        IElementWrapper Until(Func<IActionProvider, bool> testFunc, double? timeout = null);
    }

    public interface IElementWrapper
    {
        IElementWrapper AutoFill(string requestName, object seed = null);

        IElementWrapper AutoFillAs<T>(string requestName, object seed = null);

        IElementWrapper Click();

        IElementWrapper DoubleClick();

        IElementWrapper FillWith(object fillObject);

        IWebElement Raw { get; }

        IElementWrapper Submit();
        
        string Text { get; }

        string Value { get; }

        string TagName { get; }

        T Yields<T>(string requestName = null, object constraints = null);
    }

    public class ElementWrapper
    {
        private readonly IWebElement _element;
        private readonly Fixture _fixture;

        public ElementWrapper(Fixture fixture, IWebElement element)
        {
            _element = element;
            _fixture = fixture;
        }

        public ElementWrapper Clear()
        {
            _element.Clear();

            return this;
        }

        public ElementWrapper SendKeys(string keys)
        {
            _element.SendKeys(keys);

            return this;
        }

        public ElementWrapper Click()
        {
            _element.Click();

            return this;
        }
        
        public ElementWrapper DoubleClick()
        {
            Actions doubleClick = new Actions(_fixture.Driver);
            
            doubleClick.DoubleClick(_element).Perform();

            return this;
        }

        public ElementWrapper ClickAndWaitForAjax(int timeOut = 5)
        {
            return this;
        }
        
        public ElementWrapper DoubleClickAndWaitForAjax(int timeOut = 5)
        {
            var doubleClick = new Actions(_fixture.Driver);

            doubleClick.DoubleClick(_element).Perform();

            //_fixture.WaitForAjax(timeOut);

            return this;
        }

        public T ClickYields<T>()
        {
            _element.Click();

            return _fixture.Data.Locate<T>();
        }

        public T DoubleClickYields<T>()
        {
            Actions doubleClick = new Actions(_fixture.Driver);

            doubleClick.DoubleClick(_element).Perform();

            return _fixture.Data.Locate<T>();
        }

        public T ClickAndWaitForAjaxYields<T>(int timeOut = 5)
        {
            _element.Click();

            //_fixture.WaitForAjax(timeOut);

            return _fixture.Data.Locate<T>();
        }

        public T DoubleClickAndWaitForAjaxYields<T>(int timeOut = 5)
        {
            var doubleClick = new Actions(_fixture.Driver);

            doubleClick.DoubleClick(_element).Perform();

            //_fixture.WaitForAjax(timeOut);

            return _fixture.Data.Locate<T>();
        }
    }
}
