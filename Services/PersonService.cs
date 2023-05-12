using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracrs;
using ServiceContracrs.DTO;
using Services.Helpers;

namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly List<Person> _persons;
        private readonly ICountriesService _countriesService;

        public PersonService() { 
            _persons = new List<Person>();
            _countriesService = new CountriesServices();
        }

        private PersonResponse ConvertPersonToPersonResponse(Person person) {
            PersonResponse res = person.ToPersonResponse();
            res.CountryName = _countriesService.GetCountryByID(person.CountryID)?.CountryName;
            return res;
        }

      

        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            if(personAddRequest == null)
                throw new ArgumentNullException(nameof(personAddRequest));

            // Model validation
            ValidationHelper.ModelValidation(personAddRequest);

            Person person = personAddRequest.ToPerson();

            person.PersonId =   Guid.NewGuid();
            _persons.Add(person);

            return ConvertPersonToPersonResponse(person);


        }

        public List<PersonResponse> GetAllPersons()
        {
            return _persons.Select(p => p.ToPersonResponse()).ToList() as List<PersonResponse>;
        }

        public PersonResponse? GetPersonByPersonID(Guid? id)
        {
            if (id == null) return null;

            return _persons.FirstOrDefault(p => p.PersonId == id)?.ToPersonResponse();
        }

        public List<PersonResponse> GetFilteredPerson(string searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = GetAllPersons();
            List<PersonResponse> matchPersons = allPersons;
            if (string.IsNullOrEmpty(searchBy) && string.IsNullOrEmpty(searchString))
            {
                return allPersons;
            }
                

            switch (searchBy) {
                case (nameof(Person.PersonName)):
                    matchPersons = allPersons.Where(p => (!string.IsNullOrEmpty(p.PersonName) ?
                    p.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case (nameof(Person.Email)):
                    matchPersons = allPersons.Where(p => (!string.IsNullOrEmpty(p.Email) ?
                    p.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case (nameof(Person.DateOfBirth)):
                    matchPersons = allPersons.Where(p => (p.DateOfBirth != null ?
                    p.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case (nameof(Person.Gender)):
                    matchPersons = allPersons.Where(p => (!string.IsNullOrEmpty(p.Gender) ?
                    p.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case (nameof(Person.CountryID)):
                    matchPersons = allPersons.Where(p => (!string.IsNullOrEmpty(p.CountryName) ?
                    p.CountryName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case (nameof(Person.Address)):
                    matchPersons = allPersons.Where(p => (!string.IsNullOrEmpty(p.Address) ?
                    p.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                default: matchPersons = allPersons;
                    break;

            }
            return matchPersons;

        }
    }
}
