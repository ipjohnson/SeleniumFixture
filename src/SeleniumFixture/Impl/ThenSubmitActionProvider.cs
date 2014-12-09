using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace SeleniumFixture.Impl
{
    /// <summary>
    /// TheSubmit provider
    /// </summary>
    public interface IThenSubmitActionProvider : IActionProvider
    {
        /// <summary>
        /// Submit the form you just filled
        /// </summary>
        /// <returns>this</returns>
        IYieldsActionProvider ThenSubmit();
    }

    /// <summary>
    /// Then submit provider
    /// </summary>
    public class ThenSubmitActionProvider : FixtureActionProvider, IThenSubmitActionProvider
    {
        private readonly IWebElement _formElement;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="fixture"></param>
        /// <param name="formElement"></param>
        public ThenSubmitActionProvider(Fixture fixture, IWebElement formElement) : base(fixture)
        {
            _formElement = formElement;
        }

        /// <summary>
        /// Submit the form you just filled
        /// </summary>
        /// <returns>this</returns>
        public IYieldsActionProvider ThenSubmit()
        {
            _formElement.Submit();

            return new YieldsActionProvider(_fixture);
        }
    }
}
