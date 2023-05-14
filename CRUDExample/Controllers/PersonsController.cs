using Microsoft.AspNetCore.Mvc;
using ServiceContracrs;
using ServiceContracrs.DTO;
using ServiceContracrs.Enums;

namespace CRUDExample.Controllers
{
    public class PersonsController : Controller
    {
        //private fields
        private readonly IPersonService _personService;

        public PersonsController(IPersonService personService)
        {
            _personService = personService;
        }

        [Route("persons/index")]
        [Route("/")]
        public IActionResult Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrder sortOrder = SortOrder.ASC)
        {
            // Searching
            ViewBag.SearchFields = new Dictionary<string, string>() {
                { nameof(PersonResponse.PersonName), "Person Name"},
                { nameof(PersonResponse.Email), "Email"},
                { nameof(PersonResponse.DateOfBirth ), "Date of Birth"},
                { nameof(PersonResponse.Gender), "Gender"},
                { nameof(PersonResponse.CountryID), "Country"},
                { nameof(PersonResponse.Address), "Address"}
            };

            List<PersonResponse> persons = _personService.GetFilteredPerson(searchBy, searchString);
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            //Sorting
            List<PersonResponse>  sortedPersons = _personService.GetSortedPerson(persons, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder;
            return View(sortedPersons);
        }
    }
}
