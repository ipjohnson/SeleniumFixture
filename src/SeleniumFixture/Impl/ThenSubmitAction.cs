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
    public interface IThenSubmitAction : IActionProvider
    {
        /// <summary>
        /// Submit the form you just filled
        /// </summary>
        /// <returns>this</returns>
        IYieldsAction ThenSubmit();
    }

    /// <summary>
    /// Then submit provider
    /// </summary>
    public class ThenSubmitAction : FixtureActionProvider, IThenSubmitAction
    {
        private readonly IWebElement _formElement;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="fixture"></param>
        /// <param name="formElement"></param>
        public ThenSubmitAction(Fixture fixture, IWebElement formElement) : base(fixture)
        {
            _formElement = formElement;
        }

        /// <summary>
        /// Submit the form you just filled
        /// </summary>
        /// <returns>this</returns>
        public IYieldsAction ThenSubmit()
        {
            _formElement.Submit();

            return new YieldsAction(_fixture);
        }
    }
}
