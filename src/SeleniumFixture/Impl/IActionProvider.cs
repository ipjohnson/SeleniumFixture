using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace SeleniumFixture.Impl
{
    /// <summary>
    /// Click mode
    /// </summary>
    public enum ClickMode
    {
        /// <summary>
        /// Click all elements returned, throws exception when there are none
        /// </summary>
        ClickAll,

        /// <summary>
        /// Click any element returned, does not throw an exception if no elements are found
        /// </summary>
        ClickAny,

        /// <summary>
        /// Click one and only one element, throws exception when there isn't exactly one element
        /// </summary>
        ClickOne,

        /// <summary>
        /// Click the first element, throws exception when there are no element
        /// </summary>
        ClickFirst,
    }

    /// <summary>
    /// Performs actions on IWebDriver
    /// </summary>
    public interface IActionProvider : ISearchContext
    {
        /// <summary>
        /// Autofill elements using data from SimpleFixture
        /// </summary>
        /// <param name="selector">selector</param>
        /// <param name="seedWith">seed data</param>
        /// <returns>this</returns>
        IThenSubmitActionProvider AutoFill(string selector, object seedWith = null);

        /// <summary>
        /// Autofill elements using data from SimpleFixture
        /// </summary>
        /// <param name="selector">selector</param>
        /// <param name="seedWith">seed data</param>
        /// <returns>this</returns>
        IThenSubmitActionProvider AutoFill(By selector, object seedWith = null);

        /// <summary>
        /// Autofill elements using data from SimpleFixture
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="seedWith"></param>
        /// <returns></returns>
        IThenSubmitActionProvider AutoFill(IEnumerable<IWebElement> elements, object seedWith = null);

        /// <summary>
        /// Auto fill elements as a specific type
        /// </summary>
        /// <typeparam name="T">Type of data to generate</typeparam>
        /// <param name="selector">selector for elements</param>
        /// <param name="requestName">request name</param>
        /// <param name="constraints">constraints for generation</param>
        /// <returns>this</returns>
        IThenSubmitActionProvider AutoFillAs<T>(string selector, string requestName = null, object constraints = null);

        /// <summary>
        /// Auto fill elements as a specific type
        /// </summary>
        /// <typeparam name="T">Type of data to generate</typeparam>
        /// <param name="selector">selector for elements</param>
        /// <param name="requestName">request name</param>
        /// <param name="constraints">constraints for generation</param>
        /// <returns>this</returns>
        IThenSubmitActionProvider AutoFillAs<T>(By selector, string requestName = null, object constraints = null);

        /// <summary>
        /// Auto fill elements as a specific type
        /// </summary>
        /// <typeparam name="T">Type of data to generate</typeparam>
        /// <param name="elements">elements</param>
        /// <param name="requestName">request name</param>
        /// <param name="constraints">constraints for generation</param>
        /// <returns>this</returns>
        IThenSubmitActionProvider AutoFillAs<T>(IEnumerable<IWebElement> elements, string requestName = null, object constraints = null);

        /// <summary>
        /// Check for the element specified in the selector
        /// </summary>
        /// <param name="selector">selector to look for</param>
        /// <returns>true if element exists</returns>
        bool CheckForElement(string selector);

        /// <summary>
        /// Check for the element specified in the selector
        /// </summary>
        /// <param name="selector">selector to look for</param>
        /// <returns>true if element exists</returns>
        bool CheckForElement(By selector);
        
        /// <summary>
        /// Click the elements returned by the selector
        /// </summary>
        /// <param name="selector">selector to use when find elements to click</param>
        /// <param name="clickMode">click mode, by default </param>
        /// <returns>this</returns>
        IActionProvider Click(string selector, ClickMode clickMode = ClickMode.ClickOne);

        /// <summary>
        /// Click the elements returned by the selector
        /// </summary>
        /// <param name="selector">selector to use when find elements to click</param>
        /// <param name="clickMode">click mode, by default </param>
        /// <returns>this</returns>
        IActionProvider Click(By selector, ClickMode clickMode = ClickMode.ClickOne);

        /// <summary>
        /// Count the number of elements present
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns>count of elements</returns>
        int Count(string selector);

        /// <summary>
        /// Count the number of elements present
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns>count of elements</returns>
        int Count(By selector);

        /// <summary>
        /// Double click the selected elements
        /// </summary>
        /// <param name="selector">selector</param>
        /// <param name="clickMode">click mode</param>
        /// <returns>this</returns>
        IActionProvider DoubleClick(string selector, ClickMode clickMode = ClickMode.ClickOne);

        /// <summary>
        /// Double click the selected elements
        /// </summary>
        /// <param name="selector">selector</param>
        /// <param name="clickMode">click mode</param>
        /// <returns>this</returns>
        IActionProvider DoubleClick(By selector, ClickMode clickMode = ClickMode.ClickOne);

        /// <summary>
        /// Fill elements with values
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns>fill action</returns>
        IFillActionProvider Fill(string selector);

        /// <summary>
        /// Fill elements with values
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns>fill action</returns>
        IFillActionProvider Fill(By selector);

        /// <summary>
        /// Fill elements with values
        /// </summary>
        /// <param name="elements">elements</param>
        /// <returns>fill action</returns>
        IFillActionProvider Fill(IEnumerable<IWebElement> elements);

        /// <summary>
        /// Find a specified element by selector
        /// </summary>
        /// <param name="selector">selector to use to locate element</param>
        /// <returns>element or throws an exception</returns>
        IWebElement FindElement(string selector);

        /// <summary>
        /// Find All elements meeting the specified selector
        /// </summary>
        /// <param name="selector">selector to use to find elements</param>
        /// <returns>elements</returns>
        ReadOnlyCollection<IWebElement> FindElements(string selector);

        /// <summary>
        /// Generate a random T
        /// </summary>
        /// <typeparam name="T">data type to generate</typeparam>
        /// <param name="requestName">request name to use</param>
        /// <param name="constraints">constraints for the generate</param>
        /// <returns>new type to generate</returns>
        T Generate<T>(string requestName = null, object constraints = null);

        /// <summary>
        /// Get values from a web element
        /// </summary>
        IGetActionProvider Get { get; }

        /// <summary>
        /// Move the mouse to a give element or x,y
        /// </summary>
        /// <param name="selector">selector</param>
        /// <param name="x">x offset</param>
        /// <param name="y">y offset</param>
        /// <returns></returns>
        IActionProvider MoveTheMouseTo(string selector, int? x = null, int? y = null);

        /// <summary>
        /// Move the mouse to a give element or x,y
        /// </summary>
        /// <param name="selector">selector</param>
        /// <param name="x">x offset</param>
        /// <param name="y">y offset</param>
        /// <returns></returns>
        IActionProvider MoveTheMouseTo(By selector, int? x = null, int? y = null);

        /// <summary>
        /// Navigate the fixture
        /// </summary>
        INavigateActionProvider Navigate { get; }

        /// <summary>
        /// Submit form.
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        IYieldsActionProvider Submit(string selector);

        /// <summary>
        /// Submit form.
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns></returns>
        IYieldsActionProvider Submit(By selector);

        /// <summary>
        /// Switch to
        /// </summary>
        ISwitchToActionProvider SwitchTo { get; }

        /// <summary>
        /// Fixture for this action provider
        /// </summary>
        Fixture UsingFixture { get; }

        /// <summary>
        /// Wait for something to happen
        /// </summary>
        IWaitActionProvider Wait { get; }

        /// <summary>
        /// Yields a Page Object using SimpleFixture
        /// </summary>
        /// <typeparam name="T">Type of object to Generate</typeparam>
        /// <param name="requestName">request name</param>
        /// <param name="constraints">constraints for the locate</param>
        /// <returns>new T</returns>
        T Yields<T>(string requestName = null, object constraints = null);

        /// <summary>
        /// Yields a Page Object using SimpleFixture
        /// </summary>
        /// <param name="type">Type of object to Generate</param>
        /// <param name="requestName">request name</param>
        /// <param name="constraints">constraints for the locate</param>
        /// <returns>new instance</returns>
        object Yields(Type type, string requestName = null, object constraints = null);
    }
}
