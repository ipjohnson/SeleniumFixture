using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using OpenQA.Selenium;

namespace SeleniumFixture.xUnit.ExampleTests
{
    public class FillTests
    {
        [SeleniumTheory]
        [ChromeDriver]
        [FirefoxDriver]
        public void Fixture_FillForm_PopulatesCorrectly(Fixture fixture)
        {
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

            fixture.Get.Value.From("#FirstName").Should().Be(fillInfo.FirstName);

            fixture.Get.Value.From("#LastName").Should().Be(fillInfo.LastName);

            fixture.Get.Value.From("#Email").Should().Be(fillInfo.Email);

            fixture.Get.Value.From("#Password").Should().Be(fillInfo.Password);

            fixture.Get.ValueAs<bool>().From("#Active").Should().Be(fillInfo.Active);

            fixture.Get.ValueAs<bool>().From("#OptionsCheckbox1").Should().Be(fillInfo.OptionsCheckbox1);

            fixture.Get.ValueAs<bool>().From("#OptionsCheckbox2").Should().Be(fillInfo.OptionsCheckbox2);

            fixture.Get.ValueAs<bool>().From("#OptionsRadios1").Should().Be(true);

            fixture.Get.ValueAs<bool>().From("#OptionsRadios2").Should().Be(false);

            fixture.Get.ValueAs<string>().From("#AccountType").Should().Be(fillInfo.AccountType);
        }
    }
}
