using System.Reflection;

namespace SeleniumFixture.xUnit
{
    public interface IFixtureFinalizerAttribute
    {
        void FixtureFinalizerAttribute(MethodInfo method, Fixture fixture);
    }
}
