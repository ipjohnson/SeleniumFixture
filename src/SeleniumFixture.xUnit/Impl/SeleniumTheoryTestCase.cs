using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace SeleniumFixture.xUnit.Impl
{
    public class SeleniumTheoryTestCase : XunitTheoryTestCase
    {
        /// <summary/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the de-serializer", error: true)]
        public SeleniumTheoryTestCase() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="XunitTheoryTestCase"/> class.
        /// </summary>
        /// <param name="testMethod">The method under test.</param>
        public SeleniumTheoryTestCase(ITestMethod testMethod)
            : base(testMethod) { }

        /// <inheritdoc />
        protected SeleniumTheoryTestCase(SerializationInfo info, StreamingContext context) : base(info, context) { }


        /// <inheritdoc />
        public override Task<RunSummary> RunAsync(IMessageBus messageBus, object[] constructorArguments, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
        {
            return new SeleniumTheoryTestCaseRunner(this, DisplayName, SkipReason, constructorArguments, messageBus, aggregator, cancellationTokenSource).RunAsync();
        }
    }

}
