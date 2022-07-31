using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Contract
{
    public interface IPokemonRepository
    {
        Task<ICollection<Pokemon>> GetPokemonsAsync();
    }
}
