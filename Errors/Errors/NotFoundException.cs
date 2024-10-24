using API.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errors.Errors
{
    public class NotFoundException : CustomException
    {
        public NotFoundException(string message)
              : base(message, "NOT_FOUND", 404)

        {
        }
    }
}
