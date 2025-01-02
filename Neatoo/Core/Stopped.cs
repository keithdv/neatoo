using Neatoo.Portal;
using System;

namespace Neatoo.Core
{

    public class Stopped : IDisposable
    {
        IPortalTarget Target { get; }
        public Stopped(IPortalTarget target)
        {
            this.Target = target;
        }

        public void Dispose()
        {
            Target.StartAllActions();
        }
    }


}
