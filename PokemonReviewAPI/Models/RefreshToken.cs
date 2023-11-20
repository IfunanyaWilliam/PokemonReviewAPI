namespace PokemonReviewAPI.Models
{
    public class RefreshToken
    {
        public Guid Id { get; set; }

        public string Token { get; set; }

        public string UserEmail { get; set; }

        public DateTime Expires { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Revoked { get; set; }

        public bool IsActive => Expires > DateTime.UtcNow && Revoked == null;
    }
}
