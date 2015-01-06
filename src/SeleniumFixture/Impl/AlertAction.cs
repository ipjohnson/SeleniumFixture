using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumFixture.Impl
{
    /// <summary>
    /// Alert action
    /// </summary>
    public interface IAlertAction
    {
        /// <summary>
        /// Accept alert
        /// </summary>
        /// <returns>action provider</returns>
        IActionProvider Accept();

        /// <summary>
        /// Dismiss alert
        /// </summary>
        /// <returns>action provider</returns>
        IActionProvider Dismiss();
    }

    /// <summary>
    /// Alert action
    /// </summary>
    public class AlertAction : IAlertAction
    {
        private IActionProvider _actionProvider;

        public AlertAction(IActionProvider actionProvider)
        {
            _actionProvider = actionProvider;
        }

        /// <summary>
        /// Accept alert
        /// </summary>
        /// <returns>action provider</returns>
        public IActionProvider Accept()
        {
            _actionProvider.UsingFixture.Driver.SwitchTo().Alert().Accept();

            return _actionProvider;
        }

        /// <summary>
        /// Dismiss alert
        /// </summary>
        /// <returns>action provider</returns>
        public IActionProvider Dismiss()
        {
            _actionProvider.UsingFixture.Driver.SwitchTo().Alert().Dismiss();

            return _actionProvider;
        }
    }
}
