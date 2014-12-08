using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace SeleniumFixture.Impl
{
    public interface IWaitActionProvider
    {
        IWaitActionProvider ForElement(string element, double? timeout = null);

        IWaitActionProvider ForElement(By element, double? timeout = null);

        IWaitActionProvider ForAjax(double? timeout = null);

        IWaitActionProvider Until(Func<IActionProvider, bool> testFunc, double? timeout = null);

        IActionProvider Then { get; }
    }

    public class WaitActionProvider : IWaitActionProvider
    {
        private readonly IActionProvider _actionProvider;

        public WaitActionProvider(IActionProvider actionProvider)
        {
            _actionProvider = actionProvider;
        }

        public IWaitActionProvider ForElement(string element, double? timeout = null)
        {
            return Until(i => i.CheckForElement(element), timeout);
        }

        public IWaitActionProvider ForElement(By element, double? timeout = null)
        {
            return Until(i => i.CheckForElement(element), timeout);
        }

        public IWaitActionProvider ForAjax(double? timeout = null)
        {
            return Until(
                i =>
                {
                    IJavaScriptExecutor executor = (IJavaScriptExecutor)_actionProvider.UsingFixture.Driver;

                    return (bool)executor.ExecuteScript(_actionProvider.UsingFixture.Configuration.AjaxActiveTest);
                },timeout);
        }

        public IWaitActionProvider Until(Func<IActionProvider, bool> testFunc, double? timeout = null)
        {
            if (!timeout.HasValue)
            {
                timeout = _actionProvider.UsingFixture.Configuration.DefaultWaitTimeout;
            }

            DateTime expire = DateTime.Now.AddSeconds(timeout.Value);
            bool untilResult = testFunc(_actionProvider);

            while (!untilResult)
            {
                Thread.Sleep((int)(_actionProvider.UsingFixture.Configuration.DefaultWaitInterval * 1000));

                untilResult = testFunc(_actionProvider);

                if (!untilResult && DateTime.Now > expire)
                {
                    break;
                }
            }

            if (!untilResult)
            {
                throw new Exception("Timedout waiting: " + timeout);    
            }

            return this;
        }

        public IActionProvider Then
        {
            get { return _actionProvider; }
        }
    }
}
