using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Contract
{
    public interface ICountryRepository
    {
        Task<ICollection<Country>> GetAllCountriesAsync();
        Task<Country> GetCountryAsync(int countryId);

        Task<Country> GetCountryByOwnerAsync(int ownerId);
        Task<ICollection<Owner>> GetOwnersFromACountryAsync(int countryId);
        bool CountryExists (int countryId);
    }
}
