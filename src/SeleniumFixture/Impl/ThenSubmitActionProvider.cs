using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="fixture"></param>
        public ThenSubmitActionProvider(Fixture fixture) : base(fixture)
        {

        }

        /// <summary>
        /// Submit the form you just filled
        /// </summary>
        /// <returns>this</returns>
        public IYieldsActionProvider ThenSubmit()
        {
            return new YieldsActionProvider(_fixture);
        }
    }
}
