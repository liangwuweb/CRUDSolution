using ServiceContracrs.DTO;
using System;


namespace ServiceContracrs
{
    public interface IPersonService
    {
        PersonResponse AddPerson(PersonAddRequest? personAddRequest);

        List<PersonResponse> GetAllPersons();

        PersonResponse? GetPersonByPersonID(Guid? id);
    }
}
