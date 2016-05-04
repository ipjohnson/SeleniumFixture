using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Firefox;
using SeleniumFixture.ExampleModels.Models;

namespace SeleniumFixture.mstest.ExampleTests
{
    [TestClass]
    public class AutoFillTests
    {
        //[TestMethod]
        public void Fixture_FillForm_PopulatesCorrectly()
        {
            using (var driver = new FirefoxDriver())
            {
                var fixture = new Fixture(driver);

                fixture.Navigate.To("http://ipjohnson.github.io/SeleniumFixture/TestSite/InputForm.html");

                fixture.AutoFill("//form");

                var newUser = fixture.Get.ValueAs<NewUserModel>().From("//form");

                newUser.FirstName.Should().Match(s => s.All(char.IsLetter));

                newUser.LastName.Should().Match(s => s.All(char.IsLetter));

                newUser.Email
                       .Should()
                       .MatchRegex("^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$");

                newUser.Gender.Should().BeOneOf("male", "female");

                newUser.Password.Should()
                    .MatchRegex(@"^(?=.*[.,;'""!@#$%^&*()\-_=+`~\[\]{}?|])(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{8,20}$");
            }
        }
    }
}
