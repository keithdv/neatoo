using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Neatoo.AuthorizationRules;
using Neatoo.Internal;
using Neatoo.Portal;
using Neatoo.Portal.Internal;
using System.Reflection;
using System.Reflection.Metadata;
using static Neatoo.UnitTest.Portal.AuthorizationAllCombinationTests;

namespace Neatoo.UnitTest.Portal;

[TestClass]
public class AuthorizationAllCombinationTests
{
    public abstract class IDd
    {
        public string UniqueIdentifier { get; set; } = Guid.NewGuid().ToString();

        public override bool Equals(object? obj)
        {
            if (obj is IDd idd)
            {
                return UniqueIdentifier == idd.UniqueIdentifier;
            }
            return base.Equals(obj);
        }

    }

    public class VoidBool : IDd { }
    public class VoidString : IDd { }
    public class VoidTaskBool : IDd { }
    public class VoidTaskString : IDd { }

    public class TrueBoolBool : IDd { }
    public class TrueBoolString : IDd { }
    public class TrueBoolTaskBool : IDd { }
    public class TrueBoolTaskString : IDd { }

    public class FalseBoolBool : IDd { }
    public class FalseBoolString : IDd { }
    public class FalseBoolTaskBool : IDd { }
    public class FalseBoolTaskString : IDd { }

    public class TaskVoidBool : IDd { }
    public class TaskVoidString : IDd { }
    public class TaskVoidTaskBool : IDd { }
    public class TaskVoidTaskString : IDd { }

    public class TaskTrueBoolBool : IDd { }
    public class TaskTrueBoolString : IDd { }
    public class TaskTrueBoolTaskBool : IDd { }
    public class TaskTrueBoolTaskString : IDd { }

    public class TaskFalseBoolBool : IDd { }
    public class TaskFalseBoolString : IDd { }
    public class TaskFalseBoolTaskBool : IDd { }
    public class TaskFalseBoolTaskString : IDd { }

    public class VoidBoolRemote : IDd { }
    public class VoidStringRemote : IDd { }
    public class VoidTaskBoolRemote : IDd { }
    public class VoidTaskStringRemote : IDd { }

    public class TrueBoolBoolRemote : IDd { }
    public class TrueBoolStringRemote : IDd { }
    public class TrueBoolTaskBoolRemote : IDd { }
    public class TrueBoolTaskStringRemote : IDd { }

    public class FalseBoolBoolRemote : IDd { }
    public class FalseBoolStringRemote : IDd { }
    public class FalseBoolTaskBoolRemote : IDd { }
    public class FalseBoolTaskStringRemote : IDd { }

    public class TaskVoidBoolRemote : IDd { }
    public class TaskVoidStringRemote : IDd { }
    public class TaskVoidTaskBoolRemote : IDd { }
    public class TaskVoidTaskStringRemote : IDd { }

    public class TaskTrueBoolBoolRemote : IDd { }
    public class TaskTrueBoolStringRemote : IDd { }
    public class TaskTrueBoolTaskBoolRemote : IDd { }
    public class TaskTrueBoolTaskStringRemote : IDd { }

    public class TaskFalseBoolBoolRemote : IDd { }
    public class TaskFalseBoolStringRemote : IDd { }
    public class TaskFalseBoolTaskBoolRemote : IDd { }
    public class TaskFalseBoolTaskStringRemote : IDd { }

    public class RemoteVoidBool : IDd { }
    public class RemoteVoidString : IDd { }
    public class RemoteVoidTaskBool : IDd { }
    public class RemoteVoidTaskString : IDd { }

    public class RemoteTrueBoolBool : IDd { }
    public class RemoteTrueBoolString : IDd { }
    public class RemoteTrueBoolTaskBool : IDd { }
    public class RemoteTrueBoolTaskString : IDd { }

    public class RemoteFalseBoolBool : IDd { }
    public class RemoteFalseBoolString : IDd { }
    public class RemoteFalseBoolTaskBool : IDd { }
    public class RemoteFalseBoolTaskString : IDd { }

    public class RemoteTaskVoidBool : IDd { }
    public class RemoteTaskVoidString : IDd { }
    public class RemoteTaskVoidTaskBool : IDd { }
    public class RemoteTaskVoidTaskString : IDd { }

    public class RemoteTaskTrueBoolBool : IDd { }
    public class RemoteTaskTrueBoolString : IDd { }
    public class RemoteTaskTrueBoolTaskBool : IDd { }
    public class RemoteTaskTrueBoolTaskString : IDd { }

    public class RemoteTaskFalseBoolBool : IDd { }
    public class RemoteTaskFalseBoolString : IDd { }
    public class RemoteTaskFalseBoolTaskBool : IDd { }
    public class RemoteTaskFalseBoolTaskString : IDd { }

    public class RemoteVoidBoolRemote : IDd { }
    public class RemoteVoidStringRemote : IDd { }
    public class RemoteVoidTaskBoolRemote : IDd { }
    public class RemoteVoidTaskStringRemote : IDd { }

    public class RemoteTrueBoolBoolRemote : IDd { }
    public class RemoteTrueBoolStringRemote : IDd { }
    public class RemoteTrueBoolTaskBoolRemote : IDd { }
    public class RemoteTrueBoolTaskStringRemote : IDd { }

    public class RemoteFalseBoolBoolRemote : IDd { }
    public class RemoteFalseBoolStringRemote : IDd { }
    public class RemoteFalseBoolTaskBoolRemote : IDd { }
    public class RemoteFalseBoolTaskStringRemote : IDd { }

    public class RemoteTaskVoidBoolRemote : IDd { }
    public class RemoteTaskVoidStringRemote : IDd { }
    public class RemoteTaskVoidTaskBoolRemote : IDd { }
    public class RemoteTaskVoidTaskStringRemote : IDd { }

    public class RemoteTaskTrueBoolBoolRemote : IDd { }
    public class RemoteTaskTrueBoolStringRemote : IDd { }
    public class RemoteTaskTrueBoolTaskBoolRemote : IDd { }
    public class RemoteTaskTrueBoolTaskStringRemote : IDd { }

    public class RemoteTaskFalseBoolBoolRemote : IDd { }
    public class RemoteTaskFalseBoolStringRemote : IDd { }
    public class RemoteTaskFalseBoolTaskBoolRemote : IDd { }
    public class RemoteTaskFalseBoolTaskStringRemote : IDd { }

    public class VoidBoolDeny : IDd { }
    public class VoidStringDeny : IDd { }
    public class VoidTaskBoolDeny : IDd { }
    public class VoidTaskStringDeny : IDd { }

    public class TrueBoolBoolDeny : IDd { }
    public class TrueBoolStringDeny : IDd { }
    public class TrueBoolTaskBoolDeny : IDd { }
    public class TrueBoolTaskStringDeny : IDd { }

    public class FalseBoolBoolDeny : IDd { }
    public class FalseBoolStringDeny : IDd { }
    public class FalseBoolTaskBoolDeny : IDd { }
    public class FalseBoolTaskStringDeny : IDd { }

    public class TaskVoidBoolDeny : IDd { }
    public class TaskVoidStringDeny : IDd { }
    public class TaskVoidTaskBoolDeny : IDd { }
    public class TaskVoidTaskStringDeny : IDd { }

    public class TaskTrueBoolBoolDeny : IDd { }
    public class TaskTrueBoolStringDeny : IDd { }
    public class TaskTrueBoolTaskBoolDeny : IDd { }
    public class TaskTrueBoolTaskStringDeny : IDd { }

    public class TaskFalseBoolBoolDeny : IDd { }
    public class TaskFalseBoolStringDeny : IDd { }
    public class TaskFalseBoolTaskBoolDeny : IDd { }
    public class TaskFalseBoolTaskStringDeny : IDd { }

    public class VoidBoolRemoteDeny : IDd { }
    public class VoidStringRemoteDeny : IDd { }
    public class VoidTaskBoolRemoteDeny : IDd { }
    public class VoidTaskStringRemoteDeny : IDd { }

    public class TrueBoolBoolRemoteDeny : IDd { }
    public class TrueBoolStringRemoteDeny : IDd { }
    public class TrueBoolTaskBoolRemoteDeny : IDd { }
    public class TrueBoolTaskStringRemoteDeny : IDd { }

    public class FalseBoolBoolRemoteDeny : IDd { }
    public class FalseBoolStringRemoteDeny : IDd { }
    public class FalseBoolTaskBoolRemoteDeny : IDd { }
    public class FalseBoolTaskStringRemoteDeny : IDd { }

    public class TaskVoidBoolRemoteDeny : IDd { }
    public class TaskVoidStringRemoteDeny : IDd { }
    public class TaskVoidTaskBoolRemoteDeny : IDd { }
    public class TaskVoidTaskStringRemoteDeny : IDd { }

    public class TaskTrueBoolBoolRemoteDeny : IDd { }
    public class TaskTrueBoolStringRemoteDeny : IDd { }
    public class TaskTrueBoolTaskBoolRemoteDeny : IDd { }
    public class TaskTrueBoolTaskStringRemoteDeny : IDd { }

    public class TaskFalseBoolBoolRemoteDeny : IDd { }
    public class TaskFalseBoolStringRemoteDeny : IDd { }
    public class TaskFalseBoolTaskBoolRemoteDeny : IDd { }
    public class TaskFalseBoolTaskStringRemoteDeny : IDd { }

    public class RemoteVoidBoolDeny : IDd { }
    public class RemoteVoidStringDeny : IDd { }
    public class RemoteVoidTaskBoolDeny : IDd { }
    public class RemoteVoidTaskStringDeny : IDd { }

    public class RemoteTrueBoolBoolDeny : IDd { }
    public class RemoteTrueBoolStringDeny : IDd { }
    public class RemoteTrueBoolTaskBoolDeny : IDd { }
    public class RemoteTrueBoolTaskStringDeny : IDd { }

    public class RemoteFalseBoolBoolDeny : IDd { }
    public class RemoteFalseBoolStringDeny : IDd { }
    public class RemoteFalseBoolTaskBoolDeny : IDd { }
    public class RemoteFalseBoolTaskStringDeny : IDd { }

    public class RemoteTaskVoidBoolDeny : IDd { }
    public class RemoteTaskVoidStringDeny : IDd { }
    public class RemoteTaskVoidTaskBoolDeny : IDd { }
    public class RemoteTaskVoidTaskStringDeny : IDd { }

    public class RemoteTaskTrueBoolBoolDeny : IDd { }
    public class RemoteTaskTrueBoolStringDeny : IDd { }
    public class RemoteTaskTrueBoolTaskBoolDeny : IDd { }
    public class RemoteTaskTrueBoolTaskStringDeny : IDd { }

    public class RemoteTaskFalseBoolBoolDeny : IDd { }
    public class RemoteTaskFalseBoolStringDeny : IDd { }
    public class RemoteTaskFalseBoolTaskBoolDeny : IDd { }
    public class RemoteTaskFalseBoolTaskStringDeny : IDd { }

    public class RemoteVoidBoolRemoteDeny : IDd { }
    public class RemoteVoidStringRemoteDeny : IDd { }
    public class RemoteVoidTaskBoolRemoteDeny : IDd { }
    public class RemoteVoidTaskStringRemoteDeny : IDd { }

