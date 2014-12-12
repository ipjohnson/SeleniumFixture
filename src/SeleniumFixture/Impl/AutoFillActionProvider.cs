using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace SeleniumFixture.Impl
{
    public class AutoFillActionProvider
    {
        private readonly IActionProvider _actionProvider;
        private readonly IEnumerable<IWebElement> _elements;
        private readonly object _seedWith;

        public AutoFillActionProvider(IActionProvider actionProvider, IEnumerable<IWebElement> elements, object seedWith)
        {
            _actionProvider = actionProvider;
            _elements = elements;
            _seedWith = seedWith;
        }

        public IThenSubmitActionProvider PerformFill()
        {


            return new ThenSubmitActionProvider(_actionProvider.UsingFixture,_elements.First());
        }

        /// <summary>
        /// Find all input and select element in find elements and their children
        /// </summary>
        /// <param name="findElements"></param>
        /// <returns></returns>
        public static IEnumerable<IWebElement> FindInputElements(ReadOnlyCollection<IWebElement> findElements)
        {
            List<IWebElement> returnList = new List<IWebElement>();

            foreach (IWebElement findElement in findElements)
            {
                if (findElement.TagName == "select")
                {
                    returnList.Add(findElement);
                }
                else if (findElement.TagName == "input")
                {
                    returnList.Add(findElement);
                }
                else
                {
                    returnList.AddRange(findElement.FindElements(By.TagName("input")));
                    
                    returnList.AddRange(findElement.FindElements(By.TagName("select")));
                }
            }

            return returnList;
        }
    }
}
