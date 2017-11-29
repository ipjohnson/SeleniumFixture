using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
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

        /// <summary>
        /// Fixture constructor takes driver and base address
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="baseAddress"></param>
        public Fixture(IWebDriver webDriver, string baseAddress = null)
            : this(webDriver, new SeleniumFixtureConfiguration { BaseAddress = baseAddress })
        {
        }

        /// <summary>
        /// Fixture constructor takes driver and configuration
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="configuration"></param>
        public Fixture(IWebDriver webDriver, SeleniumFixtureConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

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
        public INavigateAction Navigate => _actionProvider.Navigate;

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
        /// Focus an element
        /// </summary>
        /// <param name="selector"></param>
        public void Focus(string selector)
        {
            _actionProvider.Focus(selector);
        }

        /// <summary>
        /// Focus an element
        /// </summary>
        /// <param name="selector"></param>
        public void Focus(By selector)
        {
            _actionProvider.Focus(selector);
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
        public IGetAction Get => _actionProvider.Get;

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
        /// Clear elements specified
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns></returns>
        public IActionProvider Clear(string selector)
        {
            return _actionProvider.Clear(selector);
        }

        /// <summary>
        /// Clear elements specified
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns></returns>
        public IActionProvider Clear(By selector)
        {
            return _actionProvider.Clear(selector);
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
        /// SwitchTo Alert and dismiss
        /// </summary>
        /// <returns></returns>
        public IActionProvider DismissAlert()
        {
            return _actionProvider.DismissAlert();
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
        /// Execute arbitrary javascript
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="javascript"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public T ExecuteJavaScript<T>(string javascript, params object[] args)
        {
            return _actionProvider.ExecuteJavaScript<T>(javascript, args);
        }

        /// <summary>
        /// Execute arbitrary javascript
        /// </summary>
        /// <param name="javascript"></param>
        /// <param name="args"></param>
        public void ExecuteJavaScript(string javascript, params object[] args)
        {
            _actionProvider.ExecuteJavaScript(javascript, args);
        }

        /// <summary>
        /// SwitchTo alert and accept.
        /// </summary>
        /// <returns></returns>
        public IActionProvider AcceptAlert()
        {
            return _actionProvider.AcceptAlert();
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
        public IWaitAction Wait => _actionProvider.Wait;

        /// <summary>
        /// Send the value to a particular element or set of elements
        /// </summary>
        /// <param name="sendValue">value to send to elements</param>
        /// <returns></returns>
        public ISendToAction Send(object sendValue)
        {
            return _actionProvider.Send(sendValue);
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

        /// <summary>
        /// Submit form.
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns></returns>
        public IYieldsAction Submit(By selector)
        {
            return _actionProvider.Submit(selector);
        }

        /// <summary>
        /// Switch to
        /// </summary>
        public ISwitchToAction SwitchTo => _actionProvider.SwitchTo;

        /// <summary>
        /// Take a screen shot of the driver
        /// </summary>
        /// <param name="screenshotName">take screenshot, if null then ClassName_MethodName is used</param>
        /// <param name="throwsIfNotSupported">throw exception if screen shot is not supported by the current driver</param>
        /// <param name="format">format of image</param>
        /// <returns></returns>
        public IActionProvider TakeScreenshot(string screenshotName = null, bool throwsIfNotSupported = false, ScreenshotImageFormat format = ScreenshotImageFormat.Png)
        {
            return _actionProvider.TakeScreenshot(screenshotName, throwsIfNotSupported, format);
        }

        /// <summary>
        /// Fixture for this action provider
        /// </summary>
        public Fixture UsingFixture => _actionProvider.UsingFixture;

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

        /// <summary>
        /// Autofill provided elements with data
        /// </summary>
        /// <param name="elements">elements</param>
        /// <returns></returns>
        public IThenSubmitAction AutoFill(params IWebElement[] elements)
        {
            return _actionProvider.AutoFill(elements);
        }

        /// <summary>
        /// AutoFill the provided elements as a specific type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elements"></param>
        /// <returns></returns>
        public IThenSubmitAction AutoFillAs<T>(params IWebElement[] elements)
        {
            return _actionProvider.AutoFillAs<T>(elements);
        }

        /// <summary>
        /// Clear the specified elements
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public IActionProvider Clear(IEnumerable<IWebElement> elements)
        {
            return _actionProvider.Clear(elements);
        }

        /// <summary>
        /// Clear the specified elements
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public IActionProvider Clear(params IWebElement[] elements)
        {
            return _actionProvider.Clear(elements);
        }

        /// <summary>
        /// Click the specified elements
        /// </summary>
        /// <param name="elements">elements</param>
        /// <param name="clickMode">click mode</param>
        /// <returns></returns>
        public IActionProvider Click(IEnumerable<IWebDriver> elements, ClickMode clickMode = ClickMode.ClickOne)
        {
            return _actionProvider.Click(elements);
        }

        /// <summary>
        /// Click all of the specified elements
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public IActionProvider Click(params IWebElement[] elements)
        {
            return _actionProvider.Click(elements);
        }

        /// <summary>
        /// Double click the elements provided
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="clickMode"></param>
        /// <returns></returns>
        public IActionProvider DoubleClick(IEnumerable<IWebElement> elements, ClickMode clickMode = ClickMode.ClickOne)
        {
            return _actionProvider.DoubleClick(elements);
        }

        /// <summary>
        /// Double click all of the elements provided
        /// </summary>
        /// <param name="elements">elements</param>
        /// <returns></returns>
        public IActionProvider DoubleClick(params IWebElement[] elements)
        {
            return _actionProvider.DoubleClick(elements);
        }

        /// <summary>
        /// Fill the specified elements with data
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public IFillAction Fill(params IWebElement[] elements)
        {
            return _actionProvider.Fill(elements);
        }
        #endregion

        #region Private Members

        protected void Initialize(IWebDriver webDriver, SeleniumFixtureConfiguration configuration)
        {
            Configuration = configuration;

            var dataConfiguration = configuration.DataConfiguration;

            dataConfiguration.Export<ITypePropertySelector>(g => new SeleniumTypePropertySelector(configuration.DataConfiguration, g.Locate<IConstraintHelper>()));
            dataConfiguration.Export<IPropertySetter>(g => new Impl.PropertySetter());

            SetupDependencyInjection(webDriver, configuration, dataConfiguration);

            if (!(webDriver is IJavaScriptExecutor))
            {
                configuration.AlwaysWaitForAjax = false;
            }
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


#if !NETSTANDARD2_0
            Data.Behavior.Add((r, o) =>
                              {
                                  if (o.GetType().IsValueType || o is string)
                                  {
                                      return o;
                                  }
                                  PageFactory.InitElements(webDriver, o);


                                  return o;
                              });
#endif

            Data.Behavior.Add(ImportPropertiesOnLocate);

            _actionProvider = Configuration.ActionProvider(this);

            Data.Return(_actionProvider);

            if (Configuration.ExecuteValidate)
            {
                Data.Behavior.Add(ValidateBehavior);
            }

            Data.Export<AutoFillAction>().As<IAutoFillAction>();
            Data.Export<AutoFillAsActionProvider>().As<IAutoFillAsActionProvider>();
            Data.Export<ClearAction>().As<IClearAction>();
            Data.Export<ClickAction>().As<IClickAction>();
            Data.Export<DoubleClickAction>().As<IDoubleClickAction>();
            Data.Export<FillAction>().As<IFillAction>();
            Data.Export<GetAction>().As<IGetAction>();
            Data.Export<MouseMoveAction>().As<IMouseMoveAction>();
            Data.Export<NavigateAction>().As<INavigateAction>();
            Data.Export<SendToAction>().As<ISendToAction>();
            Data.Export<SwitchAction>().As<ISwitchToAction>();
            Data.Export<TakeScreenshotAction>().As<ITakeScreenshotAction>();
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
                            var propertyInfo = (PropertyInfo)member;

                            if (propertyInfo.GetMethod != null)
                            {
                                action = propertyInfo.GetValue(instance) as Action;
                            }
                            break;
                        case MemberTypes.Method:
                            var methodInfo = (MethodInfo)member;

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

                            var formatString = AssertionFailedException.FormatErrorMessage(exp, instance.GetType());

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

            var modelService = Data.Configuration.Locate<IModelService>();

            var constraintHelper = Data.Configuration.Locate<IConstraintHelper>();

            var typePopulator = new TypePopulator(Data.Configuration,
                                                            constraintHelper,
                                                            new ImportSeleniumTypePropertySelector(Data.Configuration, constraintHelper),
                                                            new Impl.PropertySetter(),
                                                            new TypeFieldSelector(constraintHelper), 
                                                            new FieldSetter());

            typePopulator.Populate(o, r, modelService.GetModel(r.RequestedType));

            return o;
        }

#endregion

    }
}
