using crm_api.Infrastructure;
using Xunit;

namespace crm_api.Tests
{
    public class PdfImageUrlValidatorTests
    {
        [Fact]
        public void IsDataUri_AcceptsValidDataUri()
        {
            var valid = "data:image/png;base64,iVBORw0KGgo=";
            var result = PdfImageUrlValidator.IsDataUri(valid, new[] { "image/png", "image/jpeg" }, out var reason);
            Assert.True(result);
            Assert.Null(reason);
        }

        [Fact]
        public void IsDataUri_RejectsInvalidFormat()
        {
            var invalid = "data:invalid";
            var result = PdfImageUrlValidator.IsDataUri(invalid, null, out var reason);
            Assert.False(result);
            Assert.NotNull(reason);
        }

        [Fact]
        public void IsDataUri_RejectsEmpty()
        {
            var result = PdfImageUrlValidator.IsDataUri("", null, out var reason);
            Assert.False(result);
            Assert.NotNull(reason);
        }

        [Fact]
        public void IsUrlAllowed_RejectsLocalhost()
        {
            var result = PdfImageUrlValidator.IsUrlAllowed("http://localhost/image.png", new List<string>(), out var reason);
            Assert.False(result);
            Assert.Contains("localhost", reason ?? "");
        }

        [Fact]
        public void IsUrlAllowed_RejectsWhenAllowlistEmpty()
        {
            var result = PdfImageUrlValidator.IsUrlAllowed("https://cdn.example.com/img.png", new List<string>(), out var reason);
            Assert.False(result);
            Assert.NotNull(reason);
        }

        [Fact]
        public void IsUrlAllowed_AcceptsWhenHostInAllowlist()
        {
            var result = PdfImageUrlValidator.IsUrlAllowed("https://cdn.example.com/img.png", new List<string> { "cdn.example.com" }, out var reason);
            Assert.True(result);
            Assert.Null(reason);
        }

        [Fact]
        public void IsUrlAllowed_RejectsPrivateIp()
        {
            var result = PdfImageUrlValidator.IsUrlAllowed("http://192.168.1.1/img.png", new List<string> { "192.168.1.1" }, out _);
            Assert.False(result);
        }
    }
}
