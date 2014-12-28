using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeleniumFixture.xUnit.Impl
{
    internal class XunitWorkerThread
    {
        readonly Thread thread;

        public XunitWorkerThread(Action threadProc)
        {
            thread = new Thread(() => threadProc());
            thread.Start();
        }

        public void Join()
        {
            thread.Join();
        }

        public static void QueueUserWorkItem(Action backgroundTask)
        {
            ThreadPool.QueueUserWorkItem(_ => backgroundTask());
        }
    }
}
