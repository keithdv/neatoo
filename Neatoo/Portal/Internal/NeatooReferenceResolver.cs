using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Neatoo.Portal.Internal;

class NeatooReferenceResolver : ReferenceResolver, IDisposable
{
    private uint _referenceCount;
    private Dictionary<string, object> _referenceIdToObjectMap = new Dictionary<string, object>();
    private Dictionary<object, string> _objectToReferenceIdMap = new Dictionary<object, string>(ReferenceEqualityComparer.Instance);

    public void Dispose()
    {
        _referenceCount = 0;
        _referenceIdToObjectMap.Clear();
        _objectToReferenceIdMap.Clear();
        _referenceIdToObjectMap = null;
        _objectToReferenceIdMap = null;
    }

    public override void AddReference(string referenceId, object value)
    {
        if (!_referenceIdToObjectMap.TryAdd(referenceId, value))
        {
            throw new JsonException();
        }
    }

    public bool AlreadyExists(object reference)
    {
        if (_objectToReferenceIdMap.ContainsKey(reference))
        {
            return true;
        }
        return false;
    }

    public override string GetReference(object value, out bool alreadyExists)
    {
        var type = value.GetType();
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
        {
            alreadyExists = false;
            return string.Empty;
        }

        if (_objectToReferenceIdMap.TryGetValue(value, out string referenceId))
        {
            alreadyExists = true;
        }
        else
        {
            _referenceCount++;
            referenceId = _referenceCount.ToString();
            _objectToReferenceIdMap.Add(value, referenceId);
            alreadyExists = false;
        }

        return referenceId;
    }

    public override object ResolveReference(string referenceId)
    {
        if (!_referenceIdToObjectMap.TryGetValue(referenceId, out object value))
        {
            throw new JsonException();
        }

        return value;
    }
}
