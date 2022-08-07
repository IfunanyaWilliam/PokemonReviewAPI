using Microsoft.EntityFrameworkCore;
using PokemonReviewAPI.Contract;
using PokemonReviewAPI.Data;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly AppDbContext _context;

        public OwnerRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateOwnerAsync(Owner owner)
        {
            await _context.AddAsync(owner);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteOwnerAsync(Owner owner)
        {
            _context.Remove(owner);
            return await _context.SaveChangesAsync() > 0;
        }

        public ICollection<Owner> GetAllOwners()
        {
            return _context.Owners.ToList();
        }

        public async Task<Owner> GetOwner(int ownerId)
        {
            return await _context.Owners.FirstOrDefaultAsync(i => i.Id == ownerId);
        }

        public async Task<ICollection<Owner>> GetOwnersOfAPokemon(int pokemonId)
        {
            return await _context.PokemonOwners.Where(p => p.Pokemon.Id == pokemonId).Select(o => o.Owner).ToListAsync();
        }

        public async Task<ICollection<Pokemon>> GetPokemonByOwner(int ownerId)
        {
            return await _context.PokemonOwners.Where(po => po.OwnerId == ownerId).Select(p => p.Pokemon).ToListAsync();
        }

        
        public async Task<bool> OwnerExistsAsync(int ownerId)
        {
            return await _context.Pokemons.AnyAsync(o => o.Id == ownerId);
        }

        public async Task<bool> UpdateOwnerAsync(Owner owner)
        {
            _context.Owners.Update(owner);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
