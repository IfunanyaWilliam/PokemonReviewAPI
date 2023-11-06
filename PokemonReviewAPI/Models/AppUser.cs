using Microsoft.AspNetCore.Identity;

namespace PokemonReviewAPI.Models
{
    public class AppUser : IdentityUser
    {
        public override string UserName { get; set; }

        public override string Email { get; set; }

        public List<RefreshToken> RefreshTokens { get; set; }
    }
}
