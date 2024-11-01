using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement1.Data;
using TaskManagement1.Models;
using TaskManagement1.Services;

namespace TaskManagement1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MusteriController : Controller
	{
		private readonly TaskManagementDbContext _context;
		private readonly IEmailService _emailservice;
		private readonly UserManager<AppUser> _usermanager;
		public MusteriController(TaskManagementDbContext context, IEmailService emailservice, UserManager<AppUser> userManager)
		{
			_context = context;
			_emailservice = emailservice;
			_usermanager = userManager;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			//var messages = _context.Messagess.ToList();

			var musteri = await _usermanager.GetUserAsync(User);
			var musterimail = musteri?.Email;
			ViewBag.Email = musterimail;

			var ilgilimesajlar = _context.Messagess.Where(a => a.ReceiverId == musterimail || a.SenderId==musterimail).ToList();

			return View(ilgilimesajlar);
		}

        [HttpGet("Musteri/details/id")]
        public async Task<IActionResult> Details(int? id)
        {
			var mesajlar = await _context.Messagess.FirstOrDefaultAsync(m => m.Id == id);
            mesajlar.Status = 1;
            _context.Messagess.Update(mesajlar);
            await _context.SaveChangesAsync();
            return View(mesajlar);
        }

		[HttpGet("Create/id")]
		public async Task<IActionResult> Yanıtla(int? id)
		{
			var musteri = await _usermanager.GetUserAsync(User);
			var musterimail = musteri?.Email;
			ViewBag.Email = musterimail;


			var mesaj = await _context.Messagess.FirstOrDefaultAsync(m => m.Id == id);




			return View(mesaj);
		}

		[HttpPost("send")]

		public async Task<IActionResult> Yanıtla([FromForm] Messages request)
		{
			_context.Messagess.Add(request);
			await _context.SaveChangesAsync();

			_emailservice.SendEmail(request);
			return Ok();
		}

	}
}
