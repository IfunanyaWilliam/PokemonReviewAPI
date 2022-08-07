using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Contract
{
    public interface IOwnerRepository
    {
        ICollection<Owner> GetAllOwners();
        Task<Owner> GetOwner(int ownerId);
        Task<ICollection<Owner>> GetOwnersOfAPokemon(int pokemonId);
        Task<ICollection<Pokemon>> GetPokemonByOwner(int ownerId);
        Task<bool> OwnerExistsAsync(int ownerId);
        Task<bool> CreateOwnerAsync(Owner owner);
        Task<bool> UpdateOwnerAsync(Owner owner);
    }
}
