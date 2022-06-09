using System;

namespace Hastnama.Solico.Application.UserManagement.Users.ModelDto
{
    public class UserDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string FullName { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPhoneConfirmed { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public DateTime RegisterDate { get; set; }



    }
}