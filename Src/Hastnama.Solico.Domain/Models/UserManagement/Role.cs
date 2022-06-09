using System.Collections.Generic;

namespace Hastnama.Solico.Domain.Models.UserManagement
{
    public class Role
    {

        public Role()
        {
            Users = new HashSet<User>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }

        public virtual ICollection<User> Users { get; }


        public static int Admin = 1;
        public static int User = 2;
    }
}