using Bogus;
using RecipeBook.Communication.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTestUtilities.Requests
{
    public class RequestLoginJsonBuilder
    {
        public static RequestLoginJson Build()
        {
            return new Faker<RequestLoginJson>()
                .RuleFor(r => r.Email, f => f.Internet.Email())
                .RuleFor(r => r.Password, f => f.Internet.Password());
        }
    }
}
