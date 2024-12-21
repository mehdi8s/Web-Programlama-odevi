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
        public async Task<IActionResult> Index()
        {
            var calisanlar = await _context.Calisan
                .Include(c => c.CalismaSaatleri)
                .ToListAsync();
            return View(calisanlar);
        }

        // GET: Calisans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calisanlar = await _context.Calisan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (calisanlar == null)
            {
                return NotFound();
            }

            return View(calisanlar);
        }

        // GET: Calisans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Calisans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Isim,Soyisim,UzmanlikAlani,Maas")] Calisan calisan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(calisan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(calisan);
        }

        // GET: Calisans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calisan = await _context.Calisan.FindAsync(id);
            if (calisan == null)
            {
                return NotFound();
            }
            return View(calisan);
        }

        // POST: Calisans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Isim,Soyisim,UzmanlikAlani,Maas")] Calisan calisan)
        {
            if (id != calisan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(calisan);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            return View(calisan);
        }

        // GET: Calisans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calisanlar = await _context.Calisan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (calisanlar == null)
            {
                return NotFound();
            }

            return View(calisanlar);
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
