using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Contract
{
    public interface IReviewerRepository
    {
        ICollection<Reviewer> GetAllReviewers();
        Reviewer GetReviewer(int reviewerId);
        ICollection<Review> GetReviewsByReviewer(int reviewerId);
        Task<bool> ReviewerExists(int reviewerId);
        Task<bool> CreateReviewer(Reviewer reviewer);
    }
}
