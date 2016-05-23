using OpenQA.Selenium;
using SimpleFixture.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;
using System.Reflection;
using System.IO;

namespace SeleniumFixture.xUnit.Impl
{
    public class SeleniumTheoryTestCaseRunner : XunitTestCaseRunner
    {
        static readonly object[] NoArguments = new object[0];
        static readonly System.Reflection.MethodInfo FreezeMethod;

        static SeleniumTheoryTestCaseRunner()
        {
            FreezeMethod = typeof(SeleniumTheoryTestCaseRunner).GetMethod("FreezeValue");
        }

        readonly ExceptionAggregator cleanupAggregator = new ExceptionAggregator();
        readonly IMessageSink diagnosticMessageSink;

        /// <summary>
        /// Initializes a new instance of the <see cref="XunitTheoryTestCaseRunner"/> class.
        /// </summary>
        /// <param name="testCase">The test case to be run.</param>
        /// <param name="displayName">The display name of the test case.</param>
        /// <param name="skipReason">The skip reason, if the test is to be skipped.</param>
        /// <param name="constructorArguments">The arguments to be passed to the test class constructor.</param>
        /// <param name="diagnosticMessageSink">The message sink used to send diagnostic messages</param>
        /// <param name="messageBus">The message bus to report run status to.</param>
        /// <param name="aggregator">The exception aggregator used to run code and collect exceptions.</param>
        /// <param name="cancellationTokenSource">The task cancellation token source, used to cancel the test run.</param>
        public SeleniumTheoryTestCaseRunner(IXunitTestCase testCase,
                                         string displayName,
                                         string skipReason,
                                         object[] constructorArguments,
                                         IMessageSink diagnosticMessageSink,
                                         IMessageBus messageBus,
                                         ExceptionAggregator aggregator,
                                         CancellationTokenSource cancellationTokenSource)
            : base(testCase, displayName, skipReason, constructorArguments, NoArguments, messageBus, aggregator, cancellationTokenSource)
        {
            this.diagnosticMessageSink = diagnosticMessageSink;
        }

        /// <inheritdoc/>
        protected override async Task AfterTestCaseStartingAsync()
        {
            await base.AfterTestCaseStartingAsync();
        }

        /// <inheritdoc/>
        protected override Task BeforeTestCaseFinishedAsync()
        {
            Aggregator.Aggregate(cleanupAggregator);

            return base.BeforeTestCaseFinishedAsync();
        }

        /// <inheritdoc/>
        protected override async Task<RunSummary> RunTestAsync()
        {
            var runSummary = new RunSummary();

            try
            {
                var runtimeMethod = GetMethodInfo(TestCase.TestMethod.Method);
                var driverAttributes = ReflectionHelper.GetAttributes<WebDriverAttribute>(runtimeMethod);
                var dataAttributes = new List<IAttributeInfo>(
                            TestCase.TestMethod.Method.GetCustomAttributes(typeof(DataAttribute)));

                if (dataAttributes.Count > 0)
                {
                    foreach (var dataAttribute in dataAttributes)
                    {
                        var discovererAttribute = dataAttribute.GetCustomAttributes(typeof(DataDiscovererAttribute)).First();
                        var args = discovererAttribute.GetConstructorArguments().Cast<string>().ToList();
                        var discovererType = SerializationHelper.GetType(args[1], args[0]);
                        var discoverer = ExtensibilityPointFactory.GetDataDiscoverer(diagnosticMessageSink, discovererType);

                        foreach (var dataRow in discoverer.GetData(dataAttribute, TestCase.TestMethod.Method))
                        {
                            await ExecuteTestMethod(runtimeMethod, runSummary, driverAttributes, dataRow);
                        }
                    }
                }
                else
                {
                    await ExecuteTestMethod(runtimeMethod, runSummary, driverAttributes, new object[0]);
                }
            }
            catch (Exception ex)
            {
                return RunTest_DataDiscoveryException(ex);
            }

            return runSummary;
        }

