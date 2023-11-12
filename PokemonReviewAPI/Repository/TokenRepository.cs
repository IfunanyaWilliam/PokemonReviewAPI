namespace PokemonReviewAPI.Repository
{
    using Microsoft.EntityFrameworkCore;
    using PokemonReviewAPI.Contract;
    using PokemonReviewAPI.Data;
    using PokemonReviewAPI.Models;
    using System.Reflection.Metadata.Ecma335;

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

        public async Task<RefreshToken> AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            var refreshTokenExist = await _context.RefreshTokens.
                                    FirstOrDefaultAsync(u => u.UserEmail == refreshToken.UserEmail);

            if(refreshTokenExist != null)
            {
                if (refreshTokenExist.IsActive == true)
                    return refreshTokenExist;
            }

            await _context.RefreshTokens.AddAsync(refreshToken);
            var result = await _context.SaveChangesAsync();
            if (result > 0) return refreshToken;

            return null;
        }

        public async Task<bool> UpdateRefreshTokenAsync(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Update(refreshToken);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RevokeRefreshTokenAsync(string token)
        {
            var existingToken = await _context.RefreshTokens.FirstOrDefaultAsync(r => r.Token == token);
            if (existingToken == null) return false;

            if(!existingToken.IsActive) return false;    

            existingToken.Revoked = DateTime.UtcNow;
            _context.Update(existingToken);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
