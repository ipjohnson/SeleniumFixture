using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace SeleniumFixture.xUnit
{
    public enum RemoteWebDriverCapability
    {
        FireFox,
        InternetExplorer,
        Chrome
    }

    public class RemoteDriverAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            throw new NotImplementedException();
        }

        public string Hub { get; set; }

        public RemoteWebDriverCapability Capability { get; set; }
    }
}
