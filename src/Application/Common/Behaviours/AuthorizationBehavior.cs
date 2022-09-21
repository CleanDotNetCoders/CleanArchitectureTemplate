using Application.Common.Exceptions;
using Application.Common.Security;
using Application.Services;
using Microsoft.AspNetCore.Http;
using MediatR;

namespace Application.Common.Behaviours;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ISecuredRequest
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IIdentityService _identityService;

    public AuthorizationBehavior(IHttpContextAccessor httpContextAccessor, IIdentityService identityService)
        => (_httpContextAccessor, _identityService) = (httpContextAccessor, identityService);

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        bool isNotMatchedARoleClaimWithRequestRoles = _identityService.IsInRole(request.Roles);

        if (isNotMatchedARoleClaimWithRequestRoles) throw new AuthorizationException("You are not authorized.");

        TResponse response = await next();
        return response;
    }
}