    public class RemoteTrueBoolBoolRemoteDeny : IDd { }
    public class RemoteTrueBoolStringRemoteDeny : IDd { }
    public class RemoteTrueBoolTaskBoolRemoteDeny : IDd { }
    public class RemoteTrueBoolTaskStringRemoteDeny : IDd { }

    public class RemoteFalseBoolBoolRemoteDeny : IDd { }
    public class RemoteFalseBoolStringRemoteDeny : IDd { }
    public class RemoteFalseBoolTaskBoolRemoteDeny : IDd { }
    public class RemoteFalseBoolTaskStringRemoteDeny : IDd { }

    public class RemoteTaskVoidBoolRemoteDeny : IDd { }
    public class RemoteTaskVoidStringRemoteDeny : IDd { }
    public class RemoteTaskVoidTaskBoolRemoteDeny : IDd { }
    public class RemoteTaskVoidTaskStringRemoteDeny : IDd { }

    public class RemoteTaskTrueBoolBoolRemoteDeny : IDd { }
    public class RemoteTaskTrueBoolStringRemoteDeny : IDd { }
    public class RemoteTaskTrueBoolTaskBoolRemoteDeny : IDd { }
    public class RemoteTaskTrueBoolTaskStringRemoteDeny : IDd { }

    public class RemoteTaskFalseBoolBoolRemoteDeny : IDd { }
    public class RemoteTaskFalseBoolStringRemoteDeny : IDd { }
    public class RemoteTaskFalseBoolTaskBoolRemoteDeny : IDd { }
    public class RemoteTaskFalseBoolTaskStringRemoteDeny : IDd { }

    public class AuthorizationAllCombinations
    {
        public List<object> ReadReceived { get; set; } = new List<object>();
        public List<object> WriteReceived { get; set; } = new List<object>();

