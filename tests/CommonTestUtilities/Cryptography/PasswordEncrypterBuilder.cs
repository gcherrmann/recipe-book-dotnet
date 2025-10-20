using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeBook.Application.Cryptography;


namespace CommonTestUtilities.Cryptography
{
    public class PasswordEncrypterBuilder
    {
        public static PasswordEncripter Build()
        {
            return new PasswordEncripter();
        }
    }
}
