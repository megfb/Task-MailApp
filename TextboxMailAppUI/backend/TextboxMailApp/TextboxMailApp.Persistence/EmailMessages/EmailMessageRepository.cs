using Microsoft.EntityFrameworkCore;
using TextboxMailApp.Application.Contracts.Persistence;
using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Persistence.EmailMessages
{
    public class EmailMessageRepository(AppDbContext appDbContext) : GenericRepository<EmailMessage>(appDbContext), IEmailMessageRepository
    {
        private readonly DbSet<EmailMessage> _dbSet = appDbContext.Set<EmailMessage>();


        //virtual method override edildi pagination yöntemi ile db den data çekildi
        public override async Task<IEnumerable<EmailMessage>> GetAllByPageAsync(int pageNumber, int pageSize, string userId)
        {
            return await _dbSet.Where(x => x.UserId == userId).OrderByDescending(e => e.Uid).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        //filtreleme özelliği ile paggination yöntemi ile db den data çekildi
        public async Task<IEnumerable<EmailMessage>> GetAllByPageBySearchTermAsync(int pageNumber, int pageSize, string userId, string searchTerm)
        {
            return await _dbSet
                .Where(x => x.UserId == userId && (x.Subject.Contains(searchTerm) || x.FromName.Contains(searchTerm) || x.FromAddress.Contains(searchTerm)))
                .OrderByDescending(x => x.Uid).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        //sıralama özelliği ile db den pagination 
        public async Task<IEnumerable<EmailMessage>> GetAllByPageSortedAsync(int pageNumber, int pageSize, string userId, string sortBy, bool sortDesc = true)
        {
            IQueryable<EmailMessage> query = _dbSet.Where(x => x.UserId == userId);

            //sıralama koşulu

            switch (sortBy.ToLower())
            {
                case "fromname":
                    query = sortDesc ? query.OrderByDescending(x => x.FromName)
                                     : query.OrderBy(x => x.FromName);
                    break;
                case "fromaddress":
                    query = sortDesc ? query.OrderByDescending(x => x.FromAddress)
                                     : query.OrderBy(x => x.FromAddress);
                    break;
                case "subject":
                    query = sortDesc ? query.OrderByDescending(x => x.Subject)
                                     : query.OrderBy(x => x.Subject);
                    break;
                case "date":
                    query = sortDesc ? query.OrderByDescending(x => x.Date)
                                     : query.OrderBy(x => x.Date);
                    break;
                default:
                    query = sortDesc ? query.OrderByDescending(x => x.Uid)
                                     : query.OrderBy(x => x.Uid);
                    break;
            }

            // Sayfalama
            query = query.Skip((pageNumber - 1) * pageSize)
                         .Take(pageSize);

            return await query.ToListAsync();
        }


        public async Task<EmailMessage?> GetLatestAsync(string id)
        {
            //son gelen mail alındı
            return await _dbSet.Where(x => x.UserId == id).OrderByDescending(e => e.Uid).FirstOrDefaultAsync();
        }

        public async Task<uint?> GetMinUidAsync(string userId)
        {
            //en küçük uid alındı
            return await _dbSet.Where(x => x.UserId == userId).MinAsync(x => (uint?)x.Uid);
        }
    }
}
