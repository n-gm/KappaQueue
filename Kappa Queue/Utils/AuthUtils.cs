using KappaQueue.Common.Signing;
using KappaQueue.Models.Context;
using KappaQueue.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace KappaQueue.Utils
{
    public class AuthUtils
    {
        private const string signingSecurityKey = "0d5b3235a8b403c3dab9c3f4f65c07fcalskd234n1k41230";
        public static SigningSymmetricKey signingKey = new SigningSymmetricKey(signingSecurityKey);

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

        public static string CreateToken(User user)
        {
            IdentityOptions options = new IdentityOptions();
            //Формируем JWT токен
            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                    new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                    new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                    new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString())
                };

            foreach (UserRole role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Code));
            }

            var token = new JwtSecurityToken(
                issuer: "KappaQueue",
                audience: "KappaQueueClient",
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: new SigningCredentials(
                        signingKey.GetKey(),
                        signingKey.SigningAlgorithm)
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }
    }
}
