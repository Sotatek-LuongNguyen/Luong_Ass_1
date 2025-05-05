using OrderApi.Dto;
using OrderApi.Model;

namespace OrderApi.Service.ServiceRole
{
    public interface IRoleService
    {
        Task<string> CreateRoleAsync(RoleDto roledto);
        Task<List<RoleDto>> GetAllRoleAsync();
        Task<List<EmployeeDto>> GetEmployeesByRoleNameAsync(string roleName);
        Task<List<UserDto>> GetUsersByRoleNameAsync(string roleName);
        Task<bool> DeleteRoleAsync(int roleId);
    }
}
