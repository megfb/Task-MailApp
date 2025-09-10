using TextboxMailApp.Application.Common.Responses;
using TextboxMailApp.Application.Features.EmailMessages;
using TextboxMailApp.Application.Features.EmailMessages.Queries;

namespace TextboxMailApp.Application.Contracts.Application
{
    public interface IEmailFetchService
    {
        Task<ApiResult<IEnumerable<EmailMessagesDto>>> GetEmailsFromDbAsync(GetLatestEmailsQuery request, string userId);
        Task FetchAndSaveEmailsAsync(string userId, CancellationToken cancellationToken);
        Task<ApiResult<IEnumerable<EmailMessagesDto>>> RefreshEmailsAsync(string userId, CancellationToken cancellationToken);
        Task<ApiResult<EmailMessagesDto>> GetEmailByIdAsync(string emailId);
    }
}
