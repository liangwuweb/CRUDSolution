using Entities;
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

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            if(countryAddRequest == null)
            {
                throw new ArgumentNullException("request can't be null");
            }

            if (countryAddRequest.CountryName == null)
                throw new ArgumentException(nameof(countryAddRequest.CountryName));

            if (_db.Countries.Count(p => p.CountryName == countryAddRequest.CountryName) > 0) {
                throw new ArgumentException("country name already exists");
            }

                Country country = countryAddRequest.ToCountry();

            country.CountryID = Guid.NewGuid();

            _db.Countries.Add(country);
            _db.SaveChanges();

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
           return  _db.Countries.Select(c => c.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByID(Guid? id)
        {
            if (id == null) return null;

            return _db.Countries.FirstOrDefault(c => c.CountryID == id)?.ToCountryResponse();
        }
    }
}