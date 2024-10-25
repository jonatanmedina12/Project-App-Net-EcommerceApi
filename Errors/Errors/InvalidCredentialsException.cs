using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errors.Errors
{
    public class InvalidCredentialsException :AuthenticationException
    {
        public InvalidCredentialsException()
        : base("El usuario o la contraseña son incorrectos")
        {
        }
    }
}
