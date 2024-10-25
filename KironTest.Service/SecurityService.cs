using Dapper;
using KironTest.DataModel;
using KironTest.Repository.Contracts;
using KironTest.Service.Contracts;
using KironTest.Shared.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace KironTest.Service
{
    public class SecurityService : ISecurityService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private string _jwtSecret;

        public SecurityService(IUserRepository _userRepository, IConfiguration _configuration)
        {
            this._configuration = _configuration;
            this._userRepository = _userRepository;
            _jwtSecret = _configuration.GetSection("Jwt:Key").Value;
        }

        public async Task<string> Authorize(User user)
        {
            try
            {                
                var _user = await _userRepository.GetUser(user.Username);

                if (_user == null || !BCrypt.Net.BCrypt.Verify(user.Password, _user.Password))
                    throw new UnauthorizedAccessException();

                var issuer = _configuration["Jwt:Issuer"];
                var audience = _configuration["Jwt:Audience"];
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("Id", Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                        new Claim(JwtRegisteredClaimNames.Email, user.Username),
                        new Claim(JwtRegisteredClaimNames.Jti,
                        Guid.NewGuid().ToString())
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(60),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);
                var stringToken = tokenHandler.WriteToken(token);
                return stringToken;

            }
            catch (UnauthorizedAccessException ex)
            {
                throw new ServiceException("Invalid credentials.", ex);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Could not authorize user.", ex);
            }            
        }

        public async Task<int> Register(User user)
        {
            try
            {                
                var _existingUser = await _userRepository.GetUser(user.Username);
                if (_existingUser != null)
                    throw new UserExistException();

                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);                

                return await _userRepository.AddUser(user);                
            }
            catch (UserExistException ex)
            {
                throw new ServiceException("The user already exists.", ex);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Could not register user.", ex);
            }            
        }
    }
}
