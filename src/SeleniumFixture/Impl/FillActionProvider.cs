using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace SeleniumFixture.Impl
{
    public interface IFillActionProvider
    {
        /// <summary>
        /// Data to fill input elements with.
        /// </summary>
        /// <param name="fillValues"></param>
        /// <returns></returns>
        IThenSubmitActionProvider With(object fillValues);
    }

    public class FillActionProvider : IFillActionProvider
    {
        protected readonly Fixture _fixture;
        protected readonly IEnumerable<IWebElement> _elements;

        public FillActionProvider(IEnumerable<IWebElement> elements, Fixture fixture)
        {
            _fixture = fixture;
            _elements = elements;
        }

        public IThenSubmitActionProvider With(object fillValues)
        {
            return new ThenSubmitActionProvider(_fixture);
        }
    }
}
