using Microsoft.AspNetCore.Mvc;

namespace itec_mobile_api_final.Sockets
{
    public class SocketsController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}