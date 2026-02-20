using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;
using crm_api.Helpers;
using crm_api.Interfaces;
using Microsoft.Extensions.Options;

namespace crm_api.Services
{
    /// <summary>
    /// Nominatim (OpenStreetMap) ile adres → enlem/boylam dönüşümü.
    /// Kullanım politikası: https://operations.osmfoundation.org/policies/nominatim/
    /// </summary>
    public class GeocodingService : IGeocodingService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly GeocodingOptions _options;
        private readonly ILogger<GeocodingService> _logger;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public GeocodingService(
            IHttpClientFactory httpClientFactory,
            IOptions<GeocodingOptions> options,
            ILogger<GeocodingService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<(decimal Latitude, decimal Longitude)?> GeocodeAsync(string fullAddress, CancellationToken cancellationToken = default)
        {
            if (!_options.Enabled || string.IsNullOrWhiteSpace(fullAddress))
                return null;

            var address = fullAddress.Trim();
            if (address.Length < 3)
                return null;

            if (string.Equals(_options.Provider, "Nominatim", StringComparison.OrdinalIgnoreCase))
                return await GeocodeWithNominatimAsync(address, cancellationToken).ConfigureAwait(false);

            _logger.LogWarning("Geocoding provider '{Provider}' is not supported. Only Nominatim is implemented.", _options.Provider);
            return null;
        }

        /// <summary>
        /// Bina / kapı / nokta seviyesindeki sonuçlar (en hassas). Nominatim type/class değerleri.
        /// </summary>
        private static readonly HashSet<string> PreciseLocationTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            "house", "building", "apartments", "residential", "commercial", "retail",
            "industrial", "office", "warehouse", "school", "hospital", "place"
        };

        private async Task<(decimal Latitude, decimal Longitude)?> GeocodeWithNominatimAsync(string address, CancellationToken cancellationToken)
        {
            try
            {
                var baseUrl = _options.NominatimBaseUrl.TrimEnd('/');
                var encoded = Uri.EscapeDataString(address);
                // Birden fazla aday alıp en nokta atışı olanı seçmek için limit=10, addressdetails=1
                var url = $"{baseUrl}/search?q={encoded}&format=json&limit=10&addressdetails=1";

                using var client = _httpClientFactory.CreateClient();
                client.Timeout = TimeSpan.FromSeconds(_options.TimeoutSeconds);
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("VeriiCrmApi", "1.0"));
                client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("tr"));
                client.DefaultRequestHeaders.Add("Accept-Language", "tr,en;q=0.9");

                var response = await client.GetAsync(url, cancellationToken).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                var results = JsonSerializer.Deserialize<List<NominatimResult>>(json, JsonOptions);
                if (results == null || results.Count == 0)
                {
                    _logger.LogDebug("Nominatim: no results for address '{Address}'", address);
                    return null;
                }

                var best = SelectMostPreciseResult(results);
                if (best == null)
                    return null;

                if (!decimal.TryParse(best.Lat, NumberStyles.Any, CultureInfo.InvariantCulture, out var lat) ||
                    !decimal.TryParse(best.Lon, NumberStyles.Any, CultureInfo.InvariantCulture, out var lon))
                {
                    _logger.LogWarning("Nominatim: invalid lat/lon in response for '{Address}'", address);
                    return null;
                }

                return (lat, lon);
            }
            catch (TaskCanceledException)
            {
                // HttpClient timeout genelde TaskCanceledException fırlatır; yazma işlemi koordinatsız devam etsin.
                _logger.LogWarning("Geocoding request timed out or was cancelled for address '{Address}'", address);
                return null;
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Geocoding was cancelled for address '{Address}'", address);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Geocoding failed for address '{Address}'", address);
                return null;
            }
        }

        /// <summary>
        /// En nokta atışı konumu seçer: önce bina/kapı seviyesi (type/class), yoksa Nominatim relevance (importance yüksek = daha iyi eşleşme).
        /// </summary>
        private static NominatimResult? SelectMostPreciseResult(List<NominatimResult> results)
        {
            foreach (var r in results)
            {
                if (string.IsNullOrEmpty(r.Lat) || string.IsNullOrEmpty(r.Lon))
                    continue;
                if (PreciseLocationTypes.Contains(r.Type ?? "") || PreciseLocationTypes.Contains(r.Class ?? ""))
                    return r;
            }
            // Bina seviyesi yoksa: en yüksek importance = en alakalı aday (belirsiz adreslerde doğru koordinat).
            return results
                .Where(r => !string.IsNullOrEmpty(r.Lat) && !string.IsNullOrEmpty(r.Lon))
                .OrderByDescending(r => r.Importance ?? 0)
                .FirstOrDefault();
        }

        private class NominatimResult
        {
            public string? Lat { get; set; }
            public string? Lon { get; set; }
            public string? Display_Name { get; set; }
            public string? Type { get; set; }
            public string? Class { get; set; }
            public double? Importance { get; set; }
        }
    }
}
