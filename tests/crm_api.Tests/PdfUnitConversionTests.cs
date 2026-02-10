using crm_api.Helpers;
using Xunit;

namespace crm_api.Tests
{
    public class PdfUnitConversionTests
    {
        [Theory]
        [InlineData(96, "px", 72)]
        [InlineData(72, "pt", 72)]
        [InlineData(0, "px", 0)]
        [InlineData(48, "px", 36)]
        public void ToPoints_ConvertsCorrectly(decimal value, string unit, decimal expectedPt)
        {
            var result = PdfUnitConversion.ToPoints(value, unit);
            Assert.Equal(expectedPt, result);
        }

        [Fact]
        public void ToPoints_DefaultsToPx_WhenUnitNull()
        {
            var result = PdfUnitConversion.ToPoints(96, null!);
            Assert.Equal(72, result);
        }

        [Fact]
        public void ToPoints_HandlesInches()
        {
            var result = PdfUnitConversion.ToPoints(1, "in");
            Assert.Equal(72, result);
        }
    }
}
