using OpenQA.Selenium;

namespace SeleniumFixture.ExampleModels.PageObjects
{
    public class MenuElement : BaseElement
    {
        public HomePage ClickHome()
        {
            return I.Click(By.LinkText("Home")).Yields<HomePage>();
        }

        public InputPage ClickInputForm()
        {
            return I.Click(By.LinkText("Input Form")).Yields<InputPage>();
        }

        public LinksPage ClickLinks()
        {
            return I.Click(By.LinkText("Link Page")).Yields<LinksPage>();
        }
    }
}
