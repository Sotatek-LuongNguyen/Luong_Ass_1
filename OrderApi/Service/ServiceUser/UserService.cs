using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using OrderApi.Dto;
using OrderApi.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Service.ServiceUser
{
    public class UserService : IUserService
    {
        private readonly OrderDbContext _context;

        public UserService(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<string> CreateUserAsync(UserDto userDto)
        {
            var role = await _context.Roles.FindAsync(userDto.IdRole);
            if (role == null)
            {
                return $"Lỗi: Role với Id {userDto.IdRole} không tồn tại!";
            }
            var user = new User
            {
                Username = userDto.Username,
                Password = userDto.Password,
                Email = userDto.Email,
                Phone = userDto.Phone,
                Address = userDto.Address,
                Status = userDto.Status,
                IdRole = userDto.IdRole 
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return "Thêm người dùng thành công!";
        }


        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Role)  
                .Select(u => new UserDto
                {
                    IdUser = u.IdUser,
                    Username = u.Username,
                    Email = u.Email,
                    Phone = u.Phone,
                    Address = u.Address,
                    Status = u.Status,
                    RoleName = u.Role.RoleName 
                })
                .ToListAsync();
        }


        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Role) 
                .FirstOrDefaultAsync(u => u.IdUser == userId); 

            if (user == null)
            {
                return null; 
            }

            return new UserDto
            {
                IdUser = user.IdUser,
                Username = user.Username,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                Status = user.Status,
                RoleName = user.Role.RoleName 
            };
        }


        public async Task<bool> UpdateUserAsync(int userId, UserDto userDto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            var role = await _context.Roles.FindAsync(userDto.IdRole);
            if (role == null)
            {
                return false;
            }
            user.Username = userDto.Username;
            user.Email = userDto.Email;
            user.Phone = userDto.Phone;
            user.Address = userDto.Address;
            user.Status = userDto.Status;
            user.IdRole = userDto.IdRole;

            await _context.SaveChangesAsync();
            return true; 
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return false; 
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true; 
        }
    }
}
