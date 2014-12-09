using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace SeleniumFixture.Impl
{
    public interface IDragActionProvider
    {
        IActionProvider To(string element = null, int? x = null, int? y = null);
    }

    public class DragActionProvider : IDragActionProvider
    {
        
        public IActionProvider To(string element = null, int? x = null, int? y = null)
        {
            Actions actions = new Actions(null);

            

            throw new NotImplementedException();
        }
    }
}
