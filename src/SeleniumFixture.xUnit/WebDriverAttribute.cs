using OpenQA.Selenium;
using SeleniumFixture.xUnit.Impl;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace SeleniumFixture.xUnit
{
    public abstract class WebDriverAttribute : Attribute
    {
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

        protected T GetOrCreateWebDriver<T>(Func<T> createMethod) where T : IWebDriver
        {
            if(Shared)
            {
                return InternalStorageHelper<T>.Instance.GetOrAdd(createMethod);
            }

            return createMethod();
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
