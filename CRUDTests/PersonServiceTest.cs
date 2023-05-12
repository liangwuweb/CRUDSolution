using ServiceContracrs;
using System;
using Xunit;
using ServiceContracrs.DTO;
using ServiceContracrs;
using ServiceContracrs.Enums;
using Services;
using System.Collections.Generic;
using Xunit.Abstractions;
using Entities;

namespace CRUDTests
{
    public class PersonServiceTest
    {
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _helper;

        public PersonServiceTest(ITestOutputHelper helper) {

            _personService = new PersonService();
            _countriesService = new CountriesServices();
            _helper = helper;
        }

        #region AddPerson
        [Fact]
        public void AddPerson_Null() { 
            PersonAddRequest? req = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                _personService.AddPerson(req);

            });
        
        }

        [Fact]
        public void AddPerson_NameNull()
        {
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = null};

            Assert.Throws<ArgumentException>(() =>
            {
                _personService.AddPerson(personAddRequest);

            });

        }

        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = "Demo",
                Email = "person@email.com", Address = "sample address", CountryID = Guid.NewGuid(), Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-02-02"), ReceiveNewsLetters = true
            
            };

            
                PersonResponse res = _personService.AddPerson(personAddRequest);

            Assert.True(res.PersonId != Guid.Empty);
            List<PersonResponse> list = _personService.GetAllPersons();
            Assert.Contains(res, list);

        }

        #endregion

        #region GetPersonByPersonID
        [Fact]
        public void GetPersonByPersonID_Null() {
            Assert.Null(_personService.GetPersonByPersonID(null));
        }

        [Fact]
        public void GetPersonByPerson_withPersinID() { 
            CountryAddRequest country_req = new CountryAddRequest() { 
                CountryName = "Canada"
            
            };
            CountryResponse country_res = _countriesService.AddCountry(country_req);

            PersonAddRequest person_req = new PersonAddRequest()
            {
                PersonName = "Liang",
                Email = "liangwu@gmail.com",
                Address = "Demo",
                CountryID = country_res.CountryID,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = false
            };

            PersonResponse person_res_from_add = _personService.AddPerson(person_req);

            PersonResponse person_res_from_get = _personService.GetPersonByPersonID(person_res_from_add.PersonId);

            Assert.Equal(person_res_from_add, person_res_from_get);
        }
        #endregion

        #region GetAllPersons
        [Fact]
        public void GetAllPersons_EmptyList() { 
            List<PersonResponse> persons_from_get = _personService.GetAllPersons();
            Assert.Empty(persons_from_get);
        }

        [Fact]
        public void GetAllPersons_AddFewPersons() {
            CountryAddRequest country_req_1 = new CountryAddRequest()
            {
                CountryName = "Canada"

            };
            CountryAddRequest country_req_2 = new CountryAddRequest()
            {
                CountryName = "USA"

            };
            CountryResponse country_res_1 = _countriesService.AddCountry(country_req_1);
            CountryResponse country_res_2 = _countriesService.AddCountry(country_req_2);

            PersonAddRequest person_req_1 = new PersonAddRequest()
            {
                PersonName = "Liang",
                Email = "liangwu@gmail.com",
                Address = "Demo",
                CountryID = country_res_1.CountryID,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = false
            };

            PersonAddRequest person_req_2 = new PersonAddRequest()
            {
                PersonName = "Aaron",
                Email = "Aaron@gmail.com",
                Address = "test",
                CountryID = country_res_2.CountryID,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2007-01-01"),
                ReceiveNewsLetters = false
            };

            PersonAddRequest person_req_3 = new PersonAddRequest()
            {
                PersonName = "Jue",
                Email = "juezhang@gmail.com",
                Address = "test",
                CountryID = country_res_2.CountryID,
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("1986-01-01"),
                ReceiveNewsLetters = true
            };

            List<PersonResponse> person_res_list = new List<PersonResponse>();
            List<PersonAddRequest> person_req_list = new List<PersonAddRequest>() {
                person_req_1, person_req_2, person_req_3
            };
            foreach (PersonAddRequest req in person_req_list) { 
                person_res_list.Add(_personService.AddPerson(req));
            }
            //print person 
            _helper.WriteLine("Expected: ");
            foreach (PersonResponse res in person_res_list) {
                _helper.WriteLine(res.ToString());
            }


            List<PersonResponse> actual_person_res = _personService.GetAllPersons();

            //print actual
            _helper.WriteLine("Actual: ");
            foreach (PersonResponse res in actual_person_res)
            {
                _helper.WriteLine(res.ToString());
            }

            foreach (PersonResponse res in person_res_list) {
                Assert.Contains(res, actual_person_res);
            
            }


        }

        #endregion

        #region GetFilteredPersons

        [Fact]
        public void GetFilteredPersons_EmptySearchText()
        {
            CountryAddRequest country_req_1 = new CountryAddRequest()
            {
                CountryName = "Canada"

            };
            CountryAddRequest country_req_2 = new CountryAddRequest()
            {
                CountryName = "USA"

            };
            CountryResponse country_res_1 = _countriesService.AddCountry(country_req_1);
            CountryResponse country_res_2 = _countriesService.AddCountry(country_req_2);

            PersonAddRequest person_req_1 = new PersonAddRequest()
            {
                PersonName = "Liang",
                Email = "liangwu@gmail.com",
                Address = "Demo",
                CountryID = country_res_1.CountryID,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = false
            };

            PersonAddRequest person_req_2 = new PersonAddRequest()
            {
                PersonName = "Aaron",
                Email = "Aaron@gmail.com",
                Address = "test",
                CountryID = country_res_2.CountryID,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2007-01-01"),
                ReceiveNewsLetters = false
            };

            PersonAddRequest person_req_3 = new PersonAddRequest()
            {
                PersonName = "Jue",
                Email = "juezhang@gmail.com",
                Address = "test",
                CountryID = country_res_2.CountryID,
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("1986-01-01"),
                ReceiveNewsLetters = true
            };

            List<PersonResponse> person_res_list = new List<PersonResponse>();
            List<PersonAddRequest> person_req_list = new List<PersonAddRequest>() {
                person_req_1, person_req_2, person_req_3
            };
            foreach (PersonAddRequest req in person_req_list)
            {
                person_res_list.Add(_personService.AddPerson(req));
            }
            //print person 
            _helper.WriteLine("Expected: ");
            foreach (PersonResponse res in person_res_list)
            {
                _helper.WriteLine(res.ToString());
            }


            List<PersonResponse> person_list_from_search = _personService.GetFilteredPerson(nameof(Person.PersonName), "");

            //print actual
            _helper.WriteLine("Actual: ");
            foreach (PersonResponse res in person_list_from_search)
            {
                _helper.WriteLine(res.ToString());
            }

            foreach (PersonResponse res in person_res_list)
            {
                Assert.Contains(res, person_list_from_search);

            }


        }

        [Fact]
        public void GetFilteredPersons_SearchByPersonNamet()
        {
            CountryAddRequest country_req_1 = new CountryAddRequest()
            {
                CountryName = "Canada"

            };
            CountryAddRequest country_req_2 = new CountryAddRequest()
            {
                CountryName = "USA"

            };
            CountryResponse country_res_1 = _countriesService.AddCountry(country_req_1);
            CountryResponse country_res_2 = _countriesService.AddCountry(country_req_2);

            PersonAddRequest person_req_1 = new PersonAddRequest()
            {
                PersonName = "Liang",
                Email = "liangwu@gmail.com",
                Address = "Demo",
                CountryID = country_res_1.CountryID,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = false
            };

            PersonAddRequest person_req_2 = new PersonAddRequest()
            {
                PersonName = "Aaron",
                Email = "Aaron@gmail.com",
                Address = "test",
                CountryID = country_res_2.CountryID,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2007-01-01"),
                ReceiveNewsLetters = false
            };

            PersonAddRequest person_req_3 = new PersonAddRequest()
            {
                PersonName = "Jue",
                Email = "juezhang@gmail.com",
                Address = "test",
                CountryID = country_res_2.CountryID,
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("1986-01-01"),
                ReceiveNewsLetters = true
            };

            List<PersonResponse> person_res_list = new List<PersonResponse>();
            List<PersonAddRequest> person_req_list = new List<PersonAddRequest>() {
                person_req_1, person_req_2, person_req_3
            };
            foreach (PersonAddRequest req in person_req_list)
            {
                person_res_list.Add(_personService.AddPerson(req));
            }
            //print person 
            _helper.WriteLine("Expected: ");
            foreach (PersonResponse res in person_res_list)
            {
                _helper.WriteLine(res.ToString());
            }


            List<PersonResponse> person_list_from_search = _personService.GetFilteredPerson(nameof(Person.PersonName), "li");

            //print actual
            _helper.WriteLine("Actual: ");
            foreach (PersonResponse res in person_list_from_search)
            {
                _helper.WriteLine(res.ToString());
            }

            foreach (PersonResponse res in person_res_list)
            {

                if (res.PersonName != null && res.PersonName.Contains("li", StringComparison.OrdinalIgnoreCase)) 
                {
                    Assert.Contains(res, person_list_from_search);

                }

            }


        }

        #endregion
    }
}
