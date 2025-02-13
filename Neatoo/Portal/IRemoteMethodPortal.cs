using System;
using System.Threading.Tasks;

namespace Neatoo.Portal;

public interface IRemoteMethodPortal<D> where D : Delegate
{

    Task<T> Execute<T>(params object[] p);

}
