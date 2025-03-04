using Microsoft.AspNetCore.Mvc;

namespace StreamPost.Controllers
{
    public class MessagesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ConfirmationLink()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }
    }
}
