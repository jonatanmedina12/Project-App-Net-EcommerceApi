using API.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errors.Errors
{
    public class AuthenticationException : CustomException
    {
        public AuthenticationException(string message)
       : base(message, "AUTHENTICATION_ERROR", 401)
        {
        }
    }
}
