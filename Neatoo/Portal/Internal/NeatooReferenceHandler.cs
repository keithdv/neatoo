using System.Text.Json.Serialization;
using System.Threading;

namespace Neatoo.Portal.Internal;

class NeatooReferenceHandler : ReferenceHandler
{
    public AsyncLocal<ReferenceResolver> asyncLocal = new AsyncLocal<ReferenceResolver>();

    public override ReferenceResolver CreateResolver()
    {
        return asyncLocal.Value;
    }

    
}
