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
    public class PageObject
    {
        /// <summary>
        /// Action that will be called after the page has been created and imports satisfied
        /// </summary>
        protected Action Validate { get; set; }

        /// <summary>
        /// Perform action on a page
        /// </summary>
        protected IActionProvider I { get; private set; }
    }
}
