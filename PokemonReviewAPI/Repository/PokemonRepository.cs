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

        public decimal GetPokemoneRating(int pokemonId)
        {
            var reviews = _context.Reviews.Where(p => p.Pokemon.Id == pokemonId);
            if(reviews.Count () <= 0)
            {
                return 0;
            }
            return  ((decimal)reviews.Sum(r => r.Rating) / reviews.Count());
        }

        public async Task<ICollection<Pokemon>> GetAllPokemonsAsync()
        {
            return await _context.Pokemons.OrderBy(p => p.Id).ToListAsync();
        }

        public async Task<bool> PokemonExist(int pokemonId)
        {
            return await _context.Pokemons.AnyAsync(p => p.Id == pokemonId);
        }

        public async Task<bool> CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var pokemonOwnerEntity = await _context.Owners.Where(o => o.Id == ownerId).FirstOrDefaultAsync();
            var category     = await _context.Categories.Where(c => c.Id == categoryId).FirstOrDefaultAsync();

            var pokemonOwner = new PokemonOwner()
            {
                Owner = pokemonOwnerEntity,
                Pokemon = pokemon,
            };
            await _context.AddAsync(pokemonOwner);

            var pokemonCategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = pokemon
            };
            await _context.AddAsync(pokemonCategory);
            await _context.AddAsync(pokemon);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
