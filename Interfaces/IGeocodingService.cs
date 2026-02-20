namespace crm_api.Interfaces
{
    /// <summary>
    /// Adres metnini enlem/boylam (latitude/longitude) koordinatlarına çeviren servis.
    /// </summary>
    public interface IGeocodingService
    {
        /// <summary>
        /// Verilen adres metnini geocode eder; bulunan ilk sonucun koordinatlarını döner.
        /// </summary>
        /// <param name="fullAddress">Tam adres metni (örn: "Atatürk Cad. No:1, Kadıköy, İstanbul, Türkiye")</param>
        /// <param name="cancellationToken">İptal token'ı</param>
        /// <returns>Bulunursa (Latitude, Longitude); bulunamazsa veya servis kapalıysa null</returns>
        Task<(decimal Latitude, decimal Longitude)?> GeocodeAsync(string fullAddress, CancellationToken cancellationToken = default);
    }
}
