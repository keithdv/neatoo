﻿using Neatoo.Portal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neatoo
{
    public interface IEditBase : IValidateBase, IEditMetaProperties, IPortalEditTarget
    {
        IEnumerable<string> ModifiedProperties { get; }
        bool IsChild { get; }

        /// <summary>
        /// Marks the object as deleted
        /// </summary>
        void Delete();

        Task Save();

    }



}
