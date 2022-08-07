using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Contract
{
    public interface ICountryRepository
    {
        ICollection<Country> GetAllCountries();
        Task<Country> GetCountryAsync(int countryId);

        Task<Country> GetCountryByOwnerAsync(int ownerId);
        Task<ICollection<Owner>> GetOwnersFromACountryAsync(int countryId);
        Task<bool> CountryExistsAsync(int countryId);

        Task<bool> CreateCountryAsync(Country country);
        Task<bool> UpdateCountryAsync(Country country);
        Task<bool> DeleteCountryAsync(Country country);
    }
}
