using TextboxMailApp.Application.Common.Responses;
using TextboxMailApp.Application.Features.Users;
using TextboxMailApp.Application.Features.Users.Commands;

namespace TextboxMailApp.Application.Contracts.Application
{
  public interface IUserCommandService
  {
    Task<ApiResult<UsersDto>> CreateUserAsync(CreateUserCommand request, CancellationToken cancellationToken);
    Task<ApiResult<UsersDto>> UpdateUserAsync(UpdateUserCommand request, CancellationToken cancellationToken);
    Task<ApiResult<string>> DeleteUserAsync(DeleteUserCommand request, CancellationToken cancellationToken);


  }
}
