using Microsoft.EntityFrameworkCore;
using Models.Entidades;
using Models.Extensions;
using Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public virtual async Task<List<Role>> GetRolesByNames(List<string> roleNames)
        {

            var normalizedRoleNames = roleNames.Select(r => r.NormalizeText()).ToList();

            return await _context.Roles
                .Where(r => normalizedRoleNames.Contains(r.Name.ToLower()))
                .ToListAsync();
        }
        public async Task<bool> AreAllRolesExist(List<string> roleNames)
        {
            var normalizedRoleNames = roleNames.Select(r => r.NormalizeText()).ToList();
            var existingRolesCount = await _context.Roles
                .Where(r => normalizedRoleNames.Contains(r.Name.ToLower()))
                .CountAsync();

            return existingRolesCount == roleNames.Count;
        }
    }
}
