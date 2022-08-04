using PokemonReviewAPI.Models;
using System.Linq.Expressions;

namespace PokemonReviewAPI.Contract
{
    public interface IPokemonRepository
    {
        Task<ICollection<Pokemon>> GetAllPokemonsAsync();
        Task<Pokemon> GetPokemonAsync(Expression<Func<Pokemon, bool>> predicate);

        decimal GetPokemoneRating(int pokemonId);
        Task<bool> PokemonExist(int pokemonId);
    }
}
