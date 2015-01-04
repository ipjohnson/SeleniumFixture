using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumFixture.ExampleModels.PageObjects;

namespace SeleniumFixture.xUnit.ExampleTests
{
    public class PageObjectTests
    {
        [SeleniumTheory]
        [ChromeDriver]
        [FirefoxDriver]
        [InternetExplorerDriver]
        public void Fixture_DriveApp_ClicksAroundAndAutoFillsStuff(Fixture fixture)
        {
            var homePage = fixture.Navigate.To<HomePage>("Home.html");

            var linkPage = homePage.Menu.ClickLinks();

            var inputPage = linkPage.Menu.ClickInputForm();

            var newUser = inputPage.AutoFill();

            inputPage.Submit();
        }
    }
}
