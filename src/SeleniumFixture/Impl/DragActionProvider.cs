using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumFixture.Impl
{
    public interface IDragActionProvider
    {
        IActionProvider To(string element = null, int? x = null, int? y = null);
    }

    public class DragActionProvider : IDragActionProvider
    {
        public IActionProvider To(string element = null, int? x = null, int? y = null)
        {
            throw new NotImplementedException();
        }
    }
}
