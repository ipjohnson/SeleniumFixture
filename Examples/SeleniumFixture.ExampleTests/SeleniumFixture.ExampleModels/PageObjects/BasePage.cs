namespace SeleniumFixture.ExampleModels.PageObjects
{
    public class BasePage : BaseElement
    {
        [Import]
        public MenuElement Menu { get; private set; }
    }
}
