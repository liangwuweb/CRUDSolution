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
using Microsoft.EntityFrameworkCore;

namespace CRUDTests
{
    public class PersonServiceTest
    {
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _helper;

        public PersonServiceTest(ITestOutputHelper helper) {
            _countriesService = new CountriesServices(new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options));

            _personService = new PersonService(new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options), _countriesService);
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

        #region GetSortedPersons

        [Fact]
        public void GetSortededPerson()
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


            List<PersonResponse> person_list_from_sort = _personService.GetSortedPerson(_personService.GetAllPersons(), nameof(Person.PersonName), SortOrder.DESC);
            person_res_list = person_res_list.OrderByDescending(p => p.PersonName).ToList();

            //print actual
            _helper.WriteLine("Actual: ");
            foreach (PersonResponse res in person_list_from_sort)
            {
                _helper.WriteLine(res.ToString());
            }

            for (int i = 0; i < person_res_list.Count; ++i) {
                Assert.Equal(person_res_list[i], person_list_from_sort[i]);
            }


        }
        #endregion

        #region UpdatePerson

        [Fact]
        public void Update_NullPerson() {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _personService.UpdatePerson(null);
            });
            
        }

        [Fact]
        public void Update_InvalidPersonId()
        {
            PersonUpdateRequest person_update_request = new PersonUpdateRequest() { PersonId = Guid.NewGuid()};

            Assert.Throws<ArgumentException>(() =>
            {
                _personService.UpdatePerson(person_update_request);
            });

        }

        [Fact]
        public void Update_NullPersonName()
        {
            CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "China" };

            CountryResponse country_response = _countriesService.AddCountry(country_add_request);

            PersonAddRequest person_add_req = new PersonAddRequest() {
                PersonName = "Liang",
                CountryID = country_response.CountryID, Email = "liangwu@gmail.com", Gender = GenderOptions.Male

            };
            PersonResponse perosn_res_from_add = _personService.AddPerson(person_add_req);

            PersonUpdateRequest person_update_request = perosn_res_from_add.ToPersonUpdateRequest();
            person_update_request.PersonName = null;

            Assert.Throws<ArgumentException>(() => {
                _personService.UpdatePerson(person_update_request);
            });


                
               

        }

        [Fact]
        public void Update_test03()
        {
            CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "China" };

            CountryResponse country_response = _countriesService.AddCountry(country_add_request);

            PersonAddRequest person_add_req = new PersonAddRequest()
            {
                PersonName = "Liang",
                CountryID = country_response.CountryID,
                Address = "dempo road",
                DateOfBirth = DateTime.MinValue,
                Email = "liangwuweb@gmail.com", Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,

            };
            PersonResponse perosn_res_from_add = _personService.AddPerson(person_add_req);

            PersonUpdateRequest person_update_request = perosn_res_from_add.ToPersonUpdateRequest();
            //person_update_request.PersonName = "Bob";

            PersonResponse person_res_from_update = _personService.UpdatePerson(person_update_request);

            PersonResponse person_res_from_get = _personService.GetPersonByPersonID(person_res_from_update.PersonId);

            Assert.Equal(person_res_from_update, person_res_from_get);


        }

        #endregion

        #region DeletePerson

        [Fact]
        public void DeletePerson_validpersonID() {

            CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "China" };

            CountryResponse country_response = _countriesService.AddCountry(country_add_request);

            PersonAddRequest person_add_req = new PersonAddRequest()
            {
                PersonName = "Liang",
                CountryID = country_response.CountryID,
                Address = "dempo road",
                DateOfBirth = DateTime.MinValue,
                Email = "liangwuweb@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,

            };

            PersonResponse person_add_res = _personService.AddPerson(person_add_req);

            bool isDeleted = _personService.DeletePerson(person_add_res.PersonId);
            Assert.True(isDeleted);


        }

        [Fact]
        public void DeletePerson_INvalidpersonID()
        {
            bool isDeleted = _personService.DeletePerson(Guid.NewGuid());
            Assert.False(isDeleted);
        }

        #endregion
    }
}
