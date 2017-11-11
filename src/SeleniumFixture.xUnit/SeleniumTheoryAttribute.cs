using SeleniumFixture.xUnit.Impl;
using System;
using Xunit;
using Xunit.Sdk;

namespace SeleniumFixture.xUnit
{    
    [XunitTestCaseDiscoverer(SeleniumTheoryDiscoverer.ClassName, SeleniumTheoryDiscoverer.AssemblyName)]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SeleniumTheoryAttribute : FactAttribute
    {

    }
}
