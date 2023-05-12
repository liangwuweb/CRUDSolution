using ServiceContracrs.DTO;
using System;
using System.Security.Cryptography.X509Certificates;
using ServiceContracrs.Enums;

namespace ServiceContracrs
{
    public interface IPersonService
    {
        PersonResponse AddPerson(PersonAddRequest? personAddRequest);

        List<PersonResponse> GetAllPersons();

        PersonResponse? GetPersonByPersonID(Guid? id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        List<PersonResponse> GetFilteredPerson(string searchBy, string? searchString);

        List<PersonResponse> GetSortedPerson(List<PersonResponse> allPersons, string sortBy, SortOrder sortOrder);

    }
}
