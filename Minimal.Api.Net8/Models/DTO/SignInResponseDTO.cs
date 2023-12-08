namespace Minimal.Api.Net8.Models.DTO
{
    public class SignInResponseDTO
    {
        public UserDTO User { get; set; }
        public string Token { get; set; }
    }
}