        private async Task ExecuteTestMethod(MethodInfo runtimeMethod, RunSummary runSummary, IEnumerable<WebDriverAttribute> driverAttributes, object[] dataRow)
        {
            foreach (var driverAttribute in driverAttributes)
            {
                foreach (var driver in driverAttribute.GetDrivers(runtimeMethod))
                {
                    Fixture newFixture = null;
                    object initializerReturn = null;

                    ITypeInfo[] resolvedTypes = null;
                    var methodToRun = runtimeMethod;

                    if (methodToRun.IsGenericMethodDefinition)
                    {
                        resolvedTypes = TestCase.TestMethod.Method.ResolveGenericTypes(dataRow);
                        methodToRun = methodToRun.MakeGenericMethod(resolvedTypes.Select(t => ((IReflectionTypeInfo)t).Type).ToArray());
                    }

                    List<object> parameterList = new List<object>();
                    var parameters = methodToRun.GetParameters().ToArray();

                    try
                    {
                        newFixture = FixtureCreationAttribute.GetNewFixture(driver, runtimeMethod);

                        var initializeDataAttributes = ReflectionHelper.GetAttributes<FixtureInitializationAttribute>(runtimeMethod);

                        foreach (var initializeDataAttribute in initializeDataAttributes)
                        {
                            if (initializeDataAttribute is IMethodInfoAware)
                            {
#if DNX
                                var property = initializeDataAttribute.GetType().GetRuntimeProperty("Method");

                                property.SetValue(initializeDataAttribute, runtimeMethod);
#else
                                ((IMethodInfoAware)initializeDataAttribute).Method = runtimeMethod;
#endif
                            }

                            initializeDataAttribute.Initialize(newFixture.Data);
                        }

                        var initializeAttribute = ReflectionHelper.GetAttribute<IFixtureInitializationAttribute>(runtimeMethod);

                        if (initializeAttribute != null)
                        {
                            initializerReturn = initializeAttribute.Initialize(runtimeMethod, newFixture);
                        }

                        int dataRowIndex = 0;

                        for (int i = 0; i < parameters.Length; i++)
                        {
                            var parameter = parameters[i];
                            var attributes = parameter.GetCustomAttributes(true);

                            if (parameter.ParameterType == typeof(IWebDriver))
                            {
                                parameterList.Add(driver);
                            }
                            else if (parameter.ParameterType == typeof(Fixture))
                            {
                                parameterList.Add(newFixture);
                            }
                            else if (attributes.Any(a => a is GenerateAttribute))
                            {
                                var generateAttribute = (GenerateAttribute)attributes.First(a => a is GenerateAttribute);

                                InitializeCustomAttribute(generateAttribute, runtimeMethod, parameter);

                                var constraintName = generateAttribute.ConstraintName ?? parameter.Name;
                                var min = generateAttribute.Min;
                                var max = generateAttribute.Max;

                                var value = newFixture.Data.Generate(parameter.ParameterType, constraintName, new { min, max });
                                parameterList.Add(value);
                            }
                            else if (attributes.Any(a => a is LocateAttribute))
                            {
                                var locateAttribute = (LocateAttribute)attributes.First(a => a is LocateAttribute);

                                InitializeCustomAttribute(locateAttribute, runtimeMethod, parameter);

                                var value = locateAttribute.Value;

                                if (value == null)
                                {
                                    value = newFixture.Data.Generate(new SimpleFixture.DataRequest(null,
                                                                                                   newFixture.Data,
                                                                                                   parameter.ParameterType,
                                                                                                   parameter.Name,
                                                                                                   false,
                                                                                                   null,
                                                                                                   null));
                                }

                                parameterList.Add(value);
                            }
                            else if (attributes.Any(a => a is FreezeAttribute))
                            {
                                var freeze = (FreezeAttribute)attributes.FirstOrDefault(a => a is FreezeAttribute);

                                InitializeCustomAttribute(freeze, runtimeMethod, parameter);

                                var value = freeze.Value;

                                if (value == null)
                                {
                                    var constraintName = freeze.ConstraintName ?? parameter.Name;
                                    var min = freeze.Min;
                                    var max = freeze.Max;

                                    value = newFixture.Data.Generate(parameter.ParameterType, constraintName, new { min, max });
                                }

                                parameterList.Add(value);

                                object lastObject = parameterList.Last();
                                var closedFreezeMethod =
                                    FreezeMethod.MakeGenericMethod(lastObject.GetType());

                                closedFreezeMethod.Invoke(null, new object[] { newFixture.Data, value, freeze.For });
                            }
                            else if (initializerReturn != null && parameter.ParameterType == initializerReturn.GetType())
                            {
                                parameterList.Add(initializerReturn);
                                initializerReturn = null;
                            }
                            else if (dataRowIndex < dataRow.Length)
                            {
                                var dataValue = dataRow[dataRowIndex];
                                dataRowIndex++;
                                parameterList.Add(dataValue);
                            }
                            else
                            {
                                var value = newFixture.Data.Generate(parameter.ParameterType, parameter.Name);
                                parameterList.Add(value);
                            }
                        }
                    }
                    catch (Exception exp)
                    {
                        Aggregator.Add(exp);
                    }

                    var convertedDataRow = Reflector.ConvertArguments(parameterList.ToArray(), parameters.Select(p => p.ParameterType).ToArray());
                    var theoryDisplayName =
                        TestCase.TestMethod.Method.GetDisplayNameWithArguments(DisplayName + " " + GetDriverName(driver),
                                                                               dataRow,
                                                                               resolvedTypes);

                    var test = new XunitTest(TestCase, theoryDisplayName);
                    var skipReason = SkipReason;
                    XunitTestRunner testRunner = CreateTestRunner(test, MessageBus, TestClass, ConstructorArguments, methodToRun, convertedDataRow, skipReason, BeforeAfterAttributes, Aggregator, CancellationTokenSource);

                    runSummary.Aggregate(await testRunner.RunAsync());

                    var timer = new ExecutionTimer();
                    timer.Aggregate(() => DisposeOfData(driverAttribute, driver, newFixture, dataRow));

                    runSummary.Time += timer.Total;
                }
            }
        }

