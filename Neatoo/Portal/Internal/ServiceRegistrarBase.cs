using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neatoo.Portal.Internal
{
    public abstract class ServiceRegistrarBase
    {
        public Collection<Action<IServiceCollection>> Registrations { get; } = [];
    }
}
