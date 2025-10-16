using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBook.Application.Cryptography
{
    public class PasswordEncripter
    {

        public PasswordEncripter()
        {
            
        }

        public string Encrypt(string password)
        {

            var bytes = Encoding.UTF8.GetBytes(password);
            var hashBytes = System.Security.Cryptography.SHA256.HashData(bytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
