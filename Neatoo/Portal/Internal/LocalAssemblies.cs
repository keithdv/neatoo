using System.Reflection;

namespace Neatoo;

public interface ILocalAssemblies
{
    bool HasType(Type type);
    Type? FindType(string fullName);
}

internal class LocalAssemblies : ILocalAssemblies
{
    private List<Assembly> Assemblies { get; } = new List<Assembly>();
    public LocalAssemblies(Assembly[] assemblies)
    {
        Assemblies.AddRange(assemblies);

        var foundTypes = assemblies.SelectMany(a => a.GetTypes())
        .Where(a => a != null && a.FullName != null);


        lock (lockObject)
        {
            foreach (var type in foundTypes)
            {
                TypeCache[type.FullName!] = type;
            }

            foreach (var assembly in assemblies)
            {
                var typeIds = assembly.GetTypes()
                    .Where(t => t.FullName != null && t.FullName.EndsWith("TypeId"))
                    .ToDictionary(t => (long)t.GetField("TypeId")!.GetValue(null));
                SerializationTypeIds[assembly] = typeIds;
            }
        }

    }


    private Dictionary<string, Type?> TypeCache { get; set; } = [];
    private Dictionary<string, Type?> DelegateTypeCache { get; set; } = [];
    private Dictionary<Assembly, Dictionary<long, Type?>> SerializationTypeIds { get; set; } = new();
    private object lockObject = new object();

    public bool HasType(Type type)
    {
        return Assemblies.Contains(type.Assembly);
    }

    public Type? FindType(string fullName)
    {
        lock (lockObject)
        {
            if (TypeCache.TryGetValue(fullName, out var t))
            {
                return t;
            }
        }

        var foundType = Type.GetType(fullName);

        if (foundType == null)
        {
            return null;
        }

        lock (lockObject)
        {
            TypeCache[foundType.FullName!] = foundType;
        }

        return foundType;
    }
}
