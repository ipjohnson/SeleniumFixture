using System;

namespace SeleniumFixture.xUnit
{
    public class RemoteWebDriverHubAddressAttribute : Attribute
    {
        public string Hub { get; set; }
    }
}
