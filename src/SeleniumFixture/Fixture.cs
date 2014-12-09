using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SeleniumFixture.Impl;
using SimpleFixture;
using SimpleFixture.Impl;

namespace SeleniumFixture
{
    /// <summary>
    /// Fixture to make working with Selenium and PageObjects easier
    /// </summary>
    public class Fixture : IActionProvider
    {
        #region Private Fields
        private IActionProvider _actionProvider;
        #endregion

        #region Constructor

        public Fixture(IWebDriver webDriver, string baseAddress = null)
            : this(webDriver, new SeleniumFixtureConfiguration { BaseAddress = baseAddress })
        {
        }

        public Fixture(IWebDriver webDriver, SeleniumFixtureConfiguration configuration)
        {
            Initialize(webDriver, configuration);
        }

        #endregion

        #region Public Members

        public IWebDriver Driver { get; private set; }

        public SeleniumFixtureConfiguration Configuration { get; private set; }

        public SimpleFixture.Fixture Data { get; private set; }

        #endregion

        #region IActionProvider

        /// <summary>
        /// Navigate the fixture
        /// </summary>
        public INavigateActionProvider Navigate
        {
            get { return _actionProvider.Navigate; }
        }

        /// <summary>
        /// Find a specified element by selector
        /// </summary>
        /// <param name="selector">selector to use to locate element</param>
        /// <returns>element or throws an exception</returns>
        public IWebElement FindElement(string selector)
        {
            return _actionProvider.FindElement(selector);
        }

        /// <summary>
        /// Find a specified by selector
        /// </summary>
        /// <param name="selector">by selector</param>
        /// <returns>elements</returns>
        public IWebElement FindElement(By selector)
        {
            return _actionProvider.FindElement(selector);
        }

        /// <summary>
        /// Find All elements meeting the specified selector
        /// </summary>
        /// <param name="selector">selector to use to find elements</param>
        /// <returns>elements</returns>
        public ReadOnlyCollection<IWebElement> FindElements(string selector)
        {
            return _actionProvider.FindElements(selector);
        }

        /// <summary>
        /// Find all elements meeting the specified selector
        /// </summary>
        /// <param name="selector">selector to use to find elements</param>
        /// <returns>elements</returns>
        public ReadOnlyCollection<IWebElement> FindElements(By selector)
        {
            return _actionProvider.FindElements(selector);
        }

        /// <summary>
        /// Check for the element specified in the selector
        /// </summary>
        /// <param name="selector">selector to look for</param>
        /// <returns>true if element exists</returns>
        public bool CheckForElement(string selector)
        {
            return _actionProvider.CheckForElement(selector);
        }

        /// <summary>
        /// Check for the element specified in the selector
        /// </summary>
        /// <param name="selector">selector to look for</param>
        /// <returns>true if element exists</returns>
        public bool CheckForElement(By selector)
        {
            return _actionProvider.CheckForElement(selector);
        }

        /// <summary>
        /// Count the number of elements present
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns>count of elements</returns>
        public int Count(string selector)
        {
            return _actionProvider.Count(selector);
        }

        /// <summary>
        /// Count the number of elements present
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns>count of elements</returns>
        public int Count(By selector)
        {
            return _actionProvider.Count(selector);
        }

        /// <summary>
        /// Click the elements returned by the selector
        /// </summary>
        /// <param name="selector">selector to use when find elements to click</param>
        /// <param name="clickMode">click mode, by default </param>
        /// <returns>this</returns>
        public IActionProvider Click(string selector, ClickMode clickMode = ClickMode.ClickAll)
        {
            return _actionProvider.Click(selector, clickMode);
        }

        /// <summary>
        /// Click the elements returned by the selector
        /// </summary>
        /// <param name="selector">selector to use when find elements to click</param>
        /// <param name="clickMode">click mode, by default </param>
        /// <returns>this</returns>
        public IActionProvider Click(By selector, ClickMode clickMode = ClickMode.ClickAll)
        {
            return _actionProvider.Click(selector, clickMode);
        }

        /// <summary>
        /// Double click the selected elements
        /// </summary>
        /// <param name="selector">selector</param>
        /// <param name="clickMode">click mode</param>
        /// <returns>this</returns>
        public IActionProvider DoubleClick(string selector, ClickMode clickMode = ClickMode.ClickAll)
        {
            return _actionProvider.DoubleClick(selector, clickMode);
        }

        /// <summary>
        /// Double click the selected elements
        /// </summary>
        /// <param name="selector">selector</param>
        /// <param name="clickMode">click mode</param>
        /// <returns>this</returns>
        public IActionProvider DoubleClick(By selector, ClickMode clickMode = ClickMode.ClickAll)
        {
            return _actionProvider.DoubleClick(selector, clickMode);
        }

        /// <summary>
        /// Drag an element
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns>this</returns>
        public IDragActionProvider Drag(string selector)
        {
            return _actionProvider.Drag(selector);
        }

        /// <summary>
        /// Drag an element
        /// </summary>
        /// <param name="selector">element</param>
        /// <returns>this</returns>
        public IDragActionProvider Drag(By selector)
        {
            return _actionProvider.Drag(selector);
        }

