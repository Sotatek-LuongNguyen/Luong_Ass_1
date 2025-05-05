using OrderApi.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderApi.Service.ServiceUser
{
    public interface IUserService
    {
        Task<string> CreateUserAsync(UserDto userDto);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int userId);
        Task<bool> UpdateUserAsync(int idUser, UserDto userDto); 
        Task<bool> DeleteUserAsync(int userId);
    }
}
