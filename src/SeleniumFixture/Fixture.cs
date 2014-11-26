using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SimpleFixture;
using SimpleFixture.Impl;

namespace SeleniumFixture
{
    public class Fixture : SimpleFixture.Fixture
    {
        #region Constructor
        public Fixture(IWebDriver webDriver, string baseAddress = null)
            : base(new SeleniumFixtureConfiguration())
        {
            Initialize(webDriver, baseAddress);
        }
        #endregion

        #region Public Members

        public IWebDriver WebDriver { get; private set; }

        public string BaseAddress { get; private set; }

        #endregion

        #region Find Methods

        public virtual IWebElement Find(string element)
        {
            if (!string.IsNullOrEmpty(element))
            {
                switch (element[0])
                {
                    case '#':
                        return WebDriver.FindElement(By.Id(element.Substring(1)));

                    case '/':
                        return WebDriver.FindElement(By.XPath(element));

                    default:
                        return WebDriver.FindElement(By.CssSelector(element));
                }
            }

            return null;
        }

        public virtual IEnumerable<IWebElement> FindAll(string element)
        {
            if (!string.IsNullOrEmpty(element))
            {
                switch (element[0])
                {
                    case '#':
                        return WebDriver.FindElements(By.Id(element.Substring(1)));

                    case '/':
                        return WebDriver.FindElements(By.XPath(element));

                    default:
                        return WebDriver.FindElements(By.CssSelector(element));
                }
            }

            return Enumerable.Empty<IWebElement>();
        }

        #endregion

        #region Wrapper Methods

        public FormWrapper Form(string formName = null)
        {
            if (formName == null)
            {
                formName = "//form";
            }

            IWebElement formElement = Find(formName);

            if (formElement == null)
            {
                throw new Exception("Could not find element named " + formName);
            }

            return new FormWrapper(this, formElement);
        }

        public FormWrapper Form(By element)
        {
            IWebElement formElement = WebDriver.FindElement(element);

            if (formElement == null)
            {
                throw new Exception("Could not find element by " + element);
            }

            return new FormWrapper(this, formElement);
        }

        public ElementWrapper Element(string element)
        {
            IWebElement webElement = Find(element);

            if (webElement == null)
            {
                throw new Exception("Could not find element named " + element);
            }

            return new ElementWrapper(this, webElement);
        }

        public ElementWrapper Element(By element)
        {
            IWebElement webElement = WebDriver.FindElement(element);

            if (webElement == null)
            {
                throw new Exception("Could not find element named " + element);
            }

            return new ElementWrapper(this, webElement);
        }

        #endregion

        #region Navigation Methods

        public T NavigateTo<T>(string address = null)
        {
            string navigateAddress = null;

            if (address != null && address.StartsWith("http", StringComparison.CurrentCultureIgnoreCase))
            {
                navigateAddress = address;
            }
            else
            {
                navigateAddress = BaseAddress + address;
            }

            WebDriver.Navigate().GoToUrl(navigateAddress);

            return Locate<T>();
        }

        public T NavigateBackTo<T>()
        {
            WebDriver.Navigate().Back();

            return Locate<T>();
        }

        public T NavigateForwardTo<T>()
        {
            WebDriver.Navigate().Forward();

            return Locate<T>();
        }

        public T RefreshYields<T>()
        {
            WebDriver.Navigate().Refresh();

            return Locate<T>();
        }
        
        public T ActionYields<T>(Action<Fixture> action)
        {
            action(this);

            return Locate<T>();
        }
        #endregion

        #region Ajax Methods

        public void WaitForAjax(int timeout = 20)
        {
            WebDriverWait wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(timeout));

            IJavaScriptExecutor jsScript = (IJavaScriptExecutor)WebDriver;

            wait.Until(d => (bool)jsScript.ExecuteScript("return jQuery.active == 0"));
        }

        #endregion

        #region Private Members

        private void Initialize(IWebDriver webDriver, string baseAddress)
        {
            Return(this);
            Return(webDriver);
            Return(baseAddress).WhenNamed("BaseAddress");

            WebDriver = webDriver;
            BaseAddress = baseAddress;

            Behavior.Add((r, o) =>
                         {
                             if (o.GetType().IsValueType || o is string)
                             {
                                 return o;
                             }

                             PageFactory.InitElements(webDriver, o);

                             return o;
                         });

            Behavior.Add(ImportPropertiesOnLocate);
        }

        private object ImportPropertiesOnLocate(DataRequest r, object o)
        {
            if (o.GetType().IsValueType || o is string || r.Populate)
            {
                return o;
            }

            IModelService modelService = Configuration.Locate<IModelService>();

            TypePopulator typePopulator = new TypePopulator(Configuration.Locate<IConstraintHelper>(), new ImportSeleniumTypePropertySelector(Configuration.Locate<IConstraintHelper>()));

            typePopulator.Populate(o, r, modelService.GetModel(r.RequestedType));

            return o;
        }
        #endregion
    }
}
