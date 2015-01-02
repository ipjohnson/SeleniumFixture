using System;
using FluentAssertions;

namespace SeleniumFixture.ExampleModels.PageObjects
{
    public class HomePage : BasePage
    {
        public HomePage()
        {
            Validate = () => I.Get.PageTitle.Should().Be("SeleniumFixture");
        }

        protected Action Validate { get; private set; }
    }
}
