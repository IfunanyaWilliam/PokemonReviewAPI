using PokemonReviewAPI.Models;
using System.Linq.Expressions;

namespace PokemonReviewAPI.Contract
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetAllPokemons();
        Task<Pokemon> GetPokemonAsync(Expression<Func<Pokemon, bool>> predicate);
        decimal GetPokemoneRating(int pokemonId);
        Task<bool> PokemonExistsAsync(int pokemonId);
        Task<bool> CreatePokemonAsync(int ownerId, int categoryId, Pokemon pokemon);
        Task<bool> UpdatePokemonAsync(int ownerId, int categoryId, Pokemon pokemon);
    }
}
