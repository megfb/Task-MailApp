using MediatR;
using TextboxMailApp.Application.Common.Responses;
using TextboxMailApp.Application.Contracts.Application;
using TextboxMailApp.Application.Features.Users.Queries;

namespace TextboxMailApp.Application.Features.Users.QueryHandlers
{
  public class LoginQueryHandler(IUserQueryService userQueryService) : IRequestHandler<LoginQuery, ApiResult<string>>
  {
    private readonly IUserQueryService _userQueryService = userQueryService ?? throw new ArgumentNullException(nameof(userQueryService));
    public Task<ApiResult<string>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
      return _userQueryService.LoginAsync(request, cancellationToken);
    }
  }
}
