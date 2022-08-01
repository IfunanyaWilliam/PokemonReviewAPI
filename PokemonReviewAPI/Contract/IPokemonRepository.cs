using PokemonReviewAPI.Models;
using System.Linq.Expressions;

namespace PokemonReviewAPI.Contract
{
    public interface IPokemonRepository
    {
        Task<ICollection<Pokemon>> GetAllPokemonsAsync();
        Task<Pokemon> GetPokemonAsync(Expression<Func<Pokemon, bool>> predicate);

        Task<decimal> GetPokemoneRating(int pokemonId);
        Task<bool> PokemonExistAsync(int pokemonId);
    }
}
