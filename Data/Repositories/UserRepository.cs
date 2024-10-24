using Data.Procedure;
using Data.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.DTOs.Account;
using Models.Entidades;
using Models.Extensions;
using Models.Interface;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json;

namespace Data.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ApplicationDbContext context)
        : base(context)
        {
        }

        public Task<bool> AssignRoleAsync(int userId, string roleName)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<User> CreateUserAsync(User user)
        {
            user.Username = user.Username.NormalizeText();
            user.Email = user.Email.NormalizeText();

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

                if (user == null)
                {
                    throw new Exception("email not found.");
                }

                return user;


            }
            catch (Exception ex)
            {

                throw new Exception($"Error al recuperar la usuario por correo electrónico: {ex.Message}", ex);
            }
        }

        public virtual async Task<User> GetByUsernameOrEmail(string username, string email)
        {
            var normalizedUsername = username.NormalizeText();
            var normalizedEmail = email.NormalizeText();

            return await _context.Users
                        .Where(u => u.Username.ToLower() == username.ToLower() ||
                                   u.Email.ToLower() == email.ToLower())
                        .FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            try
            {
                var usernameValidation = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
                if (usernameValidation == null)
                {
                    throw new Exception("User not found.");


                }
                return usernameValidation;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public virtual async Task<List<RoleDto>> GetUserRolesAsync(int userId)
        {
            try
            {
                // Buscamos al usuario junto con sus roles
                var user = await _context.Users
                                         .Include(u => u.UserRoles)
                                         .ThenInclude(ur => ur.Role)
                                         .FirstOrDefaultAsync(u => u.Id == userId);

                // Verificamos si el usuario existe
                if (user == null)
                {
                    throw new Exception("Usuario no encontrado.");
                }

                // Mapeamos los roles a RoleDto
                List<RoleDto> roles = user.UserRoles
                                          .Select(ur => new RoleDto
                                          {
                                              Id = ur.Role.Id,
                                              Name = ur.Role.Name
                                          }).ToList();

                return roles;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving roles for user with ID: {userId}: {ex.Message}", ex);
            }
        }

        public async Task<bool> IsUsernameOrEmailTakenAsync(int currentUserId, string username, string email)
        {
            return await _context.Users
                        .AnyAsync(u =>
                            u.Id != currentUserId && // Excluir el usuario actual
                            (u.Username.ToLower() == username.ToLower() ||
                             u.Email.ToLower() == email.ToLower()));
        }
    }
}
