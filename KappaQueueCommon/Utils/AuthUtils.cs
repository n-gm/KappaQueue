using KappaQueueCommon.Common.Signing;
using KappaQueueCommon.Models.Context;
using KappaQueueCommon.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace KappaQueueCommon.Utils
{
    public class AuthUtils
    {
        private const string signingSecurityKey = "MCgCIQDFCkfKCesUnXt07YAHPYT7XVDjZnRPNkOgUtWIXz3XPwIDAQAB"; //"0d5b3235a8b403c3dab9c3f4f65c07fcalskd234n1k41230";
        private const string encodingSecurityKey = "MIGqAgEAAiEAxQpHygnrFJ17dO2ABz2E+11Q42Z0TzZDoFLViF891z8CAwEAAQIhAKc4oVJq9nIJcNP8F7KNWEQKZdWywhRxi81C8o4AVQVxAhEA4gMUqqhHw0hvL18WzLUqNwIRAN8vHbM7eETKo3QTcrSHlzkCEFJGDtLk+WK0To0vD0yrslcCEElTlZJ5i34uhZ1xdlJR6iECEG9VMo07A0iYpKCjNn0A9lY=";//"k72gnxq3pkum9toiub48o8s8sdbjhme1tg0m3p4jfkzovsgdqzgv6t47ig3tr5d9";
        public const int TOKEN_TTL = 60;
        public static SigningSymmetricKey signingKey = new SigningSymmetricKey(signingSecurityKey);
        public static EncryptingSymmetricKey encryptionEncodingKey = new EncryptingSymmetricKey(encodingSecurityKey);

        /// <summary>
        /// Проверка аутентификации пользователя
        /// </summary>
        /// <param name="context">Контекст БД</param>
        /// <param name="username">Имя пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <returns></returns>
        public static User CheckAuth(QueueDBContext context, string username, string password)
        {
            User user = context.Users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            return user?.CheckPassword(password) ?? false ? user : null;

        }

        public static ClaimsIdentity CreateClaim(User user, string authenticationType = "Bearer")
        {
            //Формируем JWT токен
            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            //        new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()),
            //        new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                    new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                    new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString())
                };

            foreach (UserRole role in user.Roles)
            {
                foreach (UserRight right in role.UserRights)
                {
                    claims.Add(new Claim(ClaimTypes.Role, right.Code));
                }
            }

            return new ClaimsIdentity(claims, authenticationType);
        }

        public static string CreateEncryptedToken(User user)
        {
            IdentityOptions options = new IdentityOptions();
            

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateJwtSecurityToken(
                issuer: "KappaQueue",
                audience: "KappaQueueClient",
                subject: CreateClaim(user),
                notBefore: DateTime.Now,
                issuedAt: DateTime.Now,
                expires: DateTime.Now.AddMinutes(TOKEN_TTL),
                signingCredentials: new SigningCredentials(
                        signingKey.GetKey(),
                        signingKey.SigningAlgorithm),
                encryptingCredentials: new EncryptingCredentials(
                        encryptionEncodingKey.GetKey(),
                        encryptionEncodingKey.SigningAlgorithm,
                        encryptionEncodingKey.EncryptingAlgorithm)
            ) ;

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            
            return jwtToken;
        }
    }
}
