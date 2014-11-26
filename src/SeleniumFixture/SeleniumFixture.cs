using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SimpleFixture.Impl;

namespace SimpleFixture.Selenium
{
    public class SeleniumFixture : Fixture
    {
        #region Constructor
        public SeleniumFixture(IWebDriver webDriver, string baseAddress = null)
            : base(new SeleniumFixtureConfiguration())
        {
            Initialize(webDriver, baseAddress);
        }
        #endregion

        #region Public Members

        public IWebDriver WebDriver { get; private set; }

        public string BaseAddress { get; private set; }

        public void PopulateForm(string formName, object formValues)
        {
            
        }

        public T Page<T>()
        {
            return Locate<T>();
        }

        #endregion

        #region Private Members
        private void Initialize(IWebDriver webDriver, string baseAddress)
        {
            Return(this);
            Return(webDriver);
            Return(baseAddress).WhenNamed("BaseAddress");

            WebDriver = webDriver;
            BaseAddress = baseAddress;

            Behavior.Add((r, o) =>
                         {
                             if (o.GetType().IsValueType || o is string)
                             {
                                 return o;
                             }

                             PageFactory.InitElements(webDriver, o);

                             return o;
                         });

            Behavior.Add(ImportPropertiesOnLocate);
        }

        private object ImportPropertiesOnLocate(DataRequest r, object o)
        {
            if (o.GetType().IsValueType || o is string || r.Populate)
            {
                return o;
            }

            IModelService modelService = Configuration.Locate<IModelService>();

            TypePopulator typePopulator = new TypePopulator(Configuration.Locate<IConstraintHelper>(), new ImportSeleniumTypePropertySelector(Configuration.Locate<IConstraintHelper>()));

            typePopulator.Populate(o, r, modelService.GetModel(r.RequestedType));

            return 0;
        }
        #endregion

    }
}
