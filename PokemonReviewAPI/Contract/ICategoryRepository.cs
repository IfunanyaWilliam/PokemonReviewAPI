using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Contract
{
    public interface ICategoryRepository
    {
        Task<ICollection<Category>> GetAllCategories();
        Task<ICollection<Category>> GetAllCategory(int id);
        Task<ICollection<Pokemon>> GetPokemonByCategory(int categoryId);
        Task<bool> CategoryExist(int id);
    }
}
