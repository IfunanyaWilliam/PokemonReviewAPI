using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Contract
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetAllCategories();
        Task<Category> GetCategory(int id);
        Task<ICollection<Pokemon>> GetPokemonByCategory(int categoryId);
        Task<bool> CategoryExistsAsync(int id);
        Task<bool> CreateCategoryAsync(Category category);
        Task<bool> UpdateCategoryAsync(Category category);
    }
}
  