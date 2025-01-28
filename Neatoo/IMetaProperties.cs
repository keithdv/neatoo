using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Neatoo
{


    public interface IValidateMetaProperties
    {
        bool IsValid { get; }
        bool IsSelfValid { get; }
        bool IsBusy { get; }
        bool IsSelfBusy { get; }

        Task WaitForRules();
        Task CheckAllRules(CancellationToken token = new CancellationToken());
        Task CheckAllSelfRules(CancellationToken token = new CancellationToken());
    }

    public interface IEditMetaProperties : IValidateMetaProperties
    {
        bool IsChild { get; }
        bool IsModified { get; }
        bool IsSelfModified { get; }
        bool IsMarkedModified { get; }
        bool IsNew { get; }
        bool IsSavable { get; }

        bool IsDeleted { get; }
    }
}
