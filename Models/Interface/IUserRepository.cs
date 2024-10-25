using Models.DTOs;
using Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Interface
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> CreateUserAsync(User user);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetByUsernameOrEmail(string username, string email);
        Task<bool> IsUsernameOrEmailTakenAsync(int currentUserId, string username, string email);


        Task<List<RoleDto>> GetUserRolesAsync(int userId);
        Task<bool> AssignRoleAsync(int userId, string roleName);


    }
}
