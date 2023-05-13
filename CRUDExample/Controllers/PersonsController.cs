using Microsoft.AspNetCore.Mvc;
using ServiceContracrs;
using ServiceContracrs.DTO;

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
        public IActionResult Index()
        {
            List<PersonResponse> persons = _personService.GetAllPersons();
            return View(persons);
        }
    }
}
