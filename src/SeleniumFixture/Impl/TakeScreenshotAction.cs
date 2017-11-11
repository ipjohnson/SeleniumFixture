using System;
using System.Diagnostics;
using OpenQA.Selenium;

namespace SeleniumFixture.Impl
{
    /// <summary>
    /// Screenshot action
    /// </summary>
    public interface ITakeScreenshotAction
    {
        /// <summary>
        /// Take screenshot
        /// </summary>
        /// <param name="screenshotName">screenshot name</param>
        /// <param name="throwsIfNotSupported">throws if driver does not support taking a screenshot</param>
        /// <param name="format"></param>
        /// <returns></returns>
        IActionProvider TakeScreenshot(string screenshotName, bool throwsIfNotSupported, ScreenshotImageFormat format);
    }

    /// <summary>
    /// Screenshot action
    /// </summary>
    public class TakeScreenshotAction : ITakeScreenshotAction
    {
        protected readonly IActionProvider _actionProvider;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="actionProvider"></param>
        public TakeScreenshotAction(IActionProvider actionProvider)
        {
            _actionProvider = actionProvider;
        }

        /// <summary>
        /// Take screenshot
        /// </summary>
        /// <param name="screenshotName">screenshot name</param>
        /// <param name="throwsIfNotSupported">throws if driver does not support taking a screenshot</param>
        /// <param name="format"></param>
        /// <returns></returns>
        public virtual IActionProvider TakeScreenshot(string screenshotName, bool throwsIfNotSupported, ScreenshotImageFormat format)
        {
            if (string.IsNullOrEmpty(screenshotName))
            {
                screenshotName = GetScreenshotName(format);
            }

            ITakesScreenshot screenshotDriver = null;

            try
            {
                screenshotDriver = ((ITakesScreenshot)_actionProvider.UsingFixture.Driver);
            }
            catch (Exception)
            {
                if(throwsIfNotSupported)
                    throw;
            }

            if (screenshotDriver != null)
            {
                screenshotDriver.GetScreenshot().SaveAsFile(screenshotName, format);
            }

            return _actionProvider;
        }

        protected virtual string GetScreenshotName(ScreenshotImageFormat format)
        {
            var stackTrace = new StackTrace();

            StackFrame stackFrame = null;

            foreach (var frame in stackTrace.GetFrames())
            {
                if (frame.GetMethod().DeclaringType.Assembly != GetType().Assembly)
                {
                    stackFrame = frame;
                    break;
                }
            }
            
            if (stackFrame == null)
            {
                return "UNKNOWN";
            }

            var method = stackFrame.GetMethod();

            var returnName = method.DeclaringType != null ? method.DeclaringType.Name + "_" : "";

            returnName += method.Name + "." + format.ToString().ToLower();

            return returnName;
        }
    }
}
