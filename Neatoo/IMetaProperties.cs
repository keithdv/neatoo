using System.Threading.Tasks;
using System.Threading;

namespace Neatoo;


public interface IBaseMetaProperties
{
    bool IsBusy { get; }
    bool IsSelfBusy { get; }
    Task WaitForTasks();
}

public interface IValidateMetaProperties : IBaseMetaProperties
{
    bool IsValid { get; }
    bool IsSelfValid { get; }


    Task RunAllRules(CancellationToken token = new CancellationToken());
    Task RunSelfRules(CancellationToken token = new CancellationToken());

    void ClearAllErrors();
    void ClearSelfErrors();
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
