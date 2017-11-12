using OpenQA.Selenium;
using SeleniumFixture.xUnit.Impl;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SeleniumFixture.xUnit
{
    public abstract class WebDriverAttribute : Attribute
    {
        /// <summary>
        /// The default command timeout for HTTP requests in a RemoteWebDriver instance.
        /// </summary>
        protected static readonly TimeSpan DefaultCommandTimeout = TimeSpan.FromSeconds(60);

        private class InternalStorageHelper<T> where T : IWebDriver
        {
            private static InternalStorageHelper<T> _instance;
            private static object _lockObject = new object();
            private List<T> _allInstances = new List<T>();
            private Stack<T> _freeInstances = new Stack<T>();

            ~InternalStorageHelper()
            {
                _freeInstances.Clear();
                
                foreach(var instances in _allInstances)
                {
                    instances.Dispose();
                }
            }

            public T GetOrAdd(Func<T> create)
            {
                T tInstance;
                lock(_freeInstances)
                {
                    if(_freeInstances.Count > 0)
                    {
                        tInstance = _freeInstances.Pop();
                    }
                    else
                    {
                        tInstance = create();

                        _allInstances.Add(tInstance);
                    }
                }

                return tInstance;
            }

            public void ReturnInstance(T instance)
            {
                lock(_freeInstances)
                {
                    _freeInstances.Push(instance);
                }
            }

            public static InternalStorageHelper<T> Instance
            {
                get
                {
                    if(_instance == null)
                    {
                        lock(_lockObject)
                        {
                            if (_instance == null)
                            {
                                _instance = new InternalStorageHelper<T>();
                            }
                        }
                    }

                    return _instance;
                }
            }
        }

        protected virtual T GetOrCreateWebDriver<T>(MethodInfo method, Func<T> createMethod) where T : IWebDriver
        {
            if(Shared)
            {
                var sharedInstance = GetSharedInstance(createMethod);

                ResetWebDriver(method, sharedInstance);

                return sharedInstance;
            }

            return createMethod();
        }

        protected virtual void ResetWebDriver<T>(MethodInfo method, T driver) where T : IWebDriver
        {
            var resetAttribute = ReflectionHelper.GetAttribute<IWebDriverResetAttribute>(method);

            if (resetAttribute != null)
            {
                resetAttribute.ResetWebDriver(method, driver);
            }
            else
            {
                driver.Manage().Cookies.DeleteAllCookies();
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl("about:blank");
            }
        }

        protected static TimeSpan GetWebDriverCommandTimeout(MethodInfo method)
        {
            var commandTimeoutAttribute = ReflectionHelper.GetAttribute<WebDriverCommandTimeoutAttribute>(method);
            return commandTimeoutAttribute?.Timeout ?? DefaultCommandTimeout;
        }

        protected T GetSharedInstance<T>(Func<T> createMethod) where T : IWebDriver
        {
            return InternalStorageHelper<T>.Instance.GetOrAdd(createMethod);
        }

        public bool Shared { get; set; }

        public abstract IEnumerable<IWebDriver> GetDrivers(MethodInfo testMethod);

        public abstract void ReturnDriver(MethodInfo testMethod, IWebDriver driver);

        protected virtual void ReturnDriver<T>(MethodInfo testMethod, T driver) where T : IWebDriver
        {
            var finalizerAttribute = ReflectionHelper.GetAttribute<IWebDriverFinalizerAttribute>(testMethod);

            if(finalizerAttribute != null)
            {
                finalizerAttribute.Finalize(testMethod, driver);               
            }

            if(driver != null)
            {
                if (Shared)
                {
                    InternalStorageHelper<T>.Instance.ReturnInstance(driver);
                }
                else
                {
                    driver.Dispose();
                }
            }
        }
                
        public static void InitializeDriver(MethodInfo testMethod, IWebDriver driver)
        {
            var initializeAttribute = ReflectionHelper.GetAttribute<IWebDriverInitializationAttribute>(testMethod);

            if (initializeAttribute != null)
            {
                initializeAttribute.Initialize(testMethod, driver);
            }
        }
    }
}
