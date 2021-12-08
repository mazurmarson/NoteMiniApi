using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NoteMiniApi.Context;
using NoteMiniApi.Dtos;
using NoteMiniApi.Models;

namespace NoteMiniApi.Services
{
    public interface IUserService
    {
        public User Register(RegisterUserDto registerUserDto);
        public string GenerateJwt(LoginDto loginDto);
    //    public void Login(LoginDto loginDto);
    }
     public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        private readonly AuthenticationSettings _authenticationSettings;
        public UserService(AppDbContext context, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _context = context;
            _authenticationSettings = authenticationSettings;
            _passwordHasher = passwordHasher;
        }

        public User Register(RegisterUserDto registerUserDto)
        {
            var user = new User()
            {
                Name = registerUserDto.Login
            };
            var hashedPassword = _passwordHasher.HashPassword(user, registerUserDto.Password);
            user.PasswordHash = hashedPassword;

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;

        }

        

            public string GenerateJwt(LoginDto dto)
        {
            var user =  _context.Users.FirstOrDefault(x => x.Name == dto.Login);

            if(user is null)
            {
                throw new Exception("Inavalid username or password");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if(result == PasswordVerificationResult.Failed)
            {
                throw new Exception("Inavalid username or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Name}"),
                //Tu bedzie można dodać role
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //     if(user is null)
        //     {
        //         throw new BadReqyuest
        //     }
        // }
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer, _authenticationSettings.JwtIssuer, claims, expires: expires, signingCredentials:cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }


    }
}