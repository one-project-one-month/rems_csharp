using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Models.User
{
    public class UserModel
    {
        public int UserId { get; set; }

        public string Name { get; set; } = null!;
                     
        public string Email { get; set; } = null!;
                     
        public string Password { get; set; } = null!;

        public string? Phone { get; set; }

        public string Role { get; set; } = null!;

        public DateTime? DateCreated { get; set; }
    }
}
