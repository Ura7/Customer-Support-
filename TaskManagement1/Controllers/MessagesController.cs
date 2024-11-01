using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TaskManagement1.Data;
using TaskManagement1.Models;
using TaskManagement1.Services;

namespace TaskManagement1.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class MessagesController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly TaskManagementDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public MessagesController(IEmailService emailService, TaskManagementDbContext context, UserManager<AppUser> userManager) 
        {
            _emailService = emailService; 
            _context = context;
            _userManager = userManager;
        }


        //[HttpGet]
        //public IActionResult SendMail() 
        //{
        //    return ViewColumn();
        //}



        //Müşteri mesajlarını görsün



        [Authorize(Roles ="Admin,Müşteri")]

        [HttpGet("create")]
        public async Task<IActionResult> SendMail() 
        {
            var user = await _userManager.GetUserAsync(User);
            var usermail = user?.Email;
            ViewBag.usermail = usermail;

            var personel = _context.Users.Where(a=>a.Role == "Personel").Select(a=>a.Email).ToList();
            ViewBag.personel = new SelectList(personel, "Role");
            return View();
        }
        //Mail gönderdikten sonra homepage'e dönüş.
                                                           
        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm]Messages request)
        {
            _context.Messagess.Add(request);
            await _context.SaveChangesAsync();
            
            _emailService.SendEmail(request);
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public async Task<IActionResult> MyMessages()
        {
            return View();
        }
    }
}
