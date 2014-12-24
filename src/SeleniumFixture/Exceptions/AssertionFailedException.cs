using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumFixture.Exceptions
{
    /// <summary>
    /// Exception thrown while executing 
    /// </summary>
    public class AssertionFailedException : Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="pageType"></param>
        public AssertionFailedException(Exception innerException, Type pageType)
            : base(FormatErrorMessage(innerException, pageType), innerException)
        {
            PageType = pageType;
        }

        /// <summary>
        /// Type of page being constructed
        /// </summary>
        public Type PageType { get; private set; }


        private static string FormatErrorMessage(Exception innerException, Type pageType)
        {
            return string.Format("Validation failure while constructing page: {0}{1}{2}", pageType.Name, Environment.NewLine, innerException.Message);
        }
    }
}
