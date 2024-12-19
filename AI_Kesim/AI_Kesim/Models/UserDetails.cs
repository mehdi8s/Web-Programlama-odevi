using Microsoft.AspNetCore.Identity;

namespace AI_Kesim.Models
{
    public class UserDetails:IdentityUser
    {
        public string UserFirstname { get; set; }

        public string UserLastname { get; set; }
    }
}
