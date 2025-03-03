﻿using System.Threading.Tasks;
using System.Threading;

/* Unmerged change from project 'Neatoo (net7.0)'
Added:
using Neatoo;
using Neatoo.Internal;
*/

namespace Neatoo.Internal;


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


    Task RunAllRules(CancellationToken? token = null);
    Task RunSelfRules(CancellationToken? token = null);

    void ClearAllErrors();
    void ClearSelfErrors();
}

public interface  IEditMetaSaveProperties
{
    bool IsDeleted { get; }
    bool IsNew { get; }
}

public interface IEditMetaProperties : IEditMetaSaveProperties
{
    bool IsChild { get; }
    bool IsModified { get; }
    bool IsSelfModified { get; }
    bool IsMarkedModified { get; }
    bool IsSavable { get; }

}
