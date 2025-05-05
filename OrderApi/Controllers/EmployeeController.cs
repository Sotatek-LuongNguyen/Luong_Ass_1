using Microsoft.AspNetCore.Mvc;
using OrderApi.Dto;
using OrderApi.Service.ServiceEmployee;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderApi.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeDto employeeDto)
        {
            var result = await _employeeService.CreateEmployeeAsync(employeeDto);
            if (result == null) return BadRequest("Role ID không hợp lệ!");
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<EmployeeDto>>> GetAllEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpGet("{idEmployee}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeById(int idEmployee)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(idEmployee);
            if (employee == null) return NotFound("Không tìm thấy nhân viên!");
            return Ok(employee);
        }

        [HttpGet("role/{roleName}")]
        public async Task<ActionResult<List<EmployeeDto>>> GetEmployeesByRoleName(string roleName)
        {
            var employees = await _employeeService.GetEmployeesByRoleNameAsync(roleName);
            if (employees == null || employees.Count == 0)
                return NotFound($"Không tìm thấy nhân viên nào có RoleName: {roleName}");
            return Ok(employees);
        }

        [HttpPut("{idEmployee}")]
        public async Task<IActionResult> UpdateEmployee(int idEmployee, [FromBody] EmployeeDto employeeDto)
        {
            var result = await _employeeService.UpdateEmployeeAsync(idEmployee, employeeDto);
            if (!result) return BadRequest("Cập nhật thất bại! Kiểm tra IdEmployee hoặc IdRole.");
            return Ok("Cập nhật thành công!");
        }

        [HttpDelete("{idEmployee}")]
        public async Task<IActionResult> DeleteEmployee(int idEmployee)
        {
            var result = await _employeeService.DeleteEmployeeAsync(idEmployee);
            if (!result) return NotFound("Không tìm thấy nhân viên để xóa!");
            return Ok("Xóa thành công!");
        }
    }
}
