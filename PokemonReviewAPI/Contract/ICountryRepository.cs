using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Contract
{
    public interface ICountryRepository
    {
        ICollection<Country> GetAllCountries();
        Task<Country> GetCountryAsync(int countryId);

        Task<Country> GetCountryByOwnerAsync(int ownerId);
        Task<ICollection<Owner>> GetOwnersFromACountryAsync(int countryId);
        Task<bool> CountryExists (int countryId);

        Task<bool> CreateCountry(Country country);
    }
}
