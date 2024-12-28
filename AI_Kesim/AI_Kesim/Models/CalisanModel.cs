using System;
using System.ComponentModel.DataAnnotations;

namespace AI_Kesim.Models
{
    public class Calisan
    {
        public int Id { get; set; }
        public string Isim { get; set; }
        public string Soyisim { get; set; }
        public int Maas { get; set; }

        // Uzmanlık alanlarıyla ilişki
        public ICollection<CalisanUzmanlik> CalisanUzmanliklari { get; set; } = new List<CalisanUzmanlik>();
    }

    public class Uzmanlik
    {
        public int Id { get; set; }
        public string Ad { get; set; }

        [Required(ErrorMessage = "Ücret alanı zorunludur.")]
        public int Ucret { get; set; }

        // Uzmanlıkla ilişkilendirilen çalışanlar
        public ICollection<CalisanUzmanlik> CalisanUzmanliklari { get; set; } = new List<CalisanUzmanlik>();
    }

    public class CalisanUzmanlik
    {
        public int Id { get; set; }
        public int CalisanId { get; set; }
        public Calisan Calisan { get; set; }

        public int UzmanlikId { get; set; }
        public Uzmanlik Uzmanlik { get; set; }
    }
}