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

        public CountriesServices(bool initialize = true)
        {
            _countries = new List<Country>();
            if (initialize)
            {
                _countries.AddRange(new List<Country>() {
                    new Country() {  CountryID = Guid.Parse("000C76EB-62E9-4465-96D1-2C41FDB64C3B"), CountryName = "USA" },

                    new Country() { CountryID = Guid.Parse("32DA506B-3EBA-48A4-BD86-5F93A2E19E3F"), CountryName = "Canada" },

                    new Country() { CountryID = Guid.Parse("DF7C89CE-3341-4246-84AE-E01AB7BA476E"), CountryName = "UK" },

                    new Country() { CountryID = Guid.Parse("15889048-AF93-412C-B8F3-22103E943A6D"), CountryName = "India" },

                    new Country() { CountryID = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB"), CountryName = "Australia" }
                });
            }
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