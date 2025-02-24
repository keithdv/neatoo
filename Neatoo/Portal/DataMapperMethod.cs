
namespace Neatoo.Portal;

#if NETSTANDARD
internal static class DataMapper
{

#endif

    public enum DataMapperMethodType
    {
        Create = 1,
        Fetch = 2,
        Insert = 4,
        Update = 8,
        Delete = 16,
        Read = 64,
        Write = 128,
        Authorization = 256,
    }

    public enum DataMapperMethod
    {
        Execute = DataMapperMethodType.Read,
        Create = DataMapperMethodType.Create | DataMapperMethodType.Read,
        Fetch = DataMapperMethodType.Fetch | DataMapperMethodType.Read,
        Insert = DataMapperMethodType.Insert | DataMapperMethodType.Write,
        Update = DataMapperMethodType.Update | DataMapperMethodType.Write,
        Delete = DataMapperMethodType.Delete | DataMapperMethodType.Write,
        Authorize = DataMapperMethodType.Authorization,
    }



#if NETSTANDARD
}

#endif
