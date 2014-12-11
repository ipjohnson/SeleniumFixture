using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleFixture;

namespace SeleniumFixture.Impl
{
    public interface IYieldsActionProvider
    {
        /// <summary>
        /// Yields a Page Object using SimpleFixture
        /// </summary>
        /// <typeparam name="T">Type of object to Generate</typeparam>
        /// <param name="requestName">request name</param>
        /// <param name="constraints">constraints for the locate</param>
        /// <returns>new T</returns>
        T Yields<T>(string requestName = null, object constraints = null);

                /// <summary>
        /// Yields a Page Object using SimpleFixture
        /// </summary>
        /// <param name="type">Type of object to Generate</param>
        /// <param name="requestName">request name</param>
        /// <param name="constraints">constraints for the locate</param>
        /// <returns>new instance</returns>
        object Yields(Type type, string requestName = null, object constraints = null);
    }

    /// <summary>
    /// Provides fluent syntax for yields
    /// </summary>
    public class YieldsActionProvider : IYieldsActionProvider
    {
        private readonly IActionProvider _actionProvider;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="fixture">action provider</param>
        public YieldsActionProvider(IActionProvider fixture)
        {
            _actionProvider = fixture;
        }

        /// <summary>
        /// Yields a Page Object using SimpleFixture
        /// </summary>
        /// <typeparam name="T">Type of object to Generate</typeparam>
        /// <param name="requestName">request name</param>
        /// <param name="constraints">constraints for the locate</param>
        /// <returns>new T</returns>
        public T Yields<T>(string requestName = null, object constraints = null)
        {
            return (T)Yields(typeof(T), requestName, constraints);
        }

        /// <summary>
        /// Yields a Page Object using SimpleFixture
        /// </summary>
        /// <param name="type">Type of object to Generate</param>
        /// <param name="requestName">request name</param>
        /// <param name="constraints">constraints for the locate</param>
        /// <returns>new instance</returns>
        public object Yields(Type type, string requestName = null, object constraints = null)
        {
            var request = new DataRequest(null, _actionProvider.UsingFixture.Data, type, requestName, false, constraints, null);

            return _actionProvider.UsingFixture.Data.Generate(request);
        }
    }
}
