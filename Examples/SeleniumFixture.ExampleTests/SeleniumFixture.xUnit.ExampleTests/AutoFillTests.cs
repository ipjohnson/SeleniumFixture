using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace SeleniumFixture.xUnit.ExampleTests
{
    public class AutoFillTests
    {
        /// <summary>
        /// Test navigates to the input form and auto fills the form.
        /// Runs against Chrome, Firefox and Internet Explorer
        /// </summary>
        /// <param name="fixture">populated fixture</param>
        [SeleniumTheory]
        public void Fixture_FillForm_PopulatesCorrectly(Fixture fixture)
        {
            fixture.Navigate.To("http://ipjohnson.github.io/SeleniumFixture/TestSite/InputForm.html");

            fixture.AutoFill("//form");

            fixture.Get.Value.From("#FirstName").All(char.IsLetter).Should().BeTrue();

            fixture.Get.Value.From("#LastName").All(char.IsLetter).Should().BeTrue();
        }
    }
}
