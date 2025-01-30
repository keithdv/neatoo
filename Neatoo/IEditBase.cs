using Neatoo.Portal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neatoo
{
    public interface IEditBase : IValidateBase, IEditMetaProperties
    {
        IEnumerable<string> ModifiedProperties { get; }

        /// <summary>
        /// Marks the object as deleted
        /// </summary>
        void Delete();
        void UnDelete();

        Task<IEditBase> Save();
    }

}
