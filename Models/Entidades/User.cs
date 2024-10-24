using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entidades
{
    public class User : BaseEntity
    {
        public User()
        {
            UserRoles = new HashSet<UserRole>();
            Products = new HashSet<Product>();
        }

        public string Username { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool IsActive { get; set; }
        public bool IsLoginActive { get; set; }
        public string ProfilePhoto {  get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<Product> Products { get; set; }

    }
}
