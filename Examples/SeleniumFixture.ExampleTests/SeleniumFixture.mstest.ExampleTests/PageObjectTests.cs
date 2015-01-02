using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Firefox;
using SeleniumFixture.ExampleModels.PageObjects;

namespace SeleniumFixture.ExampleTests
{
    [TestClass]
    public class PageObjectTests
    {
        [TestMethod]
        public void Fixture_DriveApp_ClicksAroundAndAutoFillsStuff()
        {
            using (var driver = new FirefoxDriver())
            {
                var fixture = new Fixture(driver, "http://ipjohnson.github.io/SeleniumFixture/TestSite/");

                var homePage =
                    fixture.Navigate.To<HomePage>("Home.html");

                var linkPage = homePage.Menu.ClickLinks();

                var inputPage = linkPage.Menu.ClickInputForm();

                var newUser = inputPage.AutoFill();

                inputPage.Submit();
            }
        }
    }
}
