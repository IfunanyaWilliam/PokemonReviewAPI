using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Contract
{
    public interface IReviewRepository
    {
        Task<ICollection<Review>> GetAllReviewsAsync();
        Task<ICollection<Review>> GetReviewsAsync(int reviewId);
        Task<ICollection<Review>> GetReviewsOfAPokemonAsync(int pokemonId);
        Task<bool> ReviewExist(int reviewId);
    }
}
