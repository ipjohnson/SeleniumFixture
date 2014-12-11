using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumFixture.Impl;

namespace SeleniumFixture
{
    /// <summary>
    /// Provides custom implemenation of Selenium By class
    /// </summary>
    public static class Using
    {
        /// <summary>
        /// Automatically decide which type of selector (jQuery, CSS, XPath) to use based on the type of selector and if jQuery is enabled for the site
        /// </summary>
        /// <param name="selector">selector string</param>
        /// <returns>By implementation</returns>
        public static AutoBy Auto(string selector)
        {
            return new AutoBy(selector);
        }

        /// <summary>
        /// Select elements using jQuery selector syntax
        /// </summary>
        /// <param name="selector">jQuery selector</param>
        /// <returns>jQuery By implementation</returns>
        public static JQueryBy JQuery(string selector)
        {
            return new JQueryBy(selector);
        }
    }
}
