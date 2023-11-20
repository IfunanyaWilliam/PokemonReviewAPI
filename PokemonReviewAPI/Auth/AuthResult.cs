namespace PokemonReviewAPI.Auth
{
    using System.Text.Json.Serialization;

    public class AuthResult
    {
        public string? AccessToken { get; set; }

        //[JsonIgnore]
        public string? RefreshToken { get; set; }

        public string? Message { get; set; }

        public List<string>? Errors { get; set; }

        public bool IsAuthorized { get; set; }
    }
}
