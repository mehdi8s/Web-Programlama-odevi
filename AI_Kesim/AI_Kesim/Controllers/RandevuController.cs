using AI_Kesim.Data;
using AI_Kesim.Data.Migrations;
using AI_Kesim.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AI_Kesim.Controllers
{
    [Authorize]
    public class RandevuController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserDetails> _userManager;

        public RandevuController(ApplicationDbContext context, UserManager<UserDetails> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var userId = _userManager.GetUserId(User);
            var randevular = await _context.Randevular
                .Include(r => r.Uzmanlik)
                .Include(r => r.Calisan)
                .Where(r => r.UserId == userId)
                .ToListAsync();

            return View(randevular);
        }

        public async Task<IActionResult> Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            ViewBag.Uzmanliklar = await _context.Uzmanliklar.ToListAsync() ?? new List<Uzmanlik>();
            return View(new Randevu());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Randevu model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Uzmanliklar = await _context.Uzmanliklar.ToListAsync();
                return View(model);
            }

            model.UserId = _userManager.GetUserId(User);
            _context.Randevular.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetCalisanlar(int uzmanlikId)
        {
            //Console.WriteLine("kaccccc:" + uzmanlikId);
            //var calisanlar = await _context.CalisanUzmanliklari
            //    .Include(cu => cu.Calisan)
            //    .Select(cu => new
            //    {
            //        Id = cu.Calisan.Id,
            //        Isim = cu.Calisan.Isim,
            //        Soyisim = cu.Calisan.Soyisim
            //    })
            //    .ToListAsync();

            //return Json(calisanlar);



            //var calisanlar = await _context.CalisanUzmanliklari
            //    .Include(cu => cu.Calisan)
            //    .Select(cu => new
            //    {
            //        Id = cu.Calisan.Id,
            //        Isim = cu.Calisan.Isim,
            //        Soyisim = cu.Calisan.Soyisim
            //    })
            //    .ToListAsync();

            //return Json(calisanlar);


           int  ttttt = 1;
            Console.WriteLine("kaccccc:" + ttttt);
            var calisanlar = await _context.CalisanUzmanliklari
                .Where(cu => cu.UzmanlikId == 1)
                .Include(cu => cu.Calisan)
                .Select(cu => new
                {
                    Id = cu.Calisan.Id,
                    Isim = cu.Calisan.Isim,
                    Soyisim = cu.Calisan.Soyisim
                })
                .ToListAsync();
            return Json(calisanlar);



        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableTimes(int calisanId, DateTime tarih)
        {
            var existingRandevular = await _context.Randevular
                .Where(r => r.CalisanId == calisanId && r.RandevuTarihi.Date == tarih.Date)
                .Select(r => r.RandevuTarihi.Hour)
                .ToListAsync();

            var availableTimes = Enumerable.Range(12, 7)
                .Except(existingRandevular)
                .Select(hour => new { Hour = hour, Time = $"{hour}:00" });

            return Json(availableTimes);
        }
    }
}
