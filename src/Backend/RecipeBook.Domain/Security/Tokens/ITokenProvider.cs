﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBook.Domain.Security.Tokens
{
    public interface ITokenProvider
    {
        public string Value();
    }
}
