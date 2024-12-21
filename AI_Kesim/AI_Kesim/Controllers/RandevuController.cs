//using AI_Kesim.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace AI_Kesim.Controllers
//{
//    public class RandevuController : Controller
//    {
//        // GET: Randevu
//        public IActionResult Index()
//        {
//            var model = new RandevuModel
//            {
//                Hizmet = "Sakal Tıraşı",
//                Personel = "Kazım Tok",
//                Tarih = DateTime.Now,
//                Saat = new TimeSpan(20, 0, 0)
//            };
//            return View(model);
//        }

//        [HttpPost]
//        public IActionResult Kaydet(RandevuModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                // Burada veritabanına kaydetme işlemi yapılabilir.
//                ViewBag.Message = "Randevunuz başarıyla kaydedildi.";
//                return View("Index", model);
//            }

//            // Hatalı giriş durumunda tekrar sayfayı göster.
//            return View("Index", model);
//        }
//    }
//}
