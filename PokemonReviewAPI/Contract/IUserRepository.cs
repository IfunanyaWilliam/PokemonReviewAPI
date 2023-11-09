using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Contract
{
    public interface IUserRepository
    {
        Task<AppUser> GetAppUserAsync(string userEmail);

        Task<bool> UpdateUserAsync(AppUser user);
    }
}
