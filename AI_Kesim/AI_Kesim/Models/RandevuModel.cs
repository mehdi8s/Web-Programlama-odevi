namespace AI_Kesim.Models
{
    public class Randevu
    {
        public int Id { get; set; }

        // Randevu yapan kullanıcı
        public string UserId { get; set; }
        public UserDetails User { get; set; }

        // Hizmet (Uzmanlık)
        public int UzmanlikId { get; set; }
        public Uzmanlik Uzmanlik { get; set; }

        // Çalışan bilgisi
        public int CalisanId { get; set; }
        public Calisan Calisan { get; set; }

        // Randevu tarihi ve saati
        public DateTime RandevuTarihi { get; set; }
    }
}
