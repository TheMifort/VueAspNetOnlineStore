using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace OnlineStore.Models
{
    public class AuthOptions
    {
        /// <summary>
        /// Издатель токена
        /// </summary>
        public string Issuer { get; }

        /// <summary>
        /// Потребитель токена
        /// </summary>
        public string Audience { get; }

        /// <summary>
        /// Ключ шифрования
        /// </summary>
        private const string Key = "sASdfsd:KJ;irunb64ASD342347657567FJhfl;kghdflghkklhklhklshkldfhklsdhklfsd9";  //TODO Move

        /// <summary>
        /// Время жизни токена
        /// </summary>
        public int Lifetime { get; }

        /// <summary>
        /// Время жизни refresh token
        /// </summary>
        public int RefreshTokenLifetime { get; }

        public AuthOptions(string issuer, string audience, int lifetime, int refreshTokenLifetime)
        {
            Issuer = issuer;
            Audience = audience;
            Lifetime = lifetime;
            RefreshTokenLifetime = refreshTokenLifetime;
        }

        /// <summary>
        /// Получить семетричный ключ шифрования
        /// </summary>
        /// <returns></returns>
        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
