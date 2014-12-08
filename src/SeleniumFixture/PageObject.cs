using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumFixture.Impl;

namespace SeleniumFixture
{
    public class PageObject
    {
        private IActionProvider _i;
        /// <summary>
        /// Allows you to perform actions on the page
        /// </summary>
        protected IActionProvider I { get { return _i; } private set { _i = value;}}
    }
}
