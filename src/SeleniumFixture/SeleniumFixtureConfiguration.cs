using SimpleFixture;
using SimpleFixture.Impl;

namespace SeleniumFixture
{
    /// <summary>
    /// Algorithm to use when selecting
    /// </summary>
    public enum SelectorAlgorithm
    {
        /// <summary>
        /// Detech which method to use
        /// </summary>
        Auto,

        /// <summary>
        /// Use jQuery selector
        /// </summary>
        JQuery,

        /// <summary>
        /// CSS selector
        /// </summary>
        CSS,

        /// <summary>
        /// XPath
        /// </summary>
        XPath
    }
    
    /// <summary>
    /// Configuration object for SeleniumFoxture
    /// </summary>
    public class SeleniumFixtureConfiguration 
    {
        public SeleniumFixtureConfiguration()
        {
            Initialize();
        }

        private void Initialize()
        {
            Selector = SelectorAlgorithm.Auto;
            
            DefaultWaitTimeout = 10;
            DefaultWaitInterval = 0.1;

            AjaxActiveTest = "return jQuery.active == 0";
        }

        /// <summary>
        /// Base address for navigation
        /// </summary>
        public string BaseAddress { get; set; }

        /// <summary>
        /// Default timeout for actions, in seconds
        /// </summary>
        public double DefaultWaitTimeout { get; set; }

        /// <summary>
        /// Time to wait in between testing for wait condition
        /// </summary>
        public double DefaultWaitInterval { get; set; }

        /// <summary>
        /// Element selector algorithm
        /// </summary>
        public SelectorAlgorithm Selector { get; set; }

        /// <summary>
        /// String to test if ajax is still active
        /// </summary>
        public string AjaxActiveTest { get; set; }
    }
}
