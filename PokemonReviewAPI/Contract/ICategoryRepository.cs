using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Contract
{
    public interface ICategoryRepository
    {
        Task<ICollection<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategory(int id);
        Task<ICollection<Pokemon>> GetPokemonByCategory(int categoryId);
        bool CategoryExists(int id);
    }
}
 