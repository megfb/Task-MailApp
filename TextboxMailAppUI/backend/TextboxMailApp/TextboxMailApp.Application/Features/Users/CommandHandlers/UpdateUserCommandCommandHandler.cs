using MediatR;
using TextboxMailApp.Application.Common.Responses;
using TextboxMailApp.Application.Contracts.Application;
using TextboxMailApp.Application.Features.Users.Commands;

namespace TextboxMailApp.Application.Features.Users.CommandHandlers
{
  public class UpdateUserCommandCommandHandler(IUserCommandService userService) : IRequestHandler<UpdateUserCommand, ApiResult<UsersDto>>
  {
    private readonly IUserCommandService _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    public Task<ApiResult<UsersDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {

      return _userService.UpdateUserAsync(request, cancellationToken);

    }
  }
}
