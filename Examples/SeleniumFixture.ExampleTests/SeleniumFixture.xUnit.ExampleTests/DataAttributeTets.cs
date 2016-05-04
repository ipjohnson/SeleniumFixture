using FluentAssertions;
using SeleniumFixture.ExampleModels.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SeleniumFixture.xUnit.ExampleTests
{
    public class DataAttributeTets
    {
        /// <summary>
        /// Test that page object is created correctly and that InlineData is correctly populated into variables
        /// </summary>
        /// <param name="inputPage">page object from initialization attribute</param>
        /// <param name="helloString">Hello string</param>
        /// <param name="intValue">5 value</param>
        [SeleniumTheory]
        [InlineData("Hello",5)]
        [InitializeToInputForm]
        public void Fixture_InlineData_CorrectDataPopulated(InputPage inputPage, string helloString, int intValue)
        {
            helloString.Should().Be("Hello");

            intValue.Should().Be(5);

            inputPage.AutoFill();

            inputPage.Get.Value.From("#FirstName").All(char.IsLetter).Should().BeTrue();

            inputPage.Get.Value.From("#LastName").All(char.IsLetter).Should().BeTrue();
        }
    }
}
