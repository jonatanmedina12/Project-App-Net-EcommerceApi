using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entidades
{
    public class Role : BaseEntity
    {
        public Role()
        {
            UserRoles = new HashSet<UserRole>();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }

    }
}
