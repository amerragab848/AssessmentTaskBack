using MainModuleContext.Models;
using MainModuleDTO.DTOModels;
using MainModuleInterFace.IDataServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace MainModuleDataServices.Repository
{
    public class AccountRepository : IApplicationUserManager

    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<AuthenticatedResponse> LoginAsync(LoginModel model)
        {
            var res = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (!res.Succeeded)
            {
                return null;
            }

            var user = await GetByUserNameAsync(model.Email);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, model.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                  issuer: _configuration["Jwt:Issuer"],
                  audience: _configuration["Jwt:Audience"],
                  claims: claims,
                  expires: DateTime.Now.AddDays(2),
                  signingCredentials: signinCredentials
              );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return new AuthenticatedResponse { Token = tokenString, FirstName = user.FirstName, LastName = user.LastName, UserName = user.Email };

        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<ApplicationUser> GetByUserNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }
        public async Task<IdentityResult> SignUpAsync(SignUpModel model)
        {
            try
            {

                var user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                };
                var res = await _userManager.CreateAsync(user, model.Password);
                return res;
            }
            catch (Exception EX)
            {
                throw new Exception(EX.Message);
            }
        }
    }
}
