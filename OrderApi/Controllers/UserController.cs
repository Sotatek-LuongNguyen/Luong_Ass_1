using Microsoft.AspNetCore.Mvc;
using OrderApi.Dto;
using OrderApi.Service.ServiceUser;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        {
            var result = await _userService.CreateUserAsync(userDto);
            if (result == null) return BadRequest("Role ID không hợp lệ!");
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }


        [HttpGet("{idUser}")]
        public async Task<ActionResult<UserDto>> GetUserById(int idUser)
        {
            var user = await _userService.GetUserByIdAsync(idUser);
            if (user == null) return NotFound("Không tìm thấy người dùng!");
            return Ok(user);
        }
        [HttpPut("{idUser}")]
        public async Task<IActionResult> UpdateUser(int idUser, [FromBody] UserDto userDto)
        {
            var result = await _userService.UpdateUserAsync(idUser, userDto);
            if (!result) return BadRequest("Cập nhật thất bại! Kiểm tra IdUser hoặc IdRole.");
            return Ok("Cập nhật thành công!");
        }

        [HttpDelete("{idUser}")]
        public async Task<IActionResult> DeleteUser(int idUser)
        {
            var result = await _userService.DeleteUserAsync(idUser);
            if (!result) return NotFound("Không tìm thấy người dùng để xóa!");
            return Ok("Xóa thành công!");
        }
    }
}
