using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumFixture.xUnit
{
    public class RemoteWebDriverHubAddressAttribute : Attribute
    {
        public string Hub { get; set; }
    }
}
