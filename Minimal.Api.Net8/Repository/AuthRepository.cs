using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Minimal.Api.Net8.Data;
using Minimal.Api.Net8.Models;
using Minimal.Api.Net8.Models.DTO;
using Minimal.Api.Net8.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Minimal.Api.Net8.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private string secretKey;
        public AuthRepository(ApplicationDbContext db, IMapper mapper, IConfiguration configuration)
        {
            _db = db;
            _mapper = mapper;
            _configuration = configuration;
            secretKey = _configuration.GetValue<string>("ApiSettings:Secret");
        }
        public bool IsUniqueUser(string userName)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserName == userName);
            return user == null;
        }

        public async Task<SignInResponseDTO> SignIn(SignInRequestDTO request)
        {
            SignInResponseDTO response = new();

            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName.Equals(request.UserName) && u.Password.Equals(request.Password));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(ClaimTypes.Role,user.Role),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            response.User = _mapper.Map<UserDTO>(user);
            response.Token = new JwtSecurityTokenHandler().WriteToken(token);
            return response;
        }

        public async Task<UserDTO> SignUp(SignUpRequestDTO request)
        {
            User user = new()
            {
                UserName = request.UserName,
                Password = request.Password,
                Name = request.Name,
                Role = "Admin"
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            user.Password = request.Password;
            return _mapper.Map<UserDTO>(user);
        }

    }
}
