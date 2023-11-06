using PokemonReviewAPI.Auth;
using PokemonReviewAPI.DTO;

namespace PokemonReviewAPI.Contract
{
    public interface IAccountService
    {
        Task<AuthResult> RegisterUserAsync(UserRegistrationDto requestDto);
    }
}
