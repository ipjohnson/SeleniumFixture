using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace SeleniumFixture.xUnit
{
    public class CustomTheoryDiscoverer : IXunitTestCaseDiscoverer
    {
        readonly IMessageSink diagnosticMessageSink;

        public CustomTheoryDiscoverer(IMessageSink diagnosticMessageSink)
        {
            this.diagnosticMessageSink = diagnosticMessageSink;
        }

        public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
        {
            yield return new XunitTheoryTestCase(diagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), testMethod);
        }
    }

    [XunitTestCaseDiscoverer("SeleniumFixture.xUnit.CustomTheoryDiscoverer", "SeleniumFixture.xUnit")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CustomTheoryAttribute : TheoryAttribute
    {
    }
}
