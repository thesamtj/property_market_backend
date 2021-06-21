using Microsoft.AspNetCore.Mvc;
using property_market_backend.Dtos;
using property_market_backend.Interfaces;
using property_market_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace property_market_backend.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUnitOfWork uow;

        public AccountController(IUnitOfWork uow)
        {
            this.uow = uow;
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
            loginRes.Token = "Token to be generated";

            return Ok(loginRes);
        }

        //private string CreateJWT(User user)
        //{
        //    //var secretKey = configuration.GetSection("AppSettings:Key").Value;
        //    var key = new SymmetricSecurityKey(Encoding.UTF8
        //        .GetBytes("hmmm...this is a top secret"));

        //    var claims = new Claim[] {
        //        new Claim(ClaimTypes.Name,user.Username),
        //        new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
        //    };

        //    var signingCredentials = new SigningCredentials(
        //            key, SecurityAlgorithms.HmacSha256Signature);

        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(claims),
        //        Expires = DateTime.UtcNow.AddMinutes(1),
        //        SigningCredentials = signingCredentials
        //    };

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    return tokenHandler.WriteToken(token);
        //}


    }
}