        private XunitTestRunner CreateTestRunner(XunitTest test, IMessageBus messageBus, Type testClass, object[] constructorArguments, MethodInfo methodToRun, object[] convertedDataRow, string skipReason, IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
        {
#if DNX
            var constructor = typeof(XunitTestRunner).GetConstructors().First(c => c.GetParameters().Count() == 10);
            return (XunitTestRunner)constructor.Invoke(new object[] { test, MessageBus, TestClass, ConstructorArguments, methodToRun, convertedDataRow, skipReason, BeforeAfterAttributes, Aggregator, CancellationTokenSource });
#else
            return new XunitTestRunner(test, MessageBus, TestClass, ConstructorArguments, methodToRun, convertedDataRow, skipReason, BeforeAfterAttributes, Aggregator, CancellationTokenSource);
#endif
        }

        private void InitializeCustomAttribute(object attribute, MethodInfo runtimeMethod, ParameterInfo parameterInfo)
        {
            var parameterAware = attribute as IParameterInfoAware;
            var methodInfoAware = attribute as IMethodInfoAware;
#if DNX            
            if (methodInfoAware != null)
            {
                var property = methodInfoAware.GetType().GetRuntimeProperty("Method");

                property.SetValue(methodInfoAware, runtimeMethod);
            }

            if(parameterAware != null)
            {
                var property = parameterAware.GetType().GetRuntimeProperty("Parameter");

                property.SetValue(parameterAware, parameterInfo);
            }
#else
            if (methodInfoAware != null)
            {
                methodInfoAware.Method = runtimeMethod;
            }

            if (parameterAware != null)
            {
                parameterAware.Parameter = parameterInfo;
            }     
#endif
        }

        private void DisposeOfData(WebDriverAttribute driverAttribute, IWebDriver driver, Fixture newFixture, object[] dataRow)
        {
            var runtimeMethod = GetMethodInfo(TestCase.Method);

            var finalizeAttribute = ReflectionHelper.GetAttribute<IFixtureFinalizerAttribute>(runtimeMethod);

            if (finalizeAttribute != null)
            {
                finalizeAttribute.IFixtureFinalizerAttribute(runtimeMethod, newFixture);
            }

            foreach (var data in dataRow)
            {
                IDisposable disposable = data as IDisposable;

                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }

            driverAttribute.ReturnDriver(runtimeMethod, driver);
        }

        private MethodInfo GetMethodInfo(IMethodInfo testMethod)
        {
#if DNX
            var toRuntimeMethod = typeof(ReflectionAbstractionExtensions).GetMethod("ToRuntimeMethod");

            return (MethodInfo)toRuntimeMethod.Invoke(null, new object[] { testMethod });
#else
            return testMethod.ToRuntimeMethod();
#endif
        }

        RunSummary RunTest_DataDiscoveryException(Exception dataDiscoveryException)
        {
            var test = new XunitTest(TestCase, DisplayName);

            if (!MessageBus.QueueMessage(new TestStarting(test)))
                CancellationTokenSource.Cancel();
            else if (!MessageBus.QueueMessage(new TestFailed(test, 0, null, dataDiscoveryException.Unwrap())))
                CancellationTokenSource.Cancel();
            if (!MessageBus.QueueMessage(new TestFinished(test, 0, null)))
                CancellationTokenSource.Cancel();

            return new RunSummary { Total = 1, Failed = 1 };
        }

        private string CreateTheoryDisplayName(IMethodInfo method, string displayName, object[] convertedDataRow, ITypeInfo[] resolvedTypes)
        {
            string returnString = displayName;

            if (convertedDataRow.Length == 1)
            {
                IWebDriver driver = convertedDataRow[0] as IWebDriver;

                if (driver == null && convertedDataRow[0] is Fixture)
                {
                    driver = ((Fixture)convertedDataRow[0]).Driver;
                }

                if (driver != null)
                {
                    var property = driver.GetType().GetProperty("Capabilities");

                    if (property != null)
                    {
                        ICapabilities capabilities = property.GetValue(driver) as ICapabilities;

                        if (capabilities != null)
                        {
                            returnString += string.Format(" {0} {1}", capabilities.BrowserName, capabilities.Version);
                        }
                    }
                }
            }

            return returnString;
        }

        private string GetDriverName(IWebDriver driver)
        {
            string returnString = "";

            if (driver != null)
            {
                var property = driver.GetType().GetProperty("Capabilities");

                if (property != null)
                {
                    ICapabilities capabilities = property.GetValue(driver) as ICapabilities;

                    if (capabilities != null)
                    {
                        returnString += string.Format(" {0} {1}", capabilities.BrowserName, capabilities.Version);
                    }
                }
            }

            return returnString;
        }

        private static void FreezeValue<T>(SimpleFixture.Fixture fixture, T freezeValue, Type forType)
        {
            var returnValue = fixture.Return(freezeValue);

            if (forType != null)
            {
                returnValue.For(forType);
            }
        }
    }
}
