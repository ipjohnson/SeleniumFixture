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
        protected readonly IActionProvider ActionProvider;

        public AlertAction(IActionProvider actionProvider)
        {
            ActionProvider = actionProvider;
        }

        /// <summary>
        /// Accept alert
        /// </summary>
        /// <returns>action provider</returns>
        public virtual IActionProvider Accept()
        {
            ActionProvider.UsingFixture.Driver.SwitchTo().Alert().Accept();

            return ActionProvider;
        }

        /// <summary>
        /// Dismiss alert
        /// </summary>
        /// <returns>action provider</returns>
        public virtual IActionProvider Dismiss()
        {
            ActionProvider.UsingFixture.Driver.SwitchTo().Alert().Dismiss();

            return ActionProvider;
        }
    }
}
