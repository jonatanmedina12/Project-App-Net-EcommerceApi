using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Account
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "El Username es requerida")]

        public string Username { get; set; }
        [Required(ErrorMessage = "La Email requerido")]

        public string Email { get; set; }
        [Required(ErrorMessage = "La Contraseña requetida")]

        public string Password { get; set; }
        [Required(ErrorMessage = "La Rol Requerido")]

        public List<string> Roles { get; set; }
    }
}
