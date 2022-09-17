using MainModuleContext.Models;
using MainModuleDTO.DTOModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainModuleInterFace.IDataServices
{
    public interface IApplicationUserManager
    {
        Task<AuthenticatedResponse> LoginAsync(LoginModel model);
        Task<IdentityResult> SignUpAsync(SignUpModel model);
        Task LogoutAsync();
        Task<ApplicationUser> GetByUserNameAsync(string userName);
    }
}
