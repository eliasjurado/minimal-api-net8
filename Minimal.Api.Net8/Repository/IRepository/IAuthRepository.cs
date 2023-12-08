using Minimal.Api.Net8.Models.DTO;

namespace Minimal.Api.Net8.Repository.IRepository
{
    public interface IAuthRepository
    {
        Task<bool> IsUniqueUser(string userName);
        Task<SignInResponseDTO> SignIn(SignInRequestDTO request);
        Task<UserDTO> SignUp(SignUpRequestDTO request);
    }
}
