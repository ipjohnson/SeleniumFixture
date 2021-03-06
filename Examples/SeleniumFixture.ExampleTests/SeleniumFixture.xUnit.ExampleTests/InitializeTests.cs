﻿using FluentAssertions;
using SeleniumFixture.ExampleModels.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumFixture.xUnit.ExampleTests
{
    public class InitializeTests
    {
        /// <summary>
        /// Test case that uses attribute to navigate to the input form
        /// then autofills the page then tests that the first and last names are all letters
        /// </summary>
        /// <param name="inputPage">Page object returned from InitializeToInputForm</param>
        [SeleniumTheory]
        [InitializeToInputForm]
        public void Fixture_Initialize_ToInputForm(InputPage inputPage)
        {
            inputPage.AutoFill();

            inputPage.Get.Value.From("#FirstName").All(char.IsLetter).Should().BeTrue();

            inputPage.Get.Value.From("#LastName").All(char.IsLetter).Should().BeTrue();
        }
    }
}
