using AutoMapper;
using TextboxMailApp.Application.Common.Responses;
using TextboxMailApp.Application.Contracts.Api;
using TextboxMailApp.Application.Contracts.Application;
using TextboxMailApp.Application.Contracts.Persistence;
using TextboxMailApp.Application.Features.Users.Commands;
using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Application.Features.Users
{
  public class UserCommandService(IUserRepository userRepository, IUnitOfWork unitOfWork,
    IMapper mapper, IPasswordHasher passwordHasher, ICurrentUserService currentUserService) : IUserCommandService
  {
    private readonly ICurrentUserService _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
    private readonly IPasswordHasher _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    public async Task<ApiResult<UsersDto>> CreateUserAsync(CreateUserCommand request, CancellationToken cancellationToken)
    {
      // Kullanıcı adı veya e-posta zaten var mı kontrolü
      var existingUser = await _userRepository.GetByUserNameOrEmailAsync(request.UserName, request.EmailAddress);
      if (existingUser != null)
        return ApiResult<UsersDto>.Fail("Kullanıcı adı veya email kullanılmakta.");

      // Yeni kullanıcı nesnesi oluşturma
      var user = new User
      {
        UserName = request.UserName,
        PasswordHash = _passwordHasher.Hash(request.PasswordHash),
        EmailAddress = request.EmailAddress,
        EmailPassword = request.EmailPassword,
        ServerName = request.ServerName,
        Port = request.Port,
        CreatedAt = DateTime.UtcNow
      };

      await _userRepository.CreateAsync(user);
      await _unitOfWork.SaveChangesAsync();

      var dto = _mapper.Map<UsersDto>(user);
      return ApiResult<UsersDto>.Success(dto);
    }

    public async Task<ApiResult<UsersDto>> UpdateUserAsync(UpdateUserCommand request, CancellationToken cancellationToken)
    {
      var userId = _currentUserService.UserId;
      var user = await _userRepository.GetByIdAsync(userId);

      if (user == null)
        return ApiResult<UsersDto>.Fail("User not found.");

      user.UserName = request.UserName;
      user.PasswordHash = _passwordHasher.Hash(request.PasswordHash);
      user.EmailAddress = request.EmailAddress;
      user.EmailPassword = request.EmailPassword;
      user.ServerName = request.ServerName;
      user.Port = request.Port;
      user.UpdatedAt = DateTime.UtcNow;

      _userRepository.Update(user);
      await _unitOfWork.SaveChangesAsync();

      return ApiResult<UsersDto>.Success(_mapper.Map<UsersDto>(user));
    }

    public async Task<ApiResult<string>> DeleteUserAsync(DeleteUserCommand request, CancellationToken cancellationToken)
    {
      var userId = _currentUserService.UserId!;
      var user = await _userRepository.GetByIdAsync(userId);

      if (user == null)
        return ApiResult<string>.Fail("Kullanıcı bulunamadı.");

      _userRepository.Delete(user);
      await _unitOfWork.SaveChangesAsync(cancellationToken);

      return ApiResult<string>.Success("Kullanıcı başarıyla silindi");
    }
  }
}
