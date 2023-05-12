using Entities;
using ServiceContracrs;
using ServiceContracrs.DTO;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class CountriesServices : ICountriesService
    {
        private readonly List<Country> _countries;

        public CountriesServices()
        {
            _countries = new List<Country>();
        }
        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            if(countryAddRequest == null)
            {
                throw new ArgumentNullException("request can't be null");
            }

            if (countryAddRequest.CountryName == null)
                throw new ArgumentException(nameof(countryAddRequest.CountryName));

            if (_countries.Where(p => p.CountryName == countryAddRequest.CountryName).Count() > 0) {
                throw new ArgumentException("country name already exists");
            }

                Country country = countryAddRequest.ToCountry();

            country.CountryID = Guid.NewGuid();

            _countries.Add(country);

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
           return  _countries.Select(c => c.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByID(Guid? id)
        {
            if (id == null) return null;

            return _countries.FirstOrDefault(c => c.CountryID == id)?.ToCountryResponse();
        }
    }
}