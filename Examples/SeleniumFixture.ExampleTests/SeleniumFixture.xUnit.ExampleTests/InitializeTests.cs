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
        [SeleniumTheory]
        [InitializeToInputForm]
        public void Fixture_Initialize_ToInputForm(InputPage inputPage)
        {
            inputPage.AutoFill();
        }
    }
}
