using OpenQA.Selenium.Edge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumFixture.xUnit
{
    public abstract class EdgeOptionsAttribute : Attribute
    {
        public abstract EdgeOptions ProvideOptions(MethodInfo method);
    }
}
