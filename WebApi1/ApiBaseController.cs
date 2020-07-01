using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace WebApi1
{
    [Authorize]
    public class ApiBaseController : ControllerBase
    {
        private readonly static string SECRETKEY = "shenlilinAPPSecret";//至少16为

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public string generateToken(string userName)
        {
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRETKEY));//对称加密Key

            //一组Claims
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name,userName),
                //new Claim(JwtRegisteredClaimNames.Email,email)
            };

            var Token = new JwtSecurityToken(
                issuer: "WEBAPI1",
                audience: "Client1",
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: new SigningCredentials(Key, SecurityAlgorithms.HmacSha256)
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(Token);

            return jwtToken;
        }
    }
}
