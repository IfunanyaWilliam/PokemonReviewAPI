using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Contract
{
    public interface IOwnerRepository
    {
        ICollection<Owner> GetAllOwners();
        Task<Owner> GetOwner(int ownerId);
        Task<ICollection<Owner>> GetOwnersOfAPokemon(int pokemonId);
        Task<ICollection<Pokemon>> GetPokemonByOwner(int ownerId);
        Task<bool> OwnerExists(int ownerId);
        Task<bool> CreateOwner(Owner owner);
    }
}
