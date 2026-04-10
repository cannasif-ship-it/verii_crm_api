using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace crm_api.Modules.PdfBuilder.Infrastructure.Helpers
{
    /// <summary>
    /// Validates image URLs for PDF generation to prevent SSRF (localhost, private IPs; optional allowlist).
    /// </summary>
    public static class PdfImageUrlValidator
    {
        private static readonly Regex DataUriRegex = new(@"^data:(?<type>image/[a-zA-Z0-9+.-]+);base64,", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static bool IsDataUri(string? value, IReadOnlyList<string>? allowedContentTypes, out string? rejectReason)
        {
            rejectReason = null;
            if (string.IsNullOrWhiteSpace(value))
            {
                rejectReason = "Empty image source.";
                return false;
            }
            if (!value.TrimStart().StartsWith("data:", StringComparison.OrdinalIgnoreCase))
                return false;

            var match = DataUriRegex.Match(value);
            if (!match.Success)
            {
                rejectReason = "Invalid data: URI format for image.";
                return false;
            }
            var contentType = match.Groups["type"].Value;
            if (allowedContentTypes != null && allowedContentTypes.Count > 0 && !allowedContentTypes.Any(t => contentType.StartsWith(t.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                rejectReason = $"Content type not allowed: {contentType}.";
                return false;
            }
            return true;
        }

        public static bool IsUrlAllowed(string? url, IReadOnlyList<string> allowlistedHosts, out string? rejectReason)
        {
            rejectReason = null;
            if (string.IsNullOrWhiteSpace(url))
            {
                rejectReason = "Empty image URL.";
                return false;
            }
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) || !uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase) && !uri.Scheme.Equals("http", StringComparison.OrdinalIgnoreCase))
            {
                rejectReason = "Invalid or non-http(s) URL.";
                return false;
            }

            var host = uri.Host;
            if (string.IsNullOrEmpty(host))
            {
                rejectReason = "Missing host.";
                return false;
            }

            // Block localhost and loopback
            if (host.Equals("localhost", StringComparison.OrdinalIgnoreCase) ||
                host.Equals("127.0.0.1") ||
                host.StartsWith("127."))
            {
                rejectReason = "localhost and loopback are not allowed.";
                return false;
            }

            // Block private IPs when host is an IP address (no DNS resolution to avoid latency)
            if (IPAddress.TryParse(host, out var addr))
            {
                if (IsPrivateOrLoopback(addr))
                {
                    rejectReason = "Private or loopback IP addresses are not allowed.";
                    return false;
                }
            }
            if (allowlistedHosts == null || allowlistedHosts.Count == 0)
            {
                rejectReason = "Only data: images are allowed; external URLs are disabled.";
                return false;
            }

            var hostLower = host.ToLowerInvariant();
            var allowedHosts = allowlistedHosts
                .Select(NormalizeAllowlistHost)
                .Where(h => !string.IsNullOrWhiteSpace(h))
                .ToList();

            var allowed = allowedHosts.Any(allowedHost =>
                hostLower.Equals(allowedHost!, StringComparison.OrdinalIgnoreCase) ||
                hostLower.EndsWith("." + allowedHost, StringComparison.OrdinalIgnoreCase));
            if (!allowed)
            {
                rejectReason = "Host is not in the allowlist.";
                return false;
            }

            return true;
        }

        private static bool IsPrivateOrLoopback(IPAddress address)
        {
            if (IPAddress.IsLoopback(address)) return true;
            var bytes = address.GetAddressBytes();
            if (bytes.Length == 4)
            {
                // 10.x, 172.16-31.x, 192.168.x
                if (bytes[0] == 10) return true;
                if (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31) return true;
                if (bytes[0] == 192 && bytes[1] == 168) return true;
            }
            else if (bytes.Length == 16)
            {
                // fc00::/7 (unique local) and fe80::/10 (link-local)
                if ((bytes[0] & 0xFE) == 0xFC) return true;
                if (bytes[0] == 0xFE && (bytes[1] & 0xC0) == 0x80) return true;
            }
            return false;
        }

        private static string? NormalizeAllowlistHost(string? rawValue)
        {
            if (string.IsNullOrWhiteSpace(rawValue))
            {
                return null;
            }

            var value = rawValue.Trim();

            // Allow values as host ("cdn.example.com") or URL ("https://cdn.example.com/assets")
            if (Uri.TryCreate(value, UriKind.Absolute, out var parsedUri) && !string.IsNullOrWhiteSpace(parsedUri.Host))
            {
                return parsedUri.Host.ToLowerInvariant();
            }

            // Remove optional port when a plain host:port value is provided.
            if (value.Contains(':') && !value.Contains("://"))
            {
                var hostOnly = value.Split(':', 2, StringSplitOptions.RemoveEmptyEntries)[0];
                return hostOnly.ToLowerInvariant();
            }

            return value.ToLowerInvariant();
        }
    }
}
