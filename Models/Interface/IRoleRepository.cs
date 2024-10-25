using Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Interface
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetRolesByNames(List<string> roleNames);

    }
}
