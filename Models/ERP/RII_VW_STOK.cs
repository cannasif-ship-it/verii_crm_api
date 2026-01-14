using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace depoWebAPI.Models
{
    public class RII_VW_STOK
    {
        public short? SUBE_KODU { get; set; }
        
        public short? ISLETME_KODU { get; set; }
        
        [MaxLength(25)]
        public string? STOK_KODU { get; set; }
        
        [MaxLength(25)]
        public string? URETICI_KODU { get; set; }
        
        [MaxLength(50)]
        public string? STOK_ADI { get; set; }
        
        [MaxLength(10)]
        public string? GRUP_KODU { get; set; }
        
        [MaxLength(25)]
        public string? KOD_1 { get; set; }
        
        [MaxLength(25)]
        public string? KOD_2 { get; set; }
        
        [MaxLength(25)]
        public string? KOD_3 { get; set; }
        
        [MaxLength(25)]
        public string? KOD_4 { get; set; }
        
        [MaxLength(25)]
        public string? KOD_5 { get; set; }
        
        [MaxLength(10)]
        public string? SATICI_KODU { get; set; }
        
        [MaxLength(10)]
        public string? OLCU_BR1 { get; set; }
        
        [MaxLength(10)]
        public string? OLCU_BR2 { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? PAY_1 { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? PAYDA_1 { get; set; }
        
        [MaxLength(10)]
        public string? OLCU_BR3 { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? PAY2 { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? PAYDA2 { get; set; }
        
        public char? FIAT_BIRIMI { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? AZAMI_STOK { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? ASGARI_STOK { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? TEMIN_SURESI { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? KUL_MIK { get; set; }
        
        public short? RISK_SURESI { get; set; }
        
        [MaxLength(10)]
        public string? ZAMAN_BIRIMI { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? SATIS_FIAT1 { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? SATIS_FIAT2 { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? SATIS_FIAT3 { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? SATIS_FIAT4 { get; set; }
        
        public byte? SAT_DOV_TIP { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? DOV_ALIS_FIAT { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? DOV_MAL_FIAT { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? DOV_SATIS_FIAT { get; set; }
        
        public int? MUH_DETAYKODU { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? BIRIM_AGIRLIK { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? NAKLIYET_TUT { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? KDV_ORANI { get; set; }
        
        public byte? ALIS_DOV_TIP { get; set; }
        
        public short? DEPO_KODU { get; set; }
        
        public byte? DOV_TUR { get; set; }
        
        public byte? URET_OLCU_BR { get; set; }
        
        public char? BILESENMI { get; set; }
        
        public char? MAMULMU { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? FORMUL_TOPLAMI { get; set; }
        
        public char? UPDATE_KODU { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? MAX_ISKONTO { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? ECZACI_KARI { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? MIKTAR { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? MAL_FAZLASI { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? KDV_TENZIL_ORAN { get; set; }
        
        public char? KILIT { get; set; }
        
        [MaxLength(25)]
        public string? ONCEKI_KOD { get; set; }
        
        [MaxLength(25)]
        public string? SONRAKI_KOD { get; set; }
        
        [MaxLength(50)]
        public string? BARKOD1 { get; set; }
        
        [MaxLength(50)]
        public string? BARKOD2 { get; set; }
        
        [MaxLength(50)]
        public string? BARKOD3 { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? ALIS_KDV_KODU { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? ALIS_FIAT1 { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? ALIS_FIAT2 { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? ALIS_FIAT3 { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? ALIS_FIAT4 { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? LOT_SIZE { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? MIN_SIP_MIKTAR { get; set; }
        
        public short? SABIT_SIP_ARALIK { get; set; }
        
        public char? SIP_POLITIKASI { get; set; }
        
        public byte? OZELLIK_KODU1 { get; set; }
        
        public byte? OZELLIK_KODU2 { get; set; }
        
        public byte? OZELLIK_KODU3 { get; set; }
        
        public byte? OZELLIK_KODU4 { get; set; }
        
        public byte? OZELLIK_KODU5 { get; set; }
        
        public byte? OPSIYON_KODU1 { get; set; }
        
        public byte? OPSIYON_KODU2 { get; set; }
        
        public byte? OPSIYON_KODU3 { get; set; }
        
        public byte? OPSIYON_KODU4 { get; set; }
        
        public byte? OPSIYON_KODU5 { get; set; }
        
        public byte? BILESEN_OP_KODU { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? SIP_VER_MAL { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? ELDE_BUL_MAL { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? YIL_TAH_KUL_MIK { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? EKON_SIP_MIKTAR { get; set; }
        
        public char? ESKI_RECETE { get; set; }
        
        public char? OTOMATIK_URETIM { get; set; }
        
        [MaxLength(25)]
        public string? ALFKOD { get; set; }
        
        [MaxLength(25)]
        public string? SAFKOD { get; set; }
        
        public char? KODTURU { get; set; }
        
        [MaxLength(50)]
        public string? S_YEDEK1 { get; set; }
        
        [MaxLength(50)]
        public string? S_YEDEK2 { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? F_YEDEK3 { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? F_YEDEK4 { get; set; }
        
        public char? C_YEDEK5 { get; set; }
        
        public char? C_YEDEK6 { get; set; }
        
        public byte? B_YEDEK7 { get; set; }
        
        public short? I_YEDEK8 { get; set; }
        
        public int? L_YEDEK9 { get; set; }
        
        public DateTime? D_YEDEK10 { get; set; }
        
        public char? GIRIS_SERI { get; set; }
        
        public char? CIKIS_SERI { get; set; }
        
        public char? SERI_BAK { get; set; }
        
        public char? SERI_MIK { get; set; }
        
        public char? SERI_GIR_OT { get; set; }
        
        public char? SERI_CIK_OT { get; set; }
        
        [MaxLength(25)]
        public string? SERI_BASLANGIC { get; set; }
        
        [MaxLength(10)]
        public string? FIYATKODU { get; set; }
        
        public int? FIYATSIRASI { get; set; }
        
        public char? PLANLANACAK { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? LOT_SIZECUSTOMER { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? MIN_SIP_MIKTARCUSTOMER { get; set; }
        
        [MaxLength(20)]
        public string? GUMRUKTARIFEKODU { get; set; }
        
        [MaxLength(10)]
        public string? ABCKODU { get; set; }
        
        [MaxLength(10)]
        public string? PERFORMANSKODU { get; set; }
        
        public char? SATICISIPKILIT { get; set; }
        
        public char? MUSTERISIPKILIT { get; set; }
        
        public char? SATINALMAKILIT { get; set; }
        
        public char? SATISKILIT { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? EN { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? BOY { get; set; }
        
        [Column(TypeName = "decimal(18,4)")]
        public decimal? GENISLIK { get; set; }
        
        public char? SIPLIMITVAR { get; set; }
        
        [MaxLength(25)]
        public string? SONSTOKKODU { get; set; }
        
        public char? ONAYTIPI { get; set; }
        
        public int? ONAYNUM { get; set; }
        
        public char? FIKTIF_MAM { get; set; }
        
        public char? YAPILANDIR { get; set; }
        
        public char? SBOMVARMI { get; set; }
        
        [MaxLength(25)]
        public string? BAGLISTOKKOD { get; set; }
        
        [MaxLength(25)]
        public string? YAPKOD { get; set; }
        
        public char? ALISTALTEKKILIT { get; set; }
        
        public char? SATISTALTEKKILIT { get; set; }
        
        [MaxLength(50)]
        public string? S_YEDEK3 { get; set; }
        
        public short? STOKMEVZUAT { get; set; }
        
        public char? OTVTEVKIFAT { get; set; }
        
        public char? SERIBARKOD { get; set; }
        
        public char? ATIK_URUN { get; set; }
    }
}
