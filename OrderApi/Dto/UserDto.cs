﻿namespace OrderApi.Dto
{
    public class UserDto
    {
        public int IdUser { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public int IdRole { get; set; }
        public string RoleName { get; set; }
    }
}
