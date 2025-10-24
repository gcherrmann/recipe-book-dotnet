using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBook.Exceptions.ExceptionsBase
{
    public class ErrorOnValidationException : RecipeBookException
    {
        public IList<string> Errors { get; set; } = new List<string>();

        public ErrorOnValidationException(IList<string> errors): base(string.Empty)
        {
            Errors = errors;
        }
        
    }
}
