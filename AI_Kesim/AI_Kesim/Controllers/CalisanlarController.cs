using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AI_Kesim.Data;
using AI_Kesim.Models;

namespace AI_Kesim.Controllers
{
    public class CalisanlarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CalisanlarController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Calisans
        public IActionResult Index(DateTime? tarih)
        {
            // Seçili tarihi kontrol et, parametre yoksa bugünü kullan
            var seciliTarih = tarih ?? DateTime.Today;

            // Çalışan verilerini alın
            var calisanlar = _context.Calisan
                .Include(c => c.CalisanUzmanliklari)
                    .ThenInclude(cu => cu.Uzmanlik)
                .ToList();

            // Randevuları alın
            var randevular = _context.Randevular
                .Where(r => r.RandevuTarihi.Date == seciliTarih.Date)
                .ToList();

            // ViewData'ya gerekli verileri ekle
            ViewData["SeciliTarih"] = seciliTarih;
            ViewData["Randevular"] = randevular;

            return View(calisanlar);
        }


        // GET: Calisanlar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calisan = await _context.Calisan
                .Include(c => c.CalisanUzmanliklari)
                    .ThenInclude(cu => cu.Uzmanlik)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (calisan == null)
            {
                return NotFound();
            }

            return View(calisan);
        }

        // GET: Calisans/Create
        public IActionResult Create()
        {
            // Tüm uzmanlıkları getir ve ViewData ile View'e gönder
            var uzmanliklar = _context.Uzmanliklar
                .Select(u => new { u.Id, u.Ad })
                .ToList();

            ViewData["Uzmanliklar"] = uzmanliklar; // List olarak gönderiyoruz
            return View();
        }

        // POST: Calisans/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Calisan calisan, int[] UzmanlikIds)
        {
            if (ModelState.IsValid)
            {
                // Çalışanı kaydet
                _context.Calisan.Add(calisan);
                _context.SaveChanges();

                // Uzmanlıkları ilişkilendir ve kaydet
                foreach (var uzmanlikId in UzmanlikIds)
                {
                    var calisanUzmanlik = new CalisanUzmanlik
                    {
                        CalisanId = calisan.Id,
                        UzmanlikId = uzmanlikId
                    };
                    _context.CalisanUzmanliklari.Add(calisanUzmanlik);
                }

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            // Eğer model geçersizse uzmanlık listesini tekrar doldur
            var uzmanliklar = _context.Uzmanliklar
                .Select(u => new { u.Id, u.Ad })
                .ToList();

            ViewData["Uzmanliklar"] = uzmanliklar;
            return View(calisan);
        }

        // GET: Calisans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calisan = await _context.Calisan
                .Include(c => c.CalisanUzmanliklari)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (calisan == null)
            {
                return NotFound();
            }

            // Tüm uzmanlıkları getir
            var uzmanliklar = await _context.Uzmanliklar
                .Select(u => new { u.Id, u.Ad })
                .ToListAsync();

            // Çalışanın mevcut uzmanlık ID'lerini al
            var secilenUzmanliklar = calisan.CalisanUzmanliklari
                .Select(cu => cu.UzmanlikId)
                .ToList();

            ViewData["Uzmanliklar"] = uzmanliklar;
            ViewData["SecilenUzmanliklar"] = secilenUzmanliklar;

            return View(calisan);
        }

        // POST: Calisans/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Calisan calisan, int[] UzmanlikIds)
        {
            if (id != calisan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Mevcut çalışanı context'e bağla
                    _context.Entry(calisan).State = EntityState.Modified;

                    // Mevcut uzmanlık ilişkilerini sil
                    var mevcutUzmanliklar = await _context.CalisanUzmanliklari
                        .Where(cu => cu.CalisanId == calisan.Id)
                        .ToListAsync();
                    _context.CalisanUzmanliklari.RemoveRange(mevcutUzmanliklar);

                    // Yeni uzmanlık ilişkilerini ekle
                    if (UzmanlikIds != null)
                    {
                        foreach (var uzmanlikId in UzmanlikIds)
                        {
                            var calisanUzmanlik = new CalisanUzmanlik
                            {
                                CalisanId = calisan.Id,
                                UzmanlikId = uzmanlikId
                            };
                            _context.CalisanUzmanliklari.Add(calisanUzmanlik);
                        }
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CalisanExists(calisan.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Hata durumunda uzmanlıkları tekrar yükle
            var uzmanliklar = await _context.Uzmanliklar
                .Select(u => new { u.Id, u.Ad })
                .ToListAsync();
            ViewData["Uzmanliklar"] = uzmanliklar;

            return View(calisan);
        }

        // GET: Calisanlar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calisan = await _context.Calisan
                .Include(c => c.CalisanUzmanliklari)
                    .ThenInclude(cu => cu.Uzmanlik)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (calisan == null)
            {
                return NotFound();
            }

            return View(calisan);
        }

        // POST: Calisans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var calisanlar = await _context.Calisan.FindAsync(id);
            if (calisanlar != null)
            {
                _context.Calisan.Remove(calisanlar);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CalisanExists(int id)
        {
            return _context.Calisan.Any(e => e.Id == id);
        }
    }
}
