using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.IE;
using Xunit.Abstractions;

namespace SeleniumFixture.xUnit
{
    public abstract class InternetExplorerDriverProviderAttribute : Attribute
    {
        public abstract InternetExplorerDriver ProvideDriver(MethodInfo methodInfo);
    }
}
