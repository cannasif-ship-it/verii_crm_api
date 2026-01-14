using System.ComponentModel.DataAnnotations;

namespace depoWebAPI.Models
{
    public class RII_STGROUP
    {
        public short ISLETME_KODU { get; set; }   // smallint

        public short SUBE_KODU { get; set; }      // smallint

        [StringLength(8)]
        public string? GRUP_KOD { get; set; }
        [StringLength(30)]
        public string? GRUP_ISIM { get; set; }

    }
}
