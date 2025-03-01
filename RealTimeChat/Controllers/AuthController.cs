using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealTimeChat.Models.Dtos;
using RealTimeChat.Services;

namespace RealTimeChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserServices _userServices;
        private readonly TokenService _tokenService;

        public AuthController(UserServices userServices, TokenService tokenService)
        {
            _userServices = userServices;
            _tokenService = tokenService;
        }






        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationDto registrationDto)
        {
            bool isRegistered = await _userServices.AddUser(registrationDto);

            if (!isRegistered)
            {
                return BadRequest(new { message = "Username already exists" });
            }

            return Ok(new { message = "Success" });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userServices.UserValid(loginDto);
            if (user != null)
            {
                var token = _tokenService.GenToken(user);
                return Ok(new { token });
            }

            else
                return Unauthorized(new { message = "Incorrect Username or Password" });
        }
    }
}
