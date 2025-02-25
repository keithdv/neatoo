using Microsoft.Extensions.DependencyInjection;
using Neatoo.Portal.Internal;
using Neatoo;
using Neatoo.Portal;
using static Neatoo.UnitTest.Portal.AuthorizationAllCombinationTests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Neatoo.AuthorizationRules;
using Neatoo.Internal;
using Neatoo.Portal.Internal;
using System.Reflection;
using System.Reflection.Metadata;

/*
Debugging Messages:
Parent class: AuthorizationAllCombinationTests
For AuthorizationAllCombinations using AuthorizationAllCombinations
: IAuthorizedAllCombinations
*/
namespace Neatoo.UnitTest.Portal
{
    public interface IAuthorizedAllCombinationsFactory
    {
        Authorized<IAuthorizedAllCombinations> TryCreate(VoidBool v);
        IAuthorizedAllCombinations? Create(VoidBool v);
        Authorized<IAuthorizedAllCombinations> TryCreate(VoidString v);
        IAuthorizedAllCombinations? Create(VoidString v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(VoidTaskBool v);
        Task<IAuthorizedAllCombinations?> Create(VoidTaskBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(VoidTaskString v);
        Task<IAuthorizedAllCombinations?> Create(VoidTaskString v);
        Authorized<IAuthorizedAllCombinations> TryCreate(TrueBoolBool v);
        IAuthorizedAllCombinations? Create(TrueBoolBool v);
        Authorized<IAuthorizedAllCombinations> TryCreate(TrueBoolString v);
        IAuthorizedAllCombinations? Create(TrueBoolString v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TrueBoolTaskBool v);
        Task<IAuthorizedAllCombinations?> Create(TrueBoolTaskBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TrueBoolTaskString v);
        Task<IAuthorizedAllCombinations?> Create(TrueBoolTaskString v);
        Authorized<IAuthorizedAllCombinations> TryCreate(FalseBoolBool v);
        IAuthorizedAllCombinations? Create(FalseBoolBool v);
        Authorized<IAuthorizedAllCombinations> TryCreate(FalseBoolString v);
        IAuthorizedAllCombinations? Create(FalseBoolString v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(FalseBoolTaskBool v);
        Task<IAuthorizedAllCombinations?> Create(FalseBoolTaskBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(FalseBoolTaskString v);
        Task<IAuthorizedAllCombinations?> Create(FalseBoolTaskString v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidBool v);
        Task<IAuthorizedAllCombinations?> Create(TaskVoidBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidString v);
        Task<IAuthorizedAllCombinations?> Create(TaskVoidString v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidTaskBool v);
        Task<IAuthorizedAllCombinations?> Create(TaskVoidTaskBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidTaskString v);
        Task<IAuthorizedAllCombinations?> Create(TaskVoidTaskString v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolBool v);
        Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolString v);
        Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolString v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolTaskBool v);
        Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolTaskBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolTaskString v);
        Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolTaskString v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolBool v);
        Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolString v);
        Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolString v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolTaskBool v);
        Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolTaskBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolTaskString v);
        Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolTaskString v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(VoidBoolRemote v);
        Task<IAuthorizedAllCombinations?> Create(VoidBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(VoidStringRemote v);
        Task<IAuthorizedAllCombinations?> Create(VoidStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(VoidTaskBoolRemote v);
        Task<IAuthorizedAllCombinations?> Create(VoidTaskBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(VoidTaskStringRemote v);
        Task<IAuthorizedAllCombinations?> Create(VoidTaskStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TrueBoolBoolRemote v);
        Task<IAuthorizedAllCombinations?> Create(TrueBoolBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TrueBoolStringRemote v);
        Task<IAuthorizedAllCombinations?> Create(TrueBoolStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TrueBoolTaskBoolRemote v);
        Task<IAuthorizedAllCombinations?> Create(TrueBoolTaskBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TrueBoolTaskStringRemote v);
        Task<IAuthorizedAllCombinations?> Create(TrueBoolTaskStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(FalseBoolBoolRemote v);
        Task<IAuthorizedAllCombinations?> Create(FalseBoolBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(FalseBoolStringRemote v);
        Task<IAuthorizedAllCombinations?> Create(FalseBoolStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(FalseBoolTaskBoolRemote v);
        Task<IAuthorizedAllCombinations?> Create(FalseBoolTaskBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(FalseBoolTaskStringRemote v);
        Task<IAuthorizedAllCombinations?> Create(FalseBoolTaskStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidBoolRemote v);
        Task<IAuthorizedAllCombinations?> Create(TaskVoidBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidStringRemote v);
        Task<IAuthorizedAllCombinations?> Create(TaskVoidStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidTaskBoolRemote v);
        Task<IAuthorizedAllCombinations?> Create(TaskVoidTaskBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidTaskStringRemote v);
        Task<IAuthorizedAllCombinations?> Create(TaskVoidTaskStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolBoolRemote v);
        Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolStringRemote v);
        Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolTaskBoolRemote v);
        Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolTaskBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolTaskStringRemote v);
        Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolTaskStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolBoolRemote v);
        Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolStringRemote v);
        Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolTaskBoolRemote v);
        Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolTaskBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolTaskStringRemote v);
        Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolTaskStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidBool v);
        Task<IAuthorizedAllCombinations?> Create(RemoteVoidBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidString v);
        Task<IAuthorizedAllCombinations?> Create(RemoteVoidString v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidTaskBool v);
        Task<IAuthorizedAllCombinations?> Create(RemoteVoidTaskBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidTaskString v);
        Task<IAuthorizedAllCombinations?> Create(RemoteVoidTaskString v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolBool v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolString v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolString v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolTaskBool v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolTaskBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolTaskString v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolTaskString v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolBool v);
        Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolString v);
        Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolString v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolTaskBool v);
        Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolTaskBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolTaskString v);
        Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolTaskString v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidBool v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidString v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidString v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidTaskBool v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidTaskBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidTaskString v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidTaskString v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolBool v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolString v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolString v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolTaskBool v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolTaskBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolTaskString v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolTaskString v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolBool v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolString v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolString v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolTaskBool v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolTaskBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolTaskString v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolTaskString v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidBoolRemote v);
        Task<IAuthorizedAllCombinations?> Create(RemoteVoidBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidStringRemote v);
        Task<IAuthorizedAllCombinations?> Create(RemoteVoidStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidTaskBoolRemote v);
        Task<IAuthorizedAllCombinations?> Create(RemoteVoidTaskBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidTaskStringRemote v);
        Task<IAuthorizedAllCombinations?> Create(RemoteVoidTaskStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolBoolRemote v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolStringRemote v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolTaskBoolRemote v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolTaskBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolTaskStringRemote v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolTaskStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolBoolRemote v);
        Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolStringRemote v);
        Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolTaskBoolRemote v);
        Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolTaskBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolTaskStringRemote v);
        Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolTaskStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidBoolRemote v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidStringRemote v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidTaskBoolRemote v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidTaskBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidTaskStringRemote v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidTaskStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolBoolRemote v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolStringRemote v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolTaskBoolRemote v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolTaskBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolTaskStringRemote v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolTaskStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolBoolRemote v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolStringRemote v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolTaskBoolRemote v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolTaskBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolTaskStringRemote v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolTaskStringRemote v);
        Authorized<IAuthorizedAllCombinations> TryCreate(VoidBoolDeny v);
        IAuthorizedAllCombinations? Create(VoidBoolDeny v);
        Authorized<IAuthorizedAllCombinations> TryCreate(VoidStringDeny v);
        IAuthorizedAllCombinations? Create(VoidStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(VoidTaskBoolDeny v);
        Task<IAuthorizedAllCombinations?> Create(VoidTaskBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(VoidTaskStringDeny v);
        Task<IAuthorizedAllCombinations?> Create(VoidTaskStringDeny v);
        Authorized<IAuthorizedAllCombinations> TryCreate(TrueBoolBoolDeny v);
        IAuthorizedAllCombinations? Create(TrueBoolBoolDeny v);
        Authorized<IAuthorizedAllCombinations> TryCreate(TrueBoolStringDeny v);
        IAuthorizedAllCombinations? Create(TrueBoolStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TrueBoolTaskBoolDeny v);
        Task<IAuthorizedAllCombinations?> Create(TrueBoolTaskBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TrueBoolTaskStringDeny v);
        Task<IAuthorizedAllCombinations?> Create(TrueBoolTaskStringDeny v);
        Authorized<IAuthorizedAllCombinations> TryCreate(FalseBoolBoolDeny v);
        IAuthorizedAllCombinations? Create(FalseBoolBoolDeny v);
        Authorized<IAuthorizedAllCombinations> TryCreate(FalseBoolStringDeny v);
        IAuthorizedAllCombinations? Create(FalseBoolStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(FalseBoolTaskBoolDeny v);
        Task<IAuthorizedAllCombinations?> Create(FalseBoolTaskBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(FalseBoolTaskStringDeny v);
        Task<IAuthorizedAllCombinations?> Create(FalseBoolTaskStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidBoolDeny v);
        Task<IAuthorizedAllCombinations?> Create(TaskVoidBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidStringDeny v);
        Task<IAuthorizedAllCombinations?> Create(TaskVoidStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidTaskBoolDeny v);
        Task<IAuthorizedAllCombinations?> Create(TaskVoidTaskBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidTaskStringDeny v);
        Task<IAuthorizedAllCombinations?> Create(TaskVoidTaskStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolBoolDeny v);
        Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolStringDeny v);
        Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolTaskBoolDeny v);
        Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolTaskBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolTaskStringDeny v);
        Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolTaskStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolBoolDeny v);
        Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolStringDeny v);
        Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolTaskBoolDeny v);
        Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolTaskBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolTaskStringDeny v);
        Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolTaskStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(VoidBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(VoidBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(VoidStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(VoidStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(VoidTaskBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(VoidTaskBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(VoidTaskStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(VoidTaskStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TrueBoolBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(TrueBoolBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TrueBoolStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(TrueBoolStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TrueBoolTaskBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(TrueBoolTaskBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TrueBoolTaskStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(TrueBoolTaskStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(FalseBoolBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(FalseBoolBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(FalseBoolStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(FalseBoolStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(FalseBoolTaskBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(FalseBoolTaskBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(FalseBoolTaskStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(FalseBoolTaskStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(TaskVoidBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(TaskVoidStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidTaskBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(TaskVoidTaskBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidTaskStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(TaskVoidTaskStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolTaskBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolTaskBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolTaskStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolTaskStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolTaskBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolTaskBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolTaskStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolTaskStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidBoolDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteVoidBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidStringDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteVoidStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidTaskBoolDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteVoidTaskBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidTaskStringDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteVoidTaskStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolBoolDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolStringDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolTaskBoolDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolTaskBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolTaskStringDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolTaskStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolBoolDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolStringDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolTaskBoolDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolTaskBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolTaskStringDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolTaskStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidBoolDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidStringDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidTaskBoolDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidTaskBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidTaskStringDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidTaskStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolBoolDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolStringDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolTaskBoolDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolTaskBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolTaskStringDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolTaskStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolBoolDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolStringDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolTaskBoolDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolTaskBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolTaskStringDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolTaskStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteVoidBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteVoidStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidTaskBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteVoidTaskBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidTaskStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteVoidTaskStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolTaskBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolTaskBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolTaskStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolTaskStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolTaskBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolTaskBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolTaskStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolTaskStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidTaskBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidTaskBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidTaskStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidTaskStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolTaskBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolTaskBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolTaskStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolTaskStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolTaskBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolTaskBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolTaskStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolTaskStringRemoteDeny v);
        IAuthorizedAllCombinations? Save(IAuthorizedAllCombinations target, VoidBool v);
        Authorized<IAuthorizedAllCombinations> TrySave(IAuthorizedAllCombinations target, VoidBool v);
        IAuthorizedAllCombinations? Save(IAuthorizedAllCombinations target, VoidString v);
        Authorized<IAuthorizedAllCombinations> TrySave(IAuthorizedAllCombinations target, VoidString v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, VoidTaskBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, VoidTaskBool v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, VoidTaskString v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, VoidTaskString v);
        IAuthorizedAllCombinations? Save(IAuthorizedAllCombinations target, TrueBoolBool v);
        Authorized<IAuthorizedAllCombinations> TrySave(IAuthorizedAllCombinations target, TrueBoolBool v);
        IAuthorizedAllCombinations? Save(IAuthorizedAllCombinations target, TrueBoolString v);
        Authorized<IAuthorizedAllCombinations> TrySave(IAuthorizedAllCombinations target, TrueBoolString v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TrueBoolTaskBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TrueBoolTaskBool v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TrueBoolTaskString v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TrueBoolTaskString v);
        IAuthorizedAllCombinations? Save(IAuthorizedAllCombinations target, FalseBoolBool v);
        Authorized<IAuthorizedAllCombinations> TrySave(IAuthorizedAllCombinations target, FalseBoolBool v);
        IAuthorizedAllCombinations? Save(IAuthorizedAllCombinations target, FalseBoolString v);
        Authorized<IAuthorizedAllCombinations> TrySave(IAuthorizedAllCombinations target, FalseBoolString v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, FalseBoolTaskBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, FalseBoolTaskBool v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, FalseBoolTaskString v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, FalseBoolTaskString v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidBool v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidString v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidString v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidTaskBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidTaskBool v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidTaskString v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidTaskString v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolBool v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolString v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolString v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolTaskBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolTaskBool v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolTaskString v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolTaskString v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolBool v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolString v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolString v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolTaskBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolTaskBool v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolTaskString v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolTaskString v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, VoidBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, VoidBoolRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, VoidStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, VoidStringRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, VoidTaskBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, VoidTaskBoolRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, VoidTaskStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, VoidTaskStringRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TrueBoolBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TrueBoolBoolRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TrueBoolStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TrueBoolStringRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TrueBoolTaskBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TrueBoolTaskBoolRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TrueBoolTaskStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TrueBoolTaskStringRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, FalseBoolBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, FalseBoolBoolRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, FalseBoolStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, FalseBoolStringRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, FalseBoolTaskBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, FalseBoolTaskBoolRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, FalseBoolTaskStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, FalseBoolTaskStringRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidBoolRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidStringRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidTaskBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidTaskBoolRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidTaskStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidTaskStringRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolBoolRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolStringRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolTaskBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolTaskBoolRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolTaskStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolTaskStringRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolBoolRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolStringRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolTaskBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolTaskBoolRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolTaskStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolTaskStringRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidBool v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidString v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidString v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidTaskBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidTaskBool v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidTaskString v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidTaskString v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolBool v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolString v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolString v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolTaskBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolTaskBool v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolTaskString v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolTaskString v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolBool v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolString v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolString v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolTaskBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolTaskBool v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolTaskString v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolTaskString v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidBool v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidString v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidString v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidTaskBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidTaskBool v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidTaskString v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidTaskString v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolBool v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolString v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolString v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskBool v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskString v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskString v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolBool v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolString v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolString v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskBool v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskBool v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskString v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskString v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidBoolRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidStringRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidTaskBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidTaskBoolRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidTaskStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidTaskStringRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolBoolRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolStringRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolTaskBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolTaskBoolRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolTaskStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolTaskStringRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolBoolRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolStringRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolTaskBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolTaskBoolRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolTaskStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolTaskStringRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidBoolRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidStringRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidTaskBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidTaskBoolRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidTaskStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidTaskStringRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolBoolRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolStringRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskBoolRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskStringRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolBoolRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolStringRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskBoolRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskBoolRemote v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskStringRemote v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskStringRemote v);
        IAuthorizedAllCombinations? Save(IAuthorizedAllCombinations target, VoidBoolDeny v);
        Authorized<IAuthorizedAllCombinations> TrySave(IAuthorizedAllCombinations target, VoidBoolDeny v);
        IAuthorizedAllCombinations? Save(IAuthorizedAllCombinations target, VoidStringDeny v);
        Authorized<IAuthorizedAllCombinations> TrySave(IAuthorizedAllCombinations target, VoidStringDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, VoidTaskBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, VoidTaskBoolDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, VoidTaskStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, VoidTaskStringDeny v);
        IAuthorizedAllCombinations? Save(IAuthorizedAllCombinations target, TrueBoolBoolDeny v);
        Authorized<IAuthorizedAllCombinations> TrySave(IAuthorizedAllCombinations target, TrueBoolBoolDeny v);
        IAuthorizedAllCombinations? Save(IAuthorizedAllCombinations target, TrueBoolStringDeny v);
        Authorized<IAuthorizedAllCombinations> TrySave(IAuthorizedAllCombinations target, TrueBoolStringDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TrueBoolTaskBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TrueBoolTaskBoolDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TrueBoolTaskStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TrueBoolTaskStringDeny v);
        IAuthorizedAllCombinations? Save(IAuthorizedAllCombinations target, FalseBoolBoolDeny v);
        Authorized<IAuthorizedAllCombinations> TrySave(IAuthorizedAllCombinations target, FalseBoolBoolDeny v);
        IAuthorizedAllCombinations? Save(IAuthorizedAllCombinations target, FalseBoolStringDeny v);
        Authorized<IAuthorizedAllCombinations> TrySave(IAuthorizedAllCombinations target, FalseBoolStringDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, FalseBoolTaskBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, FalseBoolTaskBoolDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, FalseBoolTaskStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, FalseBoolTaskStringDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidBoolDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidStringDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidTaskBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidTaskBoolDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidTaskStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidTaskStringDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolBoolDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolStringDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolTaskBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolTaskBoolDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolTaskStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolTaskStringDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolBoolDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolStringDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolTaskBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolTaskBoolDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolTaskStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolTaskStringDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, VoidBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, VoidBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, VoidStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, VoidStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, VoidTaskBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, VoidTaskBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, VoidTaskStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, VoidTaskStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TrueBoolBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TrueBoolBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TrueBoolStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TrueBoolStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TrueBoolTaskBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TrueBoolTaskBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TrueBoolTaskStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TrueBoolTaskStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, FalseBoolBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, FalseBoolBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, FalseBoolStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, FalseBoolStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, FalseBoolTaskBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, FalseBoolTaskBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, FalseBoolTaskStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, FalseBoolTaskStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidTaskBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidTaskBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidTaskStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidTaskStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolTaskBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolTaskBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolTaskStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolTaskStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolTaskBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolTaskBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolTaskStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolTaskStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidBoolDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidStringDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidTaskBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidTaskBoolDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidTaskStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidTaskStringDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolBoolDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolStringDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolTaskBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolTaskBoolDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolTaskStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolTaskStringDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolBoolDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolStringDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolTaskBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolTaskBoolDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolTaskStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolTaskStringDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidBoolDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidStringDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidTaskBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidTaskBoolDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidTaskStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidTaskStringDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolBoolDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolStringDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskBoolDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskStringDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolBoolDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolStringDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskBoolDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskBoolDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskStringDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskStringDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidTaskBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidTaskBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidTaskStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidTaskStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolTaskBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolTaskBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolTaskStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolTaskStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolTaskBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolTaskBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolTaskStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolTaskStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidTaskBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidTaskBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidTaskStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidTaskStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolStringRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskBoolRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskBoolRemoteDeny v);
        Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskStringRemoteDeny v);
        Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskStringRemoteDeny v);
    }

    internal class AuthorizedAllCombinationsFactory : FactoryEditBase<AuthorizedAllCombinations>, IFactoryEditBase<AuthorizedAllCombinations>, IAuthorizedAllCombinationsFactory
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly IDoRemoteRequest DoRemoteRequest;
        public AuthorizationAllCombinations AuthorizationAllCombinations { get; }
        public Create24Delegate Create24Property { get; }
        public Create25Delegate Create25Property { get; }
        public Create26Delegate Create26Property { get; }
        public Create27Delegate Create27Property { get; }
        public Create28Delegate Create28Property { get; }
        public Create29Delegate Create29Property { get; }
        public Create30Delegate Create30Property { get; }
        public Create31Delegate Create31Property { get; }
        public Create32Delegate Create32Property { get; }
        public Create33Delegate Create33Property { get; }
        public Create34Delegate Create34Property { get; }
        public Create35Delegate Create35Property { get; }
        public Create36Delegate Create36Property { get; }
        public Create37Delegate Create37Property { get; }
        public Create38Delegate Create38Property { get; }
        public Create39Delegate Create39Property { get; }
        public Create40Delegate Create40Property { get; }
        public Create41Delegate Create41Property { get; }
        public Create42Delegate Create42Property { get; }
        public Create43Delegate Create43Property { get; }
        public Create44Delegate Create44Property { get; }
        public Create45Delegate Create45Property { get; }
        public Create46Delegate Create46Property { get; }
        public Create47Delegate Create47Property { get; }
        public Create48Delegate Create48Property { get; }
        public Create49Delegate Create49Property { get; }
        public Create50Delegate Create50Property { get; }
        public Create51Delegate Create51Property { get; }
        public Create52Delegate Create52Property { get; }
        public Create53Delegate Create53Property { get; }
        public Create54Delegate Create54Property { get; }
        public Create55Delegate Create55Property { get; }
        public Create56Delegate Create56Property { get; }
        public Create57Delegate Create57Property { get; }
        public Create58Delegate Create58Property { get; }
        public Create59Delegate Create59Property { get; }
        public Create60Delegate Create60Property { get; }
        public Create61Delegate Create61Property { get; }
        public Create62Delegate Create62Property { get; }
        public Create63Delegate Create63Property { get; }
        public Create64Delegate Create64Property { get; }
        public Create65Delegate Create65Property { get; }
        public Create66Delegate Create66Property { get; }
        public Create67Delegate Create67Property { get; }
        public Create68Delegate Create68Property { get; }
        public Create69Delegate Create69Property { get; }
        public Create70Delegate Create70Property { get; }
        public Create71Delegate Create71Property { get; }
        public Create72Delegate Create72Property { get; }
        public Create73Delegate Create73Property { get; }
        public Create74Delegate Create74Property { get; }
        public Create75Delegate Create75Property { get; }
        public Create76Delegate Create76Property { get; }
        public Create77Delegate Create77Property { get; }
        public Create78Delegate Create78Property { get; }
        public Create79Delegate Create79Property { get; }
        public Create80Delegate Create80Property { get; }
        public Create81Delegate Create81Property { get; }
        public Create82Delegate Create82Property { get; }
        public Create83Delegate Create83Property { get; }
        public Create84Delegate Create84Property { get; }
        public Create85Delegate Create85Property { get; }
        public Create86Delegate Create86Property { get; }
        public Create87Delegate Create87Property { get; }
        public Create88Delegate Create88Property { get; }
        public Create89Delegate Create89Property { get; }
        public Create90Delegate Create90Property { get; }
        public Create91Delegate Create91Property { get; }
        public Create92Delegate Create92Property { get; }
        public Create93Delegate Create93Property { get; }
        public Create94Delegate Create94Property { get; }
        public Create95Delegate Create95Property { get; }
        public Create120Delegate Create120Property { get; }
        public Create121Delegate Create121Property { get; }
        public Create122Delegate Create122Property { get; }
        public Create123Delegate Create123Property { get; }
        public Create124Delegate Create124Property { get; }
        public Create125Delegate Create125Property { get; }
        public Create126Delegate Create126Property { get; }
        public Create127Delegate Create127Property { get; }
        public Create128Delegate Create128Property { get; }
        public Create129Delegate Create129Property { get; }
        public Create130Delegate Create130Property { get; }
        public Create131Delegate Create131Property { get; }
        public Create132Delegate Create132Property { get; }
        public Create133Delegate Create133Property { get; }
        public Create134Delegate Create134Property { get; }
        public Create135Delegate Create135Property { get; }
        public Create136Delegate Create136Property { get; }
        public Create137Delegate Create137Property { get; }
        public Create138Delegate Create138Property { get; }
        public Create139Delegate Create139Property { get; }
        public Create140Delegate Create140Property { get; }
        public Create141Delegate Create141Property { get; }
        public Create142Delegate Create142Property { get; }
        public Create143Delegate Create143Property { get; }
        public Create144Delegate Create144Property { get; }
        public Create145Delegate Create145Property { get; }
        public Create146Delegate Create146Property { get; }
        public Create147Delegate Create147Property { get; }
        public Create148Delegate Create148Property { get; }
        public Create149Delegate Create149Property { get; }
        public Create150Delegate Create150Property { get; }
        public Create151Delegate Create151Property { get; }
        public Create152Delegate Create152Property { get; }
        public Create153Delegate Create153Property { get; }
        public Create154Delegate Create154Property { get; }
        public Create155Delegate Create155Property { get; }
        public Create156Delegate Create156Property { get; }
        public Create157Delegate Create157Property { get; }
        public Create158Delegate Create158Property { get; }
        public Create159Delegate Create159Property { get; }
        public Create160Delegate Create160Property { get; }
        public Create161Delegate Create161Property { get; }
        public Create162Delegate Create162Property { get; }
        public Create163Delegate Create163Property { get; }
        public Create164Delegate Create164Property { get; }
        public Create165Delegate Create165Property { get; }
        public Create166Delegate Create166Property { get; }
        public Create167Delegate Create167Property { get; }
        public Create168Delegate Create168Property { get; }
        public Create169Delegate Create169Property { get; }
        public Create170Delegate Create170Property { get; }
        public Create171Delegate Create171Property { get; }
        public Create172Delegate Create172Property { get; }
        public Create173Delegate Create173Property { get; }
        public Create174Delegate Create174Property { get; }
        public Create175Delegate Create175Property { get; }
        public Create176Delegate Create176Property { get; }
        public Create177Delegate Create177Property { get; }
        public Create178Delegate Create178Property { get; }
        public Create179Delegate Create179Property { get; }
        public Create180Delegate Create180Property { get; }
        public Create181Delegate Create181Property { get; }
        public Create182Delegate Create182Property { get; }
        public Create183Delegate Create183Property { get; }
        public Create184Delegate Create184Property { get; }
        public Create185Delegate Create185Property { get; }
        public Create186Delegate Create186Property { get; }
        public Create187Delegate Create187Property { get; }
        public Create188Delegate Create188Property { get; }
        public Create189Delegate Create189Property { get; }
        public Create190Delegate Create190Property { get; }
        public Create191Delegate Create191Property { get; }
        public Save24Delegate Save24Property { get; set; }
        public Save25Delegate Save25Property { get; set; }
        public Save26Delegate Save26Property { get; set; }
        public Save27Delegate Save27Property { get; set; }
        public Save28Delegate Save28Property { get; set; }
        public Save29Delegate Save29Property { get; set; }
        public Save30Delegate Save30Property { get; set; }
        public Save31Delegate Save31Property { get; set; }
        public Save32Delegate Save32Property { get; set; }
        public Save33Delegate Save33Property { get; set; }
        public Save34Delegate Save34Property { get; set; }
        public Save35Delegate Save35Property { get; set; }
        public Save36Delegate Save36Property { get; set; }
        public Save37Delegate Save37Property { get; set; }
        public Save38Delegate Save38Property { get; set; }
        public Save39Delegate Save39Property { get; set; }
        public Save40Delegate Save40Property { get; set; }
        public Save41Delegate Save41Property { get; set; }
        public Save42Delegate Save42Property { get; set; }
        public Save43Delegate Save43Property { get; set; }
        public Save44Delegate Save44Property { get; set; }
        public Save45Delegate Save45Property { get; set; }
        public Save46Delegate Save46Property { get; set; }
        public Save47Delegate Save47Property { get; set; }
        public Save48Delegate Save48Property { get; set; }
        public Save49Delegate Save49Property { get; set; }
        public Save50Delegate Save50Property { get; set; }
        public Save51Delegate Save51Property { get; set; }
        public Save52Delegate Save52Property { get; set; }
        public Save53Delegate Save53Property { get; set; }
        public Save54Delegate Save54Property { get; set; }
        public Save55Delegate Save55Property { get; set; }
        public Save56Delegate Save56Property { get; set; }
        public Save57Delegate Save57Property { get; set; }
        public Save58Delegate Save58Property { get; set; }
        public Save59Delegate Save59Property { get; set; }
        public Save60Delegate Save60Property { get; set; }
        public Save61Delegate Save61Property { get; set; }
        public Save62Delegate Save62Property { get; set; }
        public Save63Delegate Save63Property { get; set; }
        public Save64Delegate Save64Property { get; set; }
        public Save65Delegate Save65Property { get; set; }
        public Save66Delegate Save66Property { get; set; }
        public Save67Delegate Save67Property { get; set; }
        public Save68Delegate Save68Property { get; set; }
        public Save69Delegate Save69Property { get; set; }
        public Save70Delegate Save70Property { get; set; }
        public Save71Delegate Save71Property { get; set; }
        public Save72Delegate Save72Property { get; set; }
        public Save73Delegate Save73Property { get; set; }
        public Save74Delegate Save74Property { get; set; }
        public Save75Delegate Save75Property { get; set; }
        public Save76Delegate Save76Property { get; set; }
        public Save77Delegate Save77Property { get; set; }
        public Save78Delegate Save78Property { get; set; }
        public Save79Delegate Save79Property { get; set; }
        public Save80Delegate Save80Property { get; set; }
        public Save81Delegate Save81Property { get; set; }
        public Save82Delegate Save82Property { get; set; }
        public Save83Delegate Save83Property { get; set; }
        public Save84Delegate Save84Property { get; set; }
        public Save85Delegate Save85Property { get; set; }
        public Save86Delegate Save86Property { get; set; }
        public Save87Delegate Save87Property { get; set; }
        public Save88Delegate Save88Property { get; set; }
        public Save89Delegate Save89Property { get; set; }
        public Save90Delegate Save90Property { get; set; }
        public Save91Delegate Save91Property { get; set; }
        public Save92Delegate Save92Property { get; set; }
        public Save93Delegate Save93Property { get; set; }
        public Save94Delegate Save94Property { get; set; }
        public Save95Delegate Save95Property { get; set; }
        public Save120Delegate Save120Property { get; set; }
        public Save121Delegate Save121Property { get; set; }
        public Save122Delegate Save122Property { get; set; }
        public Save123Delegate Save123Property { get; set; }
        public Save124Delegate Save124Property { get; set; }
        public Save125Delegate Save125Property { get; set; }
        public Save126Delegate Save126Property { get; set; }
        public Save127Delegate Save127Property { get; set; }
        public Save128Delegate Save128Property { get; set; }
        public Save129Delegate Save129Property { get; set; }
        public Save130Delegate Save130Property { get; set; }
        public Save131Delegate Save131Property { get; set; }
        public Save132Delegate Save132Property { get; set; }
        public Save133Delegate Save133Property { get; set; }
        public Save134Delegate Save134Property { get; set; }
        public Save135Delegate Save135Property { get; set; }
        public Save136Delegate Save136Property { get; set; }
        public Save137Delegate Save137Property { get; set; }
        public Save138Delegate Save138Property { get; set; }
        public Save139Delegate Save139Property { get; set; }
        public Save140Delegate Save140Property { get; set; }
        public Save141Delegate Save141Property { get; set; }
        public Save142Delegate Save142Property { get; set; }
        public Save143Delegate Save143Property { get; set; }
        public Save144Delegate Save144Property { get; set; }
        public Save145Delegate Save145Property { get; set; }
        public Save146Delegate Save146Property { get; set; }
        public Save147Delegate Save147Property { get; set; }
        public Save148Delegate Save148Property { get; set; }
        public Save149Delegate Save149Property { get; set; }
        public Save150Delegate Save150Property { get; set; }
        public Save151Delegate Save151Property { get; set; }
        public Save152Delegate Save152Property { get; set; }
        public Save153Delegate Save153Property { get; set; }
        public Save154Delegate Save154Property { get; set; }
        public Save155Delegate Save155Property { get; set; }
        public Save156Delegate Save156Property { get; set; }
        public Save157Delegate Save157Property { get; set; }
        public Save158Delegate Save158Property { get; set; }
        public Save159Delegate Save159Property { get; set; }
        public Save160Delegate Save160Property { get; set; }
        public Save161Delegate Save161Property { get; set; }
        public Save162Delegate Save162Property { get; set; }
        public Save163Delegate Save163Property { get; set; }
        public Save164Delegate Save164Property { get; set; }
        public Save165Delegate Save165Property { get; set; }
        public Save166Delegate Save166Property { get; set; }
        public Save167Delegate Save167Property { get; set; }
        public Save168Delegate Save168Property { get; set; }
        public Save169Delegate Save169Property { get; set; }
        public Save170Delegate Save170Property { get; set; }
        public Save171Delegate Save171Property { get; set; }
        public Save172Delegate Save172Property { get; set; }
        public Save173Delegate Save173Property { get; set; }
        public Save174Delegate Save174Property { get; set; }
        public Save175Delegate Save175Property { get; set; }
        public Save176Delegate Save176Property { get; set; }
        public Save177Delegate Save177Property { get; set; }
        public Save178Delegate Save178Property { get; set; }
        public Save179Delegate Save179Property { get; set; }
        public Save180Delegate Save180Property { get; set; }
        public Save181Delegate Save181Property { get; set; }
        public Save182Delegate Save182Property { get; set; }
        public Save183Delegate Save183Property { get; set; }
        public Save184Delegate Save184Property { get; set; }
        public Save185Delegate Save185Property { get; set; }
        public Save186Delegate Save186Property { get; set; }
        public Save187Delegate Save187Property { get; set; }
        public Save188Delegate Save188Property { get; set; }
        public Save189Delegate Save189Property { get; set; }
        public Save190Delegate Save190Property { get; set; }
        public Save191Delegate Save191Property { get; set; }

        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create24Delegate(VoidBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create25Delegate(VoidStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create26Delegate(VoidTaskBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create27Delegate(VoidTaskStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create28Delegate(TrueBoolBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create29Delegate(TrueBoolStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create30Delegate(TrueBoolTaskBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create31Delegate(TrueBoolTaskStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create32Delegate(FalseBoolBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create33Delegate(FalseBoolStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create34Delegate(FalseBoolTaskBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create35Delegate(FalseBoolTaskStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create36Delegate(TaskVoidBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create37Delegate(TaskVoidStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create38Delegate(TaskVoidTaskBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create39Delegate(TaskVoidTaskStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create40Delegate(TaskTrueBoolBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create41Delegate(TaskTrueBoolStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create42Delegate(TaskTrueBoolTaskBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create43Delegate(TaskTrueBoolTaskStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create44Delegate(TaskFalseBoolBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create45Delegate(TaskFalseBoolStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create46Delegate(TaskFalseBoolTaskBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create47Delegate(TaskFalseBoolTaskStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create48Delegate(RemoteVoidBool v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create49Delegate(RemoteVoidString v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create50Delegate(RemoteVoidTaskBool v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create51Delegate(RemoteVoidTaskString v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create52Delegate(RemoteTrueBoolBool v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create53Delegate(RemoteTrueBoolString v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create54Delegate(RemoteTrueBoolTaskBool v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create55Delegate(RemoteTrueBoolTaskString v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create56Delegate(RemoteFalseBoolBool v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create57Delegate(RemoteFalseBoolString v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create58Delegate(RemoteFalseBoolTaskBool v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create59Delegate(RemoteFalseBoolTaskString v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create60Delegate(RemoteTaskVoidBool v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create61Delegate(RemoteTaskVoidString v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create62Delegate(RemoteTaskVoidTaskBool v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create63Delegate(RemoteTaskVoidTaskString v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create64Delegate(RemoteTaskTrueBoolBool v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create65Delegate(RemoteTaskTrueBoolString v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create66Delegate(RemoteTaskTrueBoolTaskBool v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create67Delegate(RemoteTaskTrueBoolTaskString v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create68Delegate(RemoteTaskFalseBoolBool v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create69Delegate(RemoteTaskFalseBoolString v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create70Delegate(RemoteTaskFalseBoolTaskBool v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create71Delegate(RemoteTaskFalseBoolTaskString v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create72Delegate(RemoteVoidBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create73Delegate(RemoteVoidStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create74Delegate(RemoteVoidTaskBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create75Delegate(RemoteVoidTaskStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create76Delegate(RemoteTrueBoolBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create77Delegate(RemoteTrueBoolStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create78Delegate(RemoteTrueBoolTaskBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create79Delegate(RemoteTrueBoolTaskStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create80Delegate(RemoteFalseBoolBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create81Delegate(RemoteFalseBoolStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create82Delegate(RemoteFalseBoolTaskBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create83Delegate(RemoteFalseBoolTaskStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create84Delegate(RemoteTaskVoidBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create85Delegate(RemoteTaskVoidStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create86Delegate(RemoteTaskVoidTaskBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create87Delegate(RemoteTaskVoidTaskStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create88Delegate(RemoteTaskTrueBoolBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create89Delegate(RemoteTaskTrueBoolStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create90Delegate(RemoteTaskTrueBoolTaskBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create91Delegate(RemoteTaskTrueBoolTaskStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create92Delegate(RemoteTaskFalseBoolBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create93Delegate(RemoteTaskFalseBoolStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create94Delegate(RemoteTaskFalseBoolTaskBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create95Delegate(RemoteTaskFalseBoolTaskStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create120Delegate(VoidBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create121Delegate(VoidStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create122Delegate(VoidTaskBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create123Delegate(VoidTaskStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create124Delegate(TrueBoolBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create125Delegate(TrueBoolStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create126Delegate(TrueBoolTaskBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create127Delegate(TrueBoolTaskStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create128Delegate(FalseBoolBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create129Delegate(FalseBoolStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create130Delegate(FalseBoolTaskBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create131Delegate(FalseBoolTaskStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create132Delegate(TaskVoidBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create133Delegate(TaskVoidStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create134Delegate(TaskVoidTaskBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create135Delegate(TaskVoidTaskStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create136Delegate(TaskTrueBoolBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create137Delegate(TaskTrueBoolStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create138Delegate(TaskTrueBoolTaskBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create139Delegate(TaskTrueBoolTaskStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create140Delegate(TaskFalseBoolBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create141Delegate(TaskFalseBoolStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create142Delegate(TaskFalseBoolTaskBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create143Delegate(TaskFalseBoolTaskStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create144Delegate(RemoteVoidBoolDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create145Delegate(RemoteVoidStringDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create146Delegate(RemoteVoidTaskBoolDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create147Delegate(RemoteVoidTaskStringDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create148Delegate(RemoteTrueBoolBoolDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create149Delegate(RemoteTrueBoolStringDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create150Delegate(RemoteTrueBoolTaskBoolDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create151Delegate(RemoteTrueBoolTaskStringDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create152Delegate(RemoteFalseBoolBoolDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create153Delegate(RemoteFalseBoolStringDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create154Delegate(RemoteFalseBoolTaskBoolDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create155Delegate(RemoteFalseBoolTaskStringDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create156Delegate(RemoteTaskVoidBoolDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create157Delegate(RemoteTaskVoidStringDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create158Delegate(RemoteTaskVoidTaskBoolDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create159Delegate(RemoteTaskVoidTaskStringDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create160Delegate(RemoteTaskTrueBoolBoolDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create161Delegate(RemoteTaskTrueBoolStringDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create162Delegate(RemoteTaskTrueBoolTaskBoolDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create163Delegate(RemoteTaskTrueBoolTaskStringDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create164Delegate(RemoteTaskFalseBoolBoolDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create165Delegate(RemoteTaskFalseBoolStringDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create166Delegate(RemoteTaskFalseBoolTaskBoolDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create167Delegate(RemoteTaskFalseBoolTaskStringDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create168Delegate(RemoteVoidBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create169Delegate(RemoteVoidStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create170Delegate(RemoteVoidTaskBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create171Delegate(RemoteVoidTaskStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create172Delegate(RemoteTrueBoolBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create173Delegate(RemoteTrueBoolStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create174Delegate(RemoteTrueBoolTaskBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create175Delegate(RemoteTrueBoolTaskStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create176Delegate(RemoteFalseBoolBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create177Delegate(RemoteFalseBoolStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create178Delegate(RemoteFalseBoolTaskBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create179Delegate(RemoteFalseBoolTaskStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create180Delegate(RemoteTaskVoidBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create181Delegate(RemoteTaskVoidStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create182Delegate(RemoteTaskVoidTaskBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create183Delegate(RemoteTaskVoidTaskStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create184Delegate(RemoteTaskTrueBoolBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create185Delegate(RemoteTaskTrueBoolStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create186Delegate(RemoteTaskTrueBoolTaskBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create187Delegate(RemoteTaskTrueBoolTaskStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create188Delegate(RemoteTaskFalseBoolBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create189Delegate(RemoteTaskFalseBoolStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create190Delegate(RemoteTaskFalseBoolTaskBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Create191Delegate(RemoteTaskFalseBoolTaskStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save24Delegate(IAuthorizedAllCombinations target, VoidBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save25Delegate(IAuthorizedAllCombinations target, VoidStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save26Delegate(IAuthorizedAllCombinations target, VoidTaskBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save27Delegate(IAuthorizedAllCombinations target, VoidTaskStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save28Delegate(IAuthorizedAllCombinations target, TrueBoolBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save29Delegate(IAuthorizedAllCombinations target, TrueBoolStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save30Delegate(IAuthorizedAllCombinations target, TrueBoolTaskBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save31Delegate(IAuthorizedAllCombinations target, TrueBoolTaskStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save32Delegate(IAuthorizedAllCombinations target, FalseBoolBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save33Delegate(IAuthorizedAllCombinations target, FalseBoolStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save34Delegate(IAuthorizedAllCombinations target, FalseBoolTaskBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save35Delegate(IAuthorizedAllCombinations target, FalseBoolTaskStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save36Delegate(IAuthorizedAllCombinations target, TaskVoidBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save37Delegate(IAuthorizedAllCombinations target, TaskVoidStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save38Delegate(IAuthorizedAllCombinations target, TaskVoidTaskBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save39Delegate(IAuthorizedAllCombinations target, TaskVoidTaskStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save40Delegate(IAuthorizedAllCombinations target, TaskTrueBoolBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save41Delegate(IAuthorizedAllCombinations target, TaskTrueBoolStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save42Delegate(IAuthorizedAllCombinations target, TaskTrueBoolTaskBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save43Delegate(IAuthorizedAllCombinations target, TaskTrueBoolTaskStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save44Delegate(IAuthorizedAllCombinations target, TaskFalseBoolBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save45Delegate(IAuthorizedAllCombinations target, TaskFalseBoolStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save46Delegate(IAuthorizedAllCombinations target, TaskFalseBoolTaskBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save47Delegate(IAuthorizedAllCombinations target, TaskFalseBoolTaskStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save48Delegate(IAuthorizedAllCombinations target, RemoteVoidBool v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save49Delegate(IAuthorizedAllCombinations target, RemoteVoidString v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save50Delegate(IAuthorizedAllCombinations target, RemoteVoidTaskBool v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save51Delegate(IAuthorizedAllCombinations target, RemoteVoidTaskString v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save52Delegate(IAuthorizedAllCombinations target, RemoteTrueBoolBool v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save53Delegate(IAuthorizedAllCombinations target, RemoteTrueBoolString v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save54Delegate(IAuthorizedAllCombinations target, RemoteTrueBoolTaskBool v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save55Delegate(IAuthorizedAllCombinations target, RemoteTrueBoolTaskString v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save56Delegate(IAuthorizedAllCombinations target, RemoteFalseBoolBool v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save57Delegate(IAuthorizedAllCombinations target, RemoteFalseBoolString v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save58Delegate(IAuthorizedAllCombinations target, RemoteFalseBoolTaskBool v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save59Delegate(IAuthorizedAllCombinations target, RemoteFalseBoolTaskString v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save60Delegate(IAuthorizedAllCombinations target, RemoteTaskVoidBool v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save61Delegate(IAuthorizedAllCombinations target, RemoteTaskVoidString v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save62Delegate(IAuthorizedAllCombinations target, RemoteTaskVoidTaskBool v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save63Delegate(IAuthorizedAllCombinations target, RemoteTaskVoidTaskString v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save64Delegate(IAuthorizedAllCombinations target, RemoteTaskTrueBoolBool v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save65Delegate(IAuthorizedAllCombinations target, RemoteTaskTrueBoolString v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save66Delegate(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskBool v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save67Delegate(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskString v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save68Delegate(IAuthorizedAllCombinations target, RemoteTaskFalseBoolBool v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save69Delegate(IAuthorizedAllCombinations target, RemoteTaskFalseBoolString v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save70Delegate(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskBool v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save71Delegate(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskString v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save72Delegate(IAuthorizedAllCombinations target, RemoteVoidBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save73Delegate(IAuthorizedAllCombinations target, RemoteVoidStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save74Delegate(IAuthorizedAllCombinations target, RemoteVoidTaskBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save75Delegate(IAuthorizedAllCombinations target, RemoteVoidTaskStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save76Delegate(IAuthorizedAllCombinations target, RemoteTrueBoolBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save77Delegate(IAuthorizedAllCombinations target, RemoteTrueBoolStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save78Delegate(IAuthorizedAllCombinations target, RemoteTrueBoolTaskBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save79Delegate(IAuthorizedAllCombinations target, RemoteTrueBoolTaskStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save80Delegate(IAuthorizedAllCombinations target, RemoteFalseBoolBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save81Delegate(IAuthorizedAllCombinations target, RemoteFalseBoolStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save82Delegate(IAuthorizedAllCombinations target, RemoteFalseBoolTaskBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save83Delegate(IAuthorizedAllCombinations target, RemoteFalseBoolTaskStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save84Delegate(IAuthorizedAllCombinations target, RemoteTaskVoidBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save85Delegate(IAuthorizedAllCombinations target, RemoteTaskVoidStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save86Delegate(IAuthorizedAllCombinations target, RemoteTaskVoidTaskBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save87Delegate(IAuthorizedAllCombinations target, RemoteTaskVoidTaskStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save88Delegate(IAuthorizedAllCombinations target, RemoteTaskTrueBoolBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save89Delegate(IAuthorizedAllCombinations target, RemoteTaskTrueBoolStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save90Delegate(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save91Delegate(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save92Delegate(IAuthorizedAllCombinations target, RemoteTaskFalseBoolBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save93Delegate(IAuthorizedAllCombinations target, RemoteTaskFalseBoolStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save94Delegate(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskBoolRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save95Delegate(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskStringRemote v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save120Delegate(IAuthorizedAllCombinations target, VoidBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save121Delegate(IAuthorizedAllCombinations target, VoidStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save122Delegate(IAuthorizedAllCombinations target, VoidTaskBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save123Delegate(IAuthorizedAllCombinations target, VoidTaskStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save124Delegate(IAuthorizedAllCombinations target, TrueBoolBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save125Delegate(IAuthorizedAllCombinations target, TrueBoolStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save126Delegate(IAuthorizedAllCombinations target, TrueBoolTaskBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save127Delegate(IAuthorizedAllCombinations target, TrueBoolTaskStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save128Delegate(IAuthorizedAllCombinations target, FalseBoolBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save129Delegate(IAuthorizedAllCombinations target, FalseBoolStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save130Delegate(IAuthorizedAllCombinations target, FalseBoolTaskBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save131Delegate(IAuthorizedAllCombinations target, FalseBoolTaskStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save132Delegate(IAuthorizedAllCombinations target, TaskVoidBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save133Delegate(IAuthorizedAllCombinations target, TaskVoidStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save134Delegate(IAuthorizedAllCombinations target, TaskVoidTaskBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save135Delegate(IAuthorizedAllCombinations target, TaskVoidTaskStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save136Delegate(IAuthorizedAllCombinations target, TaskTrueBoolBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save137Delegate(IAuthorizedAllCombinations target, TaskTrueBoolStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save138Delegate(IAuthorizedAllCombinations target, TaskTrueBoolTaskBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save139Delegate(IAuthorizedAllCombinations target, TaskTrueBoolTaskStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save140Delegate(IAuthorizedAllCombinations target, TaskFalseBoolBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save141Delegate(IAuthorizedAllCombinations target, TaskFalseBoolStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save142Delegate(IAuthorizedAllCombinations target, TaskFalseBoolTaskBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save143Delegate(IAuthorizedAllCombinations target, TaskFalseBoolTaskStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save144Delegate(IAuthorizedAllCombinations target, RemoteVoidBoolDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save145Delegate(IAuthorizedAllCombinations target, RemoteVoidStringDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save146Delegate(IAuthorizedAllCombinations target, RemoteVoidTaskBoolDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save147Delegate(IAuthorizedAllCombinations target, RemoteVoidTaskStringDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save148Delegate(IAuthorizedAllCombinations target, RemoteTrueBoolBoolDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save149Delegate(IAuthorizedAllCombinations target, RemoteTrueBoolStringDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save150Delegate(IAuthorizedAllCombinations target, RemoteTrueBoolTaskBoolDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save151Delegate(IAuthorizedAllCombinations target, RemoteTrueBoolTaskStringDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save152Delegate(IAuthorizedAllCombinations target, RemoteFalseBoolBoolDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save153Delegate(IAuthorizedAllCombinations target, RemoteFalseBoolStringDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save154Delegate(IAuthorizedAllCombinations target, RemoteFalseBoolTaskBoolDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save155Delegate(IAuthorizedAllCombinations target, RemoteFalseBoolTaskStringDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save156Delegate(IAuthorizedAllCombinations target, RemoteTaskVoidBoolDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save157Delegate(IAuthorizedAllCombinations target, RemoteTaskVoidStringDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save158Delegate(IAuthorizedAllCombinations target, RemoteTaskVoidTaskBoolDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save159Delegate(IAuthorizedAllCombinations target, RemoteTaskVoidTaskStringDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save160Delegate(IAuthorizedAllCombinations target, RemoteTaskTrueBoolBoolDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save161Delegate(IAuthorizedAllCombinations target, RemoteTaskTrueBoolStringDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save162Delegate(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskBoolDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save163Delegate(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskStringDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save164Delegate(IAuthorizedAllCombinations target, RemoteTaskFalseBoolBoolDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save165Delegate(IAuthorizedAllCombinations target, RemoteTaskFalseBoolStringDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save166Delegate(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskBoolDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save167Delegate(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskStringDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save168Delegate(IAuthorizedAllCombinations target, RemoteVoidBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save169Delegate(IAuthorizedAllCombinations target, RemoteVoidStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save170Delegate(IAuthorizedAllCombinations target, RemoteVoidTaskBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save171Delegate(IAuthorizedAllCombinations target, RemoteVoidTaskStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save172Delegate(IAuthorizedAllCombinations target, RemoteTrueBoolBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save173Delegate(IAuthorizedAllCombinations target, RemoteTrueBoolStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save174Delegate(IAuthorizedAllCombinations target, RemoteTrueBoolTaskBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save175Delegate(IAuthorizedAllCombinations target, RemoteTrueBoolTaskStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save176Delegate(IAuthorizedAllCombinations target, RemoteFalseBoolBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save177Delegate(IAuthorizedAllCombinations target, RemoteFalseBoolStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save178Delegate(IAuthorizedAllCombinations target, RemoteFalseBoolTaskBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save179Delegate(IAuthorizedAllCombinations target, RemoteFalseBoolTaskStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save180Delegate(IAuthorizedAllCombinations target, RemoteTaskVoidBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save181Delegate(IAuthorizedAllCombinations target, RemoteTaskVoidStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save182Delegate(IAuthorizedAllCombinations target, RemoteTaskVoidTaskBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save183Delegate(IAuthorizedAllCombinations target, RemoteTaskVoidTaskStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save184Delegate(IAuthorizedAllCombinations target, RemoteTaskTrueBoolBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save185Delegate(IAuthorizedAllCombinations target, RemoteTaskTrueBoolStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save186Delegate(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save187Delegate(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save188Delegate(IAuthorizedAllCombinations target, RemoteTaskFalseBoolBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save189Delegate(IAuthorizedAllCombinations target, RemoteTaskFalseBoolStringRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save190Delegate(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskBoolRemoteDeny v);
        public delegate Task<Authorized<IAuthorizedAllCombinations>> Save191Delegate(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskStringRemoteDeny v);
        public AuthorizedAllCombinationsFactory(IServiceProvider serviceProvider, AuthorizationAllCombinations authorizationallcombinations)
        {
            this.ServiceProvider = serviceProvider;
            this.AuthorizationAllCombinations = authorizationallcombinations;
            Create24Property = LocalCreate24;
            Create25Property = LocalCreate25;
            Create26Property = LocalCreate26;
            Create27Property = LocalCreate27;
            Create28Property = LocalCreate28;
            Create29Property = LocalCreate29;
            Create30Property = LocalCreate30;
            Create31Property = LocalCreate31;
            Create32Property = LocalCreate32;
            Create33Property = LocalCreate33;
            Create34Property = LocalCreate34;
            Create35Property = LocalCreate35;
            Create36Property = LocalCreate36;
            Create37Property = LocalCreate37;
            Create38Property = LocalCreate38;
            Create39Property = LocalCreate39;
            Create40Property = LocalCreate40;
            Create41Property = LocalCreate41;
            Create42Property = LocalCreate42;
            Create43Property = LocalCreate43;
            Create44Property = LocalCreate44;
            Create45Property = LocalCreate45;
            Create46Property = LocalCreate46;
            Create47Property = LocalCreate47;
            Create48Property = LocalCreate48;
            Create49Property = LocalCreate49;
            Create50Property = LocalCreate50;
            Create51Property = LocalCreate51;
            Create52Property = LocalCreate52;
            Create53Property = LocalCreate53;
            Create54Property = LocalCreate54;
            Create55Property = LocalCreate55;
            Create56Property = LocalCreate56;
            Create57Property = LocalCreate57;
            Create58Property = LocalCreate58;
            Create59Property = LocalCreate59;
            Create60Property = LocalCreate60;
            Create61Property = LocalCreate61;
            Create62Property = LocalCreate62;
            Create63Property = LocalCreate63;
            Create64Property = LocalCreate64;
            Create65Property = LocalCreate65;
            Create66Property = LocalCreate66;
            Create67Property = LocalCreate67;
            Create68Property = LocalCreate68;
            Create69Property = LocalCreate69;
            Create70Property = LocalCreate70;
            Create71Property = LocalCreate71;
            Create72Property = LocalCreate72;
            Create73Property = LocalCreate73;
            Create74Property = LocalCreate74;
            Create75Property = LocalCreate75;
            Create76Property = LocalCreate76;
            Create77Property = LocalCreate77;
            Create78Property = LocalCreate78;
            Create79Property = LocalCreate79;
            Create80Property = LocalCreate80;
            Create81Property = LocalCreate81;
            Create82Property = LocalCreate82;
            Create83Property = LocalCreate83;
            Create84Property = LocalCreate84;
            Create85Property = LocalCreate85;
            Create86Property = LocalCreate86;
            Create87Property = LocalCreate87;
            Create88Property = LocalCreate88;
            Create89Property = LocalCreate89;
            Create90Property = LocalCreate90;
            Create91Property = LocalCreate91;
            Create92Property = LocalCreate92;
            Create93Property = LocalCreate93;
            Create94Property = LocalCreate94;
            Create95Property = LocalCreate95;
            Create120Property = LocalCreate120;
            Create121Property = LocalCreate121;
            Create122Property = LocalCreate122;
            Create123Property = LocalCreate123;
            Create124Property = LocalCreate124;
            Create125Property = LocalCreate125;
            Create126Property = LocalCreate126;
            Create127Property = LocalCreate127;
            Create128Property = LocalCreate128;
            Create129Property = LocalCreate129;
            Create130Property = LocalCreate130;
            Create131Property = LocalCreate131;
            Create132Property = LocalCreate132;
            Create133Property = LocalCreate133;
            Create134Property = LocalCreate134;
            Create135Property = LocalCreate135;
            Create136Property = LocalCreate136;
            Create137Property = LocalCreate137;
            Create138Property = LocalCreate138;
            Create139Property = LocalCreate139;
            Create140Property = LocalCreate140;
            Create141Property = LocalCreate141;
            Create142Property = LocalCreate142;
            Create143Property = LocalCreate143;
            Create144Property = LocalCreate144;
            Create145Property = LocalCreate145;
            Create146Property = LocalCreate146;
            Create147Property = LocalCreate147;
            Create148Property = LocalCreate148;
            Create149Property = LocalCreate149;
            Create150Property = LocalCreate150;
            Create151Property = LocalCreate151;
            Create152Property = LocalCreate152;
            Create153Property = LocalCreate153;
            Create154Property = LocalCreate154;
            Create155Property = LocalCreate155;
            Create156Property = LocalCreate156;
            Create157Property = LocalCreate157;
            Create158Property = LocalCreate158;
            Create159Property = LocalCreate159;
            Create160Property = LocalCreate160;
            Create161Property = LocalCreate161;
            Create162Property = LocalCreate162;
            Create163Property = LocalCreate163;
            Create164Property = LocalCreate164;
            Create165Property = LocalCreate165;
            Create166Property = LocalCreate166;
            Create167Property = LocalCreate167;
            Create168Property = LocalCreate168;
            Create169Property = LocalCreate169;
            Create170Property = LocalCreate170;
            Create171Property = LocalCreate171;
            Create172Property = LocalCreate172;
            Create173Property = LocalCreate173;
            Create174Property = LocalCreate174;
            Create175Property = LocalCreate175;
            Create176Property = LocalCreate176;
            Create177Property = LocalCreate177;
            Create178Property = LocalCreate178;
            Create179Property = LocalCreate179;
            Create180Property = LocalCreate180;
            Create181Property = LocalCreate181;
            Create182Property = LocalCreate182;
            Create183Property = LocalCreate183;
            Create184Property = LocalCreate184;
            Create185Property = LocalCreate185;
            Create186Property = LocalCreate186;
            Create187Property = LocalCreate187;
            Create188Property = LocalCreate188;
            Create189Property = LocalCreate189;
            Create190Property = LocalCreate190;
            Create191Property = LocalCreate191;
            Save24Property = LocalSave24;
            Save25Property = LocalSave25;
            Save26Property = LocalSave26;
            Save27Property = LocalSave27;
            Save28Property = LocalSave28;
            Save29Property = LocalSave29;
            Save30Property = LocalSave30;
            Save31Property = LocalSave31;
            Save32Property = LocalSave32;
            Save33Property = LocalSave33;
            Save34Property = LocalSave34;
            Save35Property = LocalSave35;
            Save36Property = LocalSave36;
            Save37Property = LocalSave37;
            Save38Property = LocalSave38;
            Save39Property = LocalSave39;
            Save40Property = LocalSave40;
            Save41Property = LocalSave41;
            Save42Property = LocalSave42;
            Save43Property = LocalSave43;
            Save44Property = LocalSave44;
            Save45Property = LocalSave45;
            Save46Property = LocalSave46;
            Save47Property = LocalSave47;
            Save48Property = LocalSave48;
            Save49Property = LocalSave49;
            Save50Property = LocalSave50;
            Save51Property = LocalSave51;
            Save52Property = LocalSave52;
            Save53Property = LocalSave53;
            Save54Property = LocalSave54;
            Save55Property = LocalSave55;
            Save56Property = LocalSave56;
            Save57Property = LocalSave57;
            Save58Property = LocalSave58;
            Save59Property = LocalSave59;
            Save60Property = LocalSave60;
            Save61Property = LocalSave61;
            Save62Property = LocalSave62;
            Save63Property = LocalSave63;
            Save64Property = LocalSave64;
            Save65Property = LocalSave65;
            Save66Property = LocalSave66;
            Save67Property = LocalSave67;
            Save68Property = LocalSave68;
            Save69Property = LocalSave69;
            Save70Property = LocalSave70;
            Save71Property = LocalSave71;
            Save72Property = LocalSave72;
            Save73Property = LocalSave73;
            Save74Property = LocalSave74;
            Save75Property = LocalSave75;
            Save76Property = LocalSave76;
            Save77Property = LocalSave77;
            Save78Property = LocalSave78;
            Save79Property = LocalSave79;
            Save80Property = LocalSave80;
            Save81Property = LocalSave81;
            Save82Property = LocalSave82;
            Save83Property = LocalSave83;
            Save84Property = LocalSave84;
            Save85Property = LocalSave85;
            Save86Property = LocalSave86;
            Save87Property = LocalSave87;
            Save88Property = LocalSave88;
            Save89Property = LocalSave89;
            Save90Property = LocalSave90;
            Save91Property = LocalSave91;
            Save92Property = LocalSave92;
            Save93Property = LocalSave93;
            Save94Property = LocalSave94;
            Save95Property = LocalSave95;
            Save120Property = LocalSave120;
            Save121Property = LocalSave121;
            Save122Property = LocalSave122;
            Save123Property = LocalSave123;
            Save124Property = LocalSave124;
            Save125Property = LocalSave125;
            Save126Property = LocalSave126;
            Save127Property = LocalSave127;
            Save128Property = LocalSave128;
            Save129Property = LocalSave129;
            Save130Property = LocalSave130;
            Save131Property = LocalSave131;
            Save132Property = LocalSave132;
            Save133Property = LocalSave133;
            Save134Property = LocalSave134;
            Save135Property = LocalSave135;
            Save136Property = LocalSave136;
            Save137Property = LocalSave137;
            Save138Property = LocalSave138;
            Save139Property = LocalSave139;
            Save140Property = LocalSave140;
            Save141Property = LocalSave141;
            Save142Property = LocalSave142;
            Save143Property = LocalSave143;
            Save144Property = LocalSave144;
            Save145Property = LocalSave145;
            Save146Property = LocalSave146;
            Save147Property = LocalSave147;
            Save148Property = LocalSave148;
            Save149Property = LocalSave149;
            Save150Property = LocalSave150;
            Save151Property = LocalSave151;
            Save152Property = LocalSave152;
            Save153Property = LocalSave153;
            Save154Property = LocalSave154;
            Save155Property = LocalSave155;
            Save156Property = LocalSave156;
            Save157Property = LocalSave157;
            Save158Property = LocalSave158;
            Save159Property = LocalSave159;
            Save160Property = LocalSave160;
            Save161Property = LocalSave161;
            Save162Property = LocalSave162;
            Save163Property = LocalSave163;
            Save164Property = LocalSave164;
            Save165Property = LocalSave165;
            Save166Property = LocalSave166;
            Save167Property = LocalSave167;
            Save168Property = LocalSave168;
            Save169Property = LocalSave169;
            Save170Property = LocalSave170;
            Save171Property = LocalSave171;
            Save172Property = LocalSave172;
            Save173Property = LocalSave173;
            Save174Property = LocalSave174;
            Save175Property = LocalSave175;
            Save176Property = LocalSave176;
            Save177Property = LocalSave177;
            Save178Property = LocalSave178;
            Save179Property = LocalSave179;
            Save180Property = LocalSave180;
            Save181Property = LocalSave181;
            Save182Property = LocalSave182;
            Save183Property = LocalSave183;
            Save184Property = LocalSave184;
            Save185Property = LocalSave185;
            Save186Property = LocalSave186;
            Save187Property = LocalSave187;
            Save188Property = LocalSave188;
            Save189Property = LocalSave189;
            Save190Property = LocalSave190;
            Save191Property = LocalSave191;
        }

        public AuthorizedAllCombinationsFactory(IServiceProvider serviceProvider, IDoRemoteRequest remoteMethodDelegate, AuthorizationAllCombinations authorizationallcombinations)
        {
            this.ServiceProvider = serviceProvider;
            this.DoRemoteRequest = remoteMethodDelegate;
            this.AuthorizationAllCombinations = authorizationallcombinations;
            Create24Property = RemoteCreate24;
            Create25Property = RemoteCreate25;
            Create26Property = RemoteCreate26;
            Create27Property = RemoteCreate27;
            Create28Property = RemoteCreate28;
            Create29Property = RemoteCreate29;
            Create30Property = RemoteCreate30;
            Create31Property = RemoteCreate31;
            Create32Property = RemoteCreate32;
            Create33Property = RemoteCreate33;
            Create34Property = RemoteCreate34;
            Create35Property = RemoteCreate35;
            Create36Property = RemoteCreate36;
            Create37Property = RemoteCreate37;
            Create38Property = RemoteCreate38;
            Create39Property = RemoteCreate39;
            Create40Property = RemoteCreate40;
            Create41Property = RemoteCreate41;
            Create42Property = RemoteCreate42;
            Create43Property = RemoteCreate43;
            Create44Property = RemoteCreate44;
            Create45Property = RemoteCreate45;
            Create46Property = RemoteCreate46;
            Create47Property = RemoteCreate47;
            Create48Property = RemoteCreate48;
            Create49Property = RemoteCreate49;
            Create50Property = RemoteCreate50;
            Create51Property = RemoteCreate51;
            Create52Property = RemoteCreate52;
            Create53Property = RemoteCreate53;
            Create54Property = RemoteCreate54;
            Create55Property = RemoteCreate55;
            Create56Property = RemoteCreate56;
            Create57Property = RemoteCreate57;
            Create58Property = RemoteCreate58;
            Create59Property = RemoteCreate59;
            Create60Property = RemoteCreate60;
            Create61Property = RemoteCreate61;
            Create62Property = RemoteCreate62;
            Create63Property = RemoteCreate63;
            Create64Property = RemoteCreate64;
            Create65Property = RemoteCreate65;
            Create66Property = RemoteCreate66;
            Create67Property = RemoteCreate67;
            Create68Property = RemoteCreate68;
            Create69Property = RemoteCreate69;
            Create70Property = RemoteCreate70;
            Create71Property = RemoteCreate71;
            Create72Property = RemoteCreate72;
            Create73Property = RemoteCreate73;
            Create74Property = RemoteCreate74;
            Create75Property = RemoteCreate75;
            Create76Property = RemoteCreate76;
            Create77Property = RemoteCreate77;
            Create78Property = RemoteCreate78;
            Create79Property = RemoteCreate79;
            Create80Property = RemoteCreate80;
            Create81Property = RemoteCreate81;
            Create82Property = RemoteCreate82;
            Create83Property = RemoteCreate83;
            Create84Property = RemoteCreate84;
            Create85Property = RemoteCreate85;
            Create86Property = RemoteCreate86;
            Create87Property = RemoteCreate87;
            Create88Property = RemoteCreate88;
            Create89Property = RemoteCreate89;
            Create90Property = RemoteCreate90;
            Create91Property = RemoteCreate91;
            Create92Property = RemoteCreate92;
            Create93Property = RemoteCreate93;
            Create94Property = RemoteCreate94;
            Create95Property = RemoteCreate95;
            Create120Property = RemoteCreate120;
            Create121Property = RemoteCreate121;
            Create122Property = RemoteCreate122;
            Create123Property = RemoteCreate123;
            Create124Property = RemoteCreate124;
            Create125Property = RemoteCreate125;
            Create126Property = RemoteCreate126;
            Create127Property = RemoteCreate127;
            Create128Property = RemoteCreate128;
            Create129Property = RemoteCreate129;
            Create130Property = RemoteCreate130;
            Create131Property = RemoteCreate131;
            Create132Property = RemoteCreate132;
            Create133Property = RemoteCreate133;
            Create134Property = RemoteCreate134;
            Create135Property = RemoteCreate135;
            Create136Property = RemoteCreate136;
            Create137Property = RemoteCreate137;
            Create138Property = RemoteCreate138;
            Create139Property = RemoteCreate139;
            Create140Property = RemoteCreate140;
            Create141Property = RemoteCreate141;
            Create142Property = RemoteCreate142;
            Create143Property = RemoteCreate143;
            Create144Property = RemoteCreate144;
            Create145Property = RemoteCreate145;
            Create146Property = RemoteCreate146;
            Create147Property = RemoteCreate147;
            Create148Property = RemoteCreate148;
            Create149Property = RemoteCreate149;
            Create150Property = RemoteCreate150;
            Create151Property = RemoteCreate151;
            Create152Property = RemoteCreate152;
            Create153Property = RemoteCreate153;
            Create154Property = RemoteCreate154;
            Create155Property = RemoteCreate155;
            Create156Property = RemoteCreate156;
            Create157Property = RemoteCreate157;
            Create158Property = RemoteCreate158;
            Create159Property = RemoteCreate159;
            Create160Property = RemoteCreate160;
            Create161Property = RemoteCreate161;
            Create162Property = RemoteCreate162;
            Create163Property = RemoteCreate163;
            Create164Property = RemoteCreate164;
            Create165Property = RemoteCreate165;
            Create166Property = RemoteCreate166;
            Create167Property = RemoteCreate167;
            Create168Property = RemoteCreate168;
            Create169Property = RemoteCreate169;
            Create170Property = RemoteCreate170;
            Create171Property = RemoteCreate171;
            Create172Property = RemoteCreate172;
            Create173Property = RemoteCreate173;
            Create174Property = RemoteCreate174;
            Create175Property = RemoteCreate175;
            Create176Property = RemoteCreate176;
            Create177Property = RemoteCreate177;
            Create178Property = RemoteCreate178;
            Create179Property = RemoteCreate179;
            Create180Property = RemoteCreate180;
            Create181Property = RemoteCreate181;
            Create182Property = RemoteCreate182;
            Create183Property = RemoteCreate183;
            Create184Property = RemoteCreate184;
            Create185Property = RemoteCreate185;
            Create186Property = RemoteCreate186;
            Create187Property = RemoteCreate187;
            Create188Property = RemoteCreate188;
            Create189Property = RemoteCreate189;
            Create190Property = RemoteCreate190;
            Create191Property = RemoteCreate191;
            Save24Property = RemoteSave24;
            Save25Property = RemoteSave25;
            Save26Property = RemoteSave26;
            Save27Property = RemoteSave27;
            Save28Property = RemoteSave28;
            Save29Property = RemoteSave29;
            Save30Property = RemoteSave30;
            Save31Property = RemoteSave31;
            Save32Property = RemoteSave32;
            Save33Property = RemoteSave33;
            Save34Property = RemoteSave34;
            Save35Property = RemoteSave35;
            Save36Property = RemoteSave36;
            Save37Property = RemoteSave37;
            Save38Property = RemoteSave38;
            Save39Property = RemoteSave39;
            Save40Property = RemoteSave40;
            Save41Property = RemoteSave41;
            Save42Property = RemoteSave42;
            Save43Property = RemoteSave43;
            Save44Property = RemoteSave44;
            Save45Property = RemoteSave45;
            Save46Property = RemoteSave46;
            Save47Property = RemoteSave47;
            Save48Property = RemoteSave48;
            Save49Property = RemoteSave49;
            Save50Property = RemoteSave50;
            Save51Property = RemoteSave51;
            Save52Property = RemoteSave52;
            Save53Property = RemoteSave53;
            Save54Property = RemoteSave54;
            Save55Property = RemoteSave55;
            Save56Property = RemoteSave56;
            Save57Property = RemoteSave57;
            Save58Property = RemoteSave58;
            Save59Property = RemoteSave59;
            Save60Property = RemoteSave60;
            Save61Property = RemoteSave61;
            Save62Property = RemoteSave62;
            Save63Property = RemoteSave63;
            Save64Property = RemoteSave64;
            Save65Property = RemoteSave65;
            Save66Property = RemoteSave66;
            Save67Property = RemoteSave67;
            Save68Property = RemoteSave68;
            Save69Property = RemoteSave69;
            Save70Property = RemoteSave70;
            Save71Property = RemoteSave71;
            Save72Property = RemoteSave72;
            Save73Property = RemoteSave73;
            Save74Property = RemoteSave74;
            Save75Property = RemoteSave75;
            Save76Property = RemoteSave76;
            Save77Property = RemoteSave77;
            Save78Property = RemoteSave78;
            Save79Property = RemoteSave79;
            Save80Property = RemoteSave80;
            Save81Property = RemoteSave81;
            Save82Property = RemoteSave82;
            Save83Property = RemoteSave83;
            Save84Property = RemoteSave84;
            Save85Property = RemoteSave85;
            Save86Property = RemoteSave86;
            Save87Property = RemoteSave87;
            Save88Property = RemoteSave88;
            Save89Property = RemoteSave89;
            Save90Property = RemoteSave90;
            Save91Property = RemoteSave91;
            Save92Property = RemoteSave92;
            Save93Property = RemoteSave93;
            Save94Property = RemoteSave94;
            Save95Property = RemoteSave95;
            Save120Property = RemoteSave120;
            Save121Property = RemoteSave121;
            Save122Property = RemoteSave122;
            Save123Property = RemoteSave123;
            Save124Property = RemoteSave124;
            Save125Property = RemoteSave125;
            Save126Property = RemoteSave126;
            Save127Property = RemoteSave127;
            Save128Property = RemoteSave128;
            Save129Property = RemoteSave129;
            Save130Property = RemoteSave130;
            Save131Property = RemoteSave131;
            Save132Property = RemoteSave132;
            Save133Property = RemoteSave133;
            Save134Property = RemoteSave134;
            Save135Property = RemoteSave135;
            Save136Property = RemoteSave136;
            Save137Property = RemoteSave137;
            Save138Property = RemoteSave138;
            Save139Property = RemoteSave139;
            Save140Property = RemoteSave140;
            Save141Property = RemoteSave141;
            Save142Property = RemoteSave142;
            Save143Property = RemoteSave143;
            Save144Property = RemoteSave144;
            Save145Property = RemoteSave145;
            Save146Property = RemoteSave146;
            Save147Property = RemoteSave147;
            Save148Property = RemoteSave148;
            Save149Property = RemoteSave149;
            Save150Property = RemoteSave150;
            Save151Property = RemoteSave151;
            Save152Property = RemoteSave152;
            Save153Property = RemoteSave153;
            Save154Property = RemoteSave154;
            Save155Property = RemoteSave155;
            Save156Property = RemoteSave156;
            Save157Property = RemoteSave157;
            Save158Property = RemoteSave158;
            Save159Property = RemoteSave159;
            Save160Property = RemoteSave160;
            Save161Property = RemoteSave161;
            Save162Property = RemoteSave162;
            Save163Property = RemoteSave163;
            Save164Property = RemoteSave164;
            Save165Property = RemoteSave165;
            Save166Property = RemoteSave166;
            Save167Property = RemoteSave167;
            Save168Property = RemoteSave168;
            Save169Property = RemoteSave169;
            Save170Property = RemoteSave170;
            Save171Property = RemoteSave171;
            Save172Property = RemoteSave172;
            Save173Property = RemoteSave173;
            Save174Property = RemoteSave174;
            Save175Property = RemoteSave175;
            Save176Property = RemoteSave176;
            Save177Property = RemoteSave177;
            Save178Property = RemoteSave178;
            Save179Property = RemoteSave179;
            Save180Property = RemoteSave180;
            Save181Property = RemoteSave181;
            Save182Property = RemoteSave182;
            Save183Property = RemoteSave183;
            Save184Property = RemoteSave184;
            Save185Property = RemoteSave185;
            Save186Property = RemoteSave186;
            Save187Property = RemoteSave187;
            Save188Property = RemoteSave188;
            Save189Property = RemoteSave189;
            Save190Property = RemoteSave190;
            Save191Property = RemoteSave191;
        }

        public IAuthorizedAllCombinations? Create(VoidBool v)
        {
            var authorized = (TryCreate(v));
            return authorized.Result;
        }

        public IAuthorizedAllCombinations? Create(VoidString v)
        {
            var authorized = (TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(VoidTaskBool v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(VoidTaskString v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public IAuthorizedAllCombinations? Create(TrueBoolBool v)
        {
            var authorized = (TryCreate(v));
            return authorized.Result;
        }

        public IAuthorizedAllCombinations? Create(TrueBoolString v)
        {
            var authorized = (TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(TrueBoolTaskBool v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(TrueBoolTaskString v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public IAuthorizedAllCombinations? Create(FalseBoolBool v)
        {
            var authorized = (TryCreate(v));
            return authorized.Result;
        }

        public IAuthorizedAllCombinations? Create(FalseBoolString v)
        {
            var authorized = (TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(FalseBoolTaskBool v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(FalseBoolTaskString v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskVoidBool v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskVoidString v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskVoidTaskBool v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskVoidTaskString v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolBool v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolString v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolTaskBool v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolTaskString v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolBool v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolString v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolTaskBool v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolTaskString v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(VoidBoolRemote v)
        {
            return Create24Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(VoidBoolRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(VoidStringRemote v)
        {
            return Create25Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(VoidStringRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(VoidTaskBoolRemote v)
        {
            return Create26Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(VoidTaskBoolRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(VoidTaskStringRemote v)
        {
            return Create27Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(VoidTaskStringRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TrueBoolBoolRemote v)
        {
            return Create28Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TrueBoolBoolRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TrueBoolStringRemote v)
        {
            return Create29Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TrueBoolStringRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TrueBoolTaskBoolRemote v)
        {
            return Create30Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TrueBoolTaskBoolRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TrueBoolTaskStringRemote v)
        {
            return Create31Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TrueBoolTaskStringRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(FalseBoolBoolRemote v)
        {
            return Create32Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(FalseBoolBoolRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(FalseBoolStringRemote v)
        {
            return Create33Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(FalseBoolStringRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(FalseBoolTaskBoolRemote v)
        {
            return Create34Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(FalseBoolTaskBoolRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(FalseBoolTaskStringRemote v)
        {
            return Create35Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(FalseBoolTaskStringRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidBoolRemote v)
        {
            return Create36Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskVoidBoolRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidStringRemote v)
        {
            return Create37Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskVoidStringRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidTaskBoolRemote v)
        {
            return Create38Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskVoidTaskBoolRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidTaskStringRemote v)
        {
            return Create39Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskVoidTaskStringRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolBoolRemote v)
        {
            return Create40Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolBoolRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolStringRemote v)
        {
            return Create41Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolStringRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolTaskBoolRemote v)
        {
            return Create42Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolTaskBoolRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolTaskStringRemote v)
        {
            return Create43Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolTaskStringRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolBoolRemote v)
        {
            return Create44Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolBoolRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolStringRemote v)
        {
            return Create45Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolStringRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolTaskBoolRemote v)
        {
            return Create46Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolTaskBoolRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolTaskStringRemote v)
        {
            return Create47Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolTaskStringRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidBool v)
        {
            return Create48Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteVoidBool v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidString v)
        {
            return Create49Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteVoidString v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidTaskBool v)
        {
            return Create50Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteVoidTaskBool v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidTaskString v)
        {
            return Create51Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteVoidTaskString v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolBool v)
        {
            return Create52Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolBool v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolString v)
        {
            return Create53Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolString v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolTaskBool v)
        {
            return Create54Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolTaskBool v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolTaskString v)
        {
            return Create55Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolTaskString v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolBool v)
        {
            return Create56Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolBool v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolString v)
        {
            return Create57Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolString v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolTaskBool v)
        {
            return Create58Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolTaskBool v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolTaskString v)
        {
            return Create59Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolTaskString v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidBool v)
        {
            return Create60Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidBool v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidString v)
        {
            return Create61Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidString v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidTaskBool v)
        {
            return Create62Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidTaskBool v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidTaskString v)
        {
            return Create63Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidTaskString v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolBool v)
        {
            return Create64Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolBool v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolString v)
        {
            return Create65Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolString v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolTaskBool v)
        {
            return Create66Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolTaskBool v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolTaskString v)
        {
            return Create67Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolTaskString v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolBool v)
        {
            return Create68Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolBool v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolString v)
        {
            return Create69Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolString v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolTaskBool v)
        {
            return Create70Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolTaskBool v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolTaskString v)
        {
            return Create71Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolTaskString v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidBoolRemote v)
        {
            return Create72Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteVoidBoolRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidStringRemote v)
        {
            return Create73Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteVoidStringRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidTaskBoolRemote v)
        {
            return Create74Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteVoidTaskBoolRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidTaskStringRemote v)
        {
            return Create75Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteVoidTaskStringRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolBoolRemote v)
        {
            return Create76Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolBoolRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolStringRemote v)
        {
            return Create77Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolStringRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolTaskBoolRemote v)
        {
            return Create78Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolTaskBoolRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolTaskStringRemote v)
        {
            return Create79Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolTaskStringRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolBoolRemote v)
        {
            return Create80Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolBoolRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolStringRemote v)
        {
            return Create81Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolStringRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolTaskBoolRemote v)
        {
            return Create82Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolTaskBoolRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolTaskStringRemote v)
        {
            return Create83Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolTaskStringRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidBoolRemote v)
        {
            return Create84Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidBoolRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidStringRemote v)
        {
            return Create85Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidStringRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidTaskBoolRemote v)
        {
            return Create86Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidTaskBoolRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidTaskStringRemote v)
        {
            return Create87Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidTaskStringRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolBoolRemote v)
        {
            return Create88Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolBoolRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolStringRemote v)
        {
            return Create89Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolStringRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolTaskBoolRemote v)
        {
            return Create90Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolTaskBoolRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolTaskStringRemote v)
        {
            return Create91Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolTaskStringRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolBoolRemote v)
        {
            return Create92Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolBoolRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolStringRemote v)
        {
            return Create93Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolStringRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolTaskBoolRemote v)
        {
            return Create94Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolTaskBoolRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolTaskStringRemote v)
        {
            return Create95Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolTaskStringRemote v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public IAuthorizedAllCombinations? Create(VoidBoolDeny v)
        {
            var authorized = (TryCreate(v));
            return authorized.Result;
        }

        public IAuthorizedAllCombinations? Create(VoidStringDeny v)
        {
            var authorized = (TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(VoidTaskBoolDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(VoidTaskStringDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public IAuthorizedAllCombinations? Create(TrueBoolBoolDeny v)
        {
            var authorized = (TryCreate(v));
            return authorized.Result;
        }

        public IAuthorizedAllCombinations? Create(TrueBoolStringDeny v)
        {
            var authorized = (TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(TrueBoolTaskBoolDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(TrueBoolTaskStringDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public IAuthorizedAllCombinations? Create(FalseBoolBoolDeny v)
        {
            var authorized = (TryCreate(v));
            return authorized.Result;
        }

        public IAuthorizedAllCombinations? Create(FalseBoolStringDeny v)
        {
            var authorized = (TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(FalseBoolTaskBoolDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(FalseBoolTaskStringDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskVoidBoolDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskVoidStringDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskVoidTaskBoolDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskVoidTaskStringDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolBoolDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolStringDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolTaskBoolDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolTaskStringDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolBoolDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolStringDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolTaskBoolDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolTaskStringDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(VoidBoolRemoteDeny v)
        {
            return Create120Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(VoidBoolRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(VoidStringRemoteDeny v)
        {
            return Create121Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(VoidStringRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(VoidTaskBoolRemoteDeny v)
        {
            return Create122Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(VoidTaskBoolRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(VoidTaskStringRemoteDeny v)
        {
            return Create123Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(VoidTaskStringRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TrueBoolBoolRemoteDeny v)
        {
            return Create124Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TrueBoolBoolRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TrueBoolStringRemoteDeny v)
        {
            return Create125Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TrueBoolStringRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TrueBoolTaskBoolRemoteDeny v)
        {
            return Create126Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TrueBoolTaskBoolRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TrueBoolTaskStringRemoteDeny v)
        {
            return Create127Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TrueBoolTaskStringRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(FalseBoolBoolRemoteDeny v)
        {
            return Create128Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(FalseBoolBoolRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(FalseBoolStringRemoteDeny v)
        {
            return Create129Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(FalseBoolStringRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(FalseBoolTaskBoolRemoteDeny v)
        {
            return Create130Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(FalseBoolTaskBoolRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(FalseBoolTaskStringRemoteDeny v)
        {
            return Create131Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(FalseBoolTaskStringRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidBoolRemoteDeny v)
        {
            return Create132Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskVoidBoolRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidStringRemoteDeny v)
        {
            return Create133Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskVoidStringRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidTaskBoolRemoteDeny v)
        {
            return Create134Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskVoidTaskBoolRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidTaskStringRemoteDeny v)
        {
            return Create135Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskVoidTaskStringRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolBoolRemoteDeny v)
        {
            return Create136Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolBoolRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolStringRemoteDeny v)
        {
            return Create137Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolStringRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolTaskBoolRemoteDeny v)
        {
            return Create138Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolTaskBoolRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolTaskStringRemoteDeny v)
        {
            return Create139Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskTrueBoolTaskStringRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolBoolRemoteDeny v)
        {
            return Create140Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolBoolRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolStringRemoteDeny v)
        {
            return Create141Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolStringRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolTaskBoolRemoteDeny v)
        {
            return Create142Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolTaskBoolRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolTaskStringRemoteDeny v)
        {
            return Create143Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(TaskFalseBoolTaskStringRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidBoolDeny v)
        {
            return Create144Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteVoidBoolDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidStringDeny v)
        {
            return Create145Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteVoidStringDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidTaskBoolDeny v)
        {
            return Create146Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteVoidTaskBoolDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidTaskStringDeny v)
        {
            return Create147Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteVoidTaskStringDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolBoolDeny v)
        {
            return Create148Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolBoolDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolStringDeny v)
        {
            return Create149Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolStringDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolTaskBoolDeny v)
        {
            return Create150Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolTaskBoolDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolTaskStringDeny v)
        {
            return Create151Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolTaskStringDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolBoolDeny v)
        {
            return Create152Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolBoolDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolStringDeny v)
        {
            return Create153Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolStringDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolTaskBoolDeny v)
        {
            return Create154Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolTaskBoolDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolTaskStringDeny v)
        {
            return Create155Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolTaskStringDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidBoolDeny v)
        {
            return Create156Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidBoolDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidStringDeny v)
        {
            return Create157Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidStringDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidTaskBoolDeny v)
        {
            return Create158Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidTaskBoolDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidTaskStringDeny v)
        {
            return Create159Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidTaskStringDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolBoolDeny v)
        {
            return Create160Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolBoolDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolStringDeny v)
        {
            return Create161Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolStringDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolTaskBoolDeny v)
        {
            return Create162Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolTaskBoolDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolTaskStringDeny v)
        {
            return Create163Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolTaskStringDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolBoolDeny v)
        {
            return Create164Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolBoolDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolStringDeny v)
        {
            return Create165Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolStringDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolTaskBoolDeny v)
        {
            return Create166Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolTaskBoolDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolTaskStringDeny v)
        {
            return Create167Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolTaskStringDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidBoolRemoteDeny v)
        {
            return Create168Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteVoidBoolRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidStringRemoteDeny v)
        {
            return Create169Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteVoidStringRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidTaskBoolRemoteDeny v)
        {
            return Create170Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteVoidTaskBoolRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteVoidTaskStringRemoteDeny v)
        {
            return Create171Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteVoidTaskStringRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolBoolRemoteDeny v)
        {
            return Create172Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolBoolRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolStringRemoteDeny v)
        {
            return Create173Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolStringRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolTaskBoolRemoteDeny v)
        {
            return Create174Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolTaskBoolRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTrueBoolTaskStringRemoteDeny v)
        {
            return Create175Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTrueBoolTaskStringRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolBoolRemoteDeny v)
        {
            return Create176Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolBoolRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolStringRemoteDeny v)
        {
            return Create177Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolStringRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolTaskBoolRemoteDeny v)
        {
            return Create178Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolTaskBoolRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteFalseBoolTaskStringRemoteDeny v)
        {
            return Create179Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteFalseBoolTaskStringRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidBoolRemoteDeny v)
        {
            return Create180Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidBoolRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidStringRemoteDeny v)
        {
            return Create181Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidStringRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidTaskBoolRemoteDeny v)
        {
            return Create182Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidTaskBoolRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskVoidTaskStringRemoteDeny v)
        {
            return Create183Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskVoidTaskStringRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolBoolRemoteDeny v)
        {
            return Create184Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolBoolRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolStringRemoteDeny v)
        {
            return Create185Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolStringRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolTaskBoolRemoteDeny v)
        {
            return Create186Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolTaskBoolRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskTrueBoolTaskStringRemoteDeny v)
        {
            return Create187Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskTrueBoolTaskStringRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolBoolRemoteDeny v)
        {
            return Create188Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolBoolRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolStringRemoteDeny v)
        {
            return Create189Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolStringRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolTaskBoolRemoteDeny v)
        {
            return Create190Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolTaskBoolRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> TryCreate(RemoteTaskFalseBoolTaskStringRemoteDeny v)
        {
            return Create191Property(v);
        }

        public async Task<IAuthorizedAllCombinations?> Create(RemoteTaskFalseBoolTaskStringRemoteDeny v)
        {
            var authorized = (await TryCreate(v));
            return authorized.Result;
        }

        public Authorized<IAuthorizedAllCombinations> TryCreate(VoidBool v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Authorized<IAuthorizedAllCombinations> TryCreate(VoidString v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(VoidTaskBool v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(VoidTaskString v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Authorized<IAuthorizedAllCombinations> TryCreate(TrueBoolBool v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Authorized<IAuthorizedAllCombinations> TryCreate(TrueBoolString v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TrueBoolTaskBool v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TrueBoolTaskString v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Authorized<IAuthorizedAllCombinations> TryCreate(FalseBoolBool v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Authorized<IAuthorizedAllCombinations> TryCreate(FalseBoolString v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(FalseBoolTaskBool v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(FalseBoolTaskString v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidBool v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidString v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidTaskBool v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidTaskString v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolBool v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolString v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolTaskBool v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolTaskString v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolBool v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolString v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolTaskBool v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolTaskString v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate24(VoidBoolRemote v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate25(VoidStringRemote v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate26(VoidTaskBoolRemote v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate27(VoidTaskStringRemote v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate28(TrueBoolBoolRemote v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate29(TrueBoolStringRemote v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate30(TrueBoolTaskBoolRemote v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate31(TrueBoolTaskStringRemote v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate32(FalseBoolBoolRemote v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate33(FalseBoolStringRemote v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate34(FalseBoolTaskBoolRemote v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate35(FalseBoolTaskStringRemote v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate36(TaskVoidBoolRemote v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate37(TaskVoidStringRemote v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate38(TaskVoidTaskBoolRemote v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate39(TaskVoidTaskStringRemote v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate40(TaskTrueBoolBoolRemote v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate41(TaskTrueBoolStringRemote v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate42(TaskTrueBoolTaskBoolRemote v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate43(TaskTrueBoolTaskStringRemote v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate44(TaskFalseBoolBoolRemote v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate45(TaskFalseBoolStringRemote v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate46(TaskFalseBoolTaskBoolRemote v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate47(TaskFalseBoolTaskStringRemote v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate48(RemoteVoidBool v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate49(RemoteVoidString v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate50(RemoteVoidTaskBool v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate51(RemoteVoidTaskString v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate52(RemoteTrueBoolBool v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate53(RemoteTrueBoolString v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate54(RemoteTrueBoolTaskBool v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate55(RemoteTrueBoolTaskString v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate56(RemoteFalseBoolBool v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate57(RemoteFalseBoolString v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate58(RemoteFalseBoolTaskBool v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate59(RemoteFalseBoolTaskString v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate60(RemoteTaskVoidBool v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate61(RemoteTaskVoidString v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate62(RemoteTaskVoidTaskBool v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate63(RemoteTaskVoidTaskString v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate64(RemoteTaskTrueBoolBool v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate65(RemoteTaskTrueBoolString v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate66(RemoteTaskTrueBoolTaskBool v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate67(RemoteTaskTrueBoolTaskString v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate68(RemoteTaskFalseBoolBool v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate69(RemoteTaskFalseBoolString v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate70(RemoteTaskFalseBoolTaskBool v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate71(RemoteTaskFalseBoolTaskString v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate72(RemoteVoidBoolRemote v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate73(RemoteVoidStringRemote v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate74(RemoteVoidTaskBoolRemote v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate75(RemoteVoidTaskStringRemote v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate76(RemoteTrueBoolBoolRemote v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate77(RemoteTrueBoolStringRemote v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate78(RemoteTrueBoolTaskBoolRemote v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate79(RemoteTrueBoolTaskStringRemote v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate80(RemoteFalseBoolBoolRemote v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate81(RemoteFalseBoolStringRemote v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate82(RemoteFalseBoolTaskBoolRemote v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate83(RemoteFalseBoolTaskStringRemote v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate84(RemoteTaskVoidBoolRemote v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate85(RemoteTaskVoidStringRemote v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate86(RemoteTaskVoidTaskBoolRemote v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate87(RemoteTaskVoidTaskStringRemote v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate88(RemoteTaskTrueBoolBoolRemote v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate89(RemoteTaskTrueBoolStringRemote v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate90(RemoteTaskTrueBoolTaskBoolRemote v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate91(RemoteTaskTrueBoolTaskStringRemote v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate92(RemoteTaskFalseBoolBoolRemote v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate93(RemoteTaskFalseBoolStringRemote v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate94(RemoteTaskFalseBoolTaskBoolRemote v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate95(RemoteTaskFalseBoolTaskStringRemote v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Authorized<IAuthorizedAllCombinations> TryCreate(VoidBoolDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Authorized<IAuthorizedAllCombinations> TryCreate(VoidStringDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(VoidTaskBoolDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(VoidTaskStringDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Authorized<IAuthorizedAllCombinations> TryCreate(TrueBoolBoolDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Authorized<IAuthorizedAllCombinations> TryCreate(TrueBoolStringDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TrueBoolTaskBoolDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TrueBoolTaskStringDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Authorized<IAuthorizedAllCombinations> TryCreate(FalseBoolBoolDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Authorized<IAuthorizedAllCombinations> TryCreate(FalseBoolStringDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(FalseBoolTaskBoolDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(FalseBoolTaskStringDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidBoolDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidStringDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidTaskBoolDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskVoidTaskStringDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolBoolDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolStringDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolTaskBoolDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskTrueBoolTaskStringDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolBoolDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolStringDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolTaskBoolDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> TryCreate(TaskFalseBoolTaskStringDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate120(VoidBoolRemoteDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate121(VoidStringRemoteDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate122(VoidTaskBoolRemoteDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate123(VoidTaskStringRemoteDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate124(TrueBoolBoolRemoteDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate125(TrueBoolStringRemoteDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate126(TrueBoolTaskBoolRemoteDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate127(TrueBoolTaskStringRemoteDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate128(FalseBoolBoolRemoteDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate129(FalseBoolStringRemoteDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate130(FalseBoolTaskBoolRemoteDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate131(FalseBoolTaskStringRemoteDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate132(TaskVoidBoolRemoteDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate133(TaskVoidStringRemoteDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate134(TaskVoidTaskBoolRemoteDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate135(TaskVoidTaskStringRemoteDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate136(TaskTrueBoolBoolRemoteDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate137(TaskTrueBoolStringRemoteDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate138(TaskTrueBoolTaskBoolRemoteDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate139(TaskTrueBoolTaskStringRemoteDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate140(TaskFalseBoolBoolRemoteDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate141(TaskFalseBoolStringRemoteDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate142(TaskFalseBoolTaskBoolRemoteDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate143(TaskFalseBoolTaskStringRemoteDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate144(RemoteVoidBoolDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate145(RemoteVoidStringDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate146(RemoteVoidTaskBoolDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate147(RemoteVoidTaskStringDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate148(RemoteTrueBoolBoolDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate149(RemoteTrueBoolStringDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate150(RemoteTrueBoolTaskBoolDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate151(RemoteTrueBoolTaskStringDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate152(RemoteFalseBoolBoolDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate153(RemoteFalseBoolStringDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate154(RemoteFalseBoolTaskBoolDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate155(RemoteFalseBoolTaskStringDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate156(RemoteTaskVoidBoolDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate157(RemoteTaskVoidStringDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate158(RemoteTaskVoidTaskBoolDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate159(RemoteTaskVoidTaskStringDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate160(RemoteTaskTrueBoolBoolDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate161(RemoteTaskTrueBoolStringDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate162(RemoteTaskTrueBoolTaskBoolDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate163(RemoteTaskTrueBoolTaskStringDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate164(RemoteTaskFalseBoolBoolDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate165(RemoteTaskFalseBoolStringDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate166(RemoteTaskFalseBoolTaskBoolDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate167(RemoteTaskFalseBoolTaskStringDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate168(RemoteVoidBoolRemoteDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate169(RemoteVoidStringRemoteDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate170(RemoteVoidTaskBoolRemoteDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate171(RemoteVoidTaskStringRemoteDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate172(RemoteTrueBoolBoolRemoteDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate173(RemoteTrueBoolStringRemoteDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate174(RemoteTrueBoolTaskBoolRemoteDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate175(RemoteTrueBoolTaskStringRemoteDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate176(RemoteFalseBoolBoolRemoteDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public Task<Authorized<IAuthorizedAllCombinations>> LocalCreate177(RemoteFalseBoolStringRemoteDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(read));
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v))));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate178(RemoteFalseBoolTaskBoolRemoteDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate179(RemoteFalseBoolTaskStringRemoteDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate180(RemoteTaskVoidBoolRemoteDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate181(RemoteTaskVoidStringRemoteDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate182(RemoteTaskVoidTaskBoolRemoteDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate183(RemoteTaskVoidTaskStringRemoteDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate184(RemoteTaskTrueBoolBoolRemoteDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate185(RemoteTaskTrueBoolStringRemoteDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate186(RemoteTaskTrueBoolTaskBoolRemoteDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate187(RemoteTaskTrueBoolTaskStringRemoteDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate188(RemoteTaskFalseBoolBoolRemoteDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate189(RemoteTaskFalseBoolStringRemoteDeny v)
        {
            Authorized read = AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate190(RemoteTaskFalseBoolTaskBoolRemoteDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> LocalCreate191(RemoteTaskFalseBoolTaskStringRemoteDeny v)
        {
            Authorized read = await AuthorizationAllCombinations.Read(v);
            if (!read.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(read);
            }

            var target = ServiceProvider.GetRequiredService<AuthorizedAllCombinations>();
            return new Authorized<IAuthorizedAllCombinations>(await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Create, () => target.Create(v)));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert(IAuthorizedAllCombinations itarget, VoidBool v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert1(IAuthorizedAllCombinations itarget, VoidString v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert2(IAuthorizedAllCombinations itarget, VoidTaskBool v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert3(IAuthorizedAllCombinations itarget, VoidTaskString v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert4(IAuthorizedAllCombinations itarget, TrueBoolBool v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert5(IAuthorizedAllCombinations itarget, TrueBoolString v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert6(IAuthorizedAllCombinations itarget, TrueBoolTaskBool v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert7(IAuthorizedAllCombinations itarget, TrueBoolTaskString v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert8(IAuthorizedAllCombinations itarget, FalseBoolBool v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert9(IAuthorizedAllCombinations itarget, FalseBoolString v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert10(IAuthorizedAllCombinations itarget, FalseBoolTaskBool v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert11(IAuthorizedAllCombinations itarget, FalseBoolTaskString v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert12(IAuthorizedAllCombinations itarget, TaskVoidBool v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert13(IAuthorizedAllCombinations itarget, TaskVoidString v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert14(IAuthorizedAllCombinations itarget, TaskVoidTaskBool v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert15(IAuthorizedAllCombinations itarget, TaskVoidTaskString v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert16(IAuthorizedAllCombinations itarget, TaskTrueBoolBool v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert17(IAuthorizedAllCombinations itarget, TaskTrueBoolString v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert18(IAuthorizedAllCombinations itarget, TaskTrueBoolTaskBool v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert19(IAuthorizedAllCombinations itarget, TaskTrueBoolTaskString v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert20(IAuthorizedAllCombinations itarget, TaskFalseBoolBool v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert21(IAuthorizedAllCombinations itarget, TaskFalseBoolString v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert22(IAuthorizedAllCombinations itarget, TaskFalseBoolTaskBool v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert23(IAuthorizedAllCombinations itarget, TaskFalseBoolTaskString v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert24(IAuthorizedAllCombinations itarget, VoidBoolRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert25(IAuthorizedAllCombinations itarget, VoidStringRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert26(IAuthorizedAllCombinations itarget, VoidTaskBoolRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert27(IAuthorizedAllCombinations itarget, VoidTaskStringRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert28(IAuthorizedAllCombinations itarget, TrueBoolBoolRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert29(IAuthorizedAllCombinations itarget, TrueBoolStringRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert30(IAuthorizedAllCombinations itarget, TrueBoolTaskBoolRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert31(IAuthorizedAllCombinations itarget, TrueBoolTaskStringRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert32(IAuthorizedAllCombinations itarget, FalseBoolBoolRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert33(IAuthorizedAllCombinations itarget, FalseBoolStringRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert34(IAuthorizedAllCombinations itarget, FalseBoolTaskBoolRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert35(IAuthorizedAllCombinations itarget, FalseBoolTaskStringRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert36(IAuthorizedAllCombinations itarget, TaskVoidBoolRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert37(IAuthorizedAllCombinations itarget, TaskVoidStringRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert38(IAuthorizedAllCombinations itarget, TaskVoidTaskBoolRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert39(IAuthorizedAllCombinations itarget, TaskVoidTaskStringRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert40(IAuthorizedAllCombinations itarget, TaskTrueBoolBoolRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert41(IAuthorizedAllCombinations itarget, TaskTrueBoolStringRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert42(IAuthorizedAllCombinations itarget, TaskTrueBoolTaskBoolRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert43(IAuthorizedAllCombinations itarget, TaskTrueBoolTaskStringRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert44(IAuthorizedAllCombinations itarget, TaskFalseBoolBoolRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert45(IAuthorizedAllCombinations itarget, TaskFalseBoolStringRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert46(IAuthorizedAllCombinations itarget, TaskFalseBoolTaskBoolRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert47(IAuthorizedAllCombinations itarget, TaskFalseBoolTaskStringRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert48(IAuthorizedAllCombinations itarget, RemoteVoidBool v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert49(IAuthorizedAllCombinations itarget, RemoteVoidString v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert50(IAuthorizedAllCombinations itarget, RemoteVoidTaskBool v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert51(IAuthorizedAllCombinations itarget, RemoteVoidTaskString v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert52(IAuthorizedAllCombinations itarget, RemoteTrueBoolBool v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert53(IAuthorizedAllCombinations itarget, RemoteTrueBoolString v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert54(IAuthorizedAllCombinations itarget, RemoteTrueBoolTaskBool v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert55(IAuthorizedAllCombinations itarget, RemoteTrueBoolTaskString v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert56(IAuthorizedAllCombinations itarget, RemoteFalseBoolBool v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert57(IAuthorizedAllCombinations itarget, RemoteFalseBoolString v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert58(IAuthorizedAllCombinations itarget, RemoteFalseBoolTaskBool v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert59(IAuthorizedAllCombinations itarget, RemoteFalseBoolTaskString v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert60(IAuthorizedAllCombinations itarget, RemoteTaskVoidBool v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert61(IAuthorizedAllCombinations itarget, RemoteTaskVoidString v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert62(IAuthorizedAllCombinations itarget, RemoteTaskVoidTaskBool v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert63(IAuthorizedAllCombinations itarget, RemoteTaskVoidTaskString v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert64(IAuthorizedAllCombinations itarget, RemoteTaskTrueBoolBool v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert65(IAuthorizedAllCombinations itarget, RemoteTaskTrueBoolString v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert66(IAuthorizedAllCombinations itarget, RemoteTaskTrueBoolTaskBool v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert67(IAuthorizedAllCombinations itarget, RemoteTaskTrueBoolTaskString v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert68(IAuthorizedAllCombinations itarget, RemoteTaskFalseBoolBool v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert69(IAuthorizedAllCombinations itarget, RemoteTaskFalseBoolString v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert70(IAuthorizedAllCombinations itarget, RemoteTaskFalseBoolTaskBool v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert71(IAuthorizedAllCombinations itarget, RemoteTaskFalseBoolTaskString v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert72(IAuthorizedAllCombinations itarget, RemoteVoidBoolRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert73(IAuthorizedAllCombinations itarget, RemoteVoidStringRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert74(IAuthorizedAllCombinations itarget, RemoteVoidTaskBoolRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert75(IAuthorizedAllCombinations itarget, RemoteVoidTaskStringRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert76(IAuthorizedAllCombinations itarget, RemoteTrueBoolBoolRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert77(IAuthorizedAllCombinations itarget, RemoteTrueBoolStringRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert78(IAuthorizedAllCombinations itarget, RemoteTrueBoolTaskBoolRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert79(IAuthorizedAllCombinations itarget, RemoteTrueBoolTaskStringRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert80(IAuthorizedAllCombinations itarget, RemoteFalseBoolBoolRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert81(IAuthorizedAllCombinations itarget, RemoteFalseBoolStringRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert82(IAuthorizedAllCombinations itarget, RemoteFalseBoolTaskBoolRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert83(IAuthorizedAllCombinations itarget, RemoteFalseBoolTaskStringRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert84(IAuthorizedAllCombinations itarget, RemoteTaskVoidBoolRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert85(IAuthorizedAllCombinations itarget, RemoteTaskVoidStringRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert86(IAuthorizedAllCombinations itarget, RemoteTaskVoidTaskBoolRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert87(IAuthorizedAllCombinations itarget, RemoteTaskVoidTaskStringRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert88(IAuthorizedAllCombinations itarget, RemoteTaskTrueBoolBoolRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert89(IAuthorizedAllCombinations itarget, RemoteTaskTrueBoolStringRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert90(IAuthorizedAllCombinations itarget, RemoteTaskTrueBoolTaskBoolRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert91(IAuthorizedAllCombinations itarget, RemoteTaskTrueBoolTaskStringRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert92(IAuthorizedAllCombinations itarget, RemoteTaskFalseBoolBoolRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert93(IAuthorizedAllCombinations itarget, RemoteTaskFalseBoolStringRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert94(IAuthorizedAllCombinations itarget, RemoteTaskFalseBoolTaskBoolRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert95(IAuthorizedAllCombinations itarget, RemoteTaskFalseBoolTaskStringRemote v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert96(IAuthorizedAllCombinations itarget, VoidBoolDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert97(IAuthorizedAllCombinations itarget, VoidStringDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert98(IAuthorizedAllCombinations itarget, VoidTaskBoolDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert99(IAuthorizedAllCombinations itarget, VoidTaskStringDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert100(IAuthorizedAllCombinations itarget, TrueBoolBoolDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert101(IAuthorizedAllCombinations itarget, TrueBoolStringDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert102(IAuthorizedAllCombinations itarget, TrueBoolTaskBoolDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert103(IAuthorizedAllCombinations itarget, TrueBoolTaskStringDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert104(IAuthorizedAllCombinations itarget, FalseBoolBoolDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert105(IAuthorizedAllCombinations itarget, FalseBoolStringDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert106(IAuthorizedAllCombinations itarget, FalseBoolTaskBoolDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert107(IAuthorizedAllCombinations itarget, FalseBoolTaskStringDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert108(IAuthorizedAllCombinations itarget, TaskVoidBoolDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert109(IAuthorizedAllCombinations itarget, TaskVoidStringDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert110(IAuthorizedAllCombinations itarget, TaskVoidTaskBoolDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert111(IAuthorizedAllCombinations itarget, TaskVoidTaskStringDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert112(IAuthorizedAllCombinations itarget, TaskTrueBoolBoolDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert113(IAuthorizedAllCombinations itarget, TaskTrueBoolStringDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert114(IAuthorizedAllCombinations itarget, TaskTrueBoolTaskBoolDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert115(IAuthorizedAllCombinations itarget, TaskTrueBoolTaskStringDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert116(IAuthorizedAllCombinations itarget, TaskFalseBoolBoolDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert117(IAuthorizedAllCombinations itarget, TaskFalseBoolStringDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert118(IAuthorizedAllCombinations itarget, TaskFalseBoolTaskBoolDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert119(IAuthorizedAllCombinations itarget, TaskFalseBoolTaskStringDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert120(IAuthorizedAllCombinations itarget, VoidBoolRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert121(IAuthorizedAllCombinations itarget, VoidStringRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert122(IAuthorizedAllCombinations itarget, VoidTaskBoolRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert123(IAuthorizedAllCombinations itarget, VoidTaskStringRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert124(IAuthorizedAllCombinations itarget, TrueBoolBoolRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert125(IAuthorizedAllCombinations itarget, TrueBoolStringRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert126(IAuthorizedAllCombinations itarget, TrueBoolTaskBoolRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert127(IAuthorizedAllCombinations itarget, TrueBoolTaskStringRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert128(IAuthorizedAllCombinations itarget, FalseBoolBoolRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert129(IAuthorizedAllCombinations itarget, FalseBoolStringRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert130(IAuthorizedAllCombinations itarget, FalseBoolTaskBoolRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert131(IAuthorizedAllCombinations itarget, FalseBoolTaskStringRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert132(IAuthorizedAllCombinations itarget, TaskVoidBoolRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert133(IAuthorizedAllCombinations itarget, TaskVoidStringRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert134(IAuthorizedAllCombinations itarget, TaskVoidTaskBoolRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert135(IAuthorizedAllCombinations itarget, TaskVoidTaskStringRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert136(IAuthorizedAllCombinations itarget, TaskTrueBoolBoolRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert137(IAuthorizedAllCombinations itarget, TaskTrueBoolStringRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert138(IAuthorizedAllCombinations itarget, TaskTrueBoolTaskBoolRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert139(IAuthorizedAllCombinations itarget, TaskTrueBoolTaskStringRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert140(IAuthorizedAllCombinations itarget, TaskFalseBoolBoolRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert141(IAuthorizedAllCombinations itarget, TaskFalseBoolStringRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert142(IAuthorizedAllCombinations itarget, TaskFalseBoolTaskBoolRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert143(IAuthorizedAllCombinations itarget, TaskFalseBoolTaskStringRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert144(IAuthorizedAllCombinations itarget, RemoteVoidBoolDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert145(IAuthorizedAllCombinations itarget, RemoteVoidStringDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert146(IAuthorizedAllCombinations itarget, RemoteVoidTaskBoolDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert147(IAuthorizedAllCombinations itarget, RemoteVoidTaskStringDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert148(IAuthorizedAllCombinations itarget, RemoteTrueBoolBoolDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert149(IAuthorizedAllCombinations itarget, RemoteTrueBoolStringDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert150(IAuthorizedAllCombinations itarget, RemoteTrueBoolTaskBoolDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert151(IAuthorizedAllCombinations itarget, RemoteTrueBoolTaskStringDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert152(IAuthorizedAllCombinations itarget, RemoteFalseBoolBoolDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert153(IAuthorizedAllCombinations itarget, RemoteFalseBoolStringDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert154(IAuthorizedAllCombinations itarget, RemoteFalseBoolTaskBoolDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert155(IAuthorizedAllCombinations itarget, RemoteFalseBoolTaskStringDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert156(IAuthorizedAllCombinations itarget, RemoteTaskVoidBoolDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert157(IAuthorizedAllCombinations itarget, RemoteTaskVoidStringDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert158(IAuthorizedAllCombinations itarget, RemoteTaskVoidTaskBoolDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert159(IAuthorizedAllCombinations itarget, RemoteTaskVoidTaskStringDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert160(IAuthorizedAllCombinations itarget, RemoteTaskTrueBoolBoolDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert161(IAuthorizedAllCombinations itarget, RemoteTaskTrueBoolStringDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert162(IAuthorizedAllCombinations itarget, RemoteTaskTrueBoolTaskBoolDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert163(IAuthorizedAllCombinations itarget, RemoteTaskTrueBoolTaskStringDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert164(IAuthorizedAllCombinations itarget, RemoteTaskFalseBoolBoolDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert165(IAuthorizedAllCombinations itarget, RemoteTaskFalseBoolStringDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert166(IAuthorizedAllCombinations itarget, RemoteTaskFalseBoolTaskBoolDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert167(IAuthorizedAllCombinations itarget, RemoteTaskFalseBoolTaskStringDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert168(IAuthorizedAllCombinations itarget, RemoteVoidBoolRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert169(IAuthorizedAllCombinations itarget, RemoteVoidStringRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert170(IAuthorizedAllCombinations itarget, RemoteVoidTaskBoolRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert171(IAuthorizedAllCombinations itarget, RemoteVoidTaskStringRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCall<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert172(IAuthorizedAllCombinations itarget, RemoteTrueBoolBoolRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert173(IAuthorizedAllCombinations itarget, RemoteTrueBoolStringRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert174(IAuthorizedAllCombinations itarget, RemoteTrueBoolTaskBoolRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert175(IAuthorizedAllCombinations itarget, RemoteTrueBoolTaskStringRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert176(IAuthorizedAllCombinations itarget, RemoteFalseBoolBoolRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert177(IAuthorizedAllCombinations itarget, RemoteFalseBoolStringRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert178(IAuthorizedAllCombinations itarget, RemoteFalseBoolTaskBoolRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual IAuthorizedAllCombinations? LocalInsert179(IAuthorizedAllCombinations itarget, RemoteFalseBoolTaskStringRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return DoMapperMethodCallBool<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert180(IAuthorizedAllCombinations itarget, RemoteTaskVoidBoolRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert181(IAuthorizedAllCombinations itarget, RemoteTaskVoidStringRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert182(IAuthorizedAllCombinations itarget, RemoteTaskVoidTaskBoolRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert183(IAuthorizedAllCombinations itarget, RemoteTaskVoidTaskStringRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert184(IAuthorizedAllCombinations itarget, RemoteTaskTrueBoolBoolRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert185(IAuthorizedAllCombinations itarget, RemoteTaskTrueBoolStringRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert186(IAuthorizedAllCombinations itarget, RemoteTaskTrueBoolTaskBoolRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert187(IAuthorizedAllCombinations itarget, RemoteTaskTrueBoolTaskStringRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert188(IAuthorizedAllCombinations itarget, RemoteTaskFalseBoolBoolRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert189(IAuthorizedAllCombinations itarget, RemoteTaskFalseBoolStringRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert190(IAuthorizedAllCombinations itarget, RemoteTaskFalseBoolTaskBoolRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<IAuthorizedAllCombinations?> LocalInsert191(IAuthorizedAllCombinations itarget, RemoteTaskFalseBoolTaskStringRemoteDeny v)
        {
            var target = (AuthorizedAllCombinations)itarget ?? throw new Exception("AuthorizedAllCombinations must implement IAuthorizedAllCombinations");
            return await DoMapperMethodCallBoolAsync<IAuthorizedAllCombinations>(target, DataMapperMethod.Insert, () => target.Insert(v));
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate24(VoidBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create24Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate25(VoidStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create25Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate26(VoidTaskBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create26Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate27(VoidTaskStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create27Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate28(TrueBoolBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create28Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate29(TrueBoolStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create29Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate30(TrueBoolTaskBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create30Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate31(TrueBoolTaskStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create31Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate32(FalseBoolBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create32Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate33(FalseBoolStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create33Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate34(FalseBoolTaskBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create34Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate35(FalseBoolTaskStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create35Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate36(TaskVoidBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create36Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate37(TaskVoidStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create37Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate38(TaskVoidTaskBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create38Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate39(TaskVoidTaskStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create39Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate40(TaskTrueBoolBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create40Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate41(TaskTrueBoolStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create41Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate42(TaskTrueBoolTaskBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create42Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate43(TaskTrueBoolTaskStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create43Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate44(TaskFalseBoolBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create44Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate45(TaskFalseBoolStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create45Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate46(TaskFalseBoolTaskBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create46Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate47(TaskFalseBoolTaskStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create47Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate48(RemoteVoidBool v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create48Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate49(RemoteVoidString v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create49Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate50(RemoteVoidTaskBool v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create50Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate51(RemoteVoidTaskString v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create51Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate52(RemoteTrueBoolBool v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create52Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate53(RemoteTrueBoolString v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create53Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate54(RemoteTrueBoolTaskBool v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create54Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate55(RemoteTrueBoolTaskString v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create55Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate56(RemoteFalseBoolBool v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create56Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate57(RemoteFalseBoolString v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create57Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate58(RemoteFalseBoolTaskBool v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create58Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate59(RemoteFalseBoolTaskString v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create59Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate60(RemoteTaskVoidBool v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create60Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate61(RemoteTaskVoidString v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create61Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate62(RemoteTaskVoidTaskBool v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create62Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate63(RemoteTaskVoidTaskString v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create63Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate64(RemoteTaskTrueBoolBool v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create64Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate65(RemoteTaskTrueBoolString v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create65Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate66(RemoteTaskTrueBoolTaskBool v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create66Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate67(RemoteTaskTrueBoolTaskString v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create67Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate68(RemoteTaskFalseBoolBool v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create68Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate69(RemoteTaskFalseBoolString v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create69Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate70(RemoteTaskFalseBoolTaskBool v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create70Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate71(RemoteTaskFalseBoolTaskString v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create71Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate72(RemoteVoidBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create72Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate73(RemoteVoidStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create73Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate74(RemoteVoidTaskBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create74Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate75(RemoteVoidTaskStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create75Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate76(RemoteTrueBoolBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create76Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate77(RemoteTrueBoolStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create77Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate78(RemoteTrueBoolTaskBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create78Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate79(RemoteTrueBoolTaskStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create79Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate80(RemoteFalseBoolBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create80Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate81(RemoteFalseBoolStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create81Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate82(RemoteFalseBoolTaskBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create82Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate83(RemoteFalseBoolTaskStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create83Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate84(RemoteTaskVoidBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create84Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate85(RemoteTaskVoidStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create85Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate86(RemoteTaskVoidTaskBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create86Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate87(RemoteTaskVoidTaskStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create87Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate88(RemoteTaskTrueBoolBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create88Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate89(RemoteTaskTrueBoolStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create89Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate90(RemoteTaskTrueBoolTaskBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create90Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate91(RemoteTaskTrueBoolTaskStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create91Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate92(RemoteTaskFalseBoolBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create92Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate93(RemoteTaskFalseBoolStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create93Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate94(RemoteTaskFalseBoolTaskBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create94Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate95(RemoteTaskFalseBoolTaskStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create95Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate120(VoidBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create120Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate121(VoidStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create121Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate122(VoidTaskBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create122Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate123(VoidTaskStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create123Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate124(TrueBoolBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create124Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate125(TrueBoolStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create125Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate126(TrueBoolTaskBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create126Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate127(TrueBoolTaskStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create127Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate128(FalseBoolBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create128Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate129(FalseBoolStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create129Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate130(FalseBoolTaskBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create130Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate131(FalseBoolTaskStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create131Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate132(TaskVoidBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create132Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate133(TaskVoidStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create133Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate134(TaskVoidTaskBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create134Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate135(TaskVoidTaskStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create135Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate136(TaskTrueBoolBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create136Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate137(TaskTrueBoolStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create137Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate138(TaskTrueBoolTaskBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create138Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate139(TaskTrueBoolTaskStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create139Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate140(TaskFalseBoolBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create140Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate141(TaskFalseBoolStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create141Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate142(TaskFalseBoolTaskBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create142Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate143(TaskFalseBoolTaskStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create143Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate144(RemoteVoidBoolDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create144Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate145(RemoteVoidStringDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create145Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate146(RemoteVoidTaskBoolDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create146Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate147(RemoteVoidTaskStringDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create147Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate148(RemoteTrueBoolBoolDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create148Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate149(RemoteTrueBoolStringDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create149Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate150(RemoteTrueBoolTaskBoolDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create150Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate151(RemoteTrueBoolTaskStringDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create151Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate152(RemoteFalseBoolBoolDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create152Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate153(RemoteFalseBoolStringDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create153Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate154(RemoteFalseBoolTaskBoolDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create154Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate155(RemoteFalseBoolTaskStringDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create155Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate156(RemoteTaskVoidBoolDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create156Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate157(RemoteTaskVoidStringDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create157Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate158(RemoteTaskVoidTaskBoolDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create158Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate159(RemoteTaskVoidTaskStringDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create159Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate160(RemoteTaskTrueBoolBoolDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create160Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate161(RemoteTaskTrueBoolStringDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create161Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate162(RemoteTaskTrueBoolTaskBoolDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create162Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate163(RemoteTaskTrueBoolTaskStringDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create163Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate164(RemoteTaskFalseBoolBoolDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create164Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate165(RemoteTaskFalseBoolStringDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create165Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate166(RemoteTaskFalseBoolTaskBoolDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create166Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate167(RemoteTaskFalseBoolTaskStringDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create167Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate168(RemoteVoidBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create168Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate169(RemoteVoidStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create169Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate170(RemoteVoidTaskBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create170Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate171(RemoteVoidTaskStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create171Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate172(RemoteTrueBoolBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create172Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate173(RemoteTrueBoolStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create173Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate174(RemoteTrueBoolTaskBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create174Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate175(RemoteTrueBoolTaskStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create175Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate176(RemoteFalseBoolBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create176Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate177(RemoteFalseBoolStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create177Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate178(RemoteFalseBoolTaskBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create178Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate179(RemoteFalseBoolTaskStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create179Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate180(RemoteTaskVoidBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create180Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate181(RemoteTaskVoidStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create181Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate182(RemoteTaskVoidTaskBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create182Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate183(RemoteTaskVoidTaskStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create183Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate184(RemoteTaskTrueBoolBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create184Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate185(RemoteTaskTrueBoolStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create185Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate186(RemoteTaskTrueBoolTaskBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create186Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate187(RemoteTaskTrueBoolTaskStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create187Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate188(RemoteTaskFalseBoolBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create188Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate189(RemoteTaskFalseBoolStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create189Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate190(RemoteTaskFalseBoolTaskBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create190Delegate), [v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> RemoteCreate191(RemoteTaskFalseBoolTaskStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>>(typeof(Create191Delegate), [v]);
        }

        public IAuthorizedAllCombinations? Save(IAuthorizedAllCombinations target, VoidBool v)
        {
            var authorized = (TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual Authorized<IAuthorizedAllCombinations> TrySave(IAuthorizedAllCombinations target, VoidBool v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public IAuthorizedAllCombinations? Save(IAuthorizedAllCombinations target, VoidString v)
        {
            var authorized = (TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual Authorized<IAuthorizedAllCombinations> TrySave(IAuthorizedAllCombinations target, VoidString v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert1(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, VoidTaskBool v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, VoidTaskBool v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert2(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, VoidTaskString v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, VoidTaskString v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert3(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public IAuthorizedAllCombinations? Save(IAuthorizedAllCombinations target, TrueBoolBool v)
        {
            var authorized = (TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual Authorized<IAuthorizedAllCombinations> TrySave(IAuthorizedAllCombinations target, TrueBoolBool v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert4(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public IAuthorizedAllCombinations? Save(IAuthorizedAllCombinations target, TrueBoolString v)
        {
            var authorized = (TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual Authorized<IAuthorizedAllCombinations> TrySave(IAuthorizedAllCombinations target, TrueBoolString v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert5(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TrueBoolTaskBool v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TrueBoolTaskBool v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert6(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TrueBoolTaskString v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TrueBoolTaskString v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert7(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public IAuthorizedAllCombinations? Save(IAuthorizedAllCombinations target, FalseBoolBool v)
        {
            var authorized = (TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual Authorized<IAuthorizedAllCombinations> TrySave(IAuthorizedAllCombinations target, FalseBoolBool v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert8(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public IAuthorizedAllCombinations? Save(IAuthorizedAllCombinations target, FalseBoolString v)
        {
            var authorized = (TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual Authorized<IAuthorizedAllCombinations> TrySave(IAuthorizedAllCombinations target, FalseBoolString v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert9(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, FalseBoolTaskBool v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, FalseBoolTaskBool v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert10(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, FalseBoolTaskString v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, FalseBoolTaskString v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert11(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidBool v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidBool v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert12(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidString v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidString v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert13(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidTaskBool v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidTaskBool v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert14(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidTaskString v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidTaskString v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert15(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolBool v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolBool v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert16(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolString v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolString v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert17(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolTaskBool v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolTaskBool v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert18(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolTaskString v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolTaskString v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert19(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolBool v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolBool v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert20(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolString v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolString v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert21(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolTaskBool v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolTaskBool v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert22(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolTaskString v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolTaskString v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert23(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, VoidBoolRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, VoidBoolRemote v)
        {
            return Save24Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave24(IAuthorizedAllCombinations target, VoidBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save24Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave24(IAuthorizedAllCombinations target, VoidBoolRemote v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert24(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, VoidStringRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, VoidStringRemote v)
        {
            return Save25Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave25(IAuthorizedAllCombinations target, VoidStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save25Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave25(IAuthorizedAllCombinations target, VoidStringRemote v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert25(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, VoidTaskBoolRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, VoidTaskBoolRemote v)
        {
            return Save26Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave26(IAuthorizedAllCombinations target, VoidTaskBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save26Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave26(IAuthorizedAllCombinations target, VoidTaskBoolRemote v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert26(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, VoidTaskStringRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, VoidTaskStringRemote v)
        {
            return Save27Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave27(IAuthorizedAllCombinations target, VoidTaskStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save27Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave27(IAuthorizedAllCombinations target, VoidTaskStringRemote v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert27(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TrueBoolBoolRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TrueBoolBoolRemote v)
        {
            return Save28Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave28(IAuthorizedAllCombinations target, TrueBoolBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save28Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave28(IAuthorizedAllCombinations target, TrueBoolBoolRemote v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert28(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TrueBoolStringRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TrueBoolStringRemote v)
        {
            return Save29Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave29(IAuthorizedAllCombinations target, TrueBoolStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save29Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave29(IAuthorizedAllCombinations target, TrueBoolStringRemote v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert29(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TrueBoolTaskBoolRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TrueBoolTaskBoolRemote v)
        {
            return Save30Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave30(IAuthorizedAllCombinations target, TrueBoolTaskBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save30Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave30(IAuthorizedAllCombinations target, TrueBoolTaskBoolRemote v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert30(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TrueBoolTaskStringRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TrueBoolTaskStringRemote v)
        {
            return Save31Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave31(IAuthorizedAllCombinations target, TrueBoolTaskStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save31Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave31(IAuthorizedAllCombinations target, TrueBoolTaskStringRemote v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert31(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, FalseBoolBoolRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, FalseBoolBoolRemote v)
        {
            return Save32Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave32(IAuthorizedAllCombinations target, FalseBoolBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save32Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave32(IAuthorizedAllCombinations target, FalseBoolBoolRemote v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert32(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, FalseBoolStringRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, FalseBoolStringRemote v)
        {
            return Save33Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave33(IAuthorizedAllCombinations target, FalseBoolStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save33Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave33(IAuthorizedAllCombinations target, FalseBoolStringRemote v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert33(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, FalseBoolTaskBoolRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, FalseBoolTaskBoolRemote v)
        {
            return Save34Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave34(IAuthorizedAllCombinations target, FalseBoolTaskBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save34Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave34(IAuthorizedAllCombinations target, FalseBoolTaskBoolRemote v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert34(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, FalseBoolTaskStringRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, FalseBoolTaskStringRemote v)
        {
            return Save35Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave35(IAuthorizedAllCombinations target, FalseBoolTaskStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save35Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave35(IAuthorizedAllCombinations target, FalseBoolTaskStringRemote v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert35(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidBoolRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidBoolRemote v)
        {
            return Save36Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave36(IAuthorizedAllCombinations target, TaskVoidBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save36Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave36(IAuthorizedAllCombinations target, TaskVoidBoolRemote v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert36(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidStringRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidStringRemote v)
        {
            return Save37Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave37(IAuthorizedAllCombinations target, TaskVoidStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save37Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave37(IAuthorizedAllCombinations target, TaskVoidStringRemote v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert37(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidTaskBoolRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidTaskBoolRemote v)
        {
            return Save38Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave38(IAuthorizedAllCombinations target, TaskVoidTaskBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save38Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave38(IAuthorizedAllCombinations target, TaskVoidTaskBoolRemote v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert38(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidTaskStringRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidTaskStringRemote v)
        {
            return Save39Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave39(IAuthorizedAllCombinations target, TaskVoidTaskStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save39Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave39(IAuthorizedAllCombinations target, TaskVoidTaskStringRemote v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert39(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolBoolRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolBoolRemote v)
        {
            return Save40Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave40(IAuthorizedAllCombinations target, TaskTrueBoolBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save40Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave40(IAuthorizedAllCombinations target, TaskTrueBoolBoolRemote v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert40(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolStringRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolStringRemote v)
        {
            return Save41Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave41(IAuthorizedAllCombinations target, TaskTrueBoolStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save41Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave41(IAuthorizedAllCombinations target, TaskTrueBoolStringRemote v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert41(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolTaskBoolRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolTaskBoolRemote v)
        {
            return Save42Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave42(IAuthorizedAllCombinations target, TaskTrueBoolTaskBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save42Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave42(IAuthorizedAllCombinations target, TaskTrueBoolTaskBoolRemote v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert42(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolTaskStringRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolTaskStringRemote v)
        {
            return Save43Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave43(IAuthorizedAllCombinations target, TaskTrueBoolTaskStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save43Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave43(IAuthorizedAllCombinations target, TaskTrueBoolTaskStringRemote v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert43(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolBoolRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolBoolRemote v)
        {
            return Save44Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave44(IAuthorizedAllCombinations target, TaskFalseBoolBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save44Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave44(IAuthorizedAllCombinations target, TaskFalseBoolBoolRemote v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert44(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolStringRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolStringRemote v)
        {
            return Save45Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave45(IAuthorizedAllCombinations target, TaskFalseBoolStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save45Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave45(IAuthorizedAllCombinations target, TaskFalseBoolStringRemote v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert45(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolTaskBoolRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolTaskBoolRemote v)
        {
            return Save46Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave46(IAuthorizedAllCombinations target, TaskFalseBoolTaskBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save46Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave46(IAuthorizedAllCombinations target, TaskFalseBoolTaskBoolRemote v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert46(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolTaskStringRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolTaskStringRemote v)
        {
            return Save47Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave47(IAuthorizedAllCombinations target, TaskFalseBoolTaskStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save47Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave47(IAuthorizedAllCombinations target, TaskFalseBoolTaskStringRemote v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert47(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidBool v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidBool v)
        {
            return Save48Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave48(IAuthorizedAllCombinations target, RemoteVoidBool v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save48Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave48(IAuthorizedAllCombinations target, RemoteVoidBool v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert48(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidString v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidString v)
        {
            return Save49Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave49(IAuthorizedAllCombinations target, RemoteVoidString v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save49Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave49(IAuthorizedAllCombinations target, RemoteVoidString v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert49(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidTaskBool v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidTaskBool v)
        {
            return Save50Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave50(IAuthorizedAllCombinations target, RemoteVoidTaskBool v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save50Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave50(IAuthorizedAllCombinations target, RemoteVoidTaskBool v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert50(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidTaskString v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidTaskString v)
        {
            return Save51Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave51(IAuthorizedAllCombinations target, RemoteVoidTaskString v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save51Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave51(IAuthorizedAllCombinations target, RemoteVoidTaskString v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert51(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolBool v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolBool v)
        {
            return Save52Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave52(IAuthorizedAllCombinations target, RemoteTrueBoolBool v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save52Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave52(IAuthorizedAllCombinations target, RemoteTrueBoolBool v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert52(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolString v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolString v)
        {
            return Save53Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave53(IAuthorizedAllCombinations target, RemoteTrueBoolString v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save53Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave53(IAuthorizedAllCombinations target, RemoteTrueBoolString v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert53(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolTaskBool v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolTaskBool v)
        {
            return Save54Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave54(IAuthorizedAllCombinations target, RemoteTrueBoolTaskBool v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save54Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave54(IAuthorizedAllCombinations target, RemoteTrueBoolTaskBool v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert54(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolTaskString v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolTaskString v)
        {
            return Save55Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave55(IAuthorizedAllCombinations target, RemoteTrueBoolTaskString v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save55Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave55(IAuthorizedAllCombinations target, RemoteTrueBoolTaskString v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert55(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolBool v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolBool v)
        {
            return Save56Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave56(IAuthorizedAllCombinations target, RemoteFalseBoolBool v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save56Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave56(IAuthorizedAllCombinations target, RemoteFalseBoolBool v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert56(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolString v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolString v)
        {
            return Save57Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave57(IAuthorizedAllCombinations target, RemoteFalseBoolString v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save57Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave57(IAuthorizedAllCombinations target, RemoteFalseBoolString v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert57(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolTaskBool v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolTaskBool v)
        {
            return Save58Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave58(IAuthorizedAllCombinations target, RemoteFalseBoolTaskBool v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save58Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave58(IAuthorizedAllCombinations target, RemoteFalseBoolTaskBool v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert58(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolTaskString v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolTaskString v)
        {
            return Save59Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave59(IAuthorizedAllCombinations target, RemoteFalseBoolTaskString v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save59Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave59(IAuthorizedAllCombinations target, RemoteFalseBoolTaskString v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert59(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidBool v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidBool v)
        {
            return Save60Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave60(IAuthorizedAllCombinations target, RemoteTaskVoidBool v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save60Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave60(IAuthorizedAllCombinations target, RemoteTaskVoidBool v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert60(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidString v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidString v)
        {
            return Save61Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave61(IAuthorizedAllCombinations target, RemoteTaskVoidString v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save61Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave61(IAuthorizedAllCombinations target, RemoteTaskVoidString v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert61(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidTaskBool v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidTaskBool v)
        {
            return Save62Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave62(IAuthorizedAllCombinations target, RemoteTaskVoidTaskBool v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save62Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave62(IAuthorizedAllCombinations target, RemoteTaskVoidTaskBool v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert62(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidTaskString v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidTaskString v)
        {
            return Save63Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave63(IAuthorizedAllCombinations target, RemoteTaskVoidTaskString v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save63Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave63(IAuthorizedAllCombinations target, RemoteTaskVoidTaskString v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert63(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolBool v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolBool v)
        {
            return Save64Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave64(IAuthorizedAllCombinations target, RemoteTaskTrueBoolBool v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save64Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave64(IAuthorizedAllCombinations target, RemoteTaskTrueBoolBool v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert64(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolString v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolString v)
        {
            return Save65Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave65(IAuthorizedAllCombinations target, RemoteTaskTrueBoolString v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save65Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave65(IAuthorizedAllCombinations target, RemoteTaskTrueBoolString v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert65(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskBool v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskBool v)
        {
            return Save66Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave66(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskBool v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save66Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave66(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskBool v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert66(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskString v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskString v)
        {
            return Save67Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave67(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskString v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save67Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave67(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskString v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert67(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolBool v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolBool v)
        {
            return Save68Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave68(IAuthorizedAllCombinations target, RemoteTaskFalseBoolBool v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save68Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave68(IAuthorizedAllCombinations target, RemoteTaskFalseBoolBool v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert68(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolString v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolString v)
        {
            return Save69Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave69(IAuthorizedAllCombinations target, RemoteTaskFalseBoolString v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save69Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave69(IAuthorizedAllCombinations target, RemoteTaskFalseBoolString v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert69(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskBool v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskBool v)
        {
            return Save70Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave70(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskBool v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save70Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave70(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskBool v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert70(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskString v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskString v)
        {
            return Save71Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave71(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskString v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save71Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave71(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskString v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert71(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidBoolRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidBoolRemote v)
        {
            return Save72Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave72(IAuthorizedAllCombinations target, RemoteVoidBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save72Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave72(IAuthorizedAllCombinations target, RemoteVoidBoolRemote v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert72(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidStringRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidStringRemote v)
        {
            return Save73Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave73(IAuthorizedAllCombinations target, RemoteVoidStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save73Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave73(IAuthorizedAllCombinations target, RemoteVoidStringRemote v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert73(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidTaskBoolRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidTaskBoolRemote v)
        {
            return Save74Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave74(IAuthorizedAllCombinations target, RemoteVoidTaskBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save74Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave74(IAuthorizedAllCombinations target, RemoteVoidTaskBoolRemote v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert74(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidTaskStringRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidTaskStringRemote v)
        {
            return Save75Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave75(IAuthorizedAllCombinations target, RemoteVoidTaskStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save75Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave75(IAuthorizedAllCombinations target, RemoteVoidTaskStringRemote v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert75(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolBoolRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolBoolRemote v)
        {
            return Save76Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave76(IAuthorizedAllCombinations target, RemoteTrueBoolBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save76Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave76(IAuthorizedAllCombinations target, RemoteTrueBoolBoolRemote v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert76(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolStringRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolStringRemote v)
        {
            return Save77Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave77(IAuthorizedAllCombinations target, RemoteTrueBoolStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save77Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave77(IAuthorizedAllCombinations target, RemoteTrueBoolStringRemote v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert77(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolTaskBoolRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolTaskBoolRemote v)
        {
            return Save78Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave78(IAuthorizedAllCombinations target, RemoteTrueBoolTaskBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save78Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave78(IAuthorizedAllCombinations target, RemoteTrueBoolTaskBoolRemote v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert78(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolTaskStringRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolTaskStringRemote v)
        {
            return Save79Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave79(IAuthorizedAllCombinations target, RemoteTrueBoolTaskStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save79Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave79(IAuthorizedAllCombinations target, RemoteTrueBoolTaskStringRemote v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert79(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolBoolRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolBoolRemote v)
        {
            return Save80Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave80(IAuthorizedAllCombinations target, RemoteFalseBoolBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save80Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave80(IAuthorizedAllCombinations target, RemoteFalseBoolBoolRemote v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert80(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolStringRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolStringRemote v)
        {
            return Save81Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave81(IAuthorizedAllCombinations target, RemoteFalseBoolStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save81Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave81(IAuthorizedAllCombinations target, RemoteFalseBoolStringRemote v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert81(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolTaskBoolRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolTaskBoolRemote v)
        {
            return Save82Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave82(IAuthorizedAllCombinations target, RemoteFalseBoolTaskBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save82Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave82(IAuthorizedAllCombinations target, RemoteFalseBoolTaskBoolRemote v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert82(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolTaskStringRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolTaskStringRemote v)
        {
            return Save83Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave83(IAuthorizedAllCombinations target, RemoteFalseBoolTaskStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save83Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave83(IAuthorizedAllCombinations target, RemoteFalseBoolTaskStringRemote v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert83(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidBoolRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidBoolRemote v)
        {
            return Save84Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave84(IAuthorizedAllCombinations target, RemoteTaskVoidBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save84Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave84(IAuthorizedAllCombinations target, RemoteTaskVoidBoolRemote v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert84(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidStringRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidStringRemote v)
        {
            return Save85Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave85(IAuthorizedAllCombinations target, RemoteTaskVoidStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save85Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave85(IAuthorizedAllCombinations target, RemoteTaskVoidStringRemote v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert85(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidTaskBoolRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidTaskBoolRemote v)
        {
            return Save86Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave86(IAuthorizedAllCombinations target, RemoteTaskVoidTaskBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save86Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave86(IAuthorizedAllCombinations target, RemoteTaskVoidTaskBoolRemote v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert86(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidTaskStringRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidTaskStringRemote v)
        {
            return Save87Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave87(IAuthorizedAllCombinations target, RemoteTaskVoidTaskStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save87Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave87(IAuthorizedAllCombinations target, RemoteTaskVoidTaskStringRemote v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert87(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolBoolRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolBoolRemote v)
        {
            return Save88Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave88(IAuthorizedAllCombinations target, RemoteTaskTrueBoolBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save88Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave88(IAuthorizedAllCombinations target, RemoteTaskTrueBoolBoolRemote v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert88(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolStringRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolStringRemote v)
        {
            return Save89Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave89(IAuthorizedAllCombinations target, RemoteTaskTrueBoolStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save89Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave89(IAuthorizedAllCombinations target, RemoteTaskTrueBoolStringRemote v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert89(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskBoolRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskBoolRemote v)
        {
            return Save90Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave90(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save90Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave90(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskBoolRemote v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert90(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskStringRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskStringRemote v)
        {
            return Save91Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave91(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save91Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave91(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskStringRemote v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert91(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolBoolRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolBoolRemote v)
        {
            return Save92Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave92(IAuthorizedAllCombinations target, RemoteTaskFalseBoolBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save92Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave92(IAuthorizedAllCombinations target, RemoteTaskFalseBoolBoolRemote v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert92(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolStringRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolStringRemote v)
        {
            return Save93Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave93(IAuthorizedAllCombinations target, RemoteTaskFalseBoolStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save93Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave93(IAuthorizedAllCombinations target, RemoteTaskFalseBoolStringRemote v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert93(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskBoolRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskBoolRemote v)
        {
            return Save94Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave94(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskBoolRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save94Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave94(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskBoolRemote v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert94(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskStringRemote v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskStringRemote v)
        {
            return Save95Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave95(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskStringRemote v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save95Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave95(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskStringRemote v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert95(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public IAuthorizedAllCombinations? Save(IAuthorizedAllCombinations target, VoidBoolDeny v)
        {
            var authorized = (TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual Authorized<IAuthorizedAllCombinations> TrySave(IAuthorizedAllCombinations target, VoidBoolDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert96(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public IAuthorizedAllCombinations? Save(IAuthorizedAllCombinations target, VoidStringDeny v)
        {
            var authorized = (TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual Authorized<IAuthorizedAllCombinations> TrySave(IAuthorizedAllCombinations target, VoidStringDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert97(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, VoidTaskBoolDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, VoidTaskBoolDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert98(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, VoidTaskStringDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, VoidTaskStringDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert99(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public IAuthorizedAllCombinations? Save(IAuthorizedAllCombinations target, TrueBoolBoolDeny v)
        {
            var authorized = (TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual Authorized<IAuthorizedAllCombinations> TrySave(IAuthorizedAllCombinations target, TrueBoolBoolDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert100(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public IAuthorizedAllCombinations? Save(IAuthorizedAllCombinations target, TrueBoolStringDeny v)
        {
            var authorized = (TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual Authorized<IAuthorizedAllCombinations> TrySave(IAuthorizedAllCombinations target, TrueBoolStringDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert101(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TrueBoolTaskBoolDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TrueBoolTaskBoolDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert102(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TrueBoolTaskStringDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TrueBoolTaskStringDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert103(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public IAuthorizedAllCombinations? Save(IAuthorizedAllCombinations target, FalseBoolBoolDeny v)
        {
            var authorized = (TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual Authorized<IAuthorizedAllCombinations> TrySave(IAuthorizedAllCombinations target, FalseBoolBoolDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert104(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public IAuthorizedAllCombinations? Save(IAuthorizedAllCombinations target, FalseBoolStringDeny v)
        {
            var authorized = (TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual Authorized<IAuthorizedAllCombinations> TrySave(IAuthorizedAllCombinations target, FalseBoolStringDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert105(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, FalseBoolTaskBoolDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, FalseBoolTaskBoolDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert106(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, FalseBoolTaskStringDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, FalseBoolTaskStringDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert107(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidBoolDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidBoolDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert108(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidStringDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidStringDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert109(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidTaskBoolDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidTaskBoolDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert110(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidTaskStringDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidTaskStringDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert111(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolBoolDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolBoolDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert112(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolStringDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolStringDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert113(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolTaskBoolDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolTaskBoolDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert114(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolTaskStringDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolTaskStringDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert115(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolBoolDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolBoolDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert116(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolStringDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolStringDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert117(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolTaskBoolDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolTaskBoolDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert118(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolTaskStringDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolTaskStringDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert119(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, VoidBoolRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, VoidBoolRemoteDeny v)
        {
            return Save120Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave120(IAuthorizedAllCombinations target, VoidBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save120Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave120(IAuthorizedAllCombinations target, VoidBoolRemoteDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert120(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, VoidStringRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, VoidStringRemoteDeny v)
        {
            return Save121Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave121(IAuthorizedAllCombinations target, VoidStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save121Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave121(IAuthorizedAllCombinations target, VoidStringRemoteDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert121(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, VoidTaskBoolRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, VoidTaskBoolRemoteDeny v)
        {
            return Save122Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave122(IAuthorizedAllCombinations target, VoidTaskBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save122Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave122(IAuthorizedAllCombinations target, VoidTaskBoolRemoteDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert122(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, VoidTaskStringRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, VoidTaskStringRemoteDeny v)
        {
            return Save123Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave123(IAuthorizedAllCombinations target, VoidTaskStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save123Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave123(IAuthorizedAllCombinations target, VoidTaskStringRemoteDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert123(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TrueBoolBoolRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TrueBoolBoolRemoteDeny v)
        {
            return Save124Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave124(IAuthorizedAllCombinations target, TrueBoolBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save124Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave124(IAuthorizedAllCombinations target, TrueBoolBoolRemoteDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert124(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TrueBoolStringRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TrueBoolStringRemoteDeny v)
        {
            return Save125Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave125(IAuthorizedAllCombinations target, TrueBoolStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save125Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave125(IAuthorizedAllCombinations target, TrueBoolStringRemoteDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert125(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TrueBoolTaskBoolRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TrueBoolTaskBoolRemoteDeny v)
        {
            return Save126Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave126(IAuthorizedAllCombinations target, TrueBoolTaskBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save126Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave126(IAuthorizedAllCombinations target, TrueBoolTaskBoolRemoteDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert126(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TrueBoolTaskStringRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TrueBoolTaskStringRemoteDeny v)
        {
            return Save127Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave127(IAuthorizedAllCombinations target, TrueBoolTaskStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save127Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave127(IAuthorizedAllCombinations target, TrueBoolTaskStringRemoteDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert127(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, FalseBoolBoolRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, FalseBoolBoolRemoteDeny v)
        {
            return Save128Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave128(IAuthorizedAllCombinations target, FalseBoolBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save128Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave128(IAuthorizedAllCombinations target, FalseBoolBoolRemoteDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert128(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, FalseBoolStringRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, FalseBoolStringRemoteDeny v)
        {
            return Save129Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave129(IAuthorizedAllCombinations target, FalseBoolStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save129Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave129(IAuthorizedAllCombinations target, FalseBoolStringRemoteDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert129(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, FalseBoolTaskBoolRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, FalseBoolTaskBoolRemoteDeny v)
        {
            return Save130Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave130(IAuthorizedAllCombinations target, FalseBoolTaskBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save130Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave130(IAuthorizedAllCombinations target, FalseBoolTaskBoolRemoteDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert130(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, FalseBoolTaskStringRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, FalseBoolTaskStringRemoteDeny v)
        {
            return Save131Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave131(IAuthorizedAllCombinations target, FalseBoolTaskStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save131Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave131(IAuthorizedAllCombinations target, FalseBoolTaskStringRemoteDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert131(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidBoolRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidBoolRemoteDeny v)
        {
            return Save132Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave132(IAuthorizedAllCombinations target, TaskVoidBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save132Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave132(IAuthorizedAllCombinations target, TaskVoidBoolRemoteDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert132(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidStringRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidStringRemoteDeny v)
        {
            return Save133Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave133(IAuthorizedAllCombinations target, TaskVoidStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save133Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave133(IAuthorizedAllCombinations target, TaskVoidStringRemoteDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert133(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidTaskBoolRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidTaskBoolRemoteDeny v)
        {
            return Save134Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave134(IAuthorizedAllCombinations target, TaskVoidTaskBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save134Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave134(IAuthorizedAllCombinations target, TaskVoidTaskBoolRemoteDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert134(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskVoidTaskStringRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskVoidTaskStringRemoteDeny v)
        {
            return Save135Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave135(IAuthorizedAllCombinations target, TaskVoidTaskStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save135Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave135(IAuthorizedAllCombinations target, TaskVoidTaskStringRemoteDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert135(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolBoolRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolBoolRemoteDeny v)
        {
            return Save136Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave136(IAuthorizedAllCombinations target, TaskTrueBoolBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save136Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave136(IAuthorizedAllCombinations target, TaskTrueBoolBoolRemoteDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert136(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolStringRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolStringRemoteDeny v)
        {
            return Save137Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave137(IAuthorizedAllCombinations target, TaskTrueBoolStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save137Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave137(IAuthorizedAllCombinations target, TaskTrueBoolStringRemoteDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert137(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolTaskBoolRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolTaskBoolRemoteDeny v)
        {
            return Save138Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave138(IAuthorizedAllCombinations target, TaskTrueBoolTaskBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save138Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave138(IAuthorizedAllCombinations target, TaskTrueBoolTaskBoolRemoteDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert138(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskTrueBoolTaskStringRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskTrueBoolTaskStringRemoteDeny v)
        {
            return Save139Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave139(IAuthorizedAllCombinations target, TaskTrueBoolTaskStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save139Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave139(IAuthorizedAllCombinations target, TaskTrueBoolTaskStringRemoteDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert139(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolBoolRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolBoolRemoteDeny v)
        {
            return Save140Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave140(IAuthorizedAllCombinations target, TaskFalseBoolBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save140Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave140(IAuthorizedAllCombinations target, TaskFalseBoolBoolRemoteDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert140(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolStringRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolStringRemoteDeny v)
        {
            return Save141Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave141(IAuthorizedAllCombinations target, TaskFalseBoolStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save141Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave141(IAuthorizedAllCombinations target, TaskFalseBoolStringRemoteDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert141(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolTaskBoolRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolTaskBoolRemoteDeny v)
        {
            return Save142Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave142(IAuthorizedAllCombinations target, TaskFalseBoolTaskBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save142Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave142(IAuthorizedAllCombinations target, TaskFalseBoolTaskBoolRemoteDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert142(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, TaskFalseBoolTaskStringRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, TaskFalseBoolTaskStringRemoteDeny v)
        {
            return Save143Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave143(IAuthorizedAllCombinations target, TaskFalseBoolTaskStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save143Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave143(IAuthorizedAllCombinations target, TaskFalseBoolTaskStringRemoteDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert143(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidBoolDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidBoolDeny v)
        {
            return Save144Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave144(IAuthorizedAllCombinations target, RemoteVoidBoolDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save144Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave144(IAuthorizedAllCombinations target, RemoteVoidBoolDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert144(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidStringDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidStringDeny v)
        {
            return Save145Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave145(IAuthorizedAllCombinations target, RemoteVoidStringDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save145Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave145(IAuthorizedAllCombinations target, RemoteVoidStringDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert145(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidTaskBoolDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidTaskBoolDeny v)
        {
            return Save146Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave146(IAuthorizedAllCombinations target, RemoteVoidTaskBoolDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save146Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave146(IAuthorizedAllCombinations target, RemoteVoidTaskBoolDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert146(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidTaskStringDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidTaskStringDeny v)
        {
            return Save147Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave147(IAuthorizedAllCombinations target, RemoteVoidTaskStringDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save147Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave147(IAuthorizedAllCombinations target, RemoteVoidTaskStringDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert147(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolBoolDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolBoolDeny v)
        {
            return Save148Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave148(IAuthorizedAllCombinations target, RemoteTrueBoolBoolDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save148Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave148(IAuthorizedAllCombinations target, RemoteTrueBoolBoolDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert148(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolStringDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolStringDeny v)
        {
            return Save149Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave149(IAuthorizedAllCombinations target, RemoteTrueBoolStringDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save149Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave149(IAuthorizedAllCombinations target, RemoteTrueBoolStringDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert149(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolTaskBoolDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolTaskBoolDeny v)
        {
            return Save150Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave150(IAuthorizedAllCombinations target, RemoteTrueBoolTaskBoolDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save150Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave150(IAuthorizedAllCombinations target, RemoteTrueBoolTaskBoolDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert150(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolTaskStringDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolTaskStringDeny v)
        {
            return Save151Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave151(IAuthorizedAllCombinations target, RemoteTrueBoolTaskStringDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save151Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave151(IAuthorizedAllCombinations target, RemoteTrueBoolTaskStringDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert151(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolBoolDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolBoolDeny v)
        {
            return Save152Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave152(IAuthorizedAllCombinations target, RemoteFalseBoolBoolDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save152Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave152(IAuthorizedAllCombinations target, RemoteFalseBoolBoolDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert152(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolStringDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolStringDeny v)
        {
            return Save153Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave153(IAuthorizedAllCombinations target, RemoteFalseBoolStringDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save153Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave153(IAuthorizedAllCombinations target, RemoteFalseBoolStringDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert153(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolTaskBoolDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolTaskBoolDeny v)
        {
            return Save154Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave154(IAuthorizedAllCombinations target, RemoteFalseBoolTaskBoolDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save154Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave154(IAuthorizedAllCombinations target, RemoteFalseBoolTaskBoolDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert154(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolTaskStringDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolTaskStringDeny v)
        {
            return Save155Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave155(IAuthorizedAllCombinations target, RemoteFalseBoolTaskStringDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save155Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave155(IAuthorizedAllCombinations target, RemoteFalseBoolTaskStringDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert155(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidBoolDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidBoolDeny v)
        {
            return Save156Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave156(IAuthorizedAllCombinations target, RemoteTaskVoidBoolDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save156Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave156(IAuthorizedAllCombinations target, RemoteTaskVoidBoolDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert156(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidStringDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidStringDeny v)
        {
            return Save157Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave157(IAuthorizedAllCombinations target, RemoteTaskVoidStringDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save157Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave157(IAuthorizedAllCombinations target, RemoteTaskVoidStringDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert157(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidTaskBoolDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidTaskBoolDeny v)
        {
            return Save158Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave158(IAuthorizedAllCombinations target, RemoteTaskVoidTaskBoolDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save158Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave158(IAuthorizedAllCombinations target, RemoteTaskVoidTaskBoolDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert158(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidTaskStringDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidTaskStringDeny v)
        {
            return Save159Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave159(IAuthorizedAllCombinations target, RemoteTaskVoidTaskStringDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save159Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave159(IAuthorizedAllCombinations target, RemoteTaskVoidTaskStringDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert159(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolBoolDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolBoolDeny v)
        {
            return Save160Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave160(IAuthorizedAllCombinations target, RemoteTaskTrueBoolBoolDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save160Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave160(IAuthorizedAllCombinations target, RemoteTaskTrueBoolBoolDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert160(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolStringDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolStringDeny v)
        {
            return Save161Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave161(IAuthorizedAllCombinations target, RemoteTaskTrueBoolStringDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save161Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave161(IAuthorizedAllCombinations target, RemoteTaskTrueBoolStringDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert161(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskBoolDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskBoolDeny v)
        {
            return Save162Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave162(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskBoolDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save162Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave162(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskBoolDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert162(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskStringDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskStringDeny v)
        {
            return Save163Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave163(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskStringDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save163Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave163(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskStringDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert163(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolBoolDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolBoolDeny v)
        {
            return Save164Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave164(IAuthorizedAllCombinations target, RemoteTaskFalseBoolBoolDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save164Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave164(IAuthorizedAllCombinations target, RemoteTaskFalseBoolBoolDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert164(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolStringDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolStringDeny v)
        {
            return Save165Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave165(IAuthorizedAllCombinations target, RemoteTaskFalseBoolStringDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save165Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave165(IAuthorizedAllCombinations target, RemoteTaskFalseBoolStringDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert165(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskBoolDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskBoolDeny v)
        {
            return Save166Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave166(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskBoolDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save166Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave166(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskBoolDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert166(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskStringDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskStringDeny v)
        {
            return Save167Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave167(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskStringDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save167Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave167(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskStringDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert167(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidBoolRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidBoolRemoteDeny v)
        {
            return Save168Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave168(IAuthorizedAllCombinations target, RemoteVoidBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save168Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave168(IAuthorizedAllCombinations target, RemoteVoidBoolRemoteDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert168(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidStringRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidStringRemoteDeny v)
        {
            return Save169Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave169(IAuthorizedAllCombinations target, RemoteVoidStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save169Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave169(IAuthorizedAllCombinations target, RemoteVoidStringRemoteDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert169(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidTaskBoolRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidTaskBoolRemoteDeny v)
        {
            return Save170Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave170(IAuthorizedAllCombinations target, RemoteVoidTaskBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save170Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave170(IAuthorizedAllCombinations target, RemoteVoidTaskBoolRemoteDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert170(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteVoidTaskStringRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteVoidTaskStringRemoteDeny v)
        {
            return Save171Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave171(IAuthorizedAllCombinations target, RemoteVoidTaskStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save171Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave171(IAuthorizedAllCombinations target, RemoteVoidTaskStringRemoteDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert171(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolBoolRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolBoolRemoteDeny v)
        {
            return Save172Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave172(IAuthorizedAllCombinations target, RemoteTrueBoolBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save172Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave172(IAuthorizedAllCombinations target, RemoteTrueBoolBoolRemoteDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert172(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolStringRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolStringRemoteDeny v)
        {
            return Save173Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave173(IAuthorizedAllCombinations target, RemoteTrueBoolStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save173Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave173(IAuthorizedAllCombinations target, RemoteTrueBoolStringRemoteDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert173(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolTaskBoolRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolTaskBoolRemoteDeny v)
        {
            return Save174Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave174(IAuthorizedAllCombinations target, RemoteTrueBoolTaskBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save174Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave174(IAuthorizedAllCombinations target, RemoteTrueBoolTaskBoolRemoteDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert174(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTrueBoolTaskStringRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTrueBoolTaskStringRemoteDeny v)
        {
            return Save175Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave175(IAuthorizedAllCombinations target, RemoteTrueBoolTaskStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save175Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave175(IAuthorizedAllCombinations target, RemoteTrueBoolTaskStringRemoteDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert175(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolBoolRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolBoolRemoteDeny v)
        {
            return Save176Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave176(IAuthorizedAllCombinations target, RemoteFalseBoolBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save176Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave176(IAuthorizedAllCombinations target, RemoteFalseBoolBoolRemoteDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert176(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolStringRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolStringRemoteDeny v)
        {
            return Save177Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave177(IAuthorizedAllCombinations target, RemoteFalseBoolStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save177Delegate), [target, v]);
        }

        public virtual Task<Authorized<IAuthorizedAllCombinations>> LocalSave177(IAuthorizedAllCombinations target, RemoteFalseBoolStringRemoteDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(write));
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return Task.FromResult(new Authorized<IAuthorizedAllCombinations>(LocalInsert177(target, v)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolTaskBoolRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolTaskBoolRemoteDeny v)
        {
            return Save178Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave178(IAuthorizedAllCombinations target, RemoteFalseBoolTaskBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save178Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave178(IAuthorizedAllCombinations target, RemoteFalseBoolTaskBoolRemoteDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert178(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteFalseBoolTaskStringRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteFalseBoolTaskStringRemoteDeny v)
        {
            return Save179Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave179(IAuthorizedAllCombinations target, RemoteFalseBoolTaskStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save179Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave179(IAuthorizedAllCombinations target, RemoteFalseBoolTaskStringRemoteDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(LocalInsert179(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidBoolRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidBoolRemoteDeny v)
        {
            return Save180Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave180(IAuthorizedAllCombinations target, RemoteTaskVoidBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save180Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave180(IAuthorizedAllCombinations target, RemoteTaskVoidBoolRemoteDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert180(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidStringRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidStringRemoteDeny v)
        {
            return Save181Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave181(IAuthorizedAllCombinations target, RemoteTaskVoidStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save181Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave181(IAuthorizedAllCombinations target, RemoteTaskVoidStringRemoteDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert181(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidTaskBoolRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidTaskBoolRemoteDeny v)
        {
            return Save182Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave182(IAuthorizedAllCombinations target, RemoteTaskVoidTaskBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save182Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave182(IAuthorizedAllCombinations target, RemoteTaskVoidTaskBoolRemoteDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert182(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskVoidTaskStringRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskVoidTaskStringRemoteDeny v)
        {
            return Save183Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave183(IAuthorizedAllCombinations target, RemoteTaskVoidTaskStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save183Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave183(IAuthorizedAllCombinations target, RemoteTaskVoidTaskStringRemoteDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert183(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolBoolRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolBoolRemoteDeny v)
        {
            return Save184Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave184(IAuthorizedAllCombinations target, RemoteTaskTrueBoolBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save184Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave184(IAuthorizedAllCombinations target, RemoteTaskTrueBoolBoolRemoteDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert184(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolStringRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolStringRemoteDeny v)
        {
            return Save185Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave185(IAuthorizedAllCombinations target, RemoteTaskTrueBoolStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save185Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave185(IAuthorizedAllCombinations target, RemoteTaskTrueBoolStringRemoteDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert185(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskBoolRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskBoolRemoteDeny v)
        {
            return Save186Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave186(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save186Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave186(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskBoolRemoteDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert186(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskStringRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskStringRemoteDeny v)
        {
            return Save187Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave187(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save187Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave187(IAuthorizedAllCombinations target, RemoteTaskTrueBoolTaskStringRemoteDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert187(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolBoolRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolBoolRemoteDeny v)
        {
            return Save188Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave188(IAuthorizedAllCombinations target, RemoteTaskFalseBoolBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save188Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave188(IAuthorizedAllCombinations target, RemoteTaskFalseBoolBoolRemoteDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert188(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolStringRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolStringRemoteDeny v)
        {
            return Save189Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave189(IAuthorizedAllCombinations target, RemoteTaskFalseBoolStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save189Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave189(IAuthorizedAllCombinations target, RemoteTaskFalseBoolStringRemoteDeny v)
        {
            Authorized write = AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert189(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskBoolRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskBoolRemoteDeny v)
        {
            return Save190Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave190(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskBoolRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save190Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave190(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskBoolRemoteDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert190(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task<IAuthorizedAllCombinations?> Save(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskStringRemoteDeny v)
        {
            var authorized = (await TrySave(target, v));
            if (!authorized.HasAccess)
            {
                throw new NotAuthorizedException(authorized.Message);
            }

            return authorized.Result;
        }

        public Task<Authorized<IAuthorizedAllCombinations>> TrySave(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskStringRemoteDeny v)
        {
            return Save191Property(target, v);
        }

        public async Task<Authorized<IAuthorizedAllCombinations>> RemoteSave191(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskStringRemoteDeny v)
        {
            return await DoRemoteRequest.ForDelegate<Authorized<IAuthorizedAllCombinations>?>(typeof(Save191Delegate), [target, v]);
        }

        public virtual async Task<Authorized<IAuthorizedAllCombinations>> LocalSave191(IAuthorizedAllCombinations target, RemoteTaskFalseBoolTaskStringRemoteDeny v)
        {
            Authorized write = await AuthorizationAllCombinations.Write(v);
            if (!write.HasAccess)
            {
                return new Authorized<IAuthorizedAllCombinations>(write);
            }

            if (target.IsDeleted)
            {
                if (target.IsNew)
                {
                    return null;
                }

                throw new NotImplementedException();
            }
            else if (target.IsNew)
            {
                return new Authorized<IAuthorizedAllCombinations>(await LocalInsert191(target, v));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static void FactoryServiceRegistrar(IServiceCollection services)
        {
            services.AddTransient<AuthorizedAllCombinations>();
            services.AddScoped<AuthorizedAllCombinationsFactory>();
            services.AddScoped<IAuthorizedAllCombinationsFactory, AuthorizedAllCombinationsFactory>();
            services.AddTransient<IAuthorizedAllCombinations, AuthorizedAllCombinations>();
            services.AddScoped<Create24Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (VoidBoolRemote v) => factory.LocalCreate24(v);
            });
            services.AddScoped<Create25Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (VoidStringRemote v) => factory.LocalCreate25(v);
            });
            services.AddScoped<Create26Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (VoidTaskBoolRemote v) => factory.LocalCreate26(v);
            });
            services.AddScoped<Create27Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (VoidTaskStringRemote v) => factory.LocalCreate27(v);
            });
            services.AddScoped<Create28Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TrueBoolBoolRemote v) => factory.LocalCreate28(v);
            });
            services.AddScoped<Create29Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TrueBoolStringRemote v) => factory.LocalCreate29(v);
            });
            services.AddScoped<Create30Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TrueBoolTaskBoolRemote v) => factory.LocalCreate30(v);
            });
            services.AddScoped<Create31Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TrueBoolTaskStringRemote v) => factory.LocalCreate31(v);
            });
            services.AddScoped<Create32Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (FalseBoolBoolRemote v) => factory.LocalCreate32(v);
            });
            services.AddScoped<Create33Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (FalseBoolStringRemote v) => factory.LocalCreate33(v);
            });
            services.AddScoped<Create34Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (FalseBoolTaskBoolRemote v) => factory.LocalCreate34(v);
            });
            services.AddScoped<Create35Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (FalseBoolTaskStringRemote v) => factory.LocalCreate35(v);
            });
            services.AddScoped<Create36Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TaskVoidBoolRemote v) => factory.LocalCreate36(v);
            });
            services.AddScoped<Create37Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TaskVoidStringRemote v) => factory.LocalCreate37(v);
            });
            services.AddScoped<Create38Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TaskVoidTaskBoolRemote v) => factory.LocalCreate38(v);
            });
            services.AddScoped<Create39Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TaskVoidTaskStringRemote v) => factory.LocalCreate39(v);
            });
            services.AddScoped<Create40Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TaskTrueBoolBoolRemote v) => factory.LocalCreate40(v);
            });
            services.AddScoped<Create41Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TaskTrueBoolStringRemote v) => factory.LocalCreate41(v);
            });
            services.AddScoped<Create42Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TaskTrueBoolTaskBoolRemote v) => factory.LocalCreate42(v);
            });
            services.AddScoped<Create43Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TaskTrueBoolTaskStringRemote v) => factory.LocalCreate43(v);
            });
            services.AddScoped<Create44Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TaskFalseBoolBoolRemote v) => factory.LocalCreate44(v);
            });
            services.AddScoped<Create45Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TaskFalseBoolStringRemote v) => factory.LocalCreate45(v);
            });
            services.AddScoped<Create46Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TaskFalseBoolTaskBoolRemote v) => factory.LocalCreate46(v);
            });
            services.AddScoped<Create47Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TaskFalseBoolTaskStringRemote v) => factory.LocalCreate47(v);
            });
            services.AddScoped<Create48Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteVoidBool v) => factory.LocalCreate48(v);
            });
            services.AddScoped<Create49Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteVoidString v) => factory.LocalCreate49(v);
            });
            services.AddScoped<Create50Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteVoidTaskBool v) => factory.LocalCreate50(v);
            });
            services.AddScoped<Create51Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteVoidTaskString v) => factory.LocalCreate51(v);
            });
            services.AddScoped<Create52Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTrueBoolBool v) => factory.LocalCreate52(v);
            });
            services.AddScoped<Create53Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTrueBoolString v) => factory.LocalCreate53(v);
            });
            services.AddScoped<Create54Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTrueBoolTaskBool v) => factory.LocalCreate54(v);
            });
            services.AddScoped<Create55Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTrueBoolTaskString v) => factory.LocalCreate55(v);
            });
            services.AddScoped<Create56Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteFalseBoolBool v) => factory.LocalCreate56(v);
            });
            services.AddScoped<Create57Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteFalseBoolString v) => factory.LocalCreate57(v);
            });
            services.AddScoped<Create58Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteFalseBoolTaskBool v) => factory.LocalCreate58(v);
            });
            services.AddScoped<Create59Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteFalseBoolTaskString v) => factory.LocalCreate59(v);
            });
            services.AddScoped<Create60Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskVoidBool v) => factory.LocalCreate60(v);
            });
            services.AddScoped<Create61Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskVoidString v) => factory.LocalCreate61(v);
            });
            services.AddScoped<Create62Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskVoidTaskBool v) => factory.LocalCreate62(v);
            });
            services.AddScoped<Create63Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskVoidTaskString v) => factory.LocalCreate63(v);
            });
            services.AddScoped<Create64Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskTrueBoolBool v) => factory.LocalCreate64(v);
            });
            services.AddScoped<Create65Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskTrueBoolString v) => factory.LocalCreate65(v);
            });
            services.AddScoped<Create66Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskTrueBoolTaskBool v) => factory.LocalCreate66(v);
            });
            services.AddScoped<Create67Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskTrueBoolTaskString v) => factory.LocalCreate67(v);
            });
            services.AddScoped<Create68Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskFalseBoolBool v) => factory.LocalCreate68(v);
            });
            services.AddScoped<Create69Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskFalseBoolString v) => factory.LocalCreate69(v);
            });
            services.AddScoped<Create70Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskFalseBoolTaskBool v) => factory.LocalCreate70(v);
            });
            services.AddScoped<Create71Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskFalseBoolTaskString v) => factory.LocalCreate71(v);
            });
            services.AddScoped<Create72Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteVoidBoolRemote v) => factory.LocalCreate72(v);
            });
            services.AddScoped<Create73Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteVoidStringRemote v) => factory.LocalCreate73(v);
            });
            services.AddScoped<Create74Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteVoidTaskBoolRemote v) => factory.LocalCreate74(v);
            });
            services.AddScoped<Create75Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteVoidTaskStringRemote v) => factory.LocalCreate75(v);
            });
            services.AddScoped<Create76Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTrueBoolBoolRemote v) => factory.LocalCreate76(v);
            });
            services.AddScoped<Create77Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTrueBoolStringRemote v) => factory.LocalCreate77(v);
            });
            services.AddScoped<Create78Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTrueBoolTaskBoolRemote v) => factory.LocalCreate78(v);
            });
            services.AddScoped<Create79Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTrueBoolTaskStringRemote v) => factory.LocalCreate79(v);
            });
            services.AddScoped<Create80Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteFalseBoolBoolRemote v) => factory.LocalCreate80(v);
            });
            services.AddScoped<Create81Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteFalseBoolStringRemote v) => factory.LocalCreate81(v);
            });
            services.AddScoped<Create82Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteFalseBoolTaskBoolRemote v) => factory.LocalCreate82(v);
            });
            services.AddScoped<Create83Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteFalseBoolTaskStringRemote v) => factory.LocalCreate83(v);
            });
            services.AddScoped<Create84Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskVoidBoolRemote v) => factory.LocalCreate84(v);
            });
            services.AddScoped<Create85Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskVoidStringRemote v) => factory.LocalCreate85(v);
            });
            services.AddScoped<Create86Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskVoidTaskBoolRemote v) => factory.LocalCreate86(v);
            });
            services.AddScoped<Create87Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskVoidTaskStringRemote v) => factory.LocalCreate87(v);
            });
            services.AddScoped<Create88Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskTrueBoolBoolRemote v) => factory.LocalCreate88(v);
            });
            services.AddScoped<Create89Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskTrueBoolStringRemote v) => factory.LocalCreate89(v);
            });
            services.AddScoped<Create90Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskTrueBoolTaskBoolRemote v) => factory.LocalCreate90(v);
            });
            services.AddScoped<Create91Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskTrueBoolTaskStringRemote v) => factory.LocalCreate91(v);
            });
            services.AddScoped<Create92Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskFalseBoolBoolRemote v) => factory.LocalCreate92(v);
            });
            services.AddScoped<Create93Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskFalseBoolStringRemote v) => factory.LocalCreate93(v);
            });
            services.AddScoped<Create94Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskFalseBoolTaskBoolRemote v) => factory.LocalCreate94(v);
            });
            services.AddScoped<Create95Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskFalseBoolTaskStringRemote v) => factory.LocalCreate95(v);
            });
            services.AddScoped<Create120Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (VoidBoolRemoteDeny v) => factory.LocalCreate120(v);
            });
            services.AddScoped<Create121Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (VoidStringRemoteDeny v) => factory.LocalCreate121(v);
            });
            services.AddScoped<Create122Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (VoidTaskBoolRemoteDeny v) => factory.LocalCreate122(v);
            });
            services.AddScoped<Create123Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (VoidTaskStringRemoteDeny v) => factory.LocalCreate123(v);
            });
            services.AddScoped<Create124Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TrueBoolBoolRemoteDeny v) => factory.LocalCreate124(v);
            });
            services.AddScoped<Create125Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TrueBoolStringRemoteDeny v) => factory.LocalCreate125(v);
            });
            services.AddScoped<Create126Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TrueBoolTaskBoolRemoteDeny v) => factory.LocalCreate126(v);
            });
            services.AddScoped<Create127Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TrueBoolTaskStringRemoteDeny v) => factory.LocalCreate127(v);
            });
            services.AddScoped<Create128Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (FalseBoolBoolRemoteDeny v) => factory.LocalCreate128(v);
            });
            services.AddScoped<Create129Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (FalseBoolStringRemoteDeny v) => factory.LocalCreate129(v);
            });
            services.AddScoped<Create130Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (FalseBoolTaskBoolRemoteDeny v) => factory.LocalCreate130(v);
            });
            services.AddScoped<Create131Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (FalseBoolTaskStringRemoteDeny v) => factory.LocalCreate131(v);
            });
            services.AddScoped<Create132Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TaskVoidBoolRemoteDeny v) => factory.LocalCreate132(v);
            });
            services.AddScoped<Create133Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TaskVoidStringRemoteDeny v) => factory.LocalCreate133(v);
            });
            services.AddScoped<Create134Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TaskVoidTaskBoolRemoteDeny v) => factory.LocalCreate134(v);
            });
            services.AddScoped<Create135Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TaskVoidTaskStringRemoteDeny v) => factory.LocalCreate135(v);
            });
            services.AddScoped<Create136Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TaskTrueBoolBoolRemoteDeny v) => factory.LocalCreate136(v);
            });
            services.AddScoped<Create137Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TaskTrueBoolStringRemoteDeny v) => factory.LocalCreate137(v);
            });
            services.AddScoped<Create138Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TaskTrueBoolTaskBoolRemoteDeny v) => factory.LocalCreate138(v);
            });
            services.AddScoped<Create139Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TaskTrueBoolTaskStringRemoteDeny v) => factory.LocalCreate139(v);
            });
            services.AddScoped<Create140Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TaskFalseBoolBoolRemoteDeny v) => factory.LocalCreate140(v);
            });
            services.AddScoped<Create141Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TaskFalseBoolStringRemoteDeny v) => factory.LocalCreate141(v);
            });
            services.AddScoped<Create142Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TaskFalseBoolTaskBoolRemoteDeny v) => factory.LocalCreate142(v);
            });
            services.AddScoped<Create143Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (TaskFalseBoolTaskStringRemoteDeny v) => factory.LocalCreate143(v);
            });
            services.AddScoped<Create144Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteVoidBoolDeny v) => factory.LocalCreate144(v);
            });
            services.AddScoped<Create145Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteVoidStringDeny v) => factory.LocalCreate145(v);
            });
            services.AddScoped<Create146Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteVoidTaskBoolDeny v) => factory.LocalCreate146(v);
            });
            services.AddScoped<Create147Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteVoidTaskStringDeny v) => factory.LocalCreate147(v);
            });
            services.AddScoped<Create148Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTrueBoolBoolDeny v) => factory.LocalCreate148(v);
            });
            services.AddScoped<Create149Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTrueBoolStringDeny v) => factory.LocalCreate149(v);
            });
            services.AddScoped<Create150Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTrueBoolTaskBoolDeny v) => factory.LocalCreate150(v);
            });
            services.AddScoped<Create151Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTrueBoolTaskStringDeny v) => factory.LocalCreate151(v);
            });
            services.AddScoped<Create152Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteFalseBoolBoolDeny v) => factory.LocalCreate152(v);
            });
            services.AddScoped<Create153Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteFalseBoolStringDeny v) => factory.LocalCreate153(v);
            });
            services.AddScoped<Create154Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteFalseBoolTaskBoolDeny v) => factory.LocalCreate154(v);
            });
            services.AddScoped<Create155Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteFalseBoolTaskStringDeny v) => factory.LocalCreate155(v);
            });
            services.AddScoped<Create156Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskVoidBoolDeny v) => factory.LocalCreate156(v);
            });
            services.AddScoped<Create157Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskVoidStringDeny v) => factory.LocalCreate157(v);
            });
            services.AddScoped<Create158Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskVoidTaskBoolDeny v) => factory.LocalCreate158(v);
            });
            services.AddScoped<Create159Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskVoidTaskStringDeny v) => factory.LocalCreate159(v);
            });
            services.AddScoped<Create160Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskTrueBoolBoolDeny v) => factory.LocalCreate160(v);
            });
            services.AddScoped<Create161Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskTrueBoolStringDeny v) => factory.LocalCreate161(v);
            });
            services.AddScoped<Create162Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskTrueBoolTaskBoolDeny v) => factory.LocalCreate162(v);
            });
            services.AddScoped<Create163Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskTrueBoolTaskStringDeny v) => factory.LocalCreate163(v);
            });
            services.AddScoped<Create164Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskFalseBoolBoolDeny v) => factory.LocalCreate164(v);
            });
            services.AddScoped<Create165Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskFalseBoolStringDeny v) => factory.LocalCreate165(v);
            });
            services.AddScoped<Create166Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskFalseBoolTaskBoolDeny v) => factory.LocalCreate166(v);
            });
            services.AddScoped<Create167Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskFalseBoolTaskStringDeny v) => factory.LocalCreate167(v);
            });
            services.AddScoped<Create168Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteVoidBoolRemoteDeny v) => factory.LocalCreate168(v);
            });
            services.AddScoped<Create169Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteVoidStringRemoteDeny v) => factory.LocalCreate169(v);
            });
            services.AddScoped<Create170Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteVoidTaskBoolRemoteDeny v) => factory.LocalCreate170(v);
            });
            services.AddScoped<Create171Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteVoidTaskStringRemoteDeny v) => factory.LocalCreate171(v);
            });
            services.AddScoped<Create172Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTrueBoolBoolRemoteDeny v) => factory.LocalCreate172(v);
            });
            services.AddScoped<Create173Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTrueBoolStringRemoteDeny v) => factory.LocalCreate173(v);
            });
            services.AddScoped<Create174Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTrueBoolTaskBoolRemoteDeny v) => factory.LocalCreate174(v);
            });
            services.AddScoped<Create175Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTrueBoolTaskStringRemoteDeny v) => factory.LocalCreate175(v);
            });
            services.AddScoped<Create176Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteFalseBoolBoolRemoteDeny v) => factory.LocalCreate176(v);
            });
            services.AddScoped<Create177Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteFalseBoolStringRemoteDeny v) => factory.LocalCreate177(v);
            });
            services.AddScoped<Create178Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteFalseBoolTaskBoolRemoteDeny v) => factory.LocalCreate178(v);
            });
            services.AddScoped<Create179Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteFalseBoolTaskStringRemoteDeny v) => factory.LocalCreate179(v);
            });
            services.AddScoped<Create180Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskVoidBoolRemoteDeny v) => factory.LocalCreate180(v);
            });
            services.AddScoped<Create181Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskVoidStringRemoteDeny v) => factory.LocalCreate181(v);
            });
            services.AddScoped<Create182Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskVoidTaskBoolRemoteDeny v) => factory.LocalCreate182(v);
            });
            services.AddScoped<Create183Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskVoidTaskStringRemoteDeny v) => factory.LocalCreate183(v);
            });
            services.AddScoped<Create184Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskTrueBoolBoolRemoteDeny v) => factory.LocalCreate184(v);
            });
            services.AddScoped<Create185Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskTrueBoolStringRemoteDeny v) => factory.LocalCreate185(v);
            });
            services.AddScoped<Create186Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskTrueBoolTaskBoolRemoteDeny v) => factory.LocalCreate186(v);
            });
            services.AddScoped<Create187Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskTrueBoolTaskStringRemoteDeny v) => factory.LocalCreate187(v);
            });
            services.AddScoped<Create188Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskFalseBoolBoolRemoteDeny v) => factory.LocalCreate188(v);
            });
            services.AddScoped<Create189Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskFalseBoolStringRemoteDeny v) => factory.LocalCreate189(v);
            });
            services.AddScoped<Create190Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskFalseBoolTaskBoolRemoteDeny v) => factory.LocalCreate190(v);
            });
            services.AddScoped<Create191Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (RemoteTaskFalseBoolTaskStringRemoteDeny v) => factory.LocalCreate191(v);
            });
            services.AddScoped<Save24Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave24(target, v);
            });
            services.AddScoped<Save25Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave25(target, v);
            });
            services.AddScoped<Save26Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave26(target, v);
            });
            services.AddScoped<Save27Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave27(target, v);
            });
            services.AddScoped<Save28Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave28(target, v);
            });
            services.AddScoped<Save29Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave29(target, v);
            });
            services.AddScoped<Save30Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave30(target, v);
            });
            services.AddScoped<Save31Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave31(target, v);
            });
            services.AddScoped<Save32Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave32(target, v);
            });
            services.AddScoped<Save33Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave33(target, v);
            });
            services.AddScoped<Save34Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave34(target, v);
            });
            services.AddScoped<Save35Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave35(target, v);
            });
            services.AddScoped<Save36Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave36(target, v);
            });
            services.AddScoped<Save37Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave37(target, v);
            });
            services.AddScoped<Save38Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave38(target, v);
            });
            services.AddScoped<Save39Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave39(target, v);
            });
            services.AddScoped<Save40Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave40(target, v);
            });
            services.AddScoped<Save41Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave41(target, v);
            });
            services.AddScoped<Save42Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave42(target, v);
            });
            services.AddScoped<Save43Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave43(target, v);
            });
            services.AddScoped<Save44Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave44(target, v);
            });
            services.AddScoped<Save45Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave45(target, v);
            });
            services.AddScoped<Save46Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave46(target, v);
            });
            services.AddScoped<Save47Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave47(target, v);
            });
            services.AddScoped<Save48Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave48(target, v);
            });
            services.AddScoped<Save49Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave49(target, v);
            });
            services.AddScoped<Save50Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave50(target, v);
            });
            services.AddScoped<Save51Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave51(target, v);
            });
            services.AddScoped<Save52Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave52(target, v);
            });
            services.AddScoped<Save53Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave53(target, v);
            });
            services.AddScoped<Save54Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave54(target, v);
            });
            services.AddScoped<Save55Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave55(target, v);
            });
            services.AddScoped<Save56Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave56(target, v);
            });
            services.AddScoped<Save57Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave57(target, v);
            });
            services.AddScoped<Save58Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave58(target, v);
            });
            services.AddScoped<Save59Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave59(target, v);
            });
            services.AddScoped<Save60Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave60(target, v);
            });
            services.AddScoped<Save61Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave61(target, v);
            });
            services.AddScoped<Save62Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave62(target, v);
            });
            services.AddScoped<Save63Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave63(target, v);
            });
            services.AddScoped<Save64Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave64(target, v);
            });
            services.AddScoped<Save65Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave65(target, v);
            });
            services.AddScoped<Save66Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave66(target, v);
            });
            services.AddScoped<Save67Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave67(target, v);
            });
            services.AddScoped<Save68Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave68(target, v);
            });
            services.AddScoped<Save69Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave69(target, v);
            });
            services.AddScoped<Save70Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave70(target, v);
            });
            services.AddScoped<Save71Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave71(target, v);
            });
            services.AddScoped<Save72Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave72(target, v);
            });
            services.AddScoped<Save73Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave73(target, v);
            });
            services.AddScoped<Save74Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave74(target, v);
            });
            services.AddScoped<Save75Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave75(target, v);
            });
            services.AddScoped<Save76Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave76(target, v);
            });
            services.AddScoped<Save77Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave77(target, v);
            });
            services.AddScoped<Save78Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave78(target, v);
            });
            services.AddScoped<Save79Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave79(target, v);
            });
            services.AddScoped<Save80Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave80(target, v);
            });
            services.AddScoped<Save81Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave81(target, v);
            });
            services.AddScoped<Save82Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave82(target, v);
            });
            services.AddScoped<Save83Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave83(target, v);
            });
            services.AddScoped<Save84Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave84(target, v);
            });
            services.AddScoped<Save85Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave85(target, v);
            });
            services.AddScoped<Save86Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave86(target, v);
            });
            services.AddScoped<Save87Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave87(target, v);
            });
            services.AddScoped<Save88Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave88(target, v);
            });
            services.AddScoped<Save89Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave89(target, v);
            });
            services.AddScoped<Save90Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave90(target, v);
            });
            services.AddScoped<Save91Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave91(target, v);
            });
            services.AddScoped<Save92Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave92(target, v);
            });
            services.AddScoped<Save93Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave93(target, v);
            });
            services.AddScoped<Save94Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave94(target, v);
            });
            services.AddScoped<Save95Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave95(target, v);
            });
            services.AddScoped<Save120Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave120(target, v);
            });
            services.AddScoped<Save121Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave121(target, v);
            });
            services.AddScoped<Save122Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave122(target, v);
            });
            services.AddScoped<Save123Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave123(target, v);
            });
            services.AddScoped<Save124Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave124(target, v);
            });
            services.AddScoped<Save125Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave125(target, v);
            });
            services.AddScoped<Save126Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave126(target, v);
            });
            services.AddScoped<Save127Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave127(target, v);
            });
            services.AddScoped<Save128Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave128(target, v);
            });
            services.AddScoped<Save129Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave129(target, v);
            });
            services.AddScoped<Save130Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave130(target, v);
            });
            services.AddScoped<Save131Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave131(target, v);
            });
            services.AddScoped<Save132Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave132(target, v);
            });
            services.AddScoped<Save133Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave133(target, v);
            });
            services.AddScoped<Save134Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave134(target, v);
            });
            services.AddScoped<Save135Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave135(target, v);
            });
            services.AddScoped<Save136Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave136(target, v);
            });
            services.AddScoped<Save137Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave137(target, v);
            });
            services.AddScoped<Save138Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave138(target, v);
            });
            services.AddScoped<Save139Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave139(target, v);
            });
            services.AddScoped<Save140Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave140(target, v);
            });
            services.AddScoped<Save141Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave141(target, v);
            });
            services.AddScoped<Save142Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave142(target, v);
            });
            services.AddScoped<Save143Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave143(target, v);
            });
            services.AddScoped<Save144Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave144(target, v);
            });
            services.AddScoped<Save145Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave145(target, v);
            });
            services.AddScoped<Save146Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave146(target, v);
            });
            services.AddScoped<Save147Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave147(target, v);
            });
            services.AddScoped<Save148Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave148(target, v);
            });
            services.AddScoped<Save149Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave149(target, v);
            });
            services.AddScoped<Save150Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave150(target, v);
            });
            services.AddScoped<Save151Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave151(target, v);
            });
            services.AddScoped<Save152Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave152(target, v);
            });
            services.AddScoped<Save153Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave153(target, v);
            });
            services.AddScoped<Save154Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave154(target, v);
            });
            services.AddScoped<Save155Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave155(target, v);
            });
            services.AddScoped<Save156Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave156(target, v);
            });
            services.AddScoped<Save157Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave157(target, v);
            });
            services.AddScoped<Save158Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave158(target, v);
            });
            services.AddScoped<Save159Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave159(target, v);
            });
            services.AddScoped<Save160Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave160(target, v);
            });
            services.AddScoped<Save161Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave161(target, v);
            });
            services.AddScoped<Save162Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave162(target, v);
            });
            services.AddScoped<Save163Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave163(target, v);
            });
            services.AddScoped<Save164Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave164(target, v);
            });
            services.AddScoped<Save165Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave165(target, v);
            });
            services.AddScoped<Save166Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave166(target, v);
            });
            services.AddScoped<Save167Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave167(target, v);
            });
            services.AddScoped<Save168Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave168(target, v);
            });
            services.AddScoped<Save169Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave169(target, v);
            });
            services.AddScoped<Save170Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave170(target, v);
            });
            services.AddScoped<Save171Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave171(target, v);
            });
            services.AddScoped<Save172Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave172(target, v);
            });
            services.AddScoped<Save173Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave173(target, v);
            });
            services.AddScoped<Save174Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave174(target, v);
            });
            services.AddScoped<Save175Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave175(target, v);
            });
            services.AddScoped<Save176Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave176(target, v);
            });
            services.AddScoped<Save177Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave177(target, v);
            });
            services.AddScoped<Save178Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave178(target, v);
            });
            services.AddScoped<Save179Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave179(target, v);
            });
            services.AddScoped<Save180Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave180(target, v);
            });
            services.AddScoped<Save181Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave181(target, v);
            });
            services.AddScoped<Save182Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave182(target, v);
            });
            services.AddScoped<Save183Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave183(target, v);
            });
            services.AddScoped<Save184Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave184(target, v);
            });
            services.AddScoped<Save185Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave185(target, v);
            });
            services.AddScoped<Save186Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave186(target, v);
            });
            services.AddScoped<Save187Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave187(target, v);
            });
            services.AddScoped<Save188Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave188(target, v);
            });
            services.AddScoped<Save189Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave189(target, v);
            });
            services.AddScoped<Save190Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave190(target, v);
            });
            services.AddScoped<Save191Delegate>(cc =>
            {
                var factory = cc.GetRequiredService<AuthorizedAllCombinationsFactory>();
                return (target, v) => factory.LocalSave191(target, v);
            });
            services.AddScoped<IFactoryEditBase<AuthorizedAllCombinations>, AuthorizedAllCombinationsFactory>();
        }
    }
}