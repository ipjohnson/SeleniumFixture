using SeleniumFixture.xUnit.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
