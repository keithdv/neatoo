using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Neatoo.Core;
using System.Reflection.PortableExecutable;
using System.Reflection;

namespace Neatoo.Portal
{

    public class NeatooJsonSerializer : IDisposable, IPortalJsonSerializer
    {
        JsonSerializerOptions Options { get; }
        private MyReferenceHandler MyReferenceHandler { get; }
        public NeatooJsonSerializer(NeatooJsonConverterFactory neatooJsonConverterFactory)
        {

            MyReferenceHandler = new MyReferenceHandler();
            Options = new JsonSerializerOptions
            {
                ReferenceHandler = MyReferenceHandler,
                Converters = { neatooJsonConverterFactory },
                WriteIndented = true,
                IncludeFields = true
            };
        }

        public void Dispose()
        {
            MyReferenceHandler.Reset();
        }

        public string Serialize(object target)
        {
            return JsonSerializer.Serialize(target, Options);
        }

        public T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, Options);
        }

        public object Deserialize(string json, Type type)
        {
            return JsonSerializer.Deserialize(json, type, Options);
        }
    }

    public class NeatooJsonConverterFactory : JsonConverterFactory
    {
        private IServiceScope scope;

        public NeatooJsonConverterFactory(IServiceScope scope)
        {
            this.scope = scope;
        }

        public override bool CanConvert(Type typeToConvert)
        {
            if (typeToConvert.IsAssignableTo(typeof(IBase))
                    || typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Base<>))
            {
                return true;
            }
            else if (typeToConvert.IsAssignableTo(typeof(IListBase)))
            {
                return true;
            }
            return false;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeToConvert.IsAssignableTo(typeof(IBase)))
            {
                return (JsonConverter)scope.Resolve(typeof(NeatooBaseJsonTypeConverter<>).MakeGenericType(typeToConvert));
            }
            else if (typeToConvert.IsAssignableTo(typeof(IListBase)))
            {
                return (JsonConverter)scope.Resolve(typeof(NeatooListBaseJsonTypeConverter<>).MakeGenericType(typeToConvert));
            }

            return null;
        }
    }

    public class NeatooBaseJsonTypeConverter<T> : JsonConverter<T>
            where T : IBase
    {
        private readonly IServiceScope scope;

        public NeatooBaseJsonTypeConverter(IServiceScope scope)
        {
            this.scope = scope;
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            List<PropertyInfo> editProperties = null;
            var editBaseType = typeToConvert;



            T result = default;
            string id = string.Empty;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        options.ReferenceHandler.CreateResolver().AddReference(id, result);
                    }

                    if(result is IJsonOnDeserialized jsonOnDeserialized)
                    {
                        jsonOnDeserialized.OnDeserialized();
                    }

                    return result;
                }

                // Get the key.
                if (reader.TokenType != JsonTokenType.PropertyName) { throw new JsonException(); }

                string propertyName = reader.GetString();
                reader.Read();

                if (propertyName == "$ref")
                {
                    var refId = reader.GetString();

                    result = (T)options.ReferenceHandler.CreateResolver().ResolveReference(refId);

                    reader.Read();

                    return result;
                }
                else if (propertyName == "$id")
                {
                    id = reader.GetString();
                }
                else if (propertyName == "$type")
                {
                    var typeString = reader.GetString();
                    var type = IPortalJsonSerializer.ToType(typeString);
                    result = (T)scope.Resolve(type);

                    if(result is IJsonOnDeserializing jsonOnDeserializing)
                    {
                        jsonOnDeserializing.OnDeserializing();
                    }

                    editBaseType = result.GetType();

                    do
                    {
                        if (editBaseType.IsGenericType && editBaseType.GetGenericTypeDefinition() == typeof(EditBase<>))
                        {
                            editProperties = editBaseType.GetProperties().Where(p => p.SetMethod != null).ToList();
                            break;
                        }

                        editBaseType = editBaseType.BaseType;

                    } while (editBaseType != null);

                }
                else if (propertyName == "PropertyManager")
                {

                    List<IProperty> list = new List<IProperty>();

                    if (reader.TokenType != JsonTokenType.StartArray) { throw new JsonException(); }

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndArray) { break; }
                        if (reader.TokenType != JsonTokenType.StartObject) { throw new JsonException(); }

                        Type propertyType = default;

                        while (reader.Read())
                        {
                            if (reader.TokenType == JsonTokenType.EndObject) { break; }
                            if (reader.TokenType != JsonTokenType.PropertyName) { throw new JsonException(); }

                            propertyName = reader.GetString();

                            reader.Read();

                            if (propertyName == "$type")
                            {
                                var typeFullName = reader.GetString();

                                propertyType = Type.GetType(reader.GetString());
                            }
                            else if (propertyName == "$value")
                            {
                                var property = JsonSerializer.Deserialize(ref reader, propertyType, options);
                                list.Add((IProperty)property);
                            }
                        }
                    }

                    result.PropertyManager.SetProperties(list);

                    if(result.PropertyManager is IJsonOnDeserialized jsonOnDeserialized)
                    {
                        jsonOnDeserialized.OnDeserialized();
                    }

                }
                else if(editProperties != null && editProperties.Any(p => p.Name == propertyName))
                {
                    var property = editProperties.First(p => p.Name == propertyName);
                    var value = JsonSerializer.Deserialize(ref reader, property.PropertyType, options);
                    property.SetValue(result, value);
                }
            }

            throw new JsonException();
        }
        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var properties = value.PropertyManager.GetProperties.ToList();

            var reference = options.ReferenceHandler.CreateResolver().GetReference(value, out var alreadyExists);

            if (alreadyExists)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("$ref");
                writer.WriteStringValue(reference);
                writer.WriteEndObject();
            }
            else
            {
                writer.WriteStartObject();

                writer.WritePropertyName("$id");
                writer.WriteStringValue(reference);

                writer.WritePropertyName("$type");
                writer.WriteStringValue(value.GetType().FullName);

                writer.WritePropertyName("PropertyManager");

                writer.WriteStartArray();

                foreach (var p in properties)
                {
                    writer.WriteStartObject();

                    writer.WritePropertyName("$type");
                    writer.WriteStringValue(p.GetType().AssemblyQualifiedName);

                    writer.WritePropertyName("$value");

                    //writer.WriteStartObject();

                    JsonSerializer.Serialize(writer, p, p.GetType(), options);

                    writer.WriteEndObject();
                }


                writer.WriteEndArray();

                if (value is IEditMetaProperties editMetaProperties)
                {
                    var editProperties = typeof(IEditMetaProperties).GetProperties().ToList();

                    foreach (var p in editProperties)
                    {
                        writer.WritePropertyName(p.Name);
                        JsonSerializer.Serialize(writer, p.GetValue(editMetaProperties), p.PropertyType, options);
                    }
                }

                writer.WriteEndObject();
            }
        }
    }


    public class NeatooListBaseJsonTypeConverter<T> : JsonConverter<T>
            where T : IListBase
    {
        private readonly IServiceScope scope;

        public NeatooListBaseJsonTypeConverter(IServiceScope scope)
        {
            this.scope = scope;
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            T list = default;
            string id = string.Empty;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        options.ReferenceHandler.CreateResolver().AddReference(id, list);
                    }

                    if (list is IJsonOnDeserialized jsonOnDeserialized)
                    {
                        jsonOnDeserialized.OnDeserialized();
                    }

                    return list;
                }

                // Get the key.
                if (reader.TokenType != JsonTokenType.PropertyName) { throw new JsonException(); }

                string propertyName = reader.GetString();
                reader.Read();

                if (propertyName == "$ref")
                {
                    var refId = reader.GetString();
                    list = (T)options.ReferenceHandler.CreateResolver().ResolveReference(refId);
                    reader.Read();
                    return list;
                }
                else if (propertyName == "$id")
                {
                    id = reader.GetString();
                }
                else if (propertyName == "$type")
                {
                    var typeString = reader.GetString();
                    var type = IPortalJsonSerializer.ToType(typeString);
                    list = (T)scope.Resolve(type);

                    if (list is IJsonOnDeserializing jsonOnDeserializing)
                    {
                        jsonOnDeserializing.OnDeserializing();
                    }
                }
                else if (propertyName == "$items")
                {
                    if (reader.TokenType != JsonTokenType.StartArray) { throw new JsonException(); }

                    Type type = default;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndArray) { break; }
                        if (reader.TokenType != JsonTokenType.StartObject) { throw new JsonException(); }

                        while (reader.Read())
                        {
                            if (reader.TokenType == JsonTokenType.EndObject) { break; }

                            propertyName = reader.GetString();
                            reader.Read();

                            if (propertyName == "$type")
                            {
                                var typeString = reader.GetString();
                                type = IPortalJsonSerializer.ToType(typeString);
                            }
                            else if (propertyName == "$value")
                            {
                                var item = JsonSerializer.Deserialize(ref reader, type, options);

                                list.Add(item);
                            }
                        }
                    }
                }

            }

            throw new JsonException();
        }
        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var items = value.GetEnumerator();

            writer.WriteStartObject();

            var reference = options.ReferenceHandler.CreateResolver().GetReference(value, out var alreadyExists);

            writer.WritePropertyName("$id");
            writer.WriteStringValue(reference);

            writer.WritePropertyName("$type");
            writer.WriteStringValue(value.GetType().FullName);

            writer.WritePropertyName("$items");
            writer.WriteStartArray();

            while (items.MoveNext())
            {
                writer.WriteStartObject();

                writer.WritePropertyName("$type");
                writer.WriteStringValue(items.Current.GetType().FullName);

                writer.WritePropertyName("$value");

                JsonSerializer.Serialize(writer, items.Current, items.Current.GetType(), options);

                writer.WriteEndObject();
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }

    class MyReferenceHandler : ReferenceHandler
    {
        private ReferenceResolver _rootedResolver;
        public MyReferenceHandler() => Reset();
        public override ReferenceResolver CreateResolver() => _rootedResolver;
        public void Reset() => _rootedResolver = new MyReferenceResolver();

        class MyReferenceResolver : ReferenceResolver
        {
            private uint _referenceCount;
            private readonly Dictionary<string, object> _referenceIdToObjectMap = new Dictionary<string, object>();
            private readonly Dictionary<object, string> _objectToReferenceIdMap = new Dictionary<object, string>(ReferenceEqualityComparer.Instance);

            public override void AddReference(string referenceId, object value)
            {
                if (!_referenceIdToObjectMap.TryAdd(referenceId, value))
                {
                    throw new JsonException();
                }
            }

            public override string GetReference(object value, out bool alreadyExists)
            {
                var type = value.GetType();
                if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
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
    }
}
