using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using Neatoo.Core;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Neatoo.Internal;

namespace Neatoo.Portal.Internal;

public class NeatooInterfaceJsonTypeConverter<T> : JsonConverter<T>
{
    private readonly IServiceProvider scope;
    private readonly ILocalAssemblies localAssemblies;

    public NeatooInterfaceJsonTypeConverter(IServiceProvider scope, ILocalAssemblies localAssemblies)
    {
        this.scope = scope;
        this.localAssemblies = localAssemblies;
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        List<PropertyInfo> editProperties = null;
        var editBaseType = typeToConvert;


        T? result = default;
        string id = string.Empty;
        Type? concreteType = default;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    options.ReferenceHandler.CreateResolver().AddReference(id, result);
                }

                if (result is IJsonOnDeserialized jsonOnDeserialized)
                {
                    jsonOnDeserialized.OnDeserialized();
                }

                return result;
            }

            // Get the key.
            if (reader.TokenType != JsonTokenType.PropertyName) { throw new JsonException(); }

            string propertyName = reader.GetString();
            reader.Read();

            if (propertyName == "$type")
            {
                var typeName = reader.GetString();
                concreteType = localAssemblies.FindType(typeName);
            }
            else if (propertyName == "$value")
            {
                result = (T?)JsonSerializer.Deserialize(ref reader, concreteType, options);
            }
        }

        throw new JsonException();
    }
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("$type");
        var type = value.GetType().FullName;
        writer.WriteStringValue(type);


        writer.WritePropertyName("$value");
        JsonSerializer.Serialize(writer, value, value.GetType(), options);

        writer.WriteEndObject();
    }
}
