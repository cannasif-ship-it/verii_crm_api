namespace crm_api.Helpers
{
    /// <summary>
    /// Geocoding servisi ayarları. Adres → enlem/boylam dönüşümü için kullanılır.
    /// </summary>
    public class GeocodingOptions
    {
        public const string SectionName = "Geocoding";

        /// <summary>
        /// Geocoding kullanılsın mı? false ise adres gönderilse bile enlem/boylam güncellenmez.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Kullanılacak sağlayıcı: "Nominatim" (OpenStreetMap, ücretsiz) veya ileride "Google" eklenebilir.
        /// </summary>
        public string Provider { get; set; } = "Nominatim";

        /// <summary>
        /// Nominatim için base URL. Varsayılan: https://nominatim.openstreetmap.org
        /// </summary>
        public string NominatimBaseUrl { get; set; } = "https://nominatim.openstreetmap.org";

        /// <summary>
        /// İstek başına timeout (saniye).
        /// </summary>
        public int TimeoutSeconds { get; set; } = 10;

        /// <summary>
        /// Google Geocoding API key (Provider=Google kullanılırsa gerekli).
        /// </summary>
        public string? GoogleApiKey { get; set; }
    }
}
