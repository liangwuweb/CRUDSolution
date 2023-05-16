using System;
using Entities;
using ServiceContracrs.DTO;
using ServiceContracrs;
using Services;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        public CountriesServiceTest() { 
        
            _countriesService = new CountriesServices(new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options));
        }

        #region Add Country

        //When CountryAddResuest is null, it should throw ArgumentNullEWxveption
        [Fact]
        public async Task AddCountry_Null()
        {
           
            await Assert.ThrowsAsync<ArgumentNullException>(async() =>
            {
                await _countriesService.AddCountry(null);
            });
        }


        //When the Country Name is null it should throw Argument Exception
        [Fact]
        public async Task AddCountry_CountryNameNull()
        {
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = null
            };
            await Assert.ThrowsAsync<ArgumentException>(async() =>
            {
                await _countriesService.AddCountry(request);
            });
        }

        // Whe nthe CountryName is duplicate it should throw Argument Exception
        [Fact]
        public async Task AddCountry_DuplicateCountryName()
        {

            CountryAddRequest? request1 = new CountryAddRequest()
            {
                CountryName = "US"
            };
            CountryAddRequest? request2 = new CountryAddRequest()
            {
                CountryName = "US"
            };

            
            await Assert.ThrowsAsync<ArgumentException>(async() =>
            {
                await _countriesService.AddCountry(request1);
                await _countriesService.AddCountry(request2);
            });
        }

        //When you supply proper country name, it should insert the country to the existing list 
        // of countries
        [Fact]
        public async Task AddCountry_ProoerCountryDetails()
        {
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = "US"
            };

            CountryResponse  response= await _countriesService.AddCountry(request);
            List<CountryResponse> countires = await _countriesService.GetAllCountries();

            Assert.True(response.CountryID != Guid.Empty);
            Assert.Contains(response, countires);
        }

        #endregion

        #region GetAllCountries
        [Fact]
        // The list of countries should be empty by default (before adding aany countries)
        public async Task GetAllCountries_EmptyList() { 
            List<CountryResponse> actual_countr_response_list = await _countriesService.GetAllCountries();

            Assert.Empty(actual_countr_response_list);
        
        }

        [Fact]
        public async Task GetAllCountries_AddSomeCountries() { 
            List<CountryAddRequest> country_requests = new List<CountryAddRequest>();

            country_requests.Add(new CountryAddRequest() { CountryName = "USA" });
            country_requests.Add(new CountryAddRequest() { CountryName = "UK" });

            List<CountryResponse> country_responses = new List<CountryResponse>();

            foreach (CountryAddRequest req in country_requests) {
                country_responses.Add(await _countriesService.AddCountry(req));
            }

            List<CountryResponse> real_country_responses = await _countriesService.GetAllCountries();

            foreach (CountryResponse res in country_responses) {
                Assert.Contains(res, real_country_responses);
            }


        }
        #endregion

        #region GetCountryById
        [Fact]
        public async Task GetCountryById_NullId()
        { 
            Guid? id = null;

            CountryResponse? res = await _countriesService.GetCountryByID(id);

            Assert.Null(res);
        }

        [Fact]
        public async Task GetCountryById_ValidCountryId() {
            CountryAddRequest? req = new CountryAddRequest() { CountryName = "China" };
            CountryResponse res_from_add = await _countriesService.AddCountry(req);

            CountryResponse? res_from_get_id = await _countriesService.GetCountryByID(res_from_add.CountryID);

            Assert.Equal(res_from_add, res_from_get_id);

        }


        #endregion

    }
}
