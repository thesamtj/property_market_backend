using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using property_market_backend.Dtos;
using property_market_backend.Interfaces;
using property_market_backend.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace property_market_backend.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUnitOfWork uow;
        private readonly IConfiguration configuration;

        public AccountController(IUnitOfWork uow, IConfiguration configuration)
        {
            this.uow = uow;
            this.configuration = configuration;
        }

        //api/account/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginReqDto loginReq)
        {
            var user = await uow.UserRepository.Authenticate(loginReq.UserName, loginReq.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var loginRes = new LoginResDto();
            loginRes.UserName = user.Username;
            loginRes.Token = CreateJWT(user);

            return Ok(loginRes);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(LoginReqDto loginReq)
        {
            //ApiError apiError = new ApiError();

            //if (loginReq.UserName.IsEmpty() || loginReq.Password.IsEmpty())
            //{
            //    apiError.ErrorCode = BadRequest().StatusCode;
            //    apiError.ErrorMessage = "User name or password can not be blank";
            //    return BadRequest(apiError);
            //}

            if (await uow.UserRepository.UserAlreadyExists(loginReq.UserName))
            {
                //apiError.ErrorCode = BadRequest().StatusCode;
                //apiError.ErrorMessage = "User already exists, please try different user name";
                return BadRequest("User already exists, please try different user name");
            }

            uow.UserRepository.Register(loginReq.UserName, loginReq.Password);
            await uow.SaveAsync();
            return StatusCode(201);
        }


        private string CreateJWT(User user)
        {
            var secretKey = configuration.GetSection("AppSettings:Key").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(secretKey));

            var claims = new Claim[] {
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
            };

            var signingCredentials = new SigningCredentials(
                    key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(10),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


    }
}
