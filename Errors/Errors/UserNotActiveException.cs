using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errors.Errors
{
    public class UserNotActiveException : AuthenticationException
    {
        public UserNotActiveException()
       : base("La cuenta de usuario no está activa")
        {
        }
    }
}
