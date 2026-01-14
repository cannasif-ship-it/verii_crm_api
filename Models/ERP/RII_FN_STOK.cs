using System.ComponentModel.DataAnnotations;

namespace depoWebAPI.Models
{
    public class RII_FN_STOK
    {
        public short SUBE_KODU { get; set; }

        public short ISLETME_KODU { get; set; }

        [MaxLength(35)]
        public string STOK_KODU { get; set; } = string.Empty;

        [MaxLength(25)]
        public string? OLCU_BR1 { get; set; }

        [MaxLength(35)]
        public string? URETICI_KODU { get; set; }

        [MaxLength(200)]
        public string? STOK_ADI { get; set; }

        [MaxLength(8)]
        public string? GRUP_KODU { get; set; }

        [MaxLength(30)]
        public string? GRUP_ISIM { get; set; }

        [MaxLength(8)]
        public string? KOD_1 { get; set; }

        [MaxLength(30)]
        public string? KOD1_ADI { get; set; }

        [MaxLength(8)]
        public string? KOD_2 { get; set; }

        [MaxLength(30)]
        public string? KOD2_ADI { get; set; }

        [MaxLength(8)]
        public string? KOD_3 { get; set; }

        [MaxLength(30)]
        public string? KOD3_ADI { get; set; }

        [MaxLength(8)]
        public string? KOD_4 { get; set; }

        [MaxLength(30)]
        public string? KOD4_ADI { get; set; }

        [MaxLength(8)]
        public string? KOD_5 { get; set; }

        [MaxLength(30)]
        public string? KOD5_ADI { get; set; }

        [MaxLength(100)]
        public string? INGISIM { get; set; }
    }
}
