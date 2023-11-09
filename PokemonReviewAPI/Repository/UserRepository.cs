namespace PokemonReviewAPI.Repository
{
    using Microsoft.EntityFrameworkCore;
    using PokemonReviewAPI.Contract;
    using PokemonReviewAPI.Data;
    using PokemonReviewAPI.Models;

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AppUser> GetAppUserAsync(string userEmail)
        {
            return await _context.AppUsers.Include(r => r.RefreshTokens).FirstOrDefaultAsync(u => u.Email == userEmail);
        }

        public async Task<bool> UpdateUserAsync(AppUser user)
        {
            var userExist = await _context.AppUsers.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (userExist == null)
                return false;

             _context.AppUsers.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
