using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Configuration;
using TaskManagement1.Data;
using TaskManagement1.Models;
using TaskManagement1.Services;

namespace TaskManagement1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonelController : Controller
    {
        private readonly TaskManagementDbContext _context;
        private readonly IEmailService _emailservice;
        private readonly UserManager<AppUser> _usermanager;
        public PersonelController(TaskManagementDbContext context, IEmailService emailservice, UserManager<AppUser> userManager)
        { 
            _context = context;
            _emailservice = emailservice;
            _usermanager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //var messages = _context.Messagess.ToList();

            var personel = await _usermanager.GetUserAsync(User);
            var personelmail = personel?.Email;
            ViewBag.Email = personelmail; 

            var ilgilimesajlar = _context.Messagess.Where(a => a.ReceiverId == personelmail && a.Status!=3).ToList();
            

            return View(ilgilimesajlar);
        }


        [HttpGet("Create/id")]
        public async Task<IActionResult>Yanıtla(int? id)
        {
            var personel = await _usermanager.GetUserAsync(User);
            var personelmail = personel?.Email;
            ViewBag.Email = personelmail;

            
            var mesaj = await _context.Messagess.FirstOrDefaultAsync(m => m.Id == id);
            



            return View(mesaj);
        }
        // id yi doğru al ve status değiştir
        [HttpPost("send")]
        public async Task<IActionResult> Yanıtla([FromForm]Messages request, int id)
        {
            _context.Messagess.Add(request);
            var mesajlar = await _context.Messagess.FirstOrDefaultAsync(m => m.Id == id);
            mesajlar.Status = 2;
            _context.Messagess.Update(mesajlar);
            await _context.SaveChangesAsync();
            _emailservice.SendEmail(request);
            
            
            return Ok();
        }

        [HttpGet("Personel/details/id")]
        public async Task<IActionResult> Details(int? id)
        {
            var mesajlar = await _context.Messagess.FirstOrDefaultAsync(m => m.Id == id);
            mesajlar.Status = 1;
            _context.Messagess.Update(mesajlar);
            await _context.SaveChangesAsync();

            return View(mesajlar);
        }
        //Personel sadece müşterilerin mesajlarını görsün

        [HttpGet("cozuldu")]
        public async Task<IActionResult> Cozuldu(int? id)
        {
            var mesaj = await _context.Messagess.FirstOrDefaultAsync(m=>m.Id == id);
            mesaj.Status = 3;
            _context.Messagess.Update(mesaj);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }




    }
}
