using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace SeleniumFixture.xUnit.Impl
{
    public class SeleniumTestInvoker
    {
        readonly IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes;
        readonly Stack<BeforeAfterTestAttribute> beforeAfterAttributesRun = new Stack<BeforeAfterTestAttribute>();

        /// <summary>
        /// Initializes a new instance of the <see cref="XunitTestInvoker"/> class.
        /// </summary>
        /// <param name="test">The test that this invocation belongs to.</param>
        /// <param name="messageBus">The message bus to report run status to.</param>
        /// <param name="testClass">The test class that the test method belongs to.</param>
        /// <param name="constructorArguments">The arguments to be passed to the test class constructor.</param>
        /// <param name="testMethod">The test method that will be invoked.</param>
        /// <param name="testMethodArguments">The arguments to be passed to the test method.</param>
        /// <param name="beforeAfterAttributes">The list of <see cref="BeforeAfterTestAttribute"/>s for this test invocation.</param>
        /// <param name="aggregator">The exception aggregator used to run code and collect exceptions.</param>
        /// <param name="cancellationTokenSource">The task cancellation token source, used to cancel the test run.</param>
        public SeleniumTestInvoker(ITest test,
                                IMessageBus messageBus,
                                Type testClass,
                                object[] constructorArguments,
                                MethodInfo testMethod,
                                object[] testMethodArguments,
                                IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes,
                                ExceptionAggregator aggregator,
                                CancellationTokenSource cancellationTokenSource)
        {
            
            Test = test;
            MessageBus = messageBus;
            TestClass = testClass;
            ConstructorArguments = constructorArguments;
            TestMethod = testMethod;
            TestMethodArguments = testMethodArguments;
            Aggregator = aggregator;
            CancellationTokenSource = cancellationTokenSource;

            Timer = new ExecutionTimer();
            this.beforeAfterAttributes = beforeAfterAttributes;
        }

        /// <inheritdoc/>
        protected Task BeforeTestMethodInvokedAsync()
        {
            foreach (var beforeAfterAttribute in beforeAfterAttributes)
            {
                var attributeName = beforeAfterAttribute.GetType().Name;
                if (!MessageBus.QueueMessage(new BeforeTestStarting(Test, attributeName)))
                    CancellationTokenSource.Cancel();
                else
                {
                    try
                    {
                        Timer.Aggregate(() => beforeAfterAttribute.Before(TestMethod));
                        beforeAfterAttributesRun.Push(beforeAfterAttribute);
                    }
                    catch (Exception ex)
                    {
                        Aggregator.Add(ex);
                        break;
                    }
                    finally
                    {
                        if (!MessageBus.QueueMessage(new BeforeTestFinished(Test, attributeName)))
                            CancellationTokenSource.Cancel();
                    }
                }

                if (CancellationTokenSource.IsCancellationRequested)
                    break;
            }

            return Task.FromResult(0);
        }

        /// <inheritdoc/>
        protected Task AfterTestMethodInvokedAsync()
        {
            foreach (var beforeAfterAttribute in beforeAfterAttributesRun)
            {
                var attributeName = beforeAfterAttribute.GetType().Name;
                if (!MessageBus.QueueMessage(new AfterTestStarting(Test, attributeName)))
                    CancellationTokenSource.Cancel();

                Aggregator.Run(() => Timer.Aggregate(() => beforeAfterAttribute.After(TestMethod)));

                if (!MessageBus.QueueMessage(new AfterTestFinished(Test, attributeName)))
                    CancellationTokenSource.Cancel();
            }

            return Task.FromResult(0);
        }

        /// <summary>
        /// Gets or sets the exception aggregator used to run code and collect exceptions.
        /// </summary>
        protected ExceptionAggregator Aggregator { get; set; }

        /// <summary>
        /// Gets or sets the task cancellation token source, used to cancel the test run.
        /// </summary>
        protected CancellationTokenSource CancellationTokenSource { get; set; }

        /// <summary>
        /// Gets or sets the constructor arguments used to construct the test class.
        /// </summary>
        protected object[] ConstructorArguments { get; set; }

        /// <summary>
        /// Gets the display name of the invoked test.
        /// </summary>
        protected string DisplayName { get { return Test.DisplayName; } }

        /// <summary>
        /// Gets or sets the message bus to report run status to.
        /// </summary>
        protected IMessageBus MessageBus { get; set; }

        /// <summary>
        /// Gets or sets the test to be run.
        /// </summary>
        protected ITest Test { get; set; }

        /// <summary>
        /// Gets the test case to be run.
        /// </summary>
        protected ITestCase TestCase { get { return Test.TestCase; } }

        /// <summary>
        /// Gets or sets the runtime type of the class that contains the test method.
        /// </summary>
        protected Type TestClass { get; set; }

        /// <summary>
        /// Gets or sets the runtime method of the method that contains the test.
        /// </summary>
        protected MethodInfo TestMethod { get; set; }

        /// <summary>
        /// Gets or sets the arguments to pass to the test method when it's being invoked.
        /// </summary>
        protected object[] TestMethodArguments { get; set; }

        /// <summary>
        /// Gets or sets the object which measures execution time.
        /// </summary>
        protected ExecutionTimer Timer { get; set; }

        /// <summary>
        /// Creates the test class, unless the test method is static or there have already been errors.
        /// </summary>
        /// <returns>The class instance, if appropriate; <c>null</c>, otherwise</returns>
        protected object CreateTestClass()
        {
            object testClass = null;

            if (!TestMethod.IsStatic && !Aggregator.HasExceptions)
                testClass = Test.CreateTestClass(TestClass, ConstructorArguments, MessageBus, Timer, CancellationTokenSource);

            return testClass;
        }

        /// <summary>
        /// Creates the test class (if necessary), and invokes the test method.
        /// </summary>
        /// <returns>Returns the time (in seconds) spent creating the test class, running
        /// the test, and disposing of the test class.</returns>
        public Task<decimal> RunAsync()
        {
            return Aggregator.RunAsync(async () =>
            {
                if (!CancellationTokenSource.IsCancellationRequested)
                {
                    var testClassInstance = CreateTestClass();

                    if (!CancellationTokenSource.IsCancellationRequested)
                    {
                        await BeforeTestMethodInvokedAsync();

                        if (!Aggregator.HasExceptions)
                            await InvokeTestMethodAsync(testClassInstance);

                        await AfterTestMethodInvokedAsync();
                    }

                    Aggregator.Run(() => Test.DisposeTestClass(testClassInstance, MessageBus, Timer, CancellationTokenSource));
                }

                return Timer.Total;
            });
        }

        /// <summary>
        /// Invokes the test method on the given test class instance.
        /// </summary>
        /// <param name="testClassInstance">The test class instance</param>
        /// <returns>Returns the time taken to invoke the test method</returns>
        public virtual async Task<decimal> InvokeTestMethodAsync(object testClassInstance)
        {
            var oldSyncContext = SynchronizationContext.Current;

            try
            {
                var asyncSyncContext = new AsyncTestSyncContext(oldSyncContext);
                SetSynchronizationContext(asyncSyncContext);

                await Aggregator.RunAsync(
                    () => Timer.AggregateAsync(
                        async () =>
                        {
                            var result = TestMethod.Invoke(testClassInstance, TestMethodArguments);
                            var task = result as Task;
                            if (task != null)
                                await task;
                            else
                            {
                                var ex = await asyncSyncContext.WaitForCompletionAsync();
                                if (ex != null)
                                    Aggregator.Add(ex);
                            }
                        }
                    )
                );
            }
            finally
            {
                SetSynchronizationContext(oldSyncContext);
            }

            return Timer.Total;
        }

        [SecuritySafeCritical]
        static void SetSynchronizationContext(SynchronizationContext context)
        {
            SynchronizationContext.SetSynchronizationContext(context);
        }
    }
}
