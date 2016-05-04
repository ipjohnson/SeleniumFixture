using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumFixture.xUnit
{
    public interface IFixtureFinalizerAttribute
    {
        void IFixtureFinalizerAttribute(MethodInfo method, Fixture fixture);
    }
}
