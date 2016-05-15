using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace SeleniumFixture.xUnit.Impl
{
    public class SeleniumTheoryDiscoverer : IXunitTestCaseDiscoverer
    {
        readonly IMessageSink diagnosticMessageSink;

        public const string ClassName = "SeleniumFixture.xUnit.Impl.SeleniumTheoryDiscoverer";

#if DNX
        public const string AssemblyName = "SeleniumFixture.xUnit.dnx";
#else
        public const string AssemblyName = "SeleniumFixture.xUnit";
#endif

        public SeleniumTheoryDiscoverer(IMessageSink diagnosticMessageSink) 
        {
            this.diagnosticMessageSink = diagnosticMessageSink;
        }

        /// <summary>
        /// Creates a test case for a single row of data. By default, returns an instance of <see cref="XunitTestCase"/>
        /// with the data row inside of it.
        /// </summary>
        /// <param name="discoveryOptions">The discovery options to be used.</param>
        /// <param name="testMethod">The test method the test cases belong to.</param>
        /// <param name="theoryAttribute">The theory attribute attached to the test method.</param>
        /// <param name="dataRow">The row of data for this test case.</param>
        /// <returns>The test case</returns>
        protected virtual IXunitTestCase CreateTestCaseForDataRow(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo theoryAttribute, object[] dataRow)
            => new XunitTestCase(diagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), testMethod, dataRow);

        /// <summary>
        /// Creates a test case for a skipped theory. By default, returns an instance of <see cref="XunitTestCase"/>
        /// (which inherently discovers the skip reason via the fact attribute).
        /// </summary>
        /// <param name="discoveryOptions">The discovery options to be used.</param>
        /// <param name="testMethod">The test method the test cases belong to.</param>
        /// <param name="theoryAttribute">The theory attribute attached to the test method.</param>
        /// <param name="skipReason">The skip reason that decorates <paramref name="theoryAttribute"/>.</param>
        /// <returns>The test case</returns>
        protected virtual IXunitTestCase CreateTestCaseForSkip(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo theoryAttribute, string skipReason)
            => new XunitTestCase(diagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), testMethod);

        /// <summary>
        /// Creates a test case for the entire theory. This is used when one or more of the theory data items
        /// are not serializable, or if the user has requested to skip theory pre-enumeration. By default,
        /// returns an instance of <see cref="XunitTheoryTestCase"/>, which performs the data discovery at runtime.
        /// </summary>
        /// <param name="discoveryOptions">The discovery options to be used.</param>
        /// <param name="testMethod">The test method the test cases belong to.</param>
        /// <param name="theoryAttribute">The theory attribute attached to the test method.</param>
        /// <returns>The test case</returns>
        protected virtual IXunitTestCase CreateTestCaseForTheory(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo theoryAttribute)
            => new SeleniumTheoryTestCase(diagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), testMethod);

        /// <summary>
        /// Creates a test case for a single row of data. By default, returns an instance of <see cref="XunitSkippedDataRowTestCase"/>
        /// with the data row inside of it.
        /// </summary>
        /// <remarks>If this method is overridden, the implementation will have to override <see cref="TestMethodTestCase.SkipReason"/> otherwise
        /// the default behavior will look at the <see cref="TheoryAttribute"/> and the test case will not be skipped.</remarks>
        /// <param name="discoveryOptions">The discovery options to be used.</param>
        /// <param name="testMethod">The test method the test cases belong to.</param>
        /// <param name="theoryAttribute">The theory attribute attached to the test method.</param>
        /// <param name="dataRow">The row of data for this test case.</param>
        /// <param name="skipReason">The reason this test case is to be skipped</param>
        /// <returns>The test case</returns>
        protected virtual IXunitTestCase CreateTestCaseForSkippedDataRow(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo theoryAttribute, object[] dataRow, string skipReason)
            => new XunitSkippedDataRowTestCase(diagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), testMethod, skipReason, dataRow);


        public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo theoryAttribute)
        {
            try
            {
                var skipReason = theoryAttribute.GetNamedArgument<string>("Skip");
                if (skipReason != null)
                    return new[] { CreateTestCaseForSkip(discoveryOptions, testMethod, theoryAttribute, skipReason) };

                return new[] { CreateTestCaseForTheory(discoveryOptions, testMethod, theoryAttribute) };
            }
            catch(Exception exp)
            {
                File.WriteAllText(@"c:\temp\errors.txt", exp.Message);
            }

            return new IXunitTestCase[] { };
        }
    }
}
