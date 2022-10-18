using Microsoft.AspNetCore.Identity;

namespace FurStore.Models.Auth
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
    }
}
