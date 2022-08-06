using PokemonReviewAPI.Models;
using System.Linq.Expressions;

namespace PokemonReviewAPI.Contract
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetAllPokemons();
        Task<Pokemon> GetPokemonAsync(Expression<Func<Pokemon, bool>> predicate);

        decimal GetPokemoneRating(int pokemonId);
        Task<bool> PokemonExists(int pokemonId);
        Task<bool> CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);
    }
}
