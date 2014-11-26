using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace SeleniumFixture
{
    public class ElementWrapper
    {
        private IWebElement _element;
        private Fixture _fixture;

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
            Actions doubleClick = new Actions(_fixture.WebDriver);
            
            doubleClick.DoubleClick(_element).Perform();

            return this;
        }

        public ElementWrapper ClickAndWaitForAjax(int timeOut = 5)
        {
            return this;
        }
        
        public ElementWrapper DoubleClickAndWaitForAjax(int timeOut = 5)
        {
            var doubleClick = new Actions(_fixture.WebDriver);

            doubleClick.DoubleClick(_element).Perform();

            _fixture.WaitForAjax(timeOut);

            return this;
        }

        public T ClickYields<T>()
        {
            _element.Click();

            return _fixture.Locate<T>();
        }

        public T DoubleClickYields<T>()
        {
            Actions doubleClick = new Actions(_fixture.WebDriver);

            doubleClick.DoubleClick(_element).Perform();

            return _fixture.Locate<T>();
        }

        public T ClickAndWaitForAjaxYields<T>(int timeOut = 5)
        {
            _element.Click();

            _fixture.WaitForAjax(timeOut);

            return _fixture.Locate<T>();
        }

        public T DoubleClickAndWaitForAjaxYields<T>(int timeOut = 5)
        {
            var doubleClick = new Actions(_fixture.WebDriver);

            doubleClick.DoubleClick(_element).Perform();

            _fixture.WaitForAjax(timeOut);

            return _fixture.Locate<T>();
        }
    }
}
