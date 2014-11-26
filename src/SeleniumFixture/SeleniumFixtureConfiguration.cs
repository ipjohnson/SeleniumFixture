using SimpleFixture;
using SimpleFixture.Impl;

namespace SeleniumFixture
{
    public class SeleniumFixtureConfiguration : DefaultFixtureConfiguration
    {
        public SeleniumFixtureConfiguration()
        {
            Initialize();
        }

        private void Initialize()
        {
            Export<ITypePropertySelector>(g => new SeleniumTypePropertySelector(g.Locate<IConstraintHelper>()));
        }
    }
}
