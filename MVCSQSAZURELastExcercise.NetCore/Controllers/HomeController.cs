using Microsoft.AspNetCore.Mvc;
using ProduceMessageServices.NetCore.Models;
using SenderQueueMessageServices.NetCore.Services;

namespace MVCSQSAZURELastExcercise.NetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISenders _Senders;

        public HomeController(
            ISenders Senders)
        {

            _Senders = Senders;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SendMessage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessageAsync(SendQueueModel message)
        {
            await _Senders.SendAsync(message);
            return RedirectToAction("SendMessage");
        }
    }
}