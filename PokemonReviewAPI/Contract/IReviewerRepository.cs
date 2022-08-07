using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Contract
{
    public interface IReviewerRepository
    {
        ICollection<Reviewer> GetAllReviewers();
        Reviewer GetReviewer(int reviewerId);
        ICollection<Review> GetReviewsByReviewer(int reviewerId);
        Task<bool> ReviewerExistsAsync(int reviewerId);
        Task<bool> CreateReviewerAsync(Reviewer reviewer);
        Task<bool> UpdateReviewerAsync(Reviewer reviewer);
    }
}