        /// <summary>
        /// Autofill elements using data from SimpleFixture
        /// </summary>
        /// <param name="selector">selector</param>
        /// <param name="seedWith">seed data</param>
        /// <returns>this</returns>
        public IThenSubmitActionProvider AutoFill(string selector, object seedWith = null)
        {
            return _actionProvider.AutoFill(selector, seedWith);
        }

        /// <summary>
        /// Autofill elements using data from SimpleFixture
        /// </summary>
        /// <param name="selector">selector</param>
        /// <param name="seedWith">seed data</param>
        /// <returns>this</returns>
        public IThenSubmitActionProvider AutoFill(By selector, object seedWith = null)
        {
            return _actionProvider.AutoFill(selector, seedWith);
        }

        /// <summary>
        /// Autofill elements using data from SimpleFixture
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="seedWith"></param>
        /// <returns></returns>
        public IThenSubmitActionProvider AutoFill(IEnumerable<IWebElement> elements, object seedWith = null)
        {
            return _actionProvider.AutoFill(elements, seedWith);
        }

        public IThenSubmitActionProvider AutoFillAs<T>(string selector, string requestName = null, object constraints = null)
        {
            throw new NotImplementedException();
        }

        public IThenSubmitActionProvider AutoFillAs<T>(By selector, string requestName = null, object constraints = null)
        {
            throw new NotImplementedException();
        }

        public IThenSubmitActionProvider AutoFillAs<T>(IEnumerable<IWebElement> elements, string requestName = null, object constraints = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fill elements with values
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns>fill action</returns>
        public IFillActionProvider Fill(string selector)
        {
            return _actionProvider.Fill(selector);
        }

        /// <summary>
        /// Fill elements with values
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns>fill action</returns>
        public IFillActionProvider Fill(By selector)
        {
            return _actionProvider.Fill(selector);
        }

        /// <summary>
        /// Fill elements with values
        /// </summary>
        /// <param name="elements">elements</param>
        /// <returns>fill action</returns>
        public IFillActionProvider Fill(IEnumerable<IWebElement> elements)
        {
            return _actionProvider.Fill(elements);
        }

        /// <summary>
        /// Wait for something to happen
        /// </summary>
        public IWaitActionProvider Wait
        {
            get { return _actionProvider.Wait; }
        }

        /// <summary>
        /// Submit form.
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IYieldsActionProvider Submit(string selector)
        {
            return _actionProvider.Submit(selector);
        }

        public IYieldsActionProvider Submit(By selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fixture for this action provider
        /// </summary>
        public Fixture UsingFixture
        {
            get { return _actionProvider.UsingFixture; }
        }

        /// <summary>
        /// Yields a Page Object using SimpleFixture
        /// </summary>
        /// <typeparam name="T">Type of object to Generate</typeparam>
        /// <param name="requestName">request name</param>
        /// <param name="constraints">constraints for the locate</param>
        /// <returns>new T</returns>
        public T Yields<T>(string requestName = null, object constraints = null)
        {
            return _actionProvider.Yields<T>(requestName, constraints);
        }

        /// <summary>
        /// Yields a Page Object using SimpleFixture
        /// </summary>
        /// <param name="type">Type of object to Generate</param>
        /// <param name="requestName">request name</param>
        /// <param name="constraints">constraints for the locate</param>
        /// <returns>new instance</returns>
        public object Yields(Type type, string requestName = null, object constraints = null)
        {
            return _actionProvider.Yields(type, requestName, constraints);
        }
        #endregion

        #region Private Members

        protected void Initialize(IWebDriver webDriver, SeleniumFixtureConfiguration configuration)
        {
            Configuration = configuration;

            var dataConfiguration = new DefaultFixtureConfiguration();

            dataConfiguration.Export<ITypePropertySelector>(g => new SeleniumTypePropertySelector(g.Locate<IConstraintHelper>()));
            dataConfiguration.Export<IPropertySetter>(g => new Impl.PropertySetter());

            Data = new SimpleFixture.Fixture(dataConfiguration);

            Data.Return(this);
            Data.Return(webDriver);
            Data.Return(configuration.BaseAddress).WhenNamed("BaseAddress");

            Driver = webDriver;

            Data.Behavior.Add((r, o) =>
                         {
                             if (o.GetType().IsValueType || o is string)
                             {
                                 return o;
                             }

                             PageFactory.InitElements(webDriver, o);

                             return o;
                         });

            Data.Behavior.Add(ImportPropertiesOnLocate);

            _actionProvider = new FixtureActionProvider(this);

            Data.Return(_actionProvider);
        }

        private object ImportPropertiesOnLocate(DataRequest r, object o)
        {
            if (o.GetType().IsValueType || o is string || r.Populate)
            {
                return o;
            }

            IModelService modelService = Data.Configuration.Locate<IModelService>();

            TypePopulator typePopulator = new TypePopulator(Data.Configuration.Locate<IConstraintHelper>(),
                                                            new ImportSeleniumTypePropertySelector(Data.Configuration.Locate<IConstraintHelper>()),
                                                            new Impl.PropertySetter());

            typePopulator.Populate(o, r, modelService.GetModel(r.RequestedType));

            return o;
        }
        #endregion

    }
}
