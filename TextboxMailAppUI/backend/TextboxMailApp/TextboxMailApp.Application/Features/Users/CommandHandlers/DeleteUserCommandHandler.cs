using MediatR;
using TextboxMailApp.Application.Common.Responses;
using TextboxMailApp.Application.Contracts.Application;
using TextboxMailApp.Application.Features.Users.Commands;

namespace TextboxMailApp.Application.Features.Users.CommandHandlers
{
  public class DeleteUserCommandHandler(IUserCommandService userService) : IRequestHandler<DeleteUserCommand, ApiResult<string>>
  {

    private readonly IUserCommandService _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    public Task<ApiResult<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
      return _userService.DeleteUserAsync(request, cancellationToken);
    }

  }
}
