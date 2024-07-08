using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Service.HandleResponse;
using Store.Service.UserService;
using Store.Service.UserService.Dtos;

namespace Store.API.Controllers
{
    
    public class AccountController : BaseController
    {
        private readonly IUserService userService;

        AccountController(IUserService userService) 
        {
            this.userService = userService;
        }
        [HttpPost]
        public async Task<ActionResult<UserDto>> Login(LoginDto input)
        {
            var user=await userService.Login(input);
            if(user is null)
                return Unauthorized(new CustomException(401));
            return Ok(user);
        }
        [HttpPost]
        public async Task<ActionResult<UserDto>> Register(RegisterDto input)
        {
            var user= await userService.Register(input);
            if (user is null)
                return BadRequest(new CustomException(401,"Email already Exist"));
            return Ok(user);
        }
    }
}
