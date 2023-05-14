using Microsoft.AspNetCore.Mvc;
using ServiceContracrs;
using ServiceContracrs.DTO;
using ServiceContracrs.Enums;

namespace CRUDExample.Controllers
{
    [Route("persons")]
    public class PersonsController : Controller
    {
        //private fields
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;

        public PersonsController(IPersonService personService, ICountriesService countriesService)
        {
            _personService = personService;
            _countriesService = countriesService;
        }

        // url /preson/index
        [Route("index")]
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
            List<PersonResponse> sortedPersons = _personService.GetSortedPerson(persons, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder;
            return View(sortedPersons);
        }

        // url /preson/create
        [Route("create")]
        [HttpGet]
        public IActionResult Create() 
        {
            List<CountryResponse> countires = _countriesService.GetAllCountries();
            ViewBag.Countries = countires;
            return View();
        }

        // url /preson/create
        [Route("create")]
        [HttpPost]
        public IActionResult Create(PersonAddRequest personAddRequest)
        {

            if (!ModelState.IsValid) 
            {
                List<CountryResponse> countries = _countriesService.GetAllCountries();
                ViewBag.Countries = countries;
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();

            }

            PersonResponse personResponse = _personService.AddPerson(personAddRequest);
            
            //redirect to Index() action method, and makes another get request to perosns/index
            return RedirectToAction("Index", "Persons");
        }


    }
}
