using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace SeleniumFixture.xUnit.Impl
{
    public class SeleniumTheoryTestCaseRunner : XunitTestCaseRunner
    {
        static readonly object[] NoArguments = new object[0];

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
                var dataAttributes = TestCase.TestMethod.Method.GetCustomAttributes(typeof(DataAttribute));

                foreach (var dataAttribute in dataAttributes)
                {
                    var discovererAttribute = dataAttribute.GetCustomAttributes(typeof(DataDiscovererAttribute)).First();
                    var args = discovererAttribute.GetConstructorArguments().Cast<string>().ToList();
                    var discovererType = SerializationHelper.GetType(args[1], args[0]);
                    var discoverer = ExtensibilityPointFactory.GetDataDiscoverer(diagnosticMessageSink, discovererType);

                    foreach (var dataRow in discoverer.GetData(dataAttribute, TestCase.TestMethod.Method))
                    {
                        ITypeInfo[] resolvedTypes = null;
                        var methodToRun = TestMethod;

                        if (methodToRun.IsGenericMethodDefinition)
                        {
                            resolvedTypes = TestCase.TestMethod.Method.ResolveGenericTypes(dataRow);
                            methodToRun = methodToRun.MakeGenericMethod(resolvedTypes.Select(t => ((IReflectionTypeInfo)t).Type).ToArray());
                        }

                        var parameterTypes = methodToRun.GetParameters().Select(p => p.ParameterType).ToArray();
                        var convertedDataRow = Reflector.ConvertArguments(dataRow, parameterTypes);
                        var theoryDisplayName = CreateTheoryDisplayName(TestCase.TestMethod.Method, DisplayName, convertedDataRow, resolvedTypes);
                        var test = new XunitTest(TestCase, theoryDisplayName);
                        var skipReason = SkipReason;
                        var testRunner = new XunitTestRunner(test, MessageBus, TestClass, ConstructorArguments, methodToRun, convertedDataRow, skipReason, BeforeAfterAttributes, Aggregator, CancellationTokenSource);

                        runSummary.Aggregate(await testRunner.RunAsync());

                        var timer = new ExecutionTimer();
                        timer.Aggregate(() => DisposeOfData(convertedDataRow));

                        runSummary.Time += timer.Total;
                    }
                }
            }
            catch (Exception ex)
            {
                return RunTest_DataDiscoveryException(ex);
            }

            return runSummary;
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
