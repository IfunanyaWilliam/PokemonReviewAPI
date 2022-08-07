using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewAPI.Contract;
using PokemonReviewAPI.Data;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ReviewerRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CreateReviewerAsync(Reviewer reviewer)
        {
            await _context.AddAsync(reviewer);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteReviewerAsync(Reviewer reviewer)
        {
            _context.Remove(reviewer);
            return await _context.SaveChangesAsync() > 0;
        }

        public ICollection<Reviewer> GetAllReviewers()
        {
            return _context.Reviewers.ToList();
        }

        public Reviewer GetReviewer(int reviewerId)
        {
            return _context.Reviewers.Where(i => i.Id == reviewerId).Include(e => e.Reviews).FirstOrDefault();
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
            return _context.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToList();
        }

        public Task<bool> ReviewerExistsAsync(int reviewerId)
        {
            return _context.Reviewers.AnyAsync(r => r.Id == reviewerId);
        }

        public async Task<bool> UpdateReviewerAsync(Reviewer reviewer)
        {
            _context.Update(reviewer);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
