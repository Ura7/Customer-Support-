using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskManagement1.Data;
using TaskManagement1.Models;

namespace TaskManagement1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly TaskManagementDbContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, TaskManagementDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var usermail = user?.Email;
            if(user?.Role=="Personel")
            {
                var messages = _context.Messagess.Where(a => a.ReceiverId == usermail && a.Status == 0).ToList();
                var messagecount = messages.Count();
                ViewData["MessageCount"] = messagecount;

                var solvedMessages = _context.Messagess.Where(a => a.ReceiverId == usermail && a.Status == 3).ToList();
                var solvedcount = solvedMessages.Count();
                ViewData["SolvedCount"] = solvedcount;
            }
            else if(user?.Role=="Müşteri")
            {
                var messages = _context.Messagess.Where(a => a.ReceiverId == usermail && a.Status == 0).ToList();
                var messagecount = messages.Count();
                ViewData["MessageCount"] = messagecount;
            }
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}