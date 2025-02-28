using System.Text.Json.Serialization;

namespace Neatoo.AuthorizationRules;


public class Authorized
{
    public bool HasAccess { get; protected set; }
    public string? Message { get; protected set; }

    public Authorized()
    {
        HasAccess = false;
    }

    public Authorized(bool hasAccess)
    {
        HasAccess = hasAccess;
    }

    public Authorized(string message)
    {
        HasAccess = false;
        Message = message;
    }

    [JsonConstructor]
    public Authorized(bool hasAccess, string message)
    {
        HasAccess = hasAccess;
        Message = message;
    }


    public static implicit operator Authorized(string? message)
    {
        if (string.IsNullOrEmpty(message))
        {
            return new Authorized(true);
        }

        return new Authorized(message);
    }


    public static implicit operator bool(Authorized result)
    {
        return result.HasAccess;
    }

    public static implicit operator Authorized(bool result)
    {
        return new Authorized(result);
    }
}

public class Authorized<T> : Authorized
{
    public T? Result { get; protected set; }

    public Authorized(Authorized result){
        HasAccess = result.HasAccess;
        Message = result.Message;
    }

    public Authorized(T result)
    {
        Result = result;
        HasAccess = true;
    }

    [JsonConstructor]
    public Authorized(bool hasAccess, string message, T result) : base(hasAccess, message)
    {
        Result = result;
    }

    public static Authorized<T> AccessGranted(T result)
    {
        return new Authorized<T>(result);
    }
    public static implicit operator bool(Authorized<T> result)
    {
        return result.HasAccess;
    }
    public static implicit operator T(Authorized<T> result)
    {
        return result.Result;
    }
}

