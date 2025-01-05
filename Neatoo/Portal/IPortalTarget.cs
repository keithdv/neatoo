using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.Portal
{
    public interface IPortalTarget
    {
        Task<IDisposable> StopAllActions();
        void StartAllActions();
        internal Task PostPortalConstruct();
    }

    public interface IPortalEditTarget : IPortalTarget
    {
        internal void MarkAsChild();

        internal void MarkNew();

        internal void MarkOld();

        internal void MarkUnmodified();

        internal void MarkDeleted();
    }
}
