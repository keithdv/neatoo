using Neatoo.Portal;
using System;

namespace Neatoo.Core
{

    public class Paused : IDisposable
    {
        IPortalTarget Target { get; }
        public Paused(IPortalTarget target)
        {
            this.Target = target;
        }

        public void Dispose()
        {
            Target.ResumeAllActions();
        }
    }


}
