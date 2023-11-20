namespace PokemonReviewAPI.Repository
{
    using Microsoft.EntityFrameworkCore;
    using PokemonReviewAPI.Contract;
    using PokemonReviewAPI.Data;
    using PokemonReviewAPI.Models;
    using System.Linq.Expressions;
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

        public async Task<RefreshToken> AddRefreshTokenAsync(string refreshToken, AppUser user)
        {
            var existingRefreshToken = await _context.RefreshTokens.
                                    FirstOrDefaultAsync(u => u.UserEmail == user.Email);

            if(existingRefreshToken != null)
            {
                if (existingRefreshToken.IsActive == true)
                    return existingRefreshToken;
            }

            var newRefreshToken = new RefreshToken
            {
                Token = refreshToken,
                UserEmail = user.Email,
                Expires = DateTime.UtcNow.AddMinutes(10),
                Created = DateTime.UtcNow
            };

            await _context.RefreshTokens.AddAsync(newRefreshToken);
            var result = await _context.SaveChangesAsync();
            if (result > 0) return newRefreshToken;

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

        public async Task DeleteExpiredRefreshTokenAsync()
        {
            Expression<Func<RefreshToken, bool>> predicate = s => !(s.Expires > DateTime.UtcNow && s.Revoked == null);

            var refreshTokens = await _context
                .RefreshTokens
                .Where(predicate)
                .ToListAsync();

            _context.RefreshTokens.RemoveRange(refreshTokens);

            await _context.SaveChangesAsync();
        }
    }
}
