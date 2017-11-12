using System;

namespace SeleniumFixture.xUnit
{
    [AttributeUsage(AttributeTargets.Method)]
    public class WebDriverCommandTimeoutAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout">The maximum amount of time, in seconds, to wait for each command.</param>
        public WebDriverCommandTimeoutAttribute(int timeout)
        {
            if (timeout <= 0)
            {
                throw new ArgumentOutOfRangeException("timeout must be greater than zero");
            }

            Timeout = TimeSpan.FromSeconds(timeout);
        }

        /// <summary>
        /// The maximum amount of time to wait for each command.
        /// </summary>
        public TimeSpan Timeout { get; }
    }
}
