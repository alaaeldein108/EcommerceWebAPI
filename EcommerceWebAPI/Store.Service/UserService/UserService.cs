using Microsoft.AspNetCore.Identity;
using Store.Data.Entities.IdentityEntities;
using Store.Service.Services.TokenServices;
using Store.Service.UserService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.UserService
{
    public class UserService : IUserService
    {
        private readonly SignInManager<AppUser> signInManger;
        private readonly ITokenService tokenService;
        private readonly UserManager<AppUser> UserManager;

        public UserService(UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManger,ITokenService tokenService)
        {
            this.UserManager = userManager;
            this.signInManger = signInManger;
            this.tokenService = tokenService;
        }

        public async Task<UserDto> Login(LoginDto input)
        {
            var user= await UserManager.FindByEmailAsync(input.Email);
            if (user is null)
                return null;
            var result=await signInManger.CheckPasswordSignInAsync(user, input.Password,false);
            if (!result.Succeeded)
                throw new Exception("Login Failed");
            return new UserDto
            {
                Email = input.Email,
                DisplayName = user.DisplayName,
                Token = tokenService.GenerateToken(user),
            };
        }

        public async Task<UserDto> Register(RegisterDto input)
        {
            var user = await UserManager.FindByEmailAsync(input.Email);
            if (user is not null)
                return null;
            var appUser = new AppUser
            {
                DisplayName= input.DisplayName,
                Email = input.Email,
                UserName= input.DisplayName,
            };
            var result=await UserManager.CreateAsync(appUser,input.Password);

            if (!result.Succeeded)
                throw new Exception(result.Errors.Select(x=>x.Description).FirstOrDefault());
            return new UserDto
            {
                Email = input.Email,
                DisplayName = input.DisplayName,
                Token = tokenService.GenerateToken(appUser),
            };
        }
    }
}
