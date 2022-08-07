using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Contract
{
    public interface IReviewRepository
    {
        ICollection<Review> GetAllReviews();
        Review GetReview(int reviewId);
        ICollection<Review> GetReviewsOfAPokemon(int pokemonId);
        Task<bool> ReviewExistsAsync(int reviewId);
        Task<bool> CreateReviewAsync(Review review); 
        Task<bool> UpdateReviewAsync(Review review);
        Task<bool> DeleteReviewsAsync(ICollection<Review> reviews);
        Task<bool> DeleteReviewAsync(Review review);
    }
}
