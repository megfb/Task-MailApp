using TextboxMailApp.Application.Common.Responses;
using TextboxMailApp.Application.Features.Users.Queries;

namespace TextboxMailApp.Application.Contracts.Application
{
  public interface IUserQueryService
  {
    Task<ApiResult<string>> LoginAsync(LoginQuery request, CancellationToken cancellationToken);

  }
}
