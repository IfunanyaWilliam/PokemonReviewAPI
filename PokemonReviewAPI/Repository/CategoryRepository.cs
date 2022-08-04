using Microsoft.EntityFrameworkCore;
using PokemonReviewAPI.Contract;
using PokemonReviewAPI.Data;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }

        public async Task<bool> CreateCategory(Category category)
        {
            await _context.AddAsync(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public ICollection<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

        public async Task<Category> GetCategory(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<ICollection<Pokemon>> GetPokemonByCategory(int categoryId)
        {
            return await _context.PokemonCategories.Where(c => c.CategoryId == categoryId).Select(p => p.Pokemon).ToListAsync();
        }
    }
}
