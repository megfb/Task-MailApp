using AutoMapper;
using TextboxMailApp.Application.Common.Responses;
using TextboxMailApp.Application.Contracts.Application;
using TextboxMailApp.Application.Contracts.Persistence;
using TextboxMailApp.Application.Features.EmailMessages.Queries;
using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Application.Features.EmailMessages
{
    public class EmailFetchService(IEmailMessageRepository emailMessageRepository, IUserRepository userRepository, IEmailReader emailReader, IUnitOfWork unitOfWork, IMapper mapper) : IEmailFetchService
    {
        private readonly IEmailMessageRepository _emailMessageRepository = emailMessageRepository ?? throw new ArgumentNullException(nameof(emailMessageRepository));
        private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        private readonly IEmailReader _emailReader = emailReader ?? throw new ArgumentNullException(nameof(emailReader));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        //mailler adresten alındı
        public async Task FetchAndSaveEmailsAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId)
                       ?? throw new Exception("Kullanıcı bulunamadı");

            //geçmiş mail de referans noktası oluşturmak içi en küçük uid alındı
            var minUid = await _emailMessageRepository.GetMinUidAsync(userId);
            //user bilgileriyle birlikte en küçük uid den sonra ki mailler adresten çağırıldı
            var emailsFromServer = await _emailReader.GetEmailsByPageAsync(user, minUid);
            //db ye kaydedildi
            var emailsToSave = _mapper.Map<IEnumerable<EmailMessage>>(emailsFromServer);
            await _emailMessageRepository.SaveRangeAsync(emailsToSave);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<ApiResult<IEnumerable<EmailMessagesDto>>> GetEmailsFromDbAsync(GetLatestEmailsQuery request, string userId)
        {
            IEnumerable<EmailMessage> emails;

            //arama koşuluna göre mailler db den çağırıldı
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                emails = await _emailMessageRepository.GetAllByPageBySearchTermAsync(
                    request.PageNumber, request.PageSize, userId, request.SearchTerm);
            }
            else if (!string.IsNullOrEmpty(request.SortBy))
            {
                //sıralama koşuluna göre mailler db den çağırıldı
                emails = await _emailMessageRepository.GetAllByPageSortedAsync(
                    request.PageNumber, request.PageSize, userId, request.SortBy, request.SortDesc);
            }
            else
            {
                //mailler koşulsuz db den çağırıldı
                emails = await _emailMessageRepository.GetAllByPageAsync(request.PageNumber, request.PageSize, userId);
            }

            var dto = _mapper.Map<IEnumerable<EmailMessagesDto>>(emails);
            return ApiResult<IEnumerable<EmailMessagesDto>>.Success(dto);
        }

        public async Task<ApiResult<IEnumerable<EmailMessagesDto>>> RefreshEmailsAsync(string userId, CancellationToken cancellationToken)
        {


            var user = await _userRepository.GetByIdAsync(userId);
            if (user is null)
                return ApiResult<IEnumerable<EmailMessagesDto>>.Fail("Kullanıcı bulunamadı");
            //db de ki son gelen mail referans noktası için alındı
            var lastEmail = await _emailMessageRepository.GetLatestAsync(user.Id);
            var lastUid = lastEmail?.Uid ?? 0;
            //uid ve user bilgilerine göre mail adresinden varsa yeni gelenler çağırıldı
            var newEmails = await _emailReader.GetEmailsAfterUidAsync(lastUid, user);
            if (!newEmails.Any())
                return ApiResult<IEnumerable<EmailMessagesDto>>.Success(Array.Empty<EmailMessagesDto>());
            //dönüştürüldü
            var mappedEntities = _mapper.Map<List<EmailMessage>>(newEmails);

            //db ye kaydedildi
            await _emailMessageRepository.SaveRangeAsync(mappedEntities);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            //dto ya çevrildi return edildi
            var dto = _mapper.Map<IEnumerable<EmailMessagesDto>>(mappedEntities);
            return ApiResult<IEnumerable<EmailMessagesDto>>.Success(dto);
        }
        public async Task<ApiResult<EmailMessagesDto>> GetEmailByIdAsync(string emailId)
        {
            //email id sine göre db den email çağırıldı
            var email = await _emailMessageRepository.GetByIdAsync(emailId);

            var dto = _mapper.Map<EmailMessagesDto>(email);
            return ApiResult<EmailMessagesDto>.Success(dto);
        }
    }
}
