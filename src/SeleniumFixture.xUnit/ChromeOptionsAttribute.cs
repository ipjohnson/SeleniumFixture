using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;

namespace SeleniumFixture.xUnit
{
    public abstract class ChromeOptionsAttribute : Attribute
    {
        public abstract ChromeOptions ProvideOptions();
    }
}
