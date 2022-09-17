using MainModuleDTO.DTOModels;
using MainModuleInterFace.IDataServices;
using Microsoft.AspNetCore.Mvc;

namespace AssessmentTask.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IApplicationUserManager _applicationUserManager;

        public AccountController(IApplicationUserManager applicationUserManager)
        {
            _applicationUserManager = applicationUserManager;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {

            return Ok(await _applicationUserManager.LoginAsync(model));
        }

        [HttpPost]
        [Route("Reister")]
        public async Task<IActionResult> Reister(SignUpModel model)
        {
            return Ok(await _applicationUserManager.SignUpAsync(model));


        }
        [HttpGet]
        [Route("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            await _applicationUserManager.LogoutAsync();
            return Ok();

        }
    }
}