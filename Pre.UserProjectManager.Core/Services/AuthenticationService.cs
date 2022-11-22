using Microsoft.IdentityModel.Tokens;
using Pre.UserProjectManager.Core.Constants;
using Pre.UserProjectManager.Core.DTO;
using Pre.UserProjectManager.Core.Entity;
using Pre.UserProjectManager.Core.Interfaces.Core;
using Pre.UserProjectManager.Core.Interfaces.Infrastructure;
using Pre.UserProjectManager.Core.Models.APIResponse;
using Pre.UserProjectManager.Core.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pre.UserProjectManager.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IProjectManagerCommand _projectManagerCommands;
        private readonly IProjectManagerQuery _projectManagerQueries;
        //private readonly ILoggerService loggerService;

        public AuthenticationService(IProjectManagerCommand projectManagerCommands, IProjectManagerQuery projectManagerQueries)
        {
            _projectManagerCommands = projectManagerCommands;
            _projectManagerQueries = projectManagerQueries;
            //loggerService = _loggerService;
        }
        public async Task<GenericResponse<LoginResponse>> AuthenticateUserAsync(LoginRequest loginRequest)
        {
            try
            {
                var user = await _projectManagerQueries.GetUserByUserNameAsync(loginRequest.UserName);
                if (user == null)
                    return GenericResponse<LoginResponse>.UnAuthorized(MESSAGES.INVALID_LOGIN);

                var passwordHash = PasswordHelper.HashPassword(loginRequest.Password, user.PasswordSalt);
                if (passwordHash != user.Password)
                    return GenericResponse<LoginResponse>.UnAuthorized(MESSAGES.INVALID_LOGIN);

                var token = GenerateToken(loginRequest.UserName, user.Id);

                LoginResponse loginResponse = new()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Username = user.UserName,
                    UserId = user.Id,
                    Token = token
                };

                user.LastLoginDate = DateTime.Now;
                await _projectManagerCommands.UpdateExistingUserAsync(user);

                return GenericResponse<LoginResponse>.Success(MESSAGES.LOGIN_SUCCESS, loginResponse);
            }
            catch (Exception ex)
            {
                //loggerService.LogError(ex, ex.Message);
                return GenericResponse<LoginResponse>.Error(MESSAGES.GENERAL_FAILURE);
            }
        }
        public async Task<GenericResponse<CreatedUser>> AddNewUserAsync(NewUserRequest request)
        {
            try
            {
                var user = await _projectManagerQueries.GetUserByUserNameAsync(request.UserName);

                if (user != null)
                    return GenericResponse<CreatedUser>.Fail(MESSAGES.INVALID_USERNAME);

                string salt = PasswordHelper.GenerateSalt();
                var now = DateTime.Now;
                User newUser = new()
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    UserName = request.UserName,
                    PasswordSalt = salt,
                    Password = PasswordHelper.HashPassword(request.Password, salt),
                    DateCreated = now.Date,
                    TimeCreated = now,
                    LastUpdatedTime = now,
                };
                await _projectManagerCommands.CreateNewUserAsync(newUser);

                return GenericResponse<CreatedUser>.Created(MESSAGES.USER_ADDED, new()
                {
                    UserName = request.UserName,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Id = newUser.Id,
                    TimeCreated = newUser.TimeCreated
                });
            }
            catch (Exception ex)
            {
                //loggerService.LogError(ex, ex.Message);
                return GenericResponse<CreatedUser>.Error(MESSAGES.GENERAL_FAILURE);
            }
        }

        private string GenerateToken(string userName, long userId)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = AppSettings.Secret;
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(AppSettings.UserIdClaimType, userId.ToString())

                }),
                Expires = DateTime.UtcNow.AddMinutes(3000),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
