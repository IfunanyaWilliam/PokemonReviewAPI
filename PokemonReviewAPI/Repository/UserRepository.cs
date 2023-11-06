namespace PokemonReviewAPI.Repository
{
    using Microsoft.EntityFrameworkCore;
    using PokemonReviewAPI.Data;

    public class UserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateUserAsync(string email)
        {
            var existingUser = await _context.AppUsers.FirstOrDefaultAsync(x => x.Email == email);
            if (existingUser != null)
                return false;

            var result = _context.AppUsers.Update(existingUser);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
