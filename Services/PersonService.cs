using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracrs;
using ServiceContracrs.DTO;
using ServiceContracrs.Enums;
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

        public List<PersonResponse> GetSortedPerson(List<PersonResponse> allPersons, string sortBy, SortOrder sortOrder)
        {
            if(string.IsNullOrEmpty(sortBy))
                return allPersons;

            List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
            {

                (nameof(PersonResponse.PersonName), SortOrder.ASC) => allPersons.OrderBy(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonName), SortOrder.DESC) => allPersons.OrderByDescending(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrder.ASC) => allPersons.OrderBy(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrder.DESC) => allPersons.OrderByDescending(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrder.ASC) => allPersons.OrderBy(p => p.DateOfBirth).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrder.DESC) => allPersons.OrderByDescending(p => p.DateOfBirth).ToList(),

                (nameof(PersonResponse.Age), SortOrder.ASC) => allPersons.OrderBy(p => p.Age).ToList(),

                (nameof(PersonResponse.Age), SortOrder.DESC) => allPersons.OrderByDescending(p => p.Age).ToList(),

                (nameof(PersonResponse.Gender), SortOrder.ASC) => allPersons.OrderBy(p => p.Gender).ToList(),

                (nameof(PersonResponse.Gender), SortOrder.DESC) => allPersons.OrderByDescending(p => p.Gender).ToList(),

                (nameof(PersonResponse.CountryName), SortOrder.ASC) => allPersons.OrderBy(p => p.CountryName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.CountryName), SortOrder.DESC) => allPersons.OrderByDescending(p => p.CountryName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrder.ASC) => allPersons.OrderBy(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrder.DESC) => allPersons.OrderByDescending(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrder.ASC) => allPersons.OrderBy(p => p.ReceiveNewsLetters).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrder.DESC) => allPersons.OrderByDescending(p => p.ReceiveNewsLetters).ToList(),

                _ => allPersons
            };
            return sortedPersons;
        }

        public PersonResponse UpdatePerson(PersonUpdateRequest personUpdateReq)
        {
            if (personUpdateReq == null) { 
                throw new ArgumentNullException(nameof(personUpdateReq));
            }

            ValidationHelper.ModelValidation(personUpdateReq);

            // get matching person object
            Person? mactch_person = _persons.FirstOrDefault(temp => temp.PersonId == personUpdateReq.PersonId);
            if (mactch_person == null) {
                throw new ArgumentException("person does not exist");

            }

            //update all details
            mactch_person.PersonName = personUpdateReq.PersonName;
            mactch_person.Email = personUpdateReq.Email;
            mactch_person.DateOfBirth = personUpdateReq.DateOfBirth;
            mactch_person.Gender = personUpdateReq.Gender.ToString();
            mactch_person.Address = personUpdateReq.Address;
            mactch_person.ReceiveNewsLetters = personUpdateReq.ReceiveNewsLetters;
            mactch_person.CountryID = personUpdateReq.CountryID;

            return mactch_person.ToPersonResponse();
        }

        public bool DeletePerson(Guid? id)
        {
            if(id == null)
                return false;
            Person? person = _persons.FirstOrDefault(p => p.PersonId == id);
            if(person == null) return false;
            _persons.Remove(person);
            return true;
        }
    }
}
