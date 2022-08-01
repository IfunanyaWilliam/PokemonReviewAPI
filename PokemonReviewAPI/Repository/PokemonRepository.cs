using Microsoft.EntityFrameworkCore;
using PokemonReviewAPI.Contract;
using PokemonReviewAPI.Data;
using PokemonReviewAPI.Models;
using System.Linq.Expressions;

namespace PokemonReviewAPI.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly AppDbContext _context;

        public PokemonRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Pokemon> GetPokemonAsync(Expression<Func<Pokemon, bool>> predicate)
        {
            return await _context.Pokemons.AsQueryable().FirstOrDefaultAsync(predicate);
        }

        public async Task<decimal> GetPokemoneRating(int pokemonId)
        {
            var reviews = _context.Reviews.Where(p => p.Pokemon.Id == pokemonId);
            if(reviews.Count() <= 0)
            {
                return 0;
            }
            return  ((decimal)reviews.Sum(r => r.Rating) / reviews.Count());
        }

        public async Task<ICollection<Pokemon>> GetAllPokemonsAsync()
        {
            return await _context.Pokemons.OrderBy(p => p.Id).ToListAsync();
        }

        public async Task<bool> PokemonExistAsync(int pokemonId)
        {
            return _context.Pokemons.Any(p => p.Id == pokemonId);
        }
    }
}
