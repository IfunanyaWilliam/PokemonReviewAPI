using Microsoft.EntityFrameworkCore;
using PokemonReviewAPI.Contract;
using PokemonReviewAPI.Data;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }
        public ICollection<Review> GetAllReviews()
        {
            return _context.Reviews.ToList();
        }

        public Review GetReview(int reviewId)
        {
            return _context.Reviews.FirstOrDefault(r => r.Id == reviewId);
        }

        public ICollection<Review> GetReviewsOfAPokemon(int pokemonId)
        {
            return _context.Reviews.Where(r => r.Pokemon.Id == pokemonId).ToList();
        }

        public async Task<bool> ReviewExist(int reviewId)
        {
            return await _context.Reviews.AnyAsync(r => r.Id == reviewId);
        }
    }
}
