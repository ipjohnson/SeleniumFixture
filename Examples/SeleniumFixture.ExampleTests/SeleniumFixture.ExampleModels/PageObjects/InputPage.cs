using SeleniumFixture.ExampleModels.Models;

namespace SeleniumFixture.ExampleModels.PageObjects
{
    public class InputPage : BasePage
    {
        public void FillForm(object formData)
        {
            I.Fill("//form").With(formData);
        }

        public NewUserModel AutoFill()
        {
            I.AutoFill("//form");

            return I.Get.DataAs<NewUserModel>().From("//form");
        }

        public void Submit()
        {
            I.Submit("//form");
        }
    }
}
