using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Firefox;

namespace SeleniumFixture.mstest.ExampleTests
{
    [TestClass]
    public class FillExampleTests
    {
        [TestMethod]
        public void Fixture_FillForm_PopulatesCorrectly()
        {
            using (var driver = new FirefoxDriver())
            {
                var fixture = new Fixture(driver);

                fixture.Navigate.To("http://ipjohnson.github.io/SeleniumFixture/TestSite/InputForm.html");

                var fillInfo = new
                               {
                                   FirstName = "Sterling",
                                   LastName = "Archer",
                                   Email = "sterling.archer@spy.gov",
                                   Password = "HelloWorld1!",
                                   Active = true,
                                   OptionsCheckbox1 = false,
                                   OptionsCheckbox2 = true,
                                   Gender = "Male",
                                   AccountType = "SuperAdmin"
                               };

                fixture.Fill("//form")
                    .With(fillInfo);

                fixture.Get.Value.From("#FirstName").ShouldBeEquivalentTo(fillInfo.FirstName);

                fixture.Get.Value.From("#LastName").ShouldBeEquivalentTo(fillInfo.LastName);

                fixture.Get.Value.From("#Email").ShouldBeEquivalentTo(fillInfo.Email);

                fixture.Get.Value.From("#Password").ShouldBeEquivalentTo(fillInfo.Password);

                fixture.Get.ValueAs<bool>().From("#Active").ShouldBeEquivalentTo(fillInfo.Active);

                fixture.Get.ValueAs<bool>().From("#OptionsCheckbox1").ShouldBeEquivalentTo(fillInfo.OptionsCheckbox1);

                fixture.Get.ValueAs<bool>().From("#OptionsCheckbox2").ShouldBeEquivalentTo(fillInfo.OptionsCheckbox2);

                fixture.Get.ValueAs<bool>().From("#OptionsRadios1").ShouldBeEquivalentTo(true);

                fixture.Get.ValueAs<bool>().From("#OptionsRadios2").ShouldBeEquivalentTo(false);

                fixture.Get.ValueAs<string>().From("#AccountType").ShouldBeEquivalentTo(fillInfo.AccountType);
            }
        }
    }
}
