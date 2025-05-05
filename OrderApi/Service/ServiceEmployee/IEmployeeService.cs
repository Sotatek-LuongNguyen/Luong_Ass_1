using OrderApi.Dto;
namespace OrderApi.Service.ServiceEmployee
{
    public interface IEmployeeService
    {
        Task<string> CreateEmployeeAsync(EmployeeDto employeeDto);
        Task<List<EmployeeDto>> GetAllEmployeesAsync();
        Task<EmployeeDto> GetEmployeeByIdAsync(int employeeId);
        Task<bool> UpdateEmployeeAsync(int employeeId, EmployeeDto employeeDto);
        Task<bool> DeleteEmployeeAsync(int employeeId);
        Task<List<EmployeeDto>> GetEmployeesByRoleNameAsync(string roleName);
    }
}
