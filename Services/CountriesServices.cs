using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracrs;
using ServiceContracrs.DTO;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class CountriesServices : ICountriesService
    {
        private readonly PersonsDbContext _db;

        public CountriesServices(PersonsDbContext personsDbContext, bool initialize = true)
        {
            _db = personsDbContext;
            
        }

        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {
            if(countryAddRequest == null)
            {
                throw new ArgumentNullException("request can't be null");
            }

            if (countryAddRequest.CountryName == null)
                throw new ArgumentException(nameof(countryAddRequest.CountryName));

            if (await _db.Countries.CountAsync(temp => temp.CountryName == countryAddRequest.CountryName) > 0)
            {
                throw new ArgumentException("country name already exists");
            }

                Country country = countryAddRequest.ToCountry();

            country.CountryID = Guid.NewGuid();

            _db.Countries.Add(country);
            _db.SaveChanges();

            return country.ToCountryResponse();
        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
           return  await _db.Countries.Select(c => c.ToCountryResponse()).ToListAsync();
        }

        public async Task<CountryResponse?> GetCountryByID(Guid? id)
        {
            if (id == null) return null;
            Country? country_res = await _db.Countries.FirstOrDefaultAsync(c => c.CountryID == id);
            return country_res == null ? null : country_res.ToCountryResponse();
        }
    }
}