using System.Reflection;

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
        /// <param name="testMethod">The test method</param>
        /// <param name="fixture">The fixture</param>
        /// <returns></returns>
        object Initialize(MethodInfo testMethod, Fixture fixture);
    }
}
