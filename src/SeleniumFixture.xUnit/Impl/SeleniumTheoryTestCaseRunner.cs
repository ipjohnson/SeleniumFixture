using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace SeleniumFixture.xUnit.Impl
{
    public class SeleniumTheoryTestCaseRunner : XunitTheoryTestCaseRunner
    {
        public SeleniumTheoryTestCaseRunner(IXunitTestCase testCase, string displayName, string skipReason, object[] constructorArguments, IMessageBus messageBus, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource) : 
            base(testCase, displayName, skipReason, constructorArguments, messageBus, aggregator, cancellationTokenSource)
        {
        }

        /// <inheritdoc/>
        protected override async Task<RunSummary> RunTestAsync()
        {

            var runSummary = new RunSummary();

            try
            {
                var dataAttributes = TestCase.TestMethod.Method.GetCustomAttributes(typeof(DataAttribute));

                if(await RunDataAttributeTests(dataAttributes, runSummary))
                {
                    return runSummary;
                }

                dataAttributes = TestCase.TestMethod.TestClass.Class.GetCustomAttributes(typeof(DataAttribute));

                if (await RunDataAttributeTests(dataAttributes, runSummary))
                {
                    return runSummary;
                }

                dataAttributes = TestCase.TestMethod.TestClass.Class.Assembly.GetCustomAttributes(typeof(DataAttribute));

                await RunDataAttributeTests(dataAttributes, runSummary);
            }
            catch (Exception ex)
            {
                var test = new XunitTest(TestCase, DisplayName);

                if (!MessageBus.QueueMessage(new TestStarting(test)))
                    CancellationTokenSource.Cancel();
                else
                {
                    if (!MessageBus.QueueMessage(new TestFailed(test, 0, null, ex.Unwrap())))
                        CancellationTokenSource.Cancel();
                }

                if (!MessageBus.QueueMessage(new TestFinished(test, 0, null)))
                    CancellationTokenSource.Cancel();

                return new RunSummary { Total = 1, Failed = 1 };
            }

            return runSummary;
        }

        private async Task<bool> RunDataAttributeTests(IEnumerable<IAttributeInfo> dataAttributes, RunSummary runSummary)
        {
            bool foundTest = false;

            foreach (IAttributeInfo dataAttribute in dataAttributes)
            {
                var discovererAttribute = dataAttribute.GetCustomAttributes(typeof(DataDiscovererAttribute)).First();
                var args = discovererAttribute.GetConstructorArguments().Cast<string>().ToList();
                var discovererType = Reflector.GetType(args[1], args[0]);
                var discoverer = ExtensibilityPointFactory.GetDataDiscoverer(discovererType);

                foreach (var dataRow in discoverer.GetData(dataAttribute, TestCase.TestMethod.Method))
                {
                    ITypeInfo[] resolvedTypes = null;
                    var methodToRun = TestMethod;

                    if (methodToRun.IsGenericMethodDefinition)
                    {
                        resolvedTypes = TypeUtility.ResolveGenericTypes(TestCase.TestMethod.Method, dataRow);
                        methodToRun =
                            methodToRun.MakeGenericMethod(resolvedTypes.Select(t => ((IReflectionTypeInfo)t).Type).ToArray());
                    }

                    var parameterTypes = methodToRun.GetParameters().Select(p => p.ParameterType).ToArray();
                    var convertedDataRow = Reflector.ConvertArguments(dataRow, parameterTypes);
                    var theoryDisplayName = CreateTheoryDisplayName(TestCase.TestMethod.Method,
                        DisplayName,
                        convertedDataRow,
                        resolvedTypes);

                    var test = new XunitTest(TestCase, theoryDisplayName);

                    var testSummary = await new XunitTestRunner(test,
                                                                MessageBus,
                                                                TestClass,
                                                                ConstructorArguments,
                                                                methodToRun,
                                                                convertedDataRow,
                                                                SkipReason,
                                                                BeforeAfterAttributes,
                                                                Aggregator,
                                                                CancellationTokenSource).RunAsync();

                    runSummary.Aggregate(testSummary);

                    if (runSummary.Failed > 0)
                    {
                        TakeScreenShot(dataRow, theoryDisplayName);
                    }

                    DisposeOfData(dataRow);

                    foundTest = true;
                }
            }

            return foundTest;
        }

        private void TakeScreenShot(IEnumerable<object> dataRow, string displayName)
        {
            Fixture fixture = (Fixture)dataRow.FirstOrDefault(o => o is Fixture);

            if (fixture != null)
            {
                fixture.TakeScreenshot(displayName);
            }
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

        private void DisposeOfData(IEnumerable<object> dataRow)
        {
            foreach (object o in dataRow)
            {
                IDisposable disposable = o as IDisposable;

                if (disposable != null)
                {
                    disposable.Dispose();
                }
                else
                {
                    Fixture fixture = o as Fixture;

                    if (fixture != null)
                    {
                        fixture.Driver.Dispose();
                    }
                }
            }
        }
    }
}
