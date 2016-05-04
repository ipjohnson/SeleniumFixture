using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumFixture.xUnit
{
    /// <summary>
    /// Attributes that implement this interface can provide a IFixtureInitializer to initialize the fixture
    /// </summary>
    public interface IFixtureInitializationAttribute
    {
        /// <summary>
        /// Provide fixture initializer
        /// </summary>
        /// <param name="fixture">fixture</param>
        /// <returns></returns>
        object Initialize(MethodInfo testMethod, Fixture fixture);
    }
}
