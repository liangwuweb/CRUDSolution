using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracrs;
using ServiceContracrs.DTO;
using ServiceContracrs.Enums;
using Services.Helpers;

namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly PersonsDbContext _db;
        private readonly ICountriesService _countriesService;

        public PersonService(PersonsDbContext personsDbContext, ICountriesService countriesService) {
            _db = personsDbContext;
            _countriesService = countriesService;
        }

        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            if(personAddRequest == null)
                throw new ArgumentNullException(nameof(personAddRequest));

            // Model validation
            ValidationHelper.ModelValidation(personAddRequest);

            Person person = personAddRequest.ToPerson();

            person.PersonId =   Guid.NewGuid();
            _db.Persons.Add(person);
            await _db.SaveChangesAsync();
            //_db.sp_InsertPerson(person);

            return person.ToPersonResponse();


        }

        public async Task<List<PersonResponse>> GetAllPersons()
        {

            //return _db.sp_GetAllPersons().Select(p => p.ToPersonResponse()).ToList() as List<PersonResponse>;

            //using entity linq query
            return await _db.Persons.Include("Country").Select(p => p.ToPersonResponse()).ToListAsync() as List<PersonResponse>;
        }

        public async Task<PersonResponse?> GetPersonByPersonID(Guid? id)
        {
            if (id == null) return null;

            Person? person = await _db.Persons.Include("Country").FirstOrDefaultAsync(p => p.PersonId == id);
            if (person == null) return null;
            return person.ToPersonResponse();

        }

        public async Task<List<PersonResponse>> GetFilteredPerson(string searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = await GetAllPersons();
            List<PersonResponse> matchPersons = allPersons;
            if (string.IsNullOrEmpty(searchBy) && string.IsNullOrEmpty(searchString))
            {
                return allPersons;
            }
                

            switch (searchBy) {
                case (nameof(PersonResponse.PersonName)):
                    matchPersons = allPersons.Where(p => (!string.IsNullOrEmpty(p.PersonName) ?
                    p.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case (nameof(PersonResponse.Email)):
                    matchPersons = allPersons.Where(p => (!string.IsNullOrEmpty(p.Email) ?
                    p.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case (nameof(PersonResponse.DateOfBirth)):
                    matchPersons = allPersons.Where(p => (p.DateOfBirth != null ?
                    p.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case (nameof(PersonResponse.Gender)):
                    matchPersons = allPersons.Where(p => (!string.IsNullOrEmpty(p.Gender) ?
                    p.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case (nameof(PersonResponse.CountryID)):
                    matchPersons = allPersons.Where(p => (!string.IsNullOrEmpty(p.CountryName) ?
                    p.CountryName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case (nameof(PersonResponse.Address)):
                    matchPersons = allPersons.Where(p => (!string.IsNullOrEmpty(p.Address) ?
                    p.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                default: matchPersons = allPersons;
                    break;

            }
            return matchPersons;

        }

        public async Task<List<PersonResponse>> GetSortedPerson(List<PersonResponse> allPersons, string sortBy, SortOrder sortOrder)
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

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest personUpdateReq)
        {
            if (personUpdateReq == null) { 
                throw new ArgumentNullException(nameof(personUpdateReq));
            }

            ValidationHelper.ModelValidation(personUpdateReq);

            // get matching person object
            Person? mactch_person = await _db.Persons.FirstOrDefaultAsync(temp => temp.PersonId == personUpdateReq.PersonId);
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

            await _db.SaveChangesAsync();

            return mactch_person.ToPersonResponse();
        }

        public async Task<bool> DeletePerson(Guid? id)
        {
            if(id == null)
                return false;
            Person? person = await _db.Persons.FirstOrDefaultAsync(p => p.PersonId == id);
            if(person == null) return false;
            _db.Persons.Remove(_db.Persons.First(p => p.PersonId == id));
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
