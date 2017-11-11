using System.Reflection;

namespace SeleniumFixture.xUnit
{
    public interface IFixtureFinalizerAttribute
    {
        void IFixtureFinalizerAttribute(MethodInfo method, Fixture fixture);
    }
}
