using Neatoo.Internal;
using Neatoo.Portal;
using System.Threading.Tasks;

namespace Neatoo;

public delegate Task<RemoteResponseDto> ServerHandlePortalRequest(RemoteRequestDto portalRequest);