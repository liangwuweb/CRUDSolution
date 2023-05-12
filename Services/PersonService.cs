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
    }
}
