using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCostsProject
{
    public class AuthOptions
    {
        public const string ISSUER = "CCTeam"; // издатель токена
        public const string AUDIENCE = "CCUsers"; // потребитель токена
        const string KEY = "mysupersecret_secretkey!1488";   // ключ для шифрации
        public const int LIFETIME = 10; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
