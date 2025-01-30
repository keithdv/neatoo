using Newtonsoft.Json;
using Neatoo.Core;
using Neatoo.Rules;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neatoo.Portal;

namespace Neatoo.Newtonsoft.Json
{
    // intermediate class that can be serialized by JSON.net
    // and contains the same data as ListBaseCollection
    public class ListBaseSurrogate
    {
        public ListBaseSurrogate(Type listType, ICollection collection)
        {
            ListType = listType;
            Collection = collection;
        }

        public Type ListType { get; }

        // the collection of ListBase elements
        public ICollection Collection { get; }

        public bool IsNew { get; set; }
        public bool IsChild { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsMarkedModified { get; set; }
        public IBase Parent { get; set; }
    }

    public class ListBaseCollectionConverter : JsonConverter
    {

        public ListBaseCollectionConverter(IServiceScope scope)
        {
            Scope = scope;
        }

        public IServiceScope Scope { get; }

        public override bool CanConvert(Type objectType)
        {
            if (objectType.IsInterface)
            {
                return objectType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IListBase<>));
            }
            else
            {
                return GetListBase(objectType) != null;
            }
        }

        public override object ReadJson(
            JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer)
        {
            var surrogate = serializer.Deserialize<ListBaseSurrogate>(reader);


            var list = (IListBase)Scope.Resolve(surrogate.ListType);
            using(var stopped = (list as IPortalEditTarget)?.StopAllActions())
            {
                foreach (var i in surrogate.Collection)
                {
                    using ((i as IPortalEditTarget)?.StopAllActions())
                    {
                        list.Add(i);
                        if (i is ISetParent setParent)
                        {
                            setParent.SetParent(list.Parent);
                        }
                    }
                }
                ((ISetParent) list).SetParent(surrogate.Parent);
            }

            return list;
        }

        private Type GetListBase(Type type)
        {
            do
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ListBase<,>))
                {
                    return type;
                }
                type = type.BaseType;
            } while (type != null);
            return null;
        }

        private Type GetValidateListBase(Type type)
        {
            do
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ValidateListBase<,>))
                {
                    return type;
                }
                type = type.BaseType;
            } while (type != null);
            return null;
        }

        private Type GetEditListBase(Type type)
        {
            do
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(EditListBase<,>))
                {
                    return type;
                }
                type = type.BaseType;
            } while (type != null);
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value,
                                       JsonSerializer serializer)
        {

            var itemType = GetListBase(value.GetType()).GetGenericArguments()[1];
            var listType = typeof(List<>).MakeGenericType(itemType);
            var list = (IList)Activator.CreateInstance(listType, value);

            // Causes a circular reference
            // Set back in ReadJson
            list.OfType<ISetParent>().ToList().ForEach(i => i.SetParent(null));

            // Get PropertyManager property
            var surrogate = new ListBaseSurrogate(value.GetType(), list);

            surrogate.Parent = (value as IBase)?.Parent;

            if (value is IEditMetaProperties edit)
            {
                surrogate.IsNew = edit.IsNew;
                surrogate.IsChild = edit.IsChild;
                surrogate.IsDeleted = edit.IsDeleted;
                surrogate.IsMarkedModified = edit.IsMarkedModified;
            }


            serializer.Serialize(writer, surrogate);

        }
    }
}
