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
            
            DefaultTimeout = 10;
            DefaultWaitInterval = 0.1;
            FixtureImplicitWait = 0.2;

            AjaxActiveTest = "return (window.jQuery || { active : 0 }).active == 0;";

            ExecuteValidate = true;
            WrapValidationExceptions = true;
            ValidateMember = "Validate";
        }

        /// <summary>
        /// Base address for navigation
        /// </summary>
        public string BaseAddress { get; set; }

        /// <summary>
        /// Default timeout for actions, in seconds
        /// </summary>
        public double DefaultTimeout { get; set; }

        /// <summary>
        /// Time to wait in between testing for wait condition
        /// </summary>
        public double DefaultWaitInterval { get; set; }

        /// <summary>
        /// Fixture will wait a small amount of time after every time Click, DoubleClick and Navigate is called
        /// </summary>
        public double FixtureImplicitWait { get; set; }

        /// <summary>
        /// Element selector algorithm
        /// </summary>
        public SelectorAlgorithm Selector { get; set; }

        /// <summary>
        /// String to test if ajax is still active
        /// </summary>
        public string AjaxActiveTest { get; set; }

        /// <summary>
        /// Execute validate on page objects
        /// </summary>
        public bool ExecuteValidate { get; set; }

        /// <summary>
        /// Name of member to execute on validation
        /// </summary>
        public string ValidateMember { get; set; }

        /// <summary>
        /// Wrap validation exception with extra information
        /// </summary>
        public bool WrapValidationExceptions { get; set; }
    }
}
