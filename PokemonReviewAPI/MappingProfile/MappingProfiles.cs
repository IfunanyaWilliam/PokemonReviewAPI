using PokemonReviewAPI.DTO;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.MappingProfile
{
    public class MappingProfiles : AutoMapper.Profile
    {
        public MappingProfiles()
        {
            CreateMap<Pokemon, PokemonDTO>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();
            CreateMap<Country, CountryDTO>();
            CreateMap<CountryDTO, Country>();
            CreateMap<Owner, OwnerDTO>();
            CreateMap<OwnerDTO, Owner>();
            CreateMap<Review, ReviewDTO>();
            CreateMap<ReviewDTO, Review>();
            CreateMap<Reviewer, ReviewerDTO>();
            CreateMap<ReviewerDTO, Reviewer>();
        }
    }
}
