using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

            ViewBag.CurrentSearchBy = searchBy;
            List<PersonResponse> persons = _personService.GetFilteredPerson(searchBy, searchString);
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
            ViewBag.Countries = countires.Select(temp => new SelectListItem() { 
                Text = temp.CountryName,
                Value = temp.CountryID.ToString(),
            });

           
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


        [Route("[action]/{PersonId}")]
        [HttpGet]
        public IActionResult Edit(Guid personID)
        {
            PersonResponse? personResponse = _personService.GetPersonByPersonID(personID);
            if (personResponse == null) {
                return RedirectToAction("Index");
            
            }
            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
            List<CountryResponse> countires = _countriesService.GetAllCountries();
            ViewBag.Countries = countires.Select(temp => new SelectListItem()
            {
                Text = temp.CountryName,
                Value = temp.CountryID.ToString(),
            });
            return View(personUpdateRequest);
        }

        [Route("[action]/{PersonId}")]
        [HttpPost]
        public IActionResult Edit(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? personResponse = _personService.GetPersonByPersonID(personUpdateRequest.PersonId);
            if (personResponse == null) {
                return RedirectToAction("Index");
            
            }
            if (ModelState.IsValid)
            {
                PersonResponse updatedPerson = _personService.UpdatePerson(personUpdateRequest);
                return RedirectToAction("Index");
            }
            
            List<CountryResponse> countries = _countriesService.GetAllCountries();
            ViewBag.Countries = countries;
            ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return View();
        }

        [Route("[action]/{PersonId}")]
        [HttpGet]
        public IActionResult Delete(Guid personID)
        {
            PersonResponse? personResponse = _personService.GetPersonByPersonID(personID);
            if (personResponse == null)
            {
                return RedirectToAction("Index");

            }
       
            List<CountryResponse> countires = _countriesService.GetAllCountries();
            ViewBag.Countries = countires.Select(temp => new SelectListItem()
            {
                Text = temp.CountryName,
                Value = temp.CountryID.ToString(),
            });
            return View(personResponse);
        }

        [Route("[action]/{PersonId}")]
        [HttpPost]
        public IActionResult Delete(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? personResponse = _personService.GetPersonByPersonID(personUpdateRequest.PersonId);
            if (personResponse == null) return RedirectToAction("Index"); 

            bool deleted = _personService.DeletePerson(personUpdateRequest.PersonId);
       
            return RedirectToAction("index");
            
        }


    }
}
