using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumFixture.Impl;

namespace SeleniumFixture
{
    /// <summary>
    /// Base object that represents a web element
    /// </summary>
    public class ElementObject
    {
        /// <summary>
        /// Allows you to perform actions on the page
        /// </summary>
        protected IActionProvider I { get; private set; }
    }
}
