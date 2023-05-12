using ServiceContracrs.DTO;
using System;
using System.Security.Cryptography.X509Certificates;

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
    }
}
