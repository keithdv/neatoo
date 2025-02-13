using System;
using System.Threading.Tasks;

namespace Neatoo.Portal.Internal;

public interface IDataMapperTarget
{
    IDisposable? PauseAllActions();
    void ResumeAllActions();
    internal Task PostPortalConstruct();
}

public interface IDataMapperEditTarget : IDataMapperTarget
{
    internal void MarkAsChild();
    internal void MarkNew();
    internal void MarkOld();
    internal void MarkUnmodified();
    internal void MarkModified();
    internal void MarkDeleted();
}
