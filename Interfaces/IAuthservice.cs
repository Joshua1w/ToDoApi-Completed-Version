using TodoApi.Dtos;

namespace TodoApi.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(UserRegisterDto dto);
        Task<string> LoginAsync(UserLoginDto dto);
    }
}
