﻿using AI_Kesim.Data;
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
            try
            {
                model.UserId = _userManager.GetUserId(User);
                model.Uzmanlik = await _context.Uzmanliklar.FindAsync(model.UzmanlikId);
                model.Calisan = await _context.Calisan.FindAsync(model.CalisanId);
                model.User = await _userManager.FindByIdAsync(model.UserId);

                if (model.Uzmanlik == null || model.Calisan == null || model.User == null)
                {
                    ModelState.AddModelError("", "Geçersiz veri");
                    ViewBag.Uzmanliklar = await _context.Uzmanliklar.ToListAsync();
                    return View(model);
                }

                _context.Randevular.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Randevu kaydedilirken bir hata oluştu.");
                ViewBag.Uzmanliklar = await _context.Uzmanliklar.ToListAsync();
                return View(model);
            }
        }

        [HttpGet("/Randevu/GetCalisanlar/{uzmanlikId}")]
        public async Task<IActionResult> GetCalisanlar(int uzmanlikId)
        {
            if (uzmanlikId == 0)
            {
                //return BadRequest("Uzmanlık ID sıfır olarak geldi!");
            }

            var calisanlar = await _context.CalisanUzmanliklari
                .Where(cu => cu.UzmanlikId == uzmanlikId)
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
                .Select(hour => new { Hour = hour, Time = $"{hour}:00" })
                .ToList();
            return Json(availableTimes);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var randevu = await _context.Randevular.FindAsync(id);
            if (randevu == null) return NotFound();

            _context.Randevular.Remove(randevu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
