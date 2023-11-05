namespace PokemonReviewAPI.Auth
{
    public class AuthResult
    {
        public string? Token { get; set; }

        public string? Message { get; set; }

        public bool Result { get; set; }

        public List<string>? Errors { get; set; }
    }
}
