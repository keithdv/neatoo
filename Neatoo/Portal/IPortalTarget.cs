using Neatoo.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.Portal
{
    public interface IPortalTarget
    {
        IDisposable StopAllActions();
        void StartAllActions();
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
