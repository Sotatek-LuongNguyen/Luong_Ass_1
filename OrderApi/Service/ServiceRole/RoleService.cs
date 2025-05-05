using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using OrderApi.Dto;
using OrderApi.Model;

namespace OrderApi.Service.ServiceRole
{
    public class RoleService : IRoleService
    {
        private readonly OrderDbContext _context;

        public RoleService(OrderDbContext context)
        {
            _context = context;
        }
        public async Task<string> CreateRoleAsync(RoleDto roleDto)
        {
            var role = new Role
            {
                IdRole = roleDto.IdRole,
                RoleName = roleDto.RoleName,
                Status = roleDto.Status
            };

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return "Role created successfully";
        }
        public async Task<List<RoleDto>> GetAllRoleAsync()
        {
            return await _context.Roles
                .Select(r => new RoleDto
                {
                    IdRole = r.IdRole,
                    RoleName = r.RoleName,
                    Status = r.Status
                })
                .ToListAsync();
        }
        public async Task<bool> DeleteRoleAsync(int roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null)
            {
                return false;
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<List<EmployeeDto>> GetEmployeesByRoleNameAsync(string roleName)
        {
            return await _context.Employees
                .Where(e => e.Role.RoleName == roleName)
                .Select(e => new EmployeeDto
                {
                    EmployeeName = e.EmployeeName,
                    Email = e.Email,
                    Phone = e.Phone,
                    Address = e.Address,
                    Status = e.Status
                })
                .ToListAsync();
        }

        public async Task<List<UserDto>> GetUsersByRoleNameAsync(string roleName)
        {
            return await _context.Users
                .Where(u => u.Role.RoleName == roleName)
                .Select(u => new UserDto
                {
                    Username = u.Username,
                    Email = u.Email,
                    Phone = u.Phone
                })
                .ToListAsync();
        }
    }
}
