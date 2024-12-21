namespace AI_Kesim.Models
{
    public class Calisan
    {
        public int Id { get; set; }
        public string Isim { get; set; }
        public string Soyisim { get; set; }
        public string UzmanlikAlani { get; set; }
        public int Maas { get; set; }

        // Çalışma saatleriyle ilişki
        public ICollection<CalismaSaati> CalismaSaatleri { get; set; } = new List<CalismaSaati>();
    }

    public class CalismaSaati
    {
        public int Id { get; set; }
        public int CalisanId { get; set; }
        public Calisan Calisan { get; set; }

        public string SaatAraligi { get; set; } // Örneğin: "10:00-11:00"
    }
}
