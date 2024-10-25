using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "El correo es requerido")]

        public string Email { get; set; }
        [Required(ErrorMessage = "La contraseña  es requerida")]

        public string Password { get; set; }
    }
}
