using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Contract
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetAllCategories();
        Task<Category> GetCategory(int id);
        Task<ICollection<Pokemon>> GetPokemonByCategory(int categoryId);
        Task<bool> CategoryExists(int id);
        Task<bool> CreateCategory(Category category);
        Task<bool> UpdateCategory(Category category);
    }
}
  