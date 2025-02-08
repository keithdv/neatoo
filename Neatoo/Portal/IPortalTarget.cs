using System;
using System.Threading.Tasks;

namespace Neatoo.Portal
{
    public interface IPortalTarget
    {
        IDisposable PauseAllActions();
        void ResumeAllActions();
        internal Task PostPortalConstruct();

    }

    public interface IPortalEditTarget : IPortalTarget
    {
        internal void MarkAsChild();

        internal void MarkNew();

        internal void MarkOld();

        internal void MarkUnmodified();
        internal void MarkModified();

        internal void MarkDeleted();
    }
}
