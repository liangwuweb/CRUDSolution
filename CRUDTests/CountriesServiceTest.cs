using System;
using Entities;
using ServiceContracrs.DTO;
using ServiceContracrs;
using Services;
using System.Diagnostics.CodeAnalysis;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        public CountriesServiceTest() { 
        
            _countriesService = new CountriesServices(false);
        }

        #region Add Country

        //When CountryAddResuest is null, it should throw ArgumentNullEWxveption
        [Fact]
        public void AddCountry_Null()
        {
           
            Assert.Throws<ArgumentNullException>(() =>
            {
                _countriesService.AddCountry(null);
            });
        }


        //When the Country Name is null it should throw Argument Exception
        [Fact]
        public void AddCountry_CountryNameNull()
        {
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = null
            };
            Assert.Throws<ArgumentException>(() =>
            {
                _countriesService.AddCountry(request);
            });
        }

        // Whe nthe CountryName is duplicate it should throw Argument Exception
        [Fact]
        public void AddCountry_DuplicateCountryName()
        {

            CountryAddRequest? request1 = new CountryAddRequest()
            {
                CountryName = "US"
            };
            CountryAddRequest? request2 = new CountryAddRequest()
            {
                CountryName = "US"
            };

            
            Assert.Throws<ArgumentException>(() =>
            {
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request2);
            });
        }

        //When you supply proper country name, it should insert the country to the existing list 
        // of countries
        [Fact]
        public void AddCountry_ProoerCountryDetails()
        {
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = "US"
            };

            CountryResponse  response= _countriesService.AddCountry(request);
            List<CountryResponse> countires = _countriesService.GetAllCountries();

            Assert.True(response.CountryID != Guid.Empty);
            Assert.Contains(response, countires);
        }

        #endregion

        #region GetAllCountries
        [Fact]
        // The list of countries should be empty by default (before adding aany countries)
        public void GetAllCountries_EmptyList() { 
            List<CountryResponse> actual_countr_response_list = _countriesService.GetAllCountries();

            Assert.Empty(actual_countr_response_list);
        
        }

        [Fact]
        public void GetAllCountries_AddSomeCountries() { 
            List<CountryAddRequest> country_requests = new List<CountryAddRequest>();

            country_requests.Add(new CountryAddRequest() { CountryName = "USA" });
            country_requests.Add(new CountryAddRequest() { CountryName = "UK" });

            List<CountryResponse> country_responses = new List<CountryResponse>();

            foreach (CountryAddRequest req in country_requests) {
                country_responses.Add(_countriesService.AddCountry(req));
            }

            List<CountryResponse> real_country_responses = _countriesService.GetAllCountries();

            foreach (CountryResponse res in country_responses) {
                Assert.Contains(res, real_country_responses);
            }


        }
        #endregion

        #region GetCountryById
        [Fact]
        public void GetCountryById_NullId()
        { 
            Guid? id = null;

            CountryResponse? res = _countriesService.GetCountryByID(id);

            Assert.Null(res);
        }

        [Fact]
        public void GetCountryById_ValidCountryId() {
            CountryAddRequest? req = new CountryAddRequest() { CountryName = "China" };
            CountryResponse res_from_add = _countriesService.AddCountry(req);

            CountryResponse? res_from_get_id = _countriesService.GetCountryByID(res_from_add.CountryID);

            Assert.Equal(res_from_add, res_from_get_id);

        }


        #endregion

    }
}
