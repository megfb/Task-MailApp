using MediatR;
using TextboxMailApp.Application.Common.Responses;
using TextboxMailApp.Application.Contracts.Application;
using TextboxMailApp.Application.Features.EmailMessages.Queries;

namespace TextboxMailApp.Application.Features.EmailMessages.QueryHandlers
{
  public class GetEmailQueryHandler(IEmailFetchService emailFetchService) : IRequestHandler<GetEmailQuery, ApiResult<EmailMessagesDto>>
  {

    private readonly IEmailFetchService _emailFetchService = emailFetchService ?? throw new ArgumentNullException(nameof(emailFetchService));
    public async Task<ApiResult<EmailMessagesDto>> Handle(GetEmailQuery request, CancellationToken cancellationToken)
    {

      //db den id ye göre spesifik bir mail çağırıldı
      return await _emailFetchService.GetEmailByIdAsync(request.Id);
    }
  }

}
