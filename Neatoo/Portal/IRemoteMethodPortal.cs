using System;
using System.Threading.Tasks;

namespace Neatoo.Portal;

public interface IRemoteMethodPortal<T>
{
    Task<T> Execute();
}

public interface IRemoteMethodPortal<P, T> 
{
    Task<T> Execute(P parameters);
}
