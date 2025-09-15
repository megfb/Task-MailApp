using MediatR;
using TextboxMailApp.Application.Common.Responses;
using TextboxMailApp.Application.Contracts.Api;
using TextboxMailApp.Application.Contracts.Application;
using TextboxMailApp.Application.Features.EmailMessages.Queries;

namespace TextboxMailApp.Application.Features.EmailMessages.QueryHandlers
{
  public class RefreshEmailQueryHandler(ICurrentUserService currentUserService, IEmailFetchService emailFetchService) : IRequestHandler<RefreshEmailQuery, ApiResult<IEnumerable<EmailMessagesDto>>>
  {
    private readonly IEmailFetchService _emailFetchService = emailFetchService ?? throw new ArgumentNullException(nameof(emailFetchService));
    private readonly ICurrentUserService _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));

    public async Task<ApiResult<IEnumerable<EmailMessagesDto>>> Handle(RefreshEmailQuery request, CancellationToken cancellationToken)
    {
      //mailler refresh edildi yeni gelenler varsa getirildi
      string id = _currentUserService.UserId!;
      return await _emailFetchService.RefreshEmailsAsync(id, cancellationToken);
    }
  }
}
