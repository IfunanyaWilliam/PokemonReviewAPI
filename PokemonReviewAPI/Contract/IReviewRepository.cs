using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Contract
{
    public interface IReviewRepository
    {
        ICollection<Review> GetAllReviews();
        Review GetReview(int reviewId);
        ICollection<Review> GetReviewsOfAPokemon(int pokemonId);
        Task<bool> ReviewExist(int reviewId);
    }
}
