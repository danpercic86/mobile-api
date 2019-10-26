using Microsoft.AspNetCore.Mvc;

namespace itec_mobile_api_final.Controllers
{
    public class ExampleController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return Ok("Muie");
        }
    }
}