using MediatR;
using TextboxMailApp.Application.Common.Responses;

namespace TextboxMailApp.Application.Features.Users.Commands
{
  public class DeleteUserCommand : IRequest<ApiResult<string>>
  {
  }
}
