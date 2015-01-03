using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SeleniumFixture.Exceptions;
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
        /// Move the mouse to a give element or x,y
        /// </summary>
        /// <param name="selector">selector</param>
        /// <param name="x">x offset</param>
        /// <param name="y">y offset</param>
        /// <returns></returns>
        public IActionProvider MoveTheMouseTo(string selector, int? x = null, int? y = null)
        {
            return _actionProvider.MoveTheMouseTo(selector, x, y);
        }

        /// <summary>
        /// Move the mouse to a give element or x,y
        /// </summary>
        /// <param name="selector">selector</param>
        /// <param name="x">x offset</param>
        /// <param name="y">y offset</param>
        /// <returns></returns>
        public IActionProvider MoveTheMouseTo(By selector, int? x = null, int? y = null)
        {
            return _actionProvider.MoveTheMouseTo(selector, x, y);
        }

        /// <summary>
        /// Navigate the fixture
        /// </summary>
        public INavigateAction Navigate
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
        /// Generate a random T
        /// </summary>
        /// <typeparam name="T">data type to generate</typeparam>
        /// <param name="requestName">request name to use</param>
        /// <param name="constraints">constraints for the generate</param>
        /// <returns>new type to generate</returns>
        public T Generate<T>(string requestName = null, object constraints = null)
        {
            return _actionProvider.Generate<T>(requestName, constraints);
        }

        /// <summary>
        /// Get values from a web element
        /// </summary>
        public IGetAction Get
        {
            get { return _actionProvider.Get; }
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
        /// Autofill elements using data from SimpleFixture
        /// </summary>
        /// <param name="selector">selector</param>
        /// <param name="seedWith">seed data</param>
        /// <returns>this</returns>
        public IThenSubmitAction AutoFill(string selector, object seedWith = null)
        {
            return _actionProvider.AutoFill(selector, seedWith);
        }

        /// <summary>
        /// Autofill elements using data from SimpleFixture
        /// </summary>
        /// <param name="selector">selector</param>
        /// <param name="seedWith">seed data</param>
        /// <returns>this</returns>
        public IThenSubmitAction AutoFill(By selector, object seedWith = null)
        {
            return _actionProvider.AutoFill(selector, seedWith);
        }

        /// <summary>
        /// Autofill elements using data from SimpleFixture
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="seedWith"></param>
        /// <returns></returns>
        public IThenSubmitAction AutoFill(IEnumerable<IWebElement> elements, object seedWith = null)
        {
            return _actionProvider.AutoFill(elements, seedWith);
        }

        /// <summary>
        /// Auto fill elements as a specific type
        /// </summary>
        /// <typeparam name="T">Type of data to generate</typeparam>
        /// <param name="selector">selector for elements</param>
        /// <param name="requestName">request name</param>
        /// <param name="constraints">constraints for generation</param>
        /// <returns>this</returns>
        public IThenSubmitAction AutoFillAs<T>(string selector, string requestName = null, object constraints = null)
        {
            return _actionProvider.AutoFillAs<T>(selector, requestName, constraints);
        }

        /// <summary>
        /// Auto fill elements as a specific type
        /// </summary>
        /// <typeparam name="T">Type of data to generate</typeparam>
        /// <param name="selector">selector for elements</param>
        /// <param name="requestName">request name</param>
        /// <param name="constraints">constraints for generation</param>
        /// <returns>this</returns>
        public IThenSubmitAction AutoFillAs<T>(By selector, string requestName = null, object constraints = null)
        {
            return _actionProvider.AutoFillAs<T>(selector, requestName, constraints);
        }

        /// <summary>
        /// Auto fill elements as a specific type
        /// </summary>
        /// <typeparam name="T">Type of data to generate</typeparam>
        /// <param name="elements">elements</param>
        /// <param name="requestName">request name</param>
        /// <param name="constraints">constraints for generation</param>
        /// <returns>this</returns>
        public IThenSubmitAction AutoFillAs<T>(IEnumerable<IWebElement> elements, string requestName = null, object constraints = null)
        {
            return _actionProvider.AutoFillAs<T>(elements, requestName, constraints);
        }

        /// <summary>
        /// Fill elements with values
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns>fill action</returns>
        public IFillAction Fill(string selector)
        {
            return _actionProvider.Fill(selector);
        }

        /// <summary>
        /// Fill elements with values
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns>fill action</returns>
        public IFillAction Fill(By selector)
        {
            return _actionProvider.Fill(selector);
        }

        /// <summary>
        /// Fill elements with values
        /// </summary>
        /// <param name="elements">elements</param>
        /// <returns>fill action</returns>
        public IFillAction Fill(IEnumerable<IWebElement> elements)
        {
            return _actionProvider.Fill(elements);
        }

        /// <summary>
        /// Wait for something to happen
        /// </summary>
        public IWaitAction Wait
        {
            get { return _actionProvider.Wait; }
        }

        /// <summary>
        /// Submit form.
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IYieldsAction Submit(string selector)
        {
            return _actionProvider.Submit(selector);
        }

        public IYieldsAction Submit(By selector)
        {
            return _actionProvider.Submit(selector);
        }

        /// <summary>
        /// Switch to
        /// </summary>
        public ISwitchToAction SwitchTo
        {
            get { return _actionProvider.SwitchTo; }
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
        /// <param name=ElementContants.TypeAttribute>Type of object to Generate</param>
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

            SetupDependencyInjection(webDriver, configuration, dataConfiguration);
        }

        private void SetupDependencyInjection(IWebDriver webDriver,
            SeleniumFixtureConfiguration configuration,
            DefaultFixtureConfiguration dataConfiguration)
        {
            Data = new SimpleFixture.Fixture(dataConfiguration);

            Data.Return(this);
            Data.Return(webDriver);
            Data.Return(configuration.BaseAddress).WhenNamed("BaseAddress");
            Data.Return(() => Data.Configuration.Locate<IConstraintHelper>());

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

            if (Configuration.ExecuteValidate)
            {
                Data.Behavior.Add(ValidateBehavior);
            }

            Data.Export<AutoFillAction>().As<IAutoFillAction>();
            Data.Export<AutoFillAsActionProvider>().As<IAutoFillAsActionProvider>();
            Data.Export<ClickAction>().As<IClickAction>();
            Data.Export<FillAction>().As<IFillAction>();
            Data.Export<GetAction>().As<IGetAction>();
            Data.Export<MouseMoveAction>().As<IMouseMoveAction>();
            Data.Export<NavigateAction>().As<INavigateAction>();
            Data.Export<WaitAction>().As<IWaitAction>();
            Data.Export<YieldsAction>().As<IYieldsAction>();
        }

        private object ValidateBehavior(DataRequest request, object instance)
        {
            if (!request.Populate &&
                !instance.GetType().IsValueType &&
                !(instance is string))
            {
                var member = instance.GetType()
                    .GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(m => m.Name == Configuration.ValidateMember);

                if (member != null)
                {
                    Action action = null;

                    switch (member.MemberType)
                    {
                        case MemberTypes.Property:
                            PropertyInfo propertyInfo = (PropertyInfo)member;

                            if (propertyInfo.GetMethod != null)
                            {
                                action = propertyInfo.GetValue(instance) as Action;
                            }
                            break;
                        case MemberTypes.Method:
                            MethodInfo methodInfo = (MethodInfo)member;

                            if (!methodInfo.GetParameters().Any() && methodInfo.ReturnType == typeof(void))
                            {
                                action = (Action)methodInfo.CreateDelegate(typeof(Action), instance);
                            }
                            break;
                    }

                    if (action != null)
                    {
                        try
                        {
                            action();
                        }
                        catch (Exception exp)
                        {
                            if (!Configuration.WrapValidationExceptions)
                            {
                                throw;
                            }

                            string formatString = AssertionFailedException.FormatErrorMessage(exp, instance.GetType());

                            Exception newException = null;

                            try
                            {
                                newException = (Exception)Activator.CreateInstance(exp.GetType(), formatString);
                            }
                            catch (Exception)
                            {
                                
                            }

                            if (newException != null)
                            {
                                throw newException;
                            }

                            throw;
                        }
                    }
                }
            }

            return instance;
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
