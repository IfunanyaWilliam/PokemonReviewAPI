using Microsoft.EntityFrameworkCore;
using PokemonReviewAPI.Contract;
using PokemonReviewAPI.Data;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly AppDbContext _context;

        public PokemonRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Pokemon>> GetPokemonsAsync()
        {
            return await _context.Pokemons.OrderBy(p => p.Id).ToListAsync();
        }
    }
}
