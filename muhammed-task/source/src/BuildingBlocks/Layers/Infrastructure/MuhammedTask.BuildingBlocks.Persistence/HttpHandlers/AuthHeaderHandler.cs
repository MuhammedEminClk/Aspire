using System.Net.Http.Headers;
using CSharpEssentials;
using MuhammedTask.BuildingBlocks.Application.Shared.Abstractions;
using MuhammedTask.BuildingBlocks.Application.Shared.Constants;

namespace MuhammedTask.BuildingBlocks.Persistence.HttpHandlers;
public sealed class AuthHeaderHandler(
    ISessionContext sessionContext) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Maybe<string> accessToken = sessionContext.AccessToken;
        Maybe<string> tenantId = sessionContext.TenantId;

        accessToken.Execute(token => request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token));
        tenantId.Execute(tenantId => request.Headers.Add(CustomHeaderNames.TenantId, tenantId));

        return base.SendAsync(request, cancellationToken);
    }
}
