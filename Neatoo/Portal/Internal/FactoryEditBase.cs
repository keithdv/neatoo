using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.Portal.Internal
{
    public interface IFactoryEditBase<in T>
    {
        Task<IEditBase?> Save(T target);
    }

    public abstract class FactoryEditBase<T> : FactoryBase, IFactoryEditBase<T>
        where T : IEditBase
    {

        public virtual Task<IEditBase?> Save(T target)
        {
            throw new Exception("Save not implemented");
        }
    }
}

