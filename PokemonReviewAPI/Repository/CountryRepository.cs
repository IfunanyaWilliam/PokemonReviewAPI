﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReviewAPI.Contract;
using PokemonReviewAPI.Data;
using PokemonReviewAPI.Models;

namespace PokemonReviewAPI.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly AppDbContext _context;

        public CountryRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CountryExistsAsync(int countryId)
        {
            return await _context.Countries.AnyAsync(c => c.Id == countryId);
        }

        public async Task<bool> CreateCountryAsync(Country country)
        {
            await _context.AddAsync(country);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCountryAsync(Country country)
        {
            _context.Remove(country);
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

        public async Task<bool> UpdateCountryAsync(Country country)
        {
            _context.Countries.Update(country);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
