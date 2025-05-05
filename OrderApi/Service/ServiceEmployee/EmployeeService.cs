using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using OrderApi.Dto;
using OrderApi.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Service.ServiceEmployee
{
    public class EmployeeService : IEmployeeService
    {
        private readonly OrderDbContext _context;

        public EmployeeService(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<string> CreateEmployeeAsync(EmployeeDto employeeDto)
        {
            var role = await _context.Roles.FindAsync(employeeDto.IdRole);
            if (role == null)
            {
                return $"Lỗi: Role với Id {employeeDto.IdRole} không tồn tại!";
            }
            var employee = new Employee
            {
                EmployeeName = employeeDto.EmployeeName,
                Password = employeeDto.Password,
                Email = employeeDto.Email,
                Phone = employeeDto.Phone,
                Address = employeeDto.Address,
                Status = employeeDto.Status,
                IdRole = employeeDto.IdRole
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return "Thêm nhân viên thành công!";
        }

        public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .Include(e => e.Role)
                .Select(e => new EmployeeDto
                {
                    IdEmplyee = e.IdEmplyee,
                    EmployeeName = e.EmployeeName,
                    Email = e.Email,
                    Phone = e.Phone,
                    Address = e.Address,
                    Status = e.Status,
                    RoleName = e.Role.RoleName
                })
                .ToListAsync();
        }

        public async Task<EmployeeDto> GetEmployeeByIdAsync(int employeeId)
        {
            var employee = await _context.Employees
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.IdEmplyee == employeeId);

            if (employee == null)
            {
                return null;
            }

            return new EmployeeDto
            {
                IdEmplyee = employee.IdEmplyee,
                EmployeeName = employee.EmployeeName,
                Email = employee.Email,
                Phone = employee.Phone,
                Address = employee.Address,
                Status = employee.Status,
                RoleName = employee.Role.RoleName
            };
        }

        public async Task<bool> UpdateEmployeeAsync(int employeeId, EmployeeDto employeeDto)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
            {
                return false;
            }

            var role = await _context.Roles.FindAsync(employeeDto.IdRole);
            if (role == null)
            {
                return false;
            }

            employee.EmployeeName = employeeDto.EmployeeName;
            employee.Email = employeeDto.Email;
            employee.Phone = employeeDto.Phone;
            employee.Address = employeeDto.Address;
            employee.Status = employeeDto.Status;
            employee.IdRole = employeeDto.IdRole;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEmployeeAsync(int employeeId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
            {
                return false;
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<EmployeeDto>> GetEmployeesByRoleNameAsync(string roleName)
        {
            return await _context.Employees
                .Include(e => e.Role)
                .Where(e => e.Role != null && e.Role.RoleName == roleName)
                .Select(e => new EmployeeDto
                {
                    IdEmplyee = e.IdEmplyee,
                    EmployeeName = e.EmployeeName,
                    Email = e.Email,
                    Phone = e.Phone,
                    Address = e.Address,
                    Status = e.Status,
                    RoleName = e.Role.RoleName
                })
                .ToListAsync();
        }
    }
}