        [Authorize(DataMapperMethodType.Read)]
        public bool Read(VoidBool v) { ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Read)]
        public string? Read(VoidString v) { ReadReceived.Add(v); return null; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(VoidTaskBool v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(VoidTaskString v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public bool Write(VoidBool v) { WriteReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public string? Write(VoidString v) { WriteReceived.Add(v); return null; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(VoidTaskBool v) { await Task.Delay(2); WriteReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(VoidTaskString v) { await Task.Delay(2); WriteReceived.Add(v); return string.Empty; }

        [Authorize(DataMapperMethodType.Read)]
        public bool Read(TrueBoolBool v) { ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Read)]
        public string? Read(TrueBoolString v) { ReadReceived.Add(v); return null; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TrueBoolTaskBool v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TrueBoolTaskString v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public bool Write(TrueBoolBool v) { WriteReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public string? Write(TrueBoolString v) { WriteReceived.Add(v); return null; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(TrueBoolTaskBool v) { await Task.Delay(2); WriteReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(TrueBoolTaskString v) { await Task.Delay(2); WriteReceived.Add(v); return string.Empty; }

        [Authorize(DataMapperMethodType.Read)]
        public bool Read(FalseBoolBool v) { ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Read)]
        public string? Read(FalseBoolString v) { ReadReceived.Add(v); return null; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(FalseBoolTaskBool v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(FalseBoolTaskString v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public bool Write(FalseBoolBool v) { WriteReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public string? Write(FalseBoolString v) { WriteReceived.Add(v); return null; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(FalseBoolTaskBool v) { await Task.Delay(2); WriteReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(FalseBoolTaskString v) { await Task.Delay(2); WriteReceived.Add(v); return string.Empty; }

        [Authorize(DataMapperMethodType.Read)]
        public bool Read(TaskVoidBool v) { ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Read)]
        public string? Read(TaskVoidString v) { ReadReceived.Add(v); return null; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TaskVoidTaskBool v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TaskVoidTaskString v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public bool Write(TaskVoidBool v) { WriteReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public string? Write(TaskVoidString v) { WriteReceived.Add(v); return null; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(TaskVoidTaskBool v) { await Task.Delay(2); WriteReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(TaskVoidTaskString v) { await Task.Delay(2); WriteReceived.Add(v); return string.Empty; }

        [Authorize(DataMapperMethodType.Read)]
        public bool Read(TaskTrueBoolBool v) { ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Read)]
        public string? Read(TaskTrueBoolString v) { ReadReceived.Add(v); return null; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TaskTrueBoolTaskBool v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TaskTrueBoolTaskString v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public bool Write(TaskTrueBoolBool v) { WriteReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public string? Write(TaskTrueBoolString v) { WriteReceived.Add(v); return null; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(TaskTrueBoolTaskBool v) { await Task.Delay(2); WriteReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(TaskTrueBoolTaskString v) { await Task.Delay(2); WriteReceived.Add(v); return string.Empty; }

        [Authorize(DataMapperMethodType.Read)]
        public bool Read(TaskFalseBoolBool v) { ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Read)]
        public string? Read(TaskFalseBoolString v) { ReadReceived.Add(v); return null; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TaskFalseBoolTaskBool v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TaskFalseBoolTaskString v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public bool Write(TaskFalseBoolBool v) { WriteReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public string? Write(TaskFalseBoolString v) { WriteReceived.Add(v); return null; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(TaskFalseBoolTaskBool v) { await Task.Delay(2); WriteReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(TaskFalseBoolTaskString v) { await Task.Delay(2); WriteReceived.Add(v); return string.Empty; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public bool Read(VoidBoolRemote v) { ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public string? Read(VoidStringRemote v) { ReadReceived.Add(v); return null; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(VoidTaskBoolRemote v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(VoidTaskStringRemote v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public bool Write(VoidBoolRemote v) { WriteReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public string? Write(VoidStringRemote v) { WriteReceived.Add(v); return null; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(VoidTaskBoolRemote v) { await Task.Delay(2); WriteReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(VoidTaskStringRemote v) { await Task.Delay(2); WriteReceived.Add(v); return string.Empty; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public bool Read(TrueBoolBoolRemote v) { ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public string? Read(TrueBoolStringRemote v) { ReadReceived.Add(v); return null; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TrueBoolTaskBoolRemote v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TrueBoolTaskStringRemote v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public bool Write(TrueBoolBoolRemote v) { WriteReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public string? Write(TrueBoolStringRemote v) { WriteReceived.Add(v); return null; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(TrueBoolTaskBoolRemote v) { await Task.Delay(2); WriteReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(TrueBoolTaskStringRemote v) { await Task.Delay(2); WriteReceived.Add(v); return string.Empty; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public bool Read(FalseBoolBoolRemote v) { ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public string? Read(FalseBoolStringRemote v) { ReadReceived.Add(v); return null; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(FalseBoolTaskBoolRemote v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(FalseBoolTaskStringRemote v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public bool Write(FalseBoolBoolRemote v) { WriteReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public string? Write(FalseBoolStringRemote v) { WriteReceived.Add(v); return null; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(FalseBoolTaskBoolRemote v) { await Task.Delay(2); WriteReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(FalseBoolTaskStringRemote v) { await Task.Delay(2); WriteReceived.Add(v); return string.Empty; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public bool Read(TaskVoidBoolRemote v) { ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public string? Read(TaskVoidStringRemote v) { ReadReceived.Add(v); return null; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TaskVoidTaskBoolRemote v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TaskVoidTaskStringRemote v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public bool Write(TaskVoidBoolRemote v) { WriteReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public string? Write(TaskVoidStringRemote v) { WriteReceived.Add(v); return null; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(TaskVoidTaskBoolRemote v) { await Task.Delay(2); WriteReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(TaskVoidTaskStringRemote v) { await Task.Delay(2); WriteReceived.Add(v); return string.Empty; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public bool Read(TaskTrueBoolBoolRemote v) { ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public string? Read(TaskTrueBoolStringRemote v) { ReadReceived.Add(v); return null; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TaskTrueBoolTaskBoolRemote v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TaskTrueBoolTaskStringRemote v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public bool Write(TaskTrueBoolBoolRemote v) { WriteReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public string? Write(TaskTrueBoolStringRemote v) { WriteReceived.Add(v); return null; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(TaskTrueBoolTaskBoolRemote v) { await Task.Delay(2); WriteReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(TaskTrueBoolTaskStringRemote v) { await Task.Delay(2); WriteReceived.Add(v); return string.Empty; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public bool Read(TaskFalseBoolBoolRemote v) { ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public string? Read(TaskFalseBoolStringRemote v) { ReadReceived.Add(v); return null; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TaskFalseBoolTaskBoolRemote v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TaskFalseBoolTaskStringRemote v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public bool Write(TaskFalseBoolBoolRemote v) { WriteReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public string? Write(TaskFalseBoolStringRemote v) { WriteReceived.Add(v); return null; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(TaskFalseBoolTaskBoolRemote v) { await Task.Delay(2); WriteReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(TaskFalseBoolTaskStringRemote v) { await Task.Delay(2); WriteReceived.Add(v); return string.Empty; }

        [Authorize(DataMapperMethodType.Read)]
        public bool Read(RemoteVoidBool v) { ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Read)]
        public string? Read(RemoteVoidString v) { ReadReceived.Add(v); return null; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteVoidTaskBool v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteVoidTaskString v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public bool Write(RemoteVoidBool v) { WriteReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public string? Write(RemoteVoidString v) { WriteReceived.Add(v); return null; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(RemoteVoidTaskBool v) { await Task.Delay(2); WriteReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(RemoteVoidTaskString v) { await Task.Delay(2); WriteReceived.Add(v); return string.Empty; }

        [Authorize(DataMapperMethodType.Read)]
        public bool Read(RemoteTrueBoolBool v) { ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Read)]
        public string? Read(RemoteTrueBoolString v) { ReadReceived.Add(v); return null; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTrueBoolTaskBool v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTrueBoolTaskString v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public bool Write(RemoteTrueBoolBool v) { WriteReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public string? Write(RemoteTrueBoolString v) { WriteReceived.Add(v); return null; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(RemoteTrueBoolTaskBool v) { await Task.Delay(2); WriteReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(RemoteTrueBoolTaskString v) { await Task.Delay(2); WriteReceived.Add(v); return string.Empty; }

        [Authorize(DataMapperMethodType.Read)]
        public bool Read(RemoteFalseBoolBool v) { ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Read)]
        public string? Read(RemoteFalseBoolString v) { ReadReceived.Add(v); return null; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteFalseBoolTaskBool v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteFalseBoolTaskString v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public bool Write(RemoteFalseBoolBool v) { WriteReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public string? Write(RemoteFalseBoolString v) { WriteReceived.Add(v); return null; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(RemoteFalseBoolTaskBool v) { await Task.Delay(2); WriteReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(RemoteFalseBoolTaskString v) { await Task.Delay(2); WriteReceived.Add(v); return string.Empty; }


        [Authorize(DataMapperMethodType.Read)]
        public bool Read(RemoteTaskVoidBool v) { ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Read)]
        public string? Read(RemoteTaskVoidString v) { ReadReceived.Add(v); return null; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTaskVoidTaskBool v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTaskVoidTaskString v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public bool Write(RemoteTaskVoidBool v) { WriteReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public string? Write(RemoteTaskVoidString v) { WriteReceived.Add(v); return null; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(RemoteTaskVoidTaskBool v) { await Task.Delay(2); WriteReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(RemoteTaskVoidTaskString v) { await Task.Delay(2); WriteReceived.Add(v); return string.Empty; }

        [Authorize(DataMapperMethodType.Read)]
        public bool Read(RemoteTaskTrueBoolBool v) { ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Read)]
        public string? Read(RemoteTaskTrueBoolString v) { ReadReceived.Add(v); return null; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTaskTrueBoolTaskBool v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTaskTrueBoolTaskString v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public bool Write(RemoteTaskTrueBoolBool v) { WriteReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public string? Write(RemoteTaskTrueBoolString v) { WriteReceived.Add(v); return null; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(RemoteTaskTrueBoolTaskBool v) { await Task.Delay(2); WriteReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(RemoteTaskTrueBoolTaskString v) { await Task.Delay(2); WriteReceived.Add(v); return string.Empty; }

        [Authorize(DataMapperMethodType.Read)]
        public bool Read(RemoteTaskFalseBoolBool v) { ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Read)]
        public string? Read(RemoteTaskFalseBoolString v) { ReadReceived.Add(v); return null; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTaskFalseBoolTaskBool v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTaskFalseBoolTaskString v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public bool Write(RemoteTaskFalseBoolBool v) { WriteReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public string? Write(RemoteTaskFalseBoolString v) { WriteReceived.Add(v); return null; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(RemoteTaskFalseBoolTaskBool v) { await Task.Delay(2); WriteReceived.Add(v); return true; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(RemoteTaskFalseBoolTaskString v) { await Task.Delay(2); WriteReceived.Add(v); return string.Empty; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public bool Read(RemoteVoidBoolRemote v) { ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public string? Read(RemoteVoidStringRemote v) { ReadReceived.Add(v); return null; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteVoidTaskBoolRemote v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteVoidTaskStringRemote v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public bool Write(RemoteVoidBoolRemote v) { WriteReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public string? Write(RemoteVoidStringRemote v) { WriteReceived.Add(v); return null; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(RemoteVoidTaskBoolRemote v) { await Task.Delay(2); WriteReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(RemoteVoidTaskStringRemote v) { await Task.Delay(2); WriteReceived.Add(v); return string.Empty; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public bool Read(RemoteTrueBoolBoolRemote v) { ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public string? Read(RemoteTrueBoolStringRemote v) { ReadReceived.Add(v); return null; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTrueBoolTaskBoolRemote v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTrueBoolTaskStringRemote v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public bool Write(RemoteTrueBoolBoolRemote v) { WriteReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public string? Write(RemoteTrueBoolStringRemote v) { WriteReceived.Add(v); return null; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(RemoteTrueBoolTaskBoolRemote v) { await Task.Delay(2); WriteReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(RemoteTrueBoolTaskStringRemote v) { await Task.Delay(2); WriteReceived.Add(v); return string.Empty; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public bool Read(RemoteFalseBoolBoolRemote v) { ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public string? Read(RemoteFalseBoolStringRemote v) { ReadReceived.Add(v); return null; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteFalseBoolTaskBoolRemote v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteFalseBoolTaskStringRemote v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public bool Write(RemoteFalseBoolBoolRemote v) { WriteReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public string? Write(RemoteFalseBoolStringRemote v) { WriteReceived.Add(v); return null; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(RemoteFalseBoolTaskBoolRemote v) { await Task.Delay(2); WriteReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(RemoteFalseBoolTaskStringRemote v) { await Task.Delay(2); WriteReceived.Add(v); return string.Empty; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public bool Read(RemoteTaskVoidBoolRemote v) { ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public string? Read(RemoteTaskVoidStringRemote v) { ReadReceived.Add(v); return null; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTaskVoidTaskBoolRemote v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTaskVoidTaskStringRemote v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public bool Write(RemoteTaskVoidBoolRemote v) { WriteReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public string? Write(RemoteTaskVoidStringRemote v) { WriteReceived.Add(v); return null; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(RemoteTaskVoidTaskBoolRemote v) { await Task.Delay(2); WriteReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(RemoteTaskVoidTaskStringRemote v) { await Task.Delay(2); WriteReceived.Add(v); return string.Empty; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public bool Read(RemoteTaskTrueBoolBoolRemote v) { ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public string? Read(RemoteTaskTrueBoolStringRemote v) { ReadReceived.Add(v); return null; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTaskTrueBoolTaskBoolRemote v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTaskTrueBoolTaskStringRemote v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public bool Write(RemoteTaskTrueBoolBoolRemote v) { WriteReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public string? Write(RemoteTaskTrueBoolStringRemote v) { WriteReceived.Add(v); return null; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(RemoteTaskTrueBoolTaskBoolRemote v) { await Task.Delay(2); WriteReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(RemoteTaskTrueBoolTaskStringRemote v) { await Task.Delay(2); WriteReceived.Add(v); return string.Empty; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public bool Read(RemoteTaskFalseBoolBoolRemote v) { ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public string? Read(RemoteTaskFalseBoolStringRemote v) { ReadReceived.Add(v); return null; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTaskFalseBoolTaskBoolRemote v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTaskFalseBoolTaskStringRemote v) { await Task.Delay(2); ReadReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public bool Write(RemoteTaskFalseBoolBoolRemote v) { WriteReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public string? Write(RemoteTaskFalseBoolStringRemote v) { WriteReceived.Add(v); return null; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(RemoteTaskFalseBoolTaskBoolRemote v) { await Task.Delay(2); WriteReceived.Add(v); return true; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(RemoteTaskFalseBoolTaskStringRemote v) { await Task.Delay(2); WriteReceived.Add(v); return string.Empty; }

        [Authorize(DataMapperMethodType.Read)]
        public bool Read(VoidBoolDeny v) { ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Read)]
        public string? Read(VoidStringDeny v) { ReadReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(VoidTaskBoolDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(VoidTaskStringDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public bool Write(VoidBoolDeny v) { WriteReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public string? Write(VoidStringDeny v) { WriteReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(VoidTaskBoolDeny v) { await Task.Delay(2); WriteReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(VoidTaskStringDeny v) { await Task.Delay(2); WriteReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Read)]
        public bool Read(TrueBoolBoolDeny v) { ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Read)]
        public string? Read(TrueBoolStringDeny v) { ReadReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TrueBoolTaskBoolDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TrueBoolTaskStringDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public bool Write(TrueBoolBoolDeny v) { WriteReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public string? Write(TrueBoolStringDeny v) { WriteReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(TrueBoolTaskBoolDeny v) { await Task.Delay(2); WriteReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(TrueBoolTaskStringDeny v) { await Task.Delay(2); WriteReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Read)]
        public bool Read(FalseBoolBoolDeny v) { ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Read)]
        public string? Read(FalseBoolStringDeny v) { ReadReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(FalseBoolTaskBoolDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(FalseBoolTaskStringDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public bool Write(FalseBoolBoolDeny v) { WriteReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public string? Write(FalseBoolStringDeny v) { WriteReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(FalseBoolTaskBoolDeny v) { await Task.Delay(2); WriteReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(FalseBoolTaskStringDeny v) { await Task.Delay(2); WriteReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Read)]
        public bool Read(TaskVoidBoolDeny v) { ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Read)]
        public string? Read(TaskVoidStringDeny v) { ReadReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TaskVoidTaskBoolDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TaskVoidTaskStringDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public bool Write(TaskVoidBoolDeny v) { WriteReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public string? Write(TaskVoidStringDeny v) { WriteReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(TaskVoidTaskBoolDeny v) { await Task.Delay(2); WriteReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(TaskVoidTaskStringDeny v) { await Task.Delay(2); WriteReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Read)]
        public bool Read(TaskTrueBoolBoolDeny v) { ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Read)]
        public string? Read(TaskTrueBoolStringDeny v) { ReadReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TaskTrueBoolTaskBoolDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TaskTrueBoolTaskStringDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public bool Write(TaskTrueBoolBoolDeny v) { WriteReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public string? Write(TaskTrueBoolStringDeny v) { WriteReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(TaskTrueBoolTaskBoolDeny v) { await Task.Delay(2); WriteReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(TaskTrueBoolTaskStringDeny v) { await Task.Delay(2); WriteReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Read)]
        public bool Read(TaskFalseBoolBoolDeny v) { ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Read)]
        public string? Read(TaskFalseBoolStringDeny v) { ReadReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TaskFalseBoolTaskBoolDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TaskFalseBoolTaskStringDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public bool Write(TaskFalseBoolBoolDeny v) { WriteReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public string? Write(TaskFalseBoolStringDeny v) { WriteReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(TaskFalseBoolTaskBoolDeny v) { await Task.Delay(2); WriteReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(TaskFalseBoolTaskStringDeny v) { await Task.Delay(2); WriteReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public bool Read(VoidBoolRemoteDeny v) { ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public string? Read(VoidStringRemoteDeny v) { ReadReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(VoidTaskBoolRemoteDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(VoidTaskStringRemoteDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public bool Write(VoidBoolRemoteDeny v) { WriteReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public string? Write(VoidStringRemoteDeny v) { WriteReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(VoidTaskBoolRemoteDeny v) { await Task.Delay(2); WriteReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(VoidTaskStringRemoteDeny v) { await Task.Delay(2); WriteReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public bool Read(TrueBoolBoolRemoteDeny v) { ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public string? Read(TrueBoolStringRemoteDeny v) { ReadReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TrueBoolTaskBoolRemoteDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TrueBoolTaskStringRemoteDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public bool Write(TrueBoolBoolRemoteDeny v) { WriteReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public string? Write(TrueBoolStringRemoteDeny v) { WriteReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(TrueBoolTaskBoolRemoteDeny v) { await Task.Delay(2); WriteReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(TrueBoolTaskStringRemoteDeny v) { await Task.Delay(2); WriteReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public bool Read(FalseBoolBoolRemoteDeny v) { ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public string? Read(FalseBoolStringRemoteDeny v) { ReadReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(FalseBoolTaskBoolRemoteDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(FalseBoolTaskStringRemoteDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public bool Write(FalseBoolBoolRemoteDeny v) { WriteReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public string? Write(FalseBoolStringRemoteDeny v) { WriteReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(FalseBoolTaskBoolRemoteDeny v) { await Task.Delay(2); WriteReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(FalseBoolTaskStringRemoteDeny v) { await Task.Delay(2); WriteReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public bool Read(TaskVoidBoolRemoteDeny v) { ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public string? Read(TaskVoidStringRemoteDeny v) { ReadReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TaskVoidTaskBoolRemoteDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TaskVoidTaskStringRemoteDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public bool Write(TaskVoidBoolRemoteDeny v) { WriteReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public string? Write(TaskVoidStringRemoteDeny v) { WriteReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(TaskVoidTaskBoolRemoteDeny v) { await Task.Delay(2); WriteReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(TaskVoidTaskStringRemoteDeny v) { await Task.Delay(2); WriteReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public bool Read(TaskTrueBoolBoolRemoteDeny v) { ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public string? Read(TaskTrueBoolStringRemoteDeny v) { ReadReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TaskTrueBoolTaskBoolRemoteDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TaskTrueBoolTaskStringRemoteDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public bool Write(TaskTrueBoolBoolRemoteDeny v) { WriteReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public string? Write(TaskTrueBoolStringRemoteDeny v) { WriteReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(TaskTrueBoolTaskBoolRemoteDeny v) { await Task.Delay(2); WriteReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(TaskTrueBoolTaskStringRemoteDeny v) { await Task.Delay(2); WriteReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public bool Read(TaskFalseBoolBoolRemoteDeny v) { ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public string? Read(TaskFalseBoolStringRemoteDeny v) { ReadReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TaskFalseBoolTaskBoolRemoteDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(TaskFalseBoolTaskStringRemoteDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public bool Write(TaskFalseBoolBoolRemoteDeny v) { WriteReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public string? Write(TaskFalseBoolStringRemoteDeny v) { WriteReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(TaskFalseBoolTaskBoolRemoteDeny v) { await Task.Delay(2); WriteReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(TaskFalseBoolTaskStringRemoteDeny v) { await Task.Delay(2); WriteReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Read)]
        public bool Read(RemoteVoidBoolDeny v) { ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Read)]
        public string? Read(RemoteVoidStringDeny v) { ReadReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteVoidTaskBoolDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteVoidTaskStringDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public bool Write(RemoteVoidBoolDeny v) { WriteReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public string? Write(RemoteVoidStringDeny v) { WriteReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(RemoteVoidTaskBoolDeny v) { await Task.Delay(2); WriteReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(RemoteVoidTaskStringDeny v) { await Task.Delay(2); WriteReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Read)]
        public bool Read(RemoteTrueBoolBoolDeny v) { ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Read)]
        public string? Read(RemoteTrueBoolStringDeny v) { ReadReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTrueBoolTaskBoolDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTrueBoolTaskStringDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public bool Write(RemoteTrueBoolBoolDeny v) { WriteReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public string? Write(RemoteTrueBoolStringDeny v) { WriteReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(RemoteTrueBoolTaskBoolDeny v) { await Task.Delay(2); WriteReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(RemoteTrueBoolTaskStringDeny v) { await Task.Delay(2); WriteReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Read)]
        public bool Read(RemoteFalseBoolBoolDeny v) { ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Read)]
        public string? Read(RemoteFalseBoolStringDeny v) { ReadReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteFalseBoolTaskBoolDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteFalseBoolTaskStringDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public bool Write(RemoteFalseBoolBoolDeny v) { WriteReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public string? Write(RemoteFalseBoolStringDeny v) { WriteReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(RemoteFalseBoolTaskBoolDeny v) { await Task.Delay(2); WriteReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(RemoteFalseBoolTaskStringDeny v) { await Task.Delay(2); WriteReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Read)]
        public bool Read(RemoteTaskVoidBoolDeny v) { ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Read)]
        public string? Read(RemoteTaskVoidStringDeny v) { ReadReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTaskVoidTaskBoolDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTaskVoidTaskStringDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public bool Write(RemoteTaskVoidBoolDeny v) { WriteReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public string? Write(RemoteTaskVoidStringDeny v) { WriteReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(RemoteTaskVoidTaskBoolDeny v) { await Task.Delay(2); WriteReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(RemoteTaskVoidTaskStringDeny v) { await Task.Delay(2); WriteReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Read)]
        public bool Read(RemoteTaskTrueBoolBoolDeny v) { ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Read)]
        public string? Read(RemoteTaskTrueBoolStringDeny v) { ReadReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTaskTrueBoolTaskBoolDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTaskTrueBoolTaskStringDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public bool Write(RemoteTaskTrueBoolBoolDeny v) { WriteReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public string? Write(RemoteTaskTrueBoolStringDeny v) { WriteReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(RemoteTaskTrueBoolTaskBoolDeny v) { await Task.Delay(2); WriteReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(RemoteTaskTrueBoolTaskStringDeny v) { await Task.Delay(2); WriteReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Read)]
        public bool Read(RemoteTaskFalseBoolBoolDeny v) { ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Read)]
        public string? Read(RemoteTaskFalseBoolStringDeny v) { ReadReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTaskFalseBoolTaskBoolDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTaskFalseBoolTaskStringDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public bool Write(RemoteTaskFalseBoolBoolDeny v) { WriteReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public string? Write(RemoteTaskFalseBoolStringDeny v) { WriteReceived.Add(v); return "deny"; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(RemoteTaskFalseBoolTaskBoolDeny v) { await Task.Delay(2); WriteReceived.Add(v); return false; }

        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(RemoteTaskFalseBoolTaskStringDeny v) { await Task.Delay(2); WriteReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public bool Read(RemoteVoidBoolRemoteDeny v) { ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public string? Read(RemoteVoidStringRemoteDeny v) { ReadReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteVoidTaskBoolRemoteDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteVoidTaskStringRemoteDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public bool Write(RemoteVoidBoolRemoteDeny v) { WriteReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public string? Write(RemoteVoidStringRemoteDeny v) { WriteReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(RemoteVoidTaskBoolRemoteDeny v) { await Task.Delay(2); WriteReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(RemoteVoidTaskStringRemoteDeny v) { await Task.Delay(2); WriteReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public bool Read(RemoteTrueBoolBoolRemoteDeny v) { ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public string? Read(RemoteTrueBoolStringRemoteDeny v) { ReadReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTrueBoolTaskBoolRemoteDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTrueBoolTaskStringRemoteDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public bool Write(RemoteTrueBoolBoolRemoteDeny v) { WriteReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public string? Write(RemoteTrueBoolStringRemoteDeny v) { WriteReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(RemoteTrueBoolTaskBoolRemoteDeny v) { await Task.Delay(2); WriteReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(RemoteTrueBoolTaskStringRemoteDeny v) { await Task.Delay(2); WriteReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public bool Read(RemoteFalseBoolBoolRemoteDeny v) { ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public string? Read(RemoteFalseBoolStringRemoteDeny v) { ReadReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteFalseBoolTaskBoolRemoteDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteFalseBoolTaskStringRemoteDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public bool Write(RemoteFalseBoolBoolRemoteDeny v) { WriteReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public string? Write(RemoteFalseBoolStringRemoteDeny v) { WriteReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(RemoteFalseBoolTaskBoolRemoteDeny v) { await Task.Delay(2); WriteReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(RemoteFalseBoolTaskStringRemoteDeny v) { await Task.Delay(2); WriteReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public bool Read(RemoteTaskVoidBoolRemoteDeny v) { ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public string? Read(RemoteTaskVoidStringRemoteDeny v) { ReadReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTaskVoidTaskBoolRemoteDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTaskVoidTaskStringRemoteDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public bool Write(RemoteTaskVoidBoolRemoteDeny v) { WriteReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public string? Write(RemoteTaskVoidStringRemoteDeny v) { WriteReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(RemoteTaskVoidTaskBoolRemoteDeny v) { await Task.Delay(2); WriteReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(RemoteTaskVoidTaskStringRemoteDeny v) { await Task.Delay(2); WriteReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public bool Read(RemoteTaskTrueBoolBoolRemoteDeny v) { ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public string? Read(RemoteTaskTrueBoolStringRemoteDeny v) { ReadReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTaskTrueBoolTaskBoolRemoteDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTaskTrueBoolTaskStringRemoteDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public bool Write(RemoteTaskTrueBoolBoolRemoteDeny v) { WriteReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public string? Write(RemoteTaskTrueBoolStringRemoteDeny v) { WriteReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(RemoteTaskTrueBoolTaskBoolRemoteDeny v) { await Task.Delay(2); WriteReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(RemoteTaskTrueBoolTaskStringRemoteDeny v) { await Task.Delay(2); WriteReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public bool Read(RemoteTaskFalseBoolBoolRemoteDeny v) { ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public string? Read(RemoteTaskFalseBoolStringRemoteDeny v) { ReadReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTaskFalseBoolTaskBoolRemoteDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Read)]
        public async Task<bool> Read(RemoteTaskFalseBoolTaskStringRemoteDeny v) { await Task.Delay(2); ReadReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public bool Write(RemoteTaskFalseBoolBoolRemoteDeny v) { WriteReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public string? Write(RemoteTaskFalseBoolStringRemoteDeny v) { WriteReceived.Add(v); return "deny"; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<bool> Write(RemoteTaskFalseBoolTaskBoolRemoteDeny v) { await Task.Delay(2); WriteReceived.Add(v); return false; }

        [Remote]
        [Authorize(DataMapperMethodType.Write)]
        public async Task<string> Write(RemoteTaskFalseBoolTaskStringRemoteDeny v)
        {
            await Task.Delay(2); WriteReceived.Add(v); return "deny";
        }
    }

    public interface IAuthorizedAllCombinations : IEditMetaSaveProperties
    {
        List<IDd> Received { get; set; }
    }

    [Factory]
    [Authorize<AuthorizationAllCombinations>]
    public class AuthorizedAllCombinations : IAuthorizedAllCombinations
    {
        public AuthorizedAllCombinations() : base()
        {
            Received = new List<IDd>();
        }

        public List<IDd> Received { get; set; } = new List<IDd>();

        public bool IsDeleted => false;

        public bool IsNew => true;

        [Create]
        public void Create(VoidBool v) { Received.Add(v); }

        [Create]
        public void Create(VoidString v) { Received.Add(v); }

        [Create]
        public void Create(VoidTaskBool v) { Received.Add(v); }

        [Create]
        public void Create(VoidTaskString v) { Received.Add(v); }

        [Insert]
        public void Insert(VoidBool v) { Received.Add(v); }

        [Insert]
        public void Insert(VoidString v) { Received.Add(v); }

        [Insert]
        public void Insert(VoidTaskBool v) { Received.Add(v); }

        [Insert]
        public void Insert(VoidTaskString v) { Received.Add(v); }

        [Create]
        public bool Create(TrueBoolBool v) { Received.Add(v); return true; }

        [Create]
        public bool Create(TrueBoolString v) { Received.Add(v); return true; }

        [Create]
        public bool Create(TrueBoolTaskBool v) { Received.Add(v); return true; }

        [Create]
        public bool Create(TrueBoolTaskString v) { Received.Add(v); return true; }

        [Insert]
        public bool Insert(TrueBoolBool v) { Received.Add(v); return true; }

        [Insert]
        public bool Insert(TrueBoolString v) { Received.Add(v); return true; }

        [Insert]
        public bool Insert(TrueBoolTaskBool v) { Received.Add(v); return true; }

        [Insert]
        public bool Insert(TrueBoolTaskString v) { Received.Add(v); return true; }

        [Create]
        public bool Create(FalseBoolBool v) { Received.Add(v); return false; }

        [Create]
        public bool Create(FalseBoolString v) { Received.Add(v); return false; }

        [Create]
        public bool Create(FalseBoolTaskBool v) { Received.Add(v); return false; }

        [Create]
        public bool Create(FalseBoolTaskString v) { Received.Add(v); return false; }

        [Insert]
        public bool Insert(FalseBoolBool v) { Received.Add(v); return false; }

        [Insert]
        public bool Insert(FalseBoolString v) { Received.Add(v); return false; }

        [Insert]
        public bool Insert(FalseBoolTaskBool v) { Received.Add(v); return false; }

        [Insert]
        public bool Insert(FalseBoolTaskString v) { Received.Add(v); return false; }

        [Create]
        public async Task Create(TaskVoidBool v) { await Task.Delay(2); Received.Add(v); }

        [Create]
        public async Task Create(TaskVoidString v) { await Task.Delay(2); Received.Add(v); }

        [Create]
        public async Task Create(TaskVoidTaskBool v) { await Task.Delay(2); Received.Add(v); }

        [Create]
        public async Task Create(TaskVoidTaskString v) { await Task.Delay(2); Received.Add(v); }

        [Insert]
        public async Task Insert(TaskVoidBool v) { await Task.Delay(2); Received.Add(v); }

        [Insert]
        public async Task Insert(TaskVoidString v) { await Task.Delay(2); Received.Add(v); }

        [Insert]
        public async Task Insert(TaskVoidTaskBool v) { await Task.Delay(2); Received.Add(v); }

        [Insert]
        public async Task Insert(TaskVoidTaskString v) { await Task.Delay(2); Received.Add(v); }

        [Create]
        public async Task<bool> Create(TaskTrueBoolBool v) { await Task.Delay(2); Received.Add(v); return true; }

        [Create]
        public async Task<bool> Create(TaskTrueBoolString v) { await Task.Delay(2); Received.Add(v); return true; }

        [Create]
        public async Task<bool> Create(TaskTrueBoolTaskBool v) { await Task.Delay(2); Received.Add(v); return true; }

        [Create]
        public async Task<bool> Create(TaskTrueBoolTaskString v) { await Task.Delay(2); Received.Add(v); return true; }

        [Insert]
        public async Task<bool> Insert(TaskTrueBoolBool v) { await Task.Delay(2); Received.Add(v); return true; }

        [Insert]
        public async Task<bool> Insert(TaskTrueBoolString v) { await Task.Delay(2); Received.Add(v); return true; }

        [Insert]
        public async Task<bool> Insert(TaskTrueBoolTaskBool v) { await Task.Delay(2); Received.Add(v); return true; }

        [Insert]
        public async Task<bool> Insert(TaskTrueBoolTaskString v) { await Task.Delay(2); Received.Add(v); return true; }

        [Create]
        public async Task<bool> Create(TaskFalseBoolBool v) { await Task.Delay(2); Received.Add(v); return false; }

        [Create]
        public async Task<bool> Create(TaskFalseBoolString v) { await Task.Delay(2); Received.Add(v); return false; }

        [Create]
        public async Task<bool> Create(TaskFalseBoolTaskBool v) { await Task.Delay(2); Received.Add(v); return false; }

        [Create]
        public async Task<bool> Create(TaskFalseBoolTaskString v) { await Task.Delay(2); Received.Add(v); return false; }

        [Insert]
        public async Task<bool> Insert(TaskFalseBoolBool v) { await Task.Delay(2); Received.Add(v); return false; }

        [Insert]
        public async Task<bool> Insert(TaskFalseBoolString v) { await Task.Delay(2); Received.Add(v); return false; }

        [Insert]
        public async Task<bool> Insert(TaskFalseBoolTaskBool v) { await Task.Delay(2); Received.Add(v); return false; }

        [Insert]
        public async Task<bool> Insert(TaskFalseBoolTaskString v) { await Task.Delay(2); Received.Add(v); return false; }

        [Create]
        public void Create(VoidBoolRemote v) { Received.Add(v); }

        [Create]
        public void Create(VoidStringRemote v) { Received.Add(v); }

        [Create]
        public void Create(VoidTaskBoolRemote v) { Received.Add(v); }

        [Create]
        public void Create(VoidTaskStringRemote v) { Received.Add(v); }

        [Insert]
        public void Insert(VoidBoolRemote v) { Received.Add(v); }

        [Insert]
        public void Insert(VoidStringRemote v) { Received.Add(v); }

        [Insert]
        public void Insert(VoidTaskBoolRemote v) { Received.Add(v); }

        [Insert]
        public void Insert(VoidTaskStringRemote v) { Received.Add(v); }

        [Create]
        public bool Create(TrueBoolBoolRemote v) { Received.Add(v); return true; }

        [Create]
        public bool Create(TrueBoolStringRemote v) { Received.Add(v); return true; }

        [Create]
        public bool Create(TrueBoolTaskBoolRemote v) { Received.Add(v); return true; }

        [Create]
        public bool Create(TrueBoolTaskStringRemote v) { Received.Add(v); return true; }

        [Insert]
        public bool Insert(TrueBoolBoolRemote v) { Received.Add(v); return true; }

        [Insert]
        public bool Insert(TrueBoolStringRemote v) { Received.Add(v); return true; }

        [Insert]
        public bool Insert(TrueBoolTaskBoolRemote v) { Received.Add(v); return true; }

        [Insert]
        public bool Insert(TrueBoolTaskStringRemote v) { Received.Add(v); return true; }

        [Create]
        public bool Create(FalseBoolBoolRemote v) { Received.Add(v); return false; }

        [Create]
        public bool Create(FalseBoolStringRemote v) { Received.Add(v); return false; }

        [Create]
        public bool Create(FalseBoolTaskBoolRemote v) { Received.Add(v); return false; }

        [Create]
        public bool Create(FalseBoolTaskStringRemote v) { Received.Add(v); return false; }

        [Insert]
        public bool Insert(FalseBoolBoolRemote v) { Received.Add(v); return false; }

        [Insert]
        public bool Insert(FalseBoolStringRemote v) { Received.Add(v); return false; }

        [Insert]
        public bool Insert(FalseBoolTaskBoolRemote v) { Received.Add(v); return false; }

        [Insert]
        public bool Insert(FalseBoolTaskStringRemote v) { Received.Add(v); return false; }

        [Create]
        public async Task Create(TaskVoidBoolRemote v) { await Task.Delay(2); Received.Add(v); }

        [Create]
        public async Task Create(TaskVoidStringRemote v) { await Task.Delay(2); Received.Add(v); }

        [Create]
        public async Task Create(TaskVoidTaskBoolRemote v) { await Task.Delay(2); Received.Add(v); }

        [Create]
        public async Task Create(TaskVoidTaskStringRemote v) { await Task.Delay(2); Received.Add(v); }

        [Insert]
        public async Task Insert(TaskVoidBoolRemote v) { await Task.Delay(2); Received.Add(v); }

        [Insert]
        public async Task Insert(TaskVoidStringRemote v) { await Task.Delay(2); Received.Add(v); }

        [Insert]
        public async Task Insert(TaskVoidTaskBoolRemote v) { await Task.Delay(2); Received.Add(v); }

        [Insert]
        public async Task Insert(TaskVoidTaskStringRemote v) { await Task.Delay(2); Received.Add(v); }

        [Create]
        public async Task<bool> Create(TaskTrueBoolBoolRemote v) { await Task.Delay(2); Received.Add(v); return true; }

        [Create]
        public async Task<bool> Create(TaskTrueBoolStringRemote v) { await Task.Delay(2); Received.Add(v); return true; }

        [Create]
        public async Task<bool> Create(TaskTrueBoolTaskBoolRemote v) { await Task.Delay(2); Received.Add(v); return true; }

        [Create]
        public async Task<bool> Create(TaskTrueBoolTaskStringRemote v) { await Task.Delay(2); Received.Add(v); return true; }

        [Insert]
        public async Task<bool> Insert(TaskTrueBoolBoolRemote v) { await Task.Delay(2); Received.Add(v); return true; }

        [Insert]
        public async Task<bool> Insert(TaskTrueBoolStringRemote v) { await Task.Delay(2); Received.Add(v); return true; }

        [Insert]
        public async Task<bool> Insert(TaskTrueBoolTaskBoolRemote v) { await Task.Delay(2); Received.Add(v); return true; }

        [Insert]
        public async Task<bool> Insert(TaskTrueBoolTaskStringRemote v) { await Task.Delay(2); Received.Add(v); return true; }

        [Create]
        public async Task<bool> Create(TaskFalseBoolBoolRemote v) { await Task.Delay(2); Received.Add(v); return false; }

        [Create]
        public async Task<bool> Create(TaskFalseBoolStringRemote v) { await Task.Delay(2); Received.Add(v); return false; }

        [Create]
        public async Task<bool> Create(TaskFalseBoolTaskBoolRemote v) { await Task.Delay(2); Received.Add(v); return false; }

        [Create]
        public async Task<bool> Create(TaskFalseBoolTaskStringRemote v) { await Task.Delay(2); Received.Add(v); return false; }

        [Insert]
        public async Task<bool> Insert(TaskFalseBoolBoolRemote v) { await Task.Delay(2); Received.Add(v); return false; }

        [Insert]
        public async Task<bool> Insert(TaskFalseBoolStringRemote v) { await Task.Delay(2); Received.Add(v); return false; }

        [Insert]
        public async Task<bool> Insert(TaskFalseBoolTaskBoolRemote v) { await Task.Delay(2); Received.Add(v); return false; }

        [Insert]
        public async Task<bool> Insert(TaskFalseBoolTaskStringRemote v) { await Task.Delay(2); Received.Add(v); return false; }


        [Remote]
        [Create]
        public void Create(RemoteVoidBool v) { Received.Add(v); }

        [Remote]
        [Create]
        public void Create(RemoteVoidString v) { Received.Add(v); }

        [Remote]
        [Create]
        public void Create(RemoteVoidTaskBool v) { Received.Add(v); }

        [Remote]
        [Create]
        public void Create(RemoteVoidTaskString v) { Received.Add(v); }

        [Remote]
        [Insert]
        public void Insert(RemoteVoidBool v) { Received.Add(v); }

        [Remote]
        [Insert]
        public void Insert(RemoteVoidString v) { Received.Add(v); }

        [Remote]
        [Insert]
        public void Insert(RemoteVoidTaskBool v) { Received.Add(v); }

        [Remote]
        [Insert]
        public void Insert(RemoteVoidTaskString v) { Received.Add(v); }

        [Remote]
        [Create]
        public bool Create(RemoteTrueBoolBool v) { Received.Add(v); return true; }

        [Remote]
        [Create]
        public bool Create(RemoteTrueBoolString v) { Received.Add(v); return true; }

        [Remote]
        [Create]
        public bool Create(RemoteTrueBoolTaskBool v) { Received.Add(v); return true; }

        [Remote]
        [Create]
        public bool Create(RemoteTrueBoolTaskString v) { Received.Add(v); return true; }

        [Remote]
        [Insert]
        public bool Insert(RemoteTrueBoolBool v) { Received.Add(v); return true; }

        [Remote]
        [Insert]
        public bool Insert(RemoteTrueBoolString v) { Received.Add(v); return true; }

        [Remote]
        [Insert]
        public bool Insert(RemoteTrueBoolTaskBool v) { Received.Add(v); return true; }

        [Remote]
        [Insert]
        public bool Insert(RemoteTrueBoolTaskString v) { Received.Add(v); return true; }

        [Remote]
        [Create]
        public bool Create(RemoteFalseBoolBool v) { Received.Add(v); return false; }

        [Remote]
        [Create]
        public bool Create(RemoteFalseBoolString v) { Received.Add(v); return false; }

        [Remote]
        [Create]
        public bool Create(RemoteFalseBoolTaskBool v) { Received.Add(v); return false; }

        [Remote]
        [Create]
        public bool Create(RemoteFalseBoolTaskString v) { Received.Add(v); return false; }

        [Remote]
        [Insert]
        public bool Insert(RemoteFalseBoolBool v) { Received.Add(v); return false; }

        [Remote]
        [Insert]
        public bool Insert(RemoteFalseBoolString v) { Received.Add(v); return false; }

        [Remote]
        [Insert]
        public bool Insert(RemoteFalseBoolTaskBool v) { Received.Add(v); return false; }

        [Remote]
        [Insert]
        public bool Insert(RemoteFalseBoolTaskString v) { Received.Add(v); return false; }

        [Remote]
        [Create]
        public async Task Create(RemoteTaskVoidBool v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Create]
        public async Task Create(RemoteTaskVoidString v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Create]
        public async Task Create(RemoteTaskVoidTaskBool v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Create]
        public async Task Create(RemoteTaskVoidTaskString v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Insert]
        public async Task Insert(RemoteTaskVoidBool v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Insert]
        public async Task Insert(RemoteTaskVoidString v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Insert]
        public async Task Insert(RemoteTaskVoidTaskBool v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Insert]
        public async Task Insert(RemoteTaskVoidTaskString v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskTrueBoolBool v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskTrueBoolString v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskTrueBoolTaskBool v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskTrueBoolTaskString v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskTrueBoolBool v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskTrueBoolString v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskTrueBoolTaskBool v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskTrueBoolTaskString v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskFalseBoolBool v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskFalseBoolString v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskFalseBoolTaskBool v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskFalseBoolTaskString v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskFalseBoolBool v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskFalseBoolString v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskFalseBoolTaskBool v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskFalseBoolTaskString v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Create]
        public void Create(RemoteVoidBoolRemote v) { Received.Add(v); }

        [Remote]
        [Create]
        public void Create(RemoteVoidStringRemote v) { Received.Add(v); }

        [Remote]
        [Create]
        public void Create(RemoteVoidTaskBoolRemote v) { Received.Add(v); }

        [Remote]
        [Create]
        public void Create(RemoteVoidTaskStringRemote v) { Received.Add(v); }

        [Remote]
        [Insert]
        public void Insert(RemoteVoidBoolRemote v) { Received.Add(v); }

        [Remote]
        [Insert]
        public void Insert(RemoteVoidStringRemote v) { Received.Add(v); }

        [Remote]
        [Insert]
        public void Insert(RemoteVoidTaskBoolRemote v) { Received.Add(v); }

        [Remote]
        [Insert]
        public void Insert(RemoteVoidTaskStringRemote v) { Received.Add(v); }

        [Remote]
        [Create]
        public bool Create(RemoteTrueBoolBoolRemote v) { Received.Add(v); return true; }

        [Remote]
        [Create]
        public bool Create(RemoteTrueBoolStringRemote v) { Received.Add(v); return true; }

        [Remote]
        [Create]
        public bool Create(RemoteTrueBoolTaskBoolRemote v) { Received.Add(v); return true; }

        [Remote]
        [Create]
        public bool Create(RemoteTrueBoolTaskStringRemote v) { Received.Add(v); return true; }

        [Remote]
        [Insert]
        public bool Insert(RemoteTrueBoolBoolRemote v) { Received.Add(v); return true; }

        [Remote]
        [Insert]
        public bool Insert(RemoteTrueBoolStringRemote v) { Received.Add(v); return true; }

        [Remote]
        [Insert]
        public bool Insert(RemoteTrueBoolTaskBoolRemote v) { Received.Add(v); return true; }

        [Remote]
        [Insert]
        public bool Insert(RemoteTrueBoolTaskStringRemote v) { Received.Add(v); return true; }

        [Remote]
        [Create]
        public bool Create(RemoteFalseBoolBoolRemote v) { Received.Add(v); return false; }

        [Remote]
        [Create]
        public bool Create(RemoteFalseBoolStringRemote v) { Received.Add(v); return false; }

        [Remote]
        [Create]
        public bool Create(RemoteFalseBoolTaskBoolRemote v) { Received.Add(v); return false; }

        [Remote]
        [Create]
        public bool Create(RemoteFalseBoolTaskStringRemote v) { Received.Add(v); return false; }

        [Remote]
        [Insert]
        public bool Insert(RemoteFalseBoolBoolRemote v) { Received.Add(v); return false; }

        [Remote]
        [Insert]
        public bool Insert(RemoteFalseBoolStringRemote v) { Received.Add(v); return false; }

        [Remote]
        [Insert]
        public bool Insert(RemoteFalseBoolTaskBoolRemote v) { Received.Add(v); return false; }

        [Remote]
        [Insert]
        public bool Insert(RemoteFalseBoolTaskStringRemote v) { Received.Add(v); return false; }

        [Remote]
        [Create]
        public async Task Create(RemoteTaskVoidBoolRemote v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Create]
        public async Task Create(RemoteTaskVoidStringRemote v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Create]
        public async Task Create(RemoteTaskVoidTaskBoolRemote v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Create]
        public async Task Create(RemoteTaskVoidTaskStringRemote v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Insert]
        public async Task Insert(RemoteTaskVoidBoolRemote v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Insert]
        public async Task Insert(RemoteTaskVoidStringRemote v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Insert]
        public async Task Insert(RemoteTaskVoidTaskBoolRemote v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Insert]
        public async Task Insert(RemoteTaskVoidTaskStringRemote v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskTrueBoolBoolRemote v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskTrueBoolStringRemote v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskTrueBoolTaskBoolRemote v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskTrueBoolTaskStringRemote v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskTrueBoolBoolRemote v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskTrueBoolStringRemote v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskTrueBoolTaskBoolRemote v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskTrueBoolTaskStringRemote v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskFalseBoolBoolRemote v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskFalseBoolStringRemote v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskFalseBoolTaskBoolRemote v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskFalseBoolTaskStringRemote v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskFalseBoolBoolRemote v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskFalseBoolStringRemote v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskFalseBoolTaskBoolRemote v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskFalseBoolTaskStringRemote v) { await Task.Delay(2); Received.Add(v); return false; }



        [Create]
        public void Create(VoidBoolDeny v) { Received.Add(v); }

        [Create]
        public void Create(VoidStringDeny v) { Received.Add(v); }

        [Create]
        public void Create(VoidTaskBoolDeny v) { Received.Add(v); }

        [Create]
        public void Create(VoidTaskStringDeny v) { Received.Add(v); }

        [Insert]
        public void Insert(VoidBoolDeny v) { Received.Add(v); }

        [Insert]
        public void Insert(VoidStringDeny v) { Received.Add(v); }

        [Insert]
        public void Insert(VoidTaskBoolDeny v) { Received.Add(v); }

        [Insert]
        public void Insert(VoidTaskStringDeny v) { Received.Add(v); }

        [Create]
        public bool Create(TrueBoolBoolDeny v) { Received.Add(v); return true; }

        [Create]
        public bool Create(TrueBoolStringDeny v) { Received.Add(v); return true; }

        [Create]
        public bool Create(TrueBoolTaskBoolDeny v) { Received.Add(v); return true; }

        [Create]
        public bool Create(TrueBoolTaskStringDeny v) { Received.Add(v); return true; }

        [Insert]
        public bool Insert(TrueBoolBoolDeny v) { Received.Add(v); return true; }

        [Insert]
        public bool Insert(TrueBoolStringDeny v) { Received.Add(v); return true; }

        [Insert]
        public bool Insert(TrueBoolTaskBoolDeny v) { Received.Add(v); return true; }

        [Insert]
        public bool Insert(TrueBoolTaskStringDeny v) { Received.Add(v); return true; }

        [Create]
        public bool Create(FalseBoolBoolDeny v) { Received.Add(v); return false; }

        [Create]
        public bool Create(FalseBoolStringDeny v) { Received.Add(v); return false; }

        [Create]
        public bool Create(FalseBoolTaskBoolDeny v) { Received.Add(v); return false; }

        [Create]
        public bool Create(FalseBoolTaskStringDeny v) { Received.Add(v); return false; }

        [Insert]
        public bool Insert(FalseBoolBoolDeny v) { Received.Add(v); return false; }

        [Insert]
        public bool Insert(FalseBoolStringDeny v) { Received.Add(v); return false; }

        [Insert]
        public bool Insert(FalseBoolTaskBoolDeny v) { Received.Add(v); return false; }

        [Insert]
        public bool Insert(FalseBoolTaskStringDeny v) { Received.Add(v); return false; }

        [Create]
        public async Task Create(TaskVoidBoolDeny v) { await Task.Delay(2); Received.Add(v); }

        [Create]
        public async Task Create(TaskVoidStringDeny v) { await Task.Delay(2); Received.Add(v); }

        [Create]
        public async Task Create(TaskVoidTaskBoolDeny v) { await Task.Delay(2); Received.Add(v); }

        [Create]
        public async Task Create(TaskVoidTaskStringDeny v) { await Task.Delay(2); Received.Add(v); }

        [Insert]
        public async Task Insert(TaskVoidBoolDeny v) { await Task.Delay(2); Received.Add(v); }

        [Insert]
        public async Task Insert(TaskVoidStringDeny v) { await Task.Delay(2); Received.Add(v); }

        [Insert]
        public async Task Insert(TaskVoidTaskBoolDeny v) { await Task.Delay(2); Received.Add(v); }

        [Insert]
        public async Task Insert(TaskVoidTaskStringDeny v) { await Task.Delay(2); Received.Add(v); }

        [Create]
        public async Task<bool> Create(TaskTrueBoolBoolDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Create]
        public async Task<bool> Create(TaskTrueBoolStringDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Create]
        public async Task<bool> Create(TaskTrueBoolTaskBoolDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Create]
        public async Task<bool> Create(TaskTrueBoolTaskStringDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Insert]
        public async Task<bool> Insert(TaskTrueBoolBoolDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Insert]
        public async Task<bool> Insert(TaskTrueBoolStringDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Insert]
        public async Task<bool> Insert(TaskTrueBoolTaskBoolDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Insert]
        public async Task<bool> Insert(TaskTrueBoolTaskStringDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Create]
        public async Task<bool> Create(TaskFalseBoolBoolDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Create]
        public async Task<bool> Create(TaskFalseBoolStringDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Create]
        public async Task<bool> Create(TaskFalseBoolTaskBoolDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Create]
        public async Task<bool> Create(TaskFalseBoolTaskStringDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Insert]
        public async Task<bool> Insert(TaskFalseBoolBoolDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Insert]
        public async Task<bool> Insert(TaskFalseBoolStringDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Insert]
        public async Task<bool> Insert(TaskFalseBoolTaskBoolDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Insert]
        public async Task<bool> Insert(TaskFalseBoolTaskStringDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Create]
        public void Create(VoidBoolRemoteDeny v) { Received.Add(v); }

        [Create]
        public void Create(VoidStringRemoteDeny v) { Received.Add(v); }

        [Create]
        public void Create(VoidTaskBoolRemoteDeny v) { Received.Add(v); }

        [Create]
        public void Create(VoidTaskStringRemoteDeny v) { Received.Add(v); }

        [Insert]
        public void Insert(VoidBoolRemoteDeny v) { Received.Add(v); }

        [Insert]
        public void Insert(VoidStringRemoteDeny v) { Received.Add(v); }

        [Insert]
        public void Insert(VoidTaskBoolRemoteDeny v) { Received.Add(v); }

        [Insert]
        public void Insert(VoidTaskStringRemoteDeny v) { Received.Add(v); }

        [Create]
        public bool Create(TrueBoolBoolRemoteDeny v) { Received.Add(v); return true; }

        [Create]
        public bool Create(TrueBoolStringRemoteDeny v) { Received.Add(v); return true; }

        [Create]
        public bool Create(TrueBoolTaskBoolRemoteDeny v) { Received.Add(v); return true; }

        [Create]
        public bool Create(TrueBoolTaskStringRemoteDeny v) { Received.Add(v); return true; }

        [Insert]
        public bool Insert(TrueBoolBoolRemoteDeny v) { Received.Add(v); return true; }

        [Insert]
        public bool Insert(TrueBoolStringRemoteDeny v) { Received.Add(v); return true; }

        [Insert]
        public bool Insert(TrueBoolTaskBoolRemoteDeny v) { Received.Add(v); return true; }

        [Insert]
        public bool Insert(TrueBoolTaskStringRemoteDeny v) { Received.Add(v); return true; }

        [Create]
        public bool Create(FalseBoolBoolRemoteDeny v) { Received.Add(v); return false; }

        [Create]
        public bool Create(FalseBoolStringRemoteDeny v) { Received.Add(v); return false; }

        [Create]
        public bool Create(FalseBoolTaskBoolRemoteDeny v) { Received.Add(v); return false; }

        [Create]
        public bool Create(FalseBoolTaskStringRemoteDeny v) { Received.Add(v); return false; }

        [Insert]
        public bool Insert(FalseBoolBoolRemoteDeny v) { Received.Add(v); return false; }

        [Insert]
        public bool Insert(FalseBoolStringRemoteDeny v) { Received.Add(v); return false; }

        [Insert]
        public bool Insert(FalseBoolTaskBoolRemoteDeny v) { Received.Add(v); return false; }

        [Insert]
        public bool Insert(FalseBoolTaskStringRemoteDeny v) { Received.Add(v); return false; }

        [Create]
        public async Task Create(TaskVoidBoolRemoteDeny v) { await Task.Delay(2); Received.Add(v); }

        [Create]
        public async Task Create(TaskVoidStringRemoteDeny v) { await Task.Delay(2); Received.Add(v); }

        [Create]
        public async Task Create(TaskVoidTaskBoolRemoteDeny v) { await Task.Delay(2); Received.Add(v); }

        [Create]
        public async Task Create(TaskVoidTaskStringRemoteDeny v) { await Task.Delay(2); Received.Add(v); }

        [Insert]
        public async Task Insert(TaskVoidBoolRemoteDeny v) { await Task.Delay(2); Received.Add(v); }

        [Insert]
        public async Task Insert(TaskVoidStringRemoteDeny v) { await Task.Delay(2); Received.Add(v); }

        [Insert]
        public async Task Insert(TaskVoidTaskBoolRemoteDeny v) { await Task.Delay(2); Received.Add(v); }

        [Insert]
        public async Task Insert(TaskVoidTaskStringRemoteDeny v) { await Task.Delay(2); Received.Add(v); }

        [Create]
        public async Task<bool> Create(TaskTrueBoolBoolRemoteDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Create]
        public async Task<bool> Create(TaskTrueBoolStringRemoteDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Create]
        public async Task<bool> Create(TaskTrueBoolTaskBoolRemoteDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Create]
        public async Task<bool> Create(TaskTrueBoolTaskStringRemoteDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Insert]
        public async Task<bool> Insert(TaskTrueBoolBoolRemoteDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Insert]
        public async Task<bool> Insert(TaskTrueBoolStringRemoteDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Insert]
        public async Task<bool> Insert(TaskTrueBoolTaskBoolRemoteDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Insert]
        public async Task<bool> Insert(TaskTrueBoolTaskStringRemoteDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Create]
        public async Task<bool> Create(TaskFalseBoolBoolRemoteDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Create]
        public async Task<bool> Create(TaskFalseBoolStringRemoteDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Create]
        public async Task<bool> Create(TaskFalseBoolTaskBoolRemoteDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Create]
        public async Task<bool> Create(TaskFalseBoolTaskStringRemoteDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Insert]
        public async Task<bool> Insert(TaskFalseBoolBoolRemoteDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Insert]
        public async Task<bool> Insert(TaskFalseBoolStringRemoteDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Insert]
        public async Task<bool> Insert(TaskFalseBoolTaskBoolRemoteDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Insert]
        public async Task<bool> Insert(TaskFalseBoolTaskStringRemoteDeny v) { await Task.Delay(2); Received.Add(v); return false; }


        [Remote]
        [Create]
        public void Create(RemoteVoidBoolDeny v) { Received.Add(v); }

        [Remote]
        [Create]
        public void Create(RemoteVoidStringDeny v) { Received.Add(v); }

        [Remote]
        [Create]
        public void Create(RemoteVoidTaskBoolDeny v) { Received.Add(v); }

        [Remote]
        [Create]
        public void Create(RemoteVoidTaskStringDeny v) { Received.Add(v); }

        [Remote]
        [Insert]
        public void Insert(RemoteVoidBoolDeny v) { Received.Add(v); }

        [Remote]
        [Insert]
        public void Insert(RemoteVoidStringDeny v) { Received.Add(v); }

        [Remote]
        [Insert]
        public void Insert(RemoteVoidTaskBoolDeny v) { Received.Add(v); }

        [Remote]
        [Insert]
        public void Insert(RemoteVoidTaskStringDeny v) { Received.Add(v); }

        [Remote]
        [Create]
        public bool Create(RemoteTrueBoolBoolDeny v) { Received.Add(v); return true; }

        [Remote]
        [Create]
        public bool Create(RemoteTrueBoolStringDeny v) { Received.Add(v); return true; }

        [Remote]
        [Create]
        public bool Create(RemoteTrueBoolTaskBoolDeny v) { Received.Add(v); return true; }

        [Remote]
        [Create]
        public bool Create(RemoteTrueBoolTaskStringDeny v) { Received.Add(v); return true; }

        [Remote]
        [Insert]
        public bool Insert(RemoteTrueBoolBoolDeny v) { Received.Add(v); return true; }

        [Remote]
        [Insert]
        public bool Insert(RemoteTrueBoolStringDeny v) { Received.Add(v); return true; }

        [Remote]
        [Insert]
        public bool Insert(RemoteTrueBoolTaskBoolDeny v) { Received.Add(v); return true; }

        [Remote]
        [Insert]
        public bool Insert(RemoteTrueBoolTaskStringDeny v) { Received.Add(v); return true; }

        [Remote]
        [Create]
        public bool Create(RemoteFalseBoolBoolDeny v) { Received.Add(v); return false; }

        [Remote]
        [Create]
        public bool Create(RemoteFalseBoolStringDeny v) { Received.Add(v); return false; }

        [Remote]
        [Create]
        public bool Create(RemoteFalseBoolTaskBoolDeny v) { Received.Add(v); return false; }

        [Remote]
        [Create]
        public bool Create(RemoteFalseBoolTaskStringDeny v) { Received.Add(v); return false; }

        [Remote]
        [Insert]
        public bool Insert(RemoteFalseBoolBoolDeny v) { Received.Add(v); return false; }

        [Remote]
        [Insert]
        public bool Insert(RemoteFalseBoolStringDeny v) { Received.Add(v); return false; }

        [Remote]
        [Insert]
        public bool Insert(RemoteFalseBoolTaskBoolDeny v) { Received.Add(v); return false; }

        [Remote]
        [Insert]
        public bool Insert(RemoteFalseBoolTaskStringDeny v) { Received.Add(v); return false; }

        [Remote]
        [Create]
        public async Task Create(RemoteTaskVoidBoolDeny v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Create]
        public async Task Create(RemoteTaskVoidStringDeny v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Create]
        public async Task Create(RemoteTaskVoidTaskBoolDeny v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Create]
        public async Task Create(RemoteTaskVoidTaskStringDeny v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Insert]
        public async Task Insert(RemoteTaskVoidBoolDeny v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Insert]
        public async Task Insert(RemoteTaskVoidStringDeny v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Insert]
        public async Task Insert(RemoteTaskVoidTaskBoolDeny v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Insert]
        public async Task Insert(RemoteTaskVoidTaskStringDeny v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskTrueBoolBoolDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskTrueBoolStringDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskTrueBoolTaskBoolDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskTrueBoolTaskStringDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskTrueBoolBoolDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskTrueBoolStringDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskTrueBoolTaskBoolDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskTrueBoolTaskStringDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskFalseBoolBoolDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskFalseBoolStringDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskFalseBoolTaskBoolDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskFalseBoolTaskStringDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskFalseBoolBoolDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskFalseBoolStringDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskFalseBoolTaskBoolDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskFalseBoolTaskStringDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Create]
        public void Create(RemoteVoidBoolRemoteDeny v) { Received.Add(v); }

        [Remote]
        [Create]
        public void Create(RemoteVoidStringRemoteDeny v) { Received.Add(v); }

        [Remote]
        [Create]
        public void Create(RemoteVoidTaskBoolRemoteDeny v) { Received.Add(v); }

        [Remote]
        [Create]
        public void Create(RemoteVoidTaskStringRemoteDeny v) { Received.Add(v); }

        [Remote]
        [Insert]
        public void Insert(RemoteVoidBoolRemoteDeny v) { Received.Add(v); }

        [Remote]
        [Insert]
        public void Insert(RemoteVoidStringRemoteDeny v) { Received.Add(v); }

        [Remote]
        [Insert]
        public void Insert(RemoteVoidTaskBoolRemoteDeny v) { Received.Add(v); }

        [Remote]
        [Insert]
        public void Insert(RemoteVoidTaskStringRemoteDeny v) { Received.Add(v); }

        [Remote]
        [Create]
        public bool Create(RemoteTrueBoolBoolRemoteDeny v) { Received.Add(v); return true; }

        [Remote]
        [Create]
        public bool Create(RemoteTrueBoolStringRemoteDeny v) { Received.Add(v); return true; }

        [Remote]
        [Create]
        public bool Create(RemoteTrueBoolTaskBoolRemoteDeny v) { Received.Add(v); return true; }

        [Remote]
        [Create]
        public bool Create(RemoteTrueBoolTaskStringRemoteDeny v) { Received.Add(v); return true; }

        [Remote]
        [Insert]
        public bool Insert(RemoteTrueBoolBoolRemoteDeny v) { Received.Add(v); return true; }

        [Remote]
        [Insert]
        public bool Insert(RemoteTrueBoolStringRemoteDeny v) { Received.Add(v); return true; }

        [Remote]
        [Insert]
        public bool Insert(RemoteTrueBoolTaskBoolRemoteDeny v) { Received.Add(v); return true; }

        [Remote]
        [Insert]
        public bool Insert(RemoteTrueBoolTaskStringRemoteDeny v) { Received.Add(v); return true; }

        [Remote]
        [Create]
        public bool Create(RemoteFalseBoolBoolRemoteDeny v) { Received.Add(v); return false; }

        [Remote]
        [Create]
        public bool Create(RemoteFalseBoolStringRemoteDeny v) { Received.Add(v); return false; }

        [Remote]
        [Create]
        public bool Create(RemoteFalseBoolTaskBoolRemoteDeny v) { Received.Add(v); return false; }

        [Remote]
        [Create]
        public bool Create(RemoteFalseBoolTaskStringRemoteDeny v) { Received.Add(v); return false; }

        [Remote]
        [Insert]
        public bool Insert(RemoteFalseBoolBoolRemoteDeny v) { Received.Add(v); return false; }

        [Remote]
        [Insert]
        public bool Insert(RemoteFalseBoolStringRemoteDeny v) { Received.Add(v); return false; }

        [Remote]
        [Insert]
        public bool Insert(RemoteFalseBoolTaskBoolRemoteDeny v) { Received.Add(v); return false; }

        [Remote]
        [Insert]
        public bool Insert(RemoteFalseBoolTaskStringRemoteDeny v) { Received.Add(v); return false; }

        [Remote]
        [Create]
        public async Task Create(RemoteTaskVoidBoolRemoteDeny v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Create]
        public async Task Create(RemoteTaskVoidStringRemoteDeny v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Create]
        public async Task Create(RemoteTaskVoidTaskBoolRemoteDeny v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Create]
        public async Task Create(RemoteTaskVoidTaskStringRemoteDeny v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Insert]
        public async Task Insert(RemoteTaskVoidBoolRemoteDeny v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Insert]
        public async Task Insert(RemoteTaskVoidStringRemoteDeny v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Insert]
        public async Task Insert(RemoteTaskVoidTaskBoolRemoteDeny v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Insert]
        public async Task Insert(RemoteTaskVoidTaskStringRemoteDeny v) { await Task.Delay(2); Received.Add(v); }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskTrueBoolBoolRemoteDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskTrueBoolStringRemoteDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskTrueBoolTaskBoolRemoteDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskTrueBoolTaskStringRemoteDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskTrueBoolBoolRemoteDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskTrueBoolStringRemoteDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskTrueBoolTaskBoolRemoteDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskTrueBoolTaskStringRemoteDeny v) { await Task.Delay(2); Received.Add(v); return true; }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskFalseBoolBoolRemoteDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskFalseBoolStringRemoteDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskFalseBoolTaskBoolRemoteDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Create]
        public async Task<bool> Create(RemoteTaskFalseBoolTaskStringRemoteDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskFalseBoolBoolRemoteDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskFalseBoolStringRemoteDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskFalseBoolTaskBoolRemoteDeny v) { await Task.Delay(2); Received.Add(v); return false; }

        [Remote]
        [Insert]
        public async Task<bool> Insert(RemoteTaskFalseBoolTaskStringRemoteDeny v) { await Task.Delay(2); Received.Add(v); return false; }
    }

    private IServiceScope clientScope;
    private IAuthorizedAllCombinationsFactory authorizedObjectFactory = null!;
    private AuthorizationAllCombinations authorizationClient;
    private AuthorizationAllCombinations authorizationServer;
    private IAuthorizedAllCombinations? writeAuthorizedObject;

    [TestInitialize]
    public void TestIntialize()
    {
        var scopes = FactoryContainers.Scopes();
        clientScope = scopes.client;
        authorizedObjectFactory = clientScope.ServiceProvider.GetRequiredService<IAuthorizedAllCombinationsFactory>();
        authorizationClient = clientScope.ServiceProvider.GetRequiredService<AuthorizationAllCombinations>();
        authorizationServer = scopes.server.ServiceProvider.GetRequiredService<AuthorizationAllCombinations>();

        var readParameter = new VoidBool();
        writeAuthorizedObject = authorizedObjectFactory.Create(readParameter);
    }

    [TestMethod]
    public async Task TestCreate()
    {
        var parameterTypes = Assembly.GetAssembly(typeof(AuthorizationAllCombinations)).GetTypes().Where(t => t.IsClass && t.BaseType.Name.ToString() == "IDd").ToList();

        foreach (var parameterType in parameterTypes)
        {
            var parameter = Activator.CreateInstance(parameterType)!;

            var factoryCreateMethod = authorizedObjectFactory.GetType().GetMethods().Where(m => m.Name == "Create" && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == parameterType).Single();

            var result = factoryCreateMethod.Invoke(authorizedObjectFactory, new object[] { parameter });

            var parameterName = parameterType.Name;

            if (parameterName.Contains("Task") || parameterName.Contains("Remote"))
            {
                Assert.IsInstanceOfType<Task>(result);
                await (Task)result;
                result = ((Task)result).GetType().GetProperty("Result").GetValue(result);
            }

            var authorizedObject = result as AuthorizedAllCombinations;

            if (parameterName.Contains("Deny") || parameterName.Contains("FalseBool"))
            {
                Assert.IsNull(result);
            }
            else
            {
                Assert.IsNotNull(result);
                CollectionAssert.Contains(authorizedObject.Received, parameter);
            }

            if (parameterType.Name.Contains("Remote"))
            {
                CollectionAssert.DoesNotContain(authorizationClient.ReadReceived, parameter);
                CollectionAssert.Contains(authorizationServer.ReadReceived, parameter);
            }
            else
            {
                CollectionAssert.Contains(authorizationClient.ReadReceived, parameter);
                CollectionAssert.DoesNotContain(authorizationServer.ReadReceived, parameter);
            }
        }
    }

    [TestMethod]
    public async Task TestTryCreate()
    {
        var parameterTypes = Assembly.GetAssembly(typeof(AuthorizationAllCombinations)).GetTypes().Where(t => t.IsClass && t.BaseType.Name.ToString() == "IDd").ToList();

        foreach (var parameterType in parameterTypes)
        {
            var parameter = Activator.CreateInstance(parameterType)!;

            var factoryCreateMethod = authorizedObjectFactory.GetType().GetMethods().Where(m => m.Name == "TryCreate" && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == parameterType).Single();

            var result = factoryCreateMethod.Invoke(authorizedObjectFactory, new object[] { parameter });

            var parameterName = parameterType.Name;

            if (parameterName.Contains("Task") || parameterName.Contains("Remote"))
            {
                Assert.IsInstanceOfType<Task>(result);
                await (Task)result;
                result = ((Task)result).GetType().GetProperty("Result").GetValue(result);
            }

            var authorized = result as Authorized<IAuthorizedAllCombinations>;

            if (parameterName.Contains("Deny"))
            {
                Assert.IsNull(authorized.Result);
                Assert.IsFalse(authorized.HasAccess);
            }
            else if (parameterName.Contains("FalseBool"))
            {
                Assert.IsNull(authorized.Result);
                Assert.IsTrue(authorized.HasAccess);
            }
            else
            {
                Assert.IsNotNull(authorized.Result);
                CollectionAssert.Contains(authorized.Result.Received, parameter);
            }

            if (parameterType.Name.Contains("Remote"))
            {
                CollectionAssert.DoesNotContain(authorizationClient.ReadReceived, parameter);
                CollectionAssert.Contains(authorizationServer.ReadReceived, parameter);
            }
            else
            {
                CollectionAssert.Contains(authorizationClient.ReadReceived, parameter);
                CollectionAssert.DoesNotContain(authorizationServer.ReadReceived, parameter);
            }
        }
    }

    [TestMethod]
    public async Task TestSave()
    {
        var parameterTypes = Assembly.GetAssembly(typeof(AuthorizationAllCombinations)).GetTypes().Where(t => t.IsClass && t.BaseType.Name.ToString() == "IDd").ToList();

        foreach (var parameterType in parameterTypes)
        {
            var parameterName = parameterType.Name;
            var parameter = Activator.CreateInstance(parameterType)!;

            var factorySaveMethod = authorizedObjectFactory.GetType().GetMethods()
                .Where(m => m.Name == "Save" && m.GetParameters().Length == 2 && m.GetParameters()[1].ParameterType == parameterType).Single();

            var methodName = factorySaveMethod.Name.ToString();
            var parameterText = string.Join(", ", factorySaveMethod.GetParameters().Select(p => p.ParameterType.Name));

            object? result;

            if (parameterType.Name.Contains("Deny"))
            {
                await Assert.ThrowsExceptionAsync<NotAuthorizedException>(() =>
                {
                    try
                    {
                        if (parameterType.Name.Contains("Task") || parameterType.Name.Contains("Remote"))
                        {
                            return (Task)factorySaveMethod.Invoke(authorizedObjectFactory, new object[] { writeAuthorizedObject, parameter });
                        }
                        else
                        {
                            return Task.FromResult(factorySaveMethod.Invoke(authorizedObjectFactory, new object[] { writeAuthorizedObject, parameter }));
                        }
                    }
                    catch (TargetInvocationException ex)
                    {
                        throw ex.InnerException;
                    }
                });

                continue;
            }
            else
            {
                result = factorySaveMethod.Invoke(authorizedObjectFactory, new object[] { writeAuthorizedObject, parameter });
            }

            if (parameterType.Name.Contains("Task") || parameterType.Name.Contains("Remote"))
            {
                Assert.IsInstanceOfType<Task>(result);
                await (Task)result;
                result = ((Task)result).GetType().GetProperty("Result").GetValue(result);
            }

            var authorizedObject = result as AuthorizedAllCombinations;

            if (parameterName.Contains("Deny") || parameterName.Contains("FalseBool"))
            {
                Assert.IsNull(result);
            }
            else
            {
                Assert.IsNotNull(result);
                CollectionAssert.Contains(authorizedObject.Received, parameter);
            }

            if (parameterType.Name.Contains("Remote"))
            {
                CollectionAssert.DoesNotContain(authorizationClient.WriteReceived, parameter);
                CollectionAssert.Contains(authorizationServer.WriteReceived, parameter);
            }
            else
            {
                CollectionAssert.Contains(authorizationClient.WriteReceived, parameter);
                CollectionAssert.DoesNotContain(authorizationServer.WriteReceived, parameter);
            }
        }
    }


    [TestMethod]
    public async Task TestTrySave()
    {
        var parameterTypes = Assembly.GetAssembly(typeof(AuthorizationAllCombinations)).GetTypes().Where(t => t.IsClass && t.BaseType.Name.ToString() == "IDd").ToList();

        foreach (var parameterType in parameterTypes)
        {
            var parameter = Activator.CreateInstance(parameterType)!;

            var factoryCreateMethod = authorizedObjectFactory.GetType().GetMethods().Where(m => m.Name == "TrySave" && m.GetParameters().Length == 2 && m.GetParameters()[1].ParameterType == parameterType).Single();

            var result = factoryCreateMethod.Invoke(authorizedObjectFactory, new object[] { writeAuthorizedObject, parameter });

            var parameterName = parameterType.Name;

            if (parameterName.Contains("Task") || parameterName.Contains("Remote"))
            {
                Assert.IsInstanceOfType<Task>(result);
                await (Task)result;
                result = ((Task)result).GetType().GetProperty("Result").GetValue(result);
            }

            var authorized = result as Authorized<IAuthorizedAllCombinations>;

            if (parameterName.Contains("Deny"))
            {
                Assert.IsNull(authorized.Result);
                Assert.IsFalse(authorized.HasAccess);
            }
            else if (parameterName.Contains("FalseBool"))
            {
                Assert.IsNull(authorized.Result);
                Assert.IsTrue(authorized.HasAccess);
            }
            else
            {
                Assert.IsNotNull(authorized.Result);
                CollectionAssert.Contains(authorized.Result.Received, parameter);
            }

            if (parameterType.Name.Contains("Remote"))
            {
                CollectionAssert.DoesNotContain(authorizationClient.WriteReceived, parameter);
                CollectionAssert.Contains(authorizationServer.WriteReceived, parameter);
            }
            else
            {
                CollectionAssert.Contains(authorizationClient.WriteReceived, parameter);
                CollectionAssert.DoesNotContain(authorizationServer.WriteReceived, parameter);
            }
        }
    }
}
