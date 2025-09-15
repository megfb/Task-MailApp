using AutoMapper;
using MediatR;
using TextboxMailApp.Application.Common.Responses;
using TextboxMailApp.Application.Contracts.Api;
using TextboxMailApp.Application.Contracts.Application;
using TextboxMailApp.Application.Features.EmailMessages.Queries;

namespace TextboxMailApp.Application.Features.EmailMessages.QueryHandlers
{
  public class GetLatestEmailsQueryHandler(IMapper mapper, ICurrentUserService currentUserService, IEmailFetchService emailFetchService) : IRequestHandler<GetLatestEmailsQuery, ApiResult<IEnumerable<EmailMessagesDto>>>
  {
    private readonly ICurrentUserService _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
    private readonly IEmailFetchService _emailFetcService = emailFetchService ?? throw new ArgumentNullException(nameof(emailFetchService));
    public async Task<ApiResult<IEnumerable<EmailMessagesDto>>> Handle(GetLatestEmailsQuery request, CancellationToken cancellationToken)
    {

      //db den mailler çağırıldı eğer db de yoksa mail adresinden çağırıldı db ye kaydedildi ve sonrasında kullanıcıya gönderildi

      var userId = _currentUserService.UserId!;

      var emails = await _emailFetcService.GetEmailsFromDbAsync(request, userId);
      if (emails.Data.Any())
      {
        return emails;
      }

      await _emailFetcService.FetchAndSaveEmailsAsync(userId, cancellationToken);
      var refreshedEmails = await _emailFetcService.GetEmailsFromDbAsync(request, userId);

      return refreshedEmails;
    }

  }
}
