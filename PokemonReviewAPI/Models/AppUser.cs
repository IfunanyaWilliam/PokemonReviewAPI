using Microsoft.AspNetCore.Identity;

namespace PokemonReviewAPI.Models
{
    public class AppUser : IdentityUser
    {
        public string UserName { get; set; }

        public string Email { get; set; }
    }
}
