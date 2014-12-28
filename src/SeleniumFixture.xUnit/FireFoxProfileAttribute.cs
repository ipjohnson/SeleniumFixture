using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Firefox;

namespace SeleniumFixture.xUnit
{
    public abstract class FireFoxProfileAttribute : Attribute
    {
        public abstract FirefoxProfile CreateProfile();
    }
}
