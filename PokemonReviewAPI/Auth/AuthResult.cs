namespace PokemonReviewAPI.Auth
{
    using System.Text.Json.Serialization;

    public class AuthResult
    {
        public string? Token { get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }

        public string? Message { get; set; }

        public bool Result { get; set; }

        public List<string>? Errors { get; set; }
    }
}
