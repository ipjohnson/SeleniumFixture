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
        T Yields<T>(string requestName = null, object constraints = null);

        object Yields(Type type, string requestName = null, object constraints = null);
    }

    public class YieldsActionProvider : IYieldsActionProvider
    {
        private readonly Fixture _fixture;

        public YieldsActionProvider(Fixture fixture)
        {
            _fixture = fixture;
        }

        public T Yields<T>(string requestName = null, object constraints = null)
        {
            return (T)Yields(typeof(T), requestName, constraints);
        }

        public object Yields(Type type, string requestName = null, object constraints = null)
        {
            var request = new DataRequest(null, _fixture.Data, type, requestName, false, constraints, null);

            return _fixture.Data.Generate(request);
        }
    }
}
