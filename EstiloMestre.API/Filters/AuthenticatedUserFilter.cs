using EstiloMestre.Communication.Responses;
using EstiloMestre.Domain.Repositories.User;
using EstiloMestre.Domain.Security.Tokens;
using EstiloMestre.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace EstiloMestre.API.Filters;

public class AuthenticatedUserFilter : TokenOnRequest, IAsyncAuthorizationFilter
{
    private readonly IAccessTokenValidator _tokenValidator;
    private readonly IUserRepository _repository;

    public AuthenticatedUserFilter(IAccessTokenValidator tokenValidator, IUserRepository repository)
    {
        _tokenValidator = tokenValidator;
        _repository = repository;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = Token(context);

            var userIdentifier = _tokenValidator.ValidateAndGetUserIdentifier(token);
            var userExist = await _repository.ExistActiveUserWithIdentifier(userIdentifier);
            if (userExist == null)
                throw new EstiloMestreException(ResourceMessagesExceptions.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE);
        } catch (SecurityTokenExpiredException)
        {
            context.Result = new UnauthorizedObjectResult(
                new ResponseErrorJson(ResourceMessagesExceptions.TOKEN_EXPIRED)
                {
                    TokenIsExpired = true
                });
        } catch (EstiloMestreException ex)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ex.Message));
        } catch
        {
            context.Result = new UnauthorizedObjectResult(
                new ResponseErrorJson(ResourceMessagesExceptions.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE));
        }
    }
}
