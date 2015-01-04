using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.IE;

[assembly: SeleniumFixture.xUnit.ExampleTests.InternetExplorerOptions]

namespace SeleniumFixture.xUnit.ExampleTests
{

    public class InternetExplorerOptionsAttribute : xUnit.InternetExplorerOptionsAttribute
    {
        public override InternetExplorerOptions ProvideOptions(MethodInfo methodInfo)
        {
            return new InternetExplorerOptions { EnableNativeEvents = false };
        }
    }
}
