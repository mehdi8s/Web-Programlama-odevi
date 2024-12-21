//using AI_Kesim.Data;
//using AI_Kesim.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;
//using System.Threading.Tasks;

//namespace AI_Kesim.Controllers
//{
//    public class AdminController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public AdminController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        // Admin panelindeki çalışanlar listesi
//        public async Task<IActionResult> Index()
//        {
//            var calisanlar = await _context.Calisanlar
//                .Include(c => c.CalismaSaatleri)
//                .ToListAsync();
//            return View(calisanlar);
//        }

//        // Yeni çalışan ekleme sayfası
//        public IActionResult CreateCalisan()
//        {
//            return View();
//        }

//        // Yeni çalışan ekleme işlemi
//        [HttpPost]
//        public async Task<IActionResult> CreateCalisan(Calisan calisan, string[] saatAraliklari)
//        {
//            if (ModelState.IsValid)
//            {
//                foreach (var saat in saatAraliklari)
//                {
//                    calisan.CalismaSaatleri.Add(new CalismaSaati { SaatAraligi = saat });
//                }

//                _context.Calisanlar.Add(calisan);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(calisan);
//        }

//        // Çalışan silme işlemi
//        public async Task<IActionResult> DeleteCalisan(int id)
//        {
//            var calisan = await _context.Calisanlar
//                .Include(c => c.CalismaSaatleri)
//                .FirstOrDefaultAsync(c => c.Id == id);

//            if (calisan == null)
//            {
//                return NotFound();
//            }

//            _context.Calisanlar.Remove(calisan);
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }
//    }
//}
