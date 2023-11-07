using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Contract
{
    public interface IUserRepository
    {
        Task<AppUser> GetAppUserByIdAsync(string id);
        Task<bool> UpdateUserAsync(AppUser user);
    }
}
