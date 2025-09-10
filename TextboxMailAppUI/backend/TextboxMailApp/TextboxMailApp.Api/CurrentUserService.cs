using System.Security.Claims;
using TextboxMailApp.Application.Contracts.Api;

namespace TextboxMailApp.Api
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
        //aktif kullanıcının id si tokenden alındı
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        public string? UserId =>
               _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
