using Neatoo.Portal.Internal;
using System;

namespace Neatoo.Core;


public class Paused : IDisposable
{
    IDataMapperTarget Target { get; }
    public Paused(IDataMapperTarget target)
    {
        this.Target = target;
    }

    public void Dispose()
    {
        Target.ResumeAllActions();
    }
}
