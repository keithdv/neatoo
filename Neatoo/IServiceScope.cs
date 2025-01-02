using System;

namespace Neatoo
{
    public interface IServiceScope : IDisposable
    {
        IServiceScope BeginNewScope(object tag = null);
        T Resolve<T>();

        object Resolve(Type t);

        bool TryResolve<T>(out T result) where T : class;

        bool TryResolve(Type T, out object result);

        Type ConcreteType(Type T);
        Type ConcreteType<T>();

        bool IsRegistered<T>();

        bool IsRegistered(Type type);
    }

}