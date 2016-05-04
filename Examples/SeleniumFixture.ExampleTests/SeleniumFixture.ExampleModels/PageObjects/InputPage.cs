using OpenQA.Selenium;
using SeleniumFixture.ExampleModels.Models;
using SeleniumFixture.Impl;
using System;

namespace SeleniumFixture.ExampleModels.PageObjects
{
    public class InputPage : BasePage
    {
        public InputPage()
        {
            Validate = () => I.CheckForElement(By.Id("LastName"));
        }

        protected Action Validate { get; private set; }
    

        public void FillForm(object formData)
        {
            I.Fill("//form").With(formData);
        }

        public NewUserModel AutoFill(object seedWith = null)
        {
            I.AutoFill("//form", seedWith);

            return I.Get.ValueAs<NewUserModel>().From("//form");
        }

        public void Submit()
        {
            I.Submit("//form");
        }

        public IGetAction Get {  get { return I.Get; } }
    }
}
