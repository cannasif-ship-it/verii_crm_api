using System.Text.Json;
using crm_api.Shared.Common.Application;
using crm_api.Shared.Infrastructure.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace crm_api.Shared.Host.WebApi.Controllers.System;

[ApiController]
[Route("api/mobile")]
public sealed class MobileVersionController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;
    private readonly ILocalizationService _localizationService;

    public MobileVersionController(
        IWebHostEnvironment environment,
        ILocalizationService localizationService)
    {
        _environment = environment;
        _localizationService = localizationService;
    }

    [HttpGet("version-check")]
    public async Task<ActionResult<ApiResponse<MobileVersionCheckDto>>> CheckVersion(
        [FromQuery] string platform = "android",
        [FromQuery] string? appVersion = null,
        [FromQuery] int? versionCode = null,
        [FromQuery] string? runtimeVersion = null,
        CancellationToken cancellationToken = default)
    {
        var manifestPath = Path.Combine(
            _environment.ContentRootPath,
            "Shared",
            "Host",
            "WebApi",
            "Assets",
            "AndroidVersions",
            "versions.json");

        if (!global::System.IO.File.Exists(manifestPath))
        {
            var message = _localizationService.GetLocalizedString("MobileVersion.ManifestNotFound");
            return NotFound(ApiResponse<MobileVersionCheckDto>.ErrorResult(
                message,
                "Android versions manifest file was not found.",
                StatusCodes.Status404NotFound));
        }

        await using var stream = global::System.IO.File.OpenRead(manifestPath);
        var manifest = await JsonSerializer.DeserializeAsync<AndroidVersionsManifest>(
            stream,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            },
            cancellationToken);

        if (manifest?.Android is null)
        {
            var message = _localizationService.GetLocalizedString("MobileVersion.InvalidManifest");
            return BadRequest(ApiResponse<MobileVersionCheckDto>.ErrorResult(
                message,
                "Android versions manifest content is invalid.",
                StatusCodes.Status400BadRequest));
        }

        var normalizedPlatform = string.IsNullOrWhiteSpace(platform)
            ? "android"
            : platform.Trim().ToLowerInvariant();

        var android = manifest.Android;
        var currentVersionCode = versionCode ?? 0;
        var minimumSupportedVersionCode = android.MinimumSupportedVersionCode;
        var latestVersionCode = android.LatestVersionCode;
        var forceUpdate = currentVersionCode > 0 && currentVersionCode < minimumSupportedVersionCode;
        var updateAvailable = currentVersionCode <= 0
            ? IsVersionLower(appVersion, android.LatestVersion)
            : currentVersionCode < latestVersionCode;

        var dto = new MobileVersionCheckDto
        {
            Platform = normalizedPlatform,
            CurrentVersion = appVersion ?? string.Empty,
            CurrentVersionCode = currentVersionCode,
            RuntimeVersion = runtimeVersion ?? string.Empty,
            LatestVersion = android.LatestVersion,
            LatestVersionCode = latestVersionCode,
            MinimumSupportedVersion = android.MinimumSupportedVersion,
            MinimumSupportedVersionCode = minimumSupportedVersionCode,
            UpdateAvailable = normalizedPlatform == "android" && updateAvailable,
            ForceUpdate = normalizedPlatform == "android" && forceUpdate,
            UpdateType = "apk",
            ApkUrl = ResolveApkUrl(android),
            ReleaseNotes = android.ReleaseNotes ?? string.Empty,
            PublishedAtUtc = android.PublishedAtUtc
        };

        return Ok(ApiResponse<MobileVersionCheckDto>.SuccessResult(
            dto,
            _localizationService.GetLocalizedString("General.OperationSuccessful")));
    }

    private string ResolveApkUrl(AndroidVersionManifestEntry entry)
    {
        if (!string.IsNullOrWhiteSpace(entry.ApkUrl))
        {
            return entry.ApkUrl;
        }

        if (string.IsNullOrWhiteSpace(entry.ApkFileName))
        {
            return string.Empty;
        }

        return $"{Request.Scheme}://{Request.Host}/android-versions/{entry.ApkFileName}";
    }

    private static bool IsVersionLower(string? currentVersion, string latestVersion)
    {
        if (string.IsNullOrWhiteSpace(currentVersion))
        {
            return true;
        }

        var current = ParseVersion(currentVersion);
        var latest = ParseVersion(latestVersion);

        for (var i = 0; i < Math.Max(current.Length, latest.Length); i++)
        {
            var currentPart = i < current.Length ? current[i] : 0;
            var latestPart = i < latest.Length ? latest[i] : 0;

            if (currentPart == latestPart)
            {
                continue;
            }

            return currentPart < latestPart;
        }

        return false;
    }

    private static int[] ParseVersion(string version)
    {
        return version
            .Split('.', StringSplitOptions.RemoveEmptyEntries)
            .Select(part => int.TryParse(part, out var parsed) ? parsed : 0)
            .ToArray();
    }

    private sealed class AndroidVersionsManifest
    {
        public AndroidVersionManifestEntry? Android { get; set; }
    }

    private sealed class AndroidVersionManifestEntry
    {
        public string LatestVersion { get; set; } = "1.0.0";
        public int LatestVersionCode { get; set; } = 1;
        public string MinimumSupportedVersion { get; set; } = "1.0.0";
        public int MinimumSupportedVersionCode { get; set; } = 1;
        public string? ApkFileName { get; set; }
        public string? ApkUrl { get; set; }
        public string? ReleaseNotes { get; set; }
        public DateTime? PublishedAtUtc { get; set; }
    }
}

public sealed class MobileVersionCheckDto
{
    public string Platform { get; set; } = "android";
    public string CurrentVersion { get; set; } = string.Empty;
    public int CurrentVersionCode { get; set; }
    public string RuntimeVersion { get; set; } = string.Empty;
    public string LatestVersion { get; set; } = string.Empty;
    public int LatestVersionCode { get; set; }
    public string MinimumSupportedVersion { get; set; } = string.Empty;
    public int MinimumSupportedVersionCode { get; set; }
    public bool UpdateAvailable { get; set; }
    public bool ForceUpdate { get; set; }
    public string UpdateType { get; set; } = "apk";
    public string ApkUrl { get; set; } = string.Empty;
    public string ReleaseNotes { get; set; } = string.Empty;
    public DateTime? PublishedAtUtc { get; set; }
}
