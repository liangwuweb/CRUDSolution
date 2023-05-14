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

        public PersonService(bool ini = true) { 
            _persons = new List<Person>();
            _countriesService = new CountriesServices();
            if (ini) {
                _persons.Add(new Person() { PersonId = Guid.Parse("8082ED0C-396D-4162-AD1D-29A13F929824"), PersonName = "Aguste", Email = "aleddy0@booking.com", DateOfBirth = DateTime.Parse("1993-01-02"), Gender = "Male", Address = "0858 Novick Terrace", ReceiveNewsLetters = false, CountryID = Guid.Parse("000C76EB-62E9-4465-96D1-2C41FDB64C3B") });

                _persons.Add(new Person() { PersonId = Guid.Parse("06D15BAD-52F4-498E-B478-ACAD847ABFAA"), PersonName = "Jasmina", Email = "jsyddie1@miibeian.gov.cn", DateOfBirth = DateTime.Parse("1991-06-24"), Gender = "Female", Address = "0742 Fieldstone Lane", ReceiveNewsLetters = true, CountryID = Guid.Parse("32DA506B-3EBA-48A4-BD86-5F93A2E19E3F") });

                _persons.Add(new Person() { PersonId = Guid.Parse("D3EA677A-0F5B-41EA-8FEF-EA2FC41900FD"), PersonName = "Kendall", Email = "khaquard2@arstechnica.com", DateOfBirth = DateTime.Parse("1993-08-13"), Gender = "Male", Address = "7050 Pawling Alley", ReceiveNewsLetters = false, CountryID = Guid.Parse("32DA506B-3EBA-48A4-BD86-5F93A2E19E3F") });

                _persons.Add(new Person() { PersonId = Guid.Parse("89452EDB-BF8C-4283-9BA4-8259FD4A7A76"), PersonName = "Kilian", Email = "kaizikowitz3@joomla.org", DateOfBirth = DateTime.Parse("1991-06-17"), Gender = "Male", Address = "233 Buhler Junction", ReceiveNewsLetters = true, CountryID = Guid.Parse("DF7C89CE-3341-4246-84AE-E01AB7BA476E") });

                _persons.Add(new Person() { PersonId = Guid.Parse("F5BD5979-1DC1-432C-B1F1-DB5BCCB0E56D"), PersonName = "Dulcinea", Email = "dbus4@pbs.org", DateOfBirth = DateTime.Parse("1996-09-02"), Gender = "Female", Address = "56 Sundown Point", ReceiveNewsLetters = false, CountryID = Guid.Parse("DF7C89CE-3341-4246-84AE-E01AB7BA476E") });

                _persons.Add(new Person() { PersonId = Guid.Parse("A795E22D-FAED-42F0-B134-F3B89B8683E5"), PersonName = "Corabelle", Email = "cadams5@t-online.de", DateOfBirth = DateTime.Parse("1993-10-23"), Gender = "Female", Address = "4489 Hazelcrest Place", ReceiveNewsLetters = false, CountryID = Guid.Parse("15889048-AF93-412C-B8F3-22103E943A6D") });

                _persons.Add(new Person() { PersonId = Guid.Parse("3C12D8E8-3C1C-4F57-B6A4-C8CAAC893D7A"), PersonName = "Faydra", Email = "fbischof6@boston.com", DateOfBirth = DateTime.Parse("1996-02-14"), Gender = "Female", Address = "2010 Farragut Pass", ReceiveNewsLetters = true, CountryID = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB") });

                _persons.Add(new Person() { PersonId = Guid.Parse("7B75097B-BFF2-459F-8EA8-63742BBD7AFB"), PersonName = "Oby", Email = "oclutheram7@foxnews.com", DateOfBirth = DateTime.Parse("1992-05-31"), Gender = "Male", Address = "2 Fallview Plaza", ReceiveNewsLetters = false, CountryID = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB") });

                _persons.Add(new Person() { PersonId = Guid.Parse("6717C42D-16EC-4F15-80D8-4C7413E250CB"), PersonName = "Seumas", Email = "ssimonitto8@biglobe.ne.jp", DateOfBirth = DateTime.Parse("1999-02-02"), Gender = "Male", Address = "76779 Norway Maple Crossing", ReceiveNewsLetters = false, CountryID = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB") });

                _persons.Add(new Person() { PersonId = Guid.Parse("6E789C86-C8A6-4F18-821C-2ABDB2E95982"), PersonName = "Freemon", Email = "faugustin9@vimeo.com", DateOfBirth = DateTime.Parse("1996-04-27"), Gender = "Male", Address = "8754 Becker Street", ReceiveNewsLetters = false, CountryID = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB") });

            }
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
