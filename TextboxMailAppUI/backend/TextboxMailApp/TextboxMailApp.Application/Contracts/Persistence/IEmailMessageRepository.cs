using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Application.Contracts.Persistence
{
    public interface IEmailMessageRepository : IGenericRepository<EmailMessage>
    {
        Task<IEnumerable<EmailMessage>> GetAllByPageBySearchTermAsync(int pageNumber, int pageSize, string userId, string searchTerm);
        Task<IEnumerable<EmailMessage>> GetAllByPageSortedAsync(int pageNumber, int pageSize, string userId, string sortBy, bool sortDesc = true);
        Task<EmailMessage?> GetLatestAsync(string id);
        Task<uint?> GetMinUidAsync(string userId);

    }
}
