using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace SeleniumFixture.xUnit
{
    [DataDiscoverer("SeleniumFixture.xUnit.Impl.InternetExplorerDriverDataDiscoverer", "SeleniumFixture.xUnit")]
    public class InternetExplorerDriverAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield break;
        }
    }
}
