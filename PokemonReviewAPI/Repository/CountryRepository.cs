using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewAPI.Contract;
using PokemonReviewAPI.Data;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CountryRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public bool CountryExists(int countryId)
        {
            return _context.Countries.Any(c => c.Id == countryId);
        }

        public async Task<bool> CreateCountry(Country country)
        {
            await _context.AddAsync(country);
            return await _context.SaveChangesAsync() > 0;
        }

        public ICollection<Country> GetAllCountries()
        {
            return _context.Countries.ToList();
        }

        public async Task<Country> GetCountryAsync(int countryId)
        {
            return await _context.Countries.FirstOrDefaultAsync(i => i.Id == countryId);
        }

        public async Task<Country> GetCountryByOwnerAsync(int ownerId)
        {
            return await _context.Owners.Where(o => o.Id == ownerId).Select(c => c.Country).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Owner>> GetOwnersFromACountryAsync(int countryId)
        {
            return await _context.Owners.Where(c => c.Country.Id == countryId).ToListAsync();
        }
    }
}
