using Microsoft.AspNetCore.Mvc;
using OrderApi.Dto;
using OrderApi.Service.ServiceRole;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleDto roleDto)
        {
            var result = await _roleService.CreateRoleAsync(roleDto);
            return Ok(new { message = result });
        }

        [HttpGet]
        public async Task<ActionResult<List<RoleDto>>> GetAllRoles()
        {
            var roles = await _roleService.GetAllRoleAsync();
            return Ok(roles);
        }
        [HttpGet("employees/{roleName}")]
        public async Task<ActionResult<List<EmployeeDto>>> GetEmployeesByRoleName(string roleName)
        {
            var employees = await _roleService.GetEmployeesByRoleNameAsync(roleName);
            if (employees == null || employees.Count == 0)
                return NotFound(new { message = "Không tìm thấy nhân viên nào có Role này" });

            return Ok(employees);
        }
        [HttpGet("users/{roleName}")]
        public async Task<ActionResult<List<UserDto>>> GetUsersByRoleName(string roleName)
        {
            var users = await _roleService.GetUsersByRoleNameAsync(roleName);
            if (users == null || users.Count == 0)
                return NotFound(new { message = "Không tìm thấy người dùng nào có Role này" });

            return Ok(users);
        }
        [HttpDelete("{idRole}")]
        public async Task<IActionResult> DeleteUser(int idRole)
        {
            var result = await _roleService.DeleteRoleAsync(idRole);
            if (!result) return NotFound("Không tìm thấy người dùng để xóa!");
            return Ok("Xóa thành công!");
        }
    }
}
