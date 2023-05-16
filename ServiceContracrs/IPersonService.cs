using ServiceContracrs.DTO;
using System;
using System.Security.Cryptography.X509Certificates;
using ServiceContracrs.Enums;

namespace ServiceContracrs
{
    public interface IPersonService
    {
        Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);

        Task<List<PersonResponse>> GetAllPersons();

        Task<PersonResponse?> GetPersonByPersonID(Guid? id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        Task<List<PersonResponse>> GetFilteredPerson(string searchBy, string? searchString);

        Task<List<PersonResponse>> GetSortedPerson(List<PersonResponse> allPersons, string sortBy, SortOrder sortOrder);

        Task<PersonResponse> UpdatePerson(PersonUpdateRequest personUpdateReq);

        Task<bool> DeletePerson(Guid? id);

    }
}
