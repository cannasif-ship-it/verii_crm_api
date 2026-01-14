using System.ComponentModel.DataAnnotations;

namespace depoWebAPI.Models
{
    public class RII_FN_2SHIPPING
    {
        [StringLength(100)]
        public string? CARI_KOD { get; set; }

        [StringLength(100)]
        public string? CARI_ISIM { get; set; }

        [StringLength(100)]
        public string? CARI_ADRES { get; set; }

        [StringLength(100)]
        public string? CARI_IL { get; set; }

        [StringLength(100)]
        public string? CARI_ILCE { get; set; }
    }
}
