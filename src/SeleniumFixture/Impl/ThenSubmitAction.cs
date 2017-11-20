using System.Threading;
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
        protected readonly IWebElement FormElement;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="fixture"></param>
        /// <param name="formElement"></param>
        public ThenSubmitAction(Fixture fixture, IWebElement formElement) : base(fixture)
        {
            FormElement = formElement;
        }

        /// <summary>
        /// Submit the form you just filled
        /// </summary>
        /// <returns>this</returns>
        public virtual IYieldsAction ThenSubmit()
        {
            FormElement.Submit();

            var configuration = Fixture.Configuration;

            var waitTime = (int)(configuration.FixtureImplicitWait * 1000);

            if (waitTime >= 0)
            {
                Thread.Sleep(waitTime);
            }

            if (configuration.AlwaysWaitForAjax)
            {
                Fixture.Wait.ForAjax();
            }

            return new YieldsAction(Fixture);
        }
    }
}
