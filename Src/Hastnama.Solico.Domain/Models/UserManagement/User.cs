using System;
using System.Collections.Generic;
using Hastnama.Solico.Domain.Models.Cms;
using Hastnama.Solico.Domain.Models.Shop;

namespace Hastnama.Solico.Domain.Models.UserManagement
{
    public class User
    {
        public User()
        {
            UserTokens = new HashSet<UserToken>();
            UserAddresses = new HashSet<UserAddress>();
            UserMessages = new HashSet<UserMessage>();
        }

        public long Id { get; set; }
        public int RoleId { get; set; }

        public string Name { get; set; }
        public string Family { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string ConfirmedNumber { get; set; }
        public string Avatar { get; set; }
        public string ActivationCode { get; set; }

        public DateTime RegisterDate { get; set; }
        public DateTime ExpiredVerification { get; set; }

        public bool IsEmailConfirmed { get; set; }
        public bool IsPhoneConfirmed { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Role Role { get; set; }
        
        public virtual AppSetting AppSetting { get; set; }
        public virtual ICollection<UserToken> UserTokens { get; }
        public virtual ICollection<UserAddress> UserAddresses { get; }
        public virtual ICollection<UserMessage> UserMessages { get; }

        
    }
}