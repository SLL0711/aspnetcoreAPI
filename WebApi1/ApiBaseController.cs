using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace WebApi1
{
    public class ApiBaseController : ControllerBase
    {
        private readonly static string SECRETKEY = "shenlilinAPPSecret";//至少16为

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public string generateToken(string userName, string email)
        {
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRETKEY));//对称加密Key

            //一组Claims
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name,userName),
                new Claim(JwtRegisteredClaimNames.Email,email)
            };

            var Token = new JwtSecurityToken(
                issuer: "WEBAPI1",
                audience: "Client1",
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: new SigningCredentials(Key, SecurityAlgorithms.HmacSha256)
            //signingCredentials: new SigningCredentials(new RsaSecurityKey(new RSACryptoServiceProvider(2048)), SecurityAlgorithms.RsaSha256Signature)
            );

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(Token);
            //string jwtToken = WriteToken(Token);

            return jwtToken;
        }


        public string WriteToken(SecurityToken token)
        {
            if (token == null)
                throw LogHelper.LogArgumentNullException(nameof(token));
            JwtSecurityToken jwtSecurityToken = token as JwtSecurityToken;
            if (jwtSecurityToken == null)
                throw LogHelper.LogExceptionMessage((Exception)new ArgumentException(LogHelper.FormatInvariant("IDX12706: '{0}' can only write SecurityTokens of type: '{1}', 'token' type is: '{2}'.", (object)this.GetType(), (object)typeof(JwtSecurityToken), (object)token.GetType()), nameof(token)));
            string encodedPayload = jwtSecurityToken.EncodedPayload;
            string rawSignature = string.Empty;
            string empty = string.Empty;

            JwtHeader header = jwtSecurityToken.EncryptingCredentials == null ? jwtSecurityToken.Header : new JwtHeader(jwtSecurityToken.SigningCredentials);
            string rawHeader = header.Base64UrlEncode();

            //var a = jwtSecurityToken.SigningCredentials;
            //var b = jwtSecurityToken.EncryptingCredentials;

            var arg1 = rawHeader + "." + encodedPayload;
            var arg2 = jwtSecurityToken.SigningCredentials;

            //if (jwtSecurityToken.SigningCredentials != null)
            //    rawSignature = JwtSecurityTokenHandler.CreateEncodedSignature(, );
            //if (jwtSecurityToken.EncryptingCredentials != null)
            //    return this.EncryptToken(new JwtSecurityToken(header, jwtSecurityToken.Payload, rawHeader, encodedPayload, rawSignature), jwtSecurityToken.EncryptingCredentials).RawData;
            return rawHeader + "." + encodedPayload + "." + rawSignature;
        }
    }
}
