using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using api.Dtos.AccountDtos;
using api.Interface;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : Controller
    {
        
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            this._userManager=userManager;
            this._tokenService = tokenService;
            this._signInManager = signInManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try{
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var appUser= new AppUser
                {
                    UserName= registerDto.UserName,
                    Email= registerDto.Email,
                    FullName= registerDto.FullName
                } ;
                var createdUser= await _userManager.CreateAsync(appUser, registerDto.Password);
                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(
                            new NewUserDto
                            {
                                UserName= appUser.UserName,
                                Email= appUser.Email,
                                Token= _tokenService.CreateToken(appUser)
                            }
                        );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch(Exception e)
            {
                return StatusCode(500, e);
            } 
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> login(LoginDto loginDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var appUser = await _userManager.Users.FirstOrDefaultAsync(u=> u.UserName==loginDto.UserName);
            if (appUser==null) return Unauthorized("Invalid UserName and/or Password");
            var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized("Invalid Username and/or Password ") ;
            return Ok(
                new NewUserDto
                {
                    UserName= appUser.UserName,
                    Email= appUser.Email,
                    Token= _tokenService.CreateToken(appUser)
                }   
            );

        }

    }
}