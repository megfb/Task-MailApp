using MediatR;
using TextboxMailApp.Application.Common.Responses;

namespace TextboxMailApp.Application.Features.EmailMessages.Queries
{
  public class GetEmailQuery : IRequest<ApiResult<EmailMessagesDto>>
  {
    public string Id { get; set; }
  }
}
