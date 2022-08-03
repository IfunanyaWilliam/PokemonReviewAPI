using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Contract
{
    public interface ICategoryRepository
    {
        Task<ICollection<Category>> GetAllCategories();
        Task<Category> GetCategory(int id);
        Task<ICollection<Pokemon>> GetPokemonByCategory(int categoryId);
        bool CategoryExist(int id);
    }
}
 