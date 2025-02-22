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
        }
    }


    private Dictionary<string, Type?> TypeCache { get; set; } = new Dictionary<string, Type?>();
    private Dictionary<string, Type?> DelegateTypeCache { get; set; } = new Dictionary<string, Type?>();
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

        var foundType = AppDomain.CurrentDomain.GetAssemblies()
                .Select(a => a.GetType(fullName))
                .Where(a => a != null && a.FullName != null)
                .FirstOrDefault() ?? throw new Exception($"Could not find type {fullName}");

        if(foundType == null)
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
