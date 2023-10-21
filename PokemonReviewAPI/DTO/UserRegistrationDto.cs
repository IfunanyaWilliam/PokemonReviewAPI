namespace PokemonReviewAPI.DTO
{
    using System.ComponentModel.DataAnnotations;

    public class UserRegistrationDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
