using Microsoft.AspNetCore.Http;

namespace MuhammedTask.BuildingBlocks.Presentation.SessionContexts;

public sealed class DefaultSessionContext(IHttpContextAccessor httpContextAccessor) : SessionContext(httpContextAccessor);
