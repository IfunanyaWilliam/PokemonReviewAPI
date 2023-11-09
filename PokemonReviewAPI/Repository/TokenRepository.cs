namespace PokemonReviewAPI.Repository
{
    using Microsoft.EntityFrameworkCore;
    using PokemonReviewAPI.Contract;
    using PokemonReviewAPI.Data;
    using PokemonReviewAPI.Models;

    public class TokenRepository : ITokenRepository
    {
        private readonly AppDbContext _context;

        public TokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(string userEmail)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(r => r.UserEmail == userEmail);
        }

        public async Task<bool> AddRefreshTokenAsync(RefreshToken refreshToken)
        {
             await _context.RefreshTokens.AddAsync(refreshToken);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
