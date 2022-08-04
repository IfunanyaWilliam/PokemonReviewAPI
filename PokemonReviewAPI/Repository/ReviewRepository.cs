using PokemonReviewAPI.Contract;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        public Task<ICollection<Review>> GetAllReviewsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Review>> GetReviewsAsync(int reviewId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Review>> GetReviewsOfAPokemonAsync(int pokemonId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ReviewExist(int reviewId)
        {
            throw new NotImplementedException();
        }
    }
}
