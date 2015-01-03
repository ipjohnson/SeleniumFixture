using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using Xunit.Abstractions;

namespace SeleniumFixture.xUnit
{
    public abstract class ChromeDriverProviderAttribute : Attribute
    {
        public abstract ChromeDriver ProvideDriver(MethodInfo testMethod);
    }
}
