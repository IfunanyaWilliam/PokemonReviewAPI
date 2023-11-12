using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Auth
{
    public class AuthorizationResult
    {
        public AppUser User { get; set; }

        public RefreshToken RefreshToken { get; set; }

        public bool IsUserModified { get; set; }

        public bool IsRefreshTokenSaved { get; set; }
    }
}
