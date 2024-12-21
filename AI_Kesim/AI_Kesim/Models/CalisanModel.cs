namespace AI_Kesim.Models
{
    public class Calisan
    {
        public int Id { get; set; }
        public string Isim { get; set; }
        public string Soyisim { get; set; }
        public int Maas { get; set; }

        // Çalışma saatleriyle ilişki
        public ICollection<CalismaSaati> CalismaSaatleri { get; set; } = new List<CalismaSaati>();

        // Uzmanlık alanlarıyla ilişki
        public ICollection<CalisanUzmanlik> CalisanUzmanliklari { get; set; } = new List<CalisanUzmanlik>();
    }

    public class CalismaSaati
    {
        public int Id { get; set; }
        public int CalisanId { get; set; }
        public Calisan Calisan { get; set; }

        public string SaatAraligi { get; set; } // Örneğin: "10:00-11:00"
    }

    public class Uzmanlik
    {
        public int Id { get; set; }
        public string Ad { get; set; }

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