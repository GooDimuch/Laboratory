using System.Globalization;

namespace UniTests;

public class ClampTotalDigitsTests
{
    [SetUp]
    public void Setup() { }

    [Test]
    [TestCase(1f, 3, "1.00")]
    [TestCase(12f, 3, "12.0")]
    [TestCase(1234f, 3, "1234")]
    [TestCase(12345f, 3, "12345")]
    [TestCase(1.1f, 3, "1.10")]
    [TestCase(1.12f, 3, "1.12")]
    [TestCase(1.12345f, 3, "1.12")]
    [TestCase(12.1f, 3, "12.1")]
    [TestCase(12.12f, 3, "12.1")]
    [TestCase(12.12345f, 3, "12.1")]
    [TestCase(1234.1f, 3, "1234")]
    [TestCase(1234.12345f, 3, "1234")]
    [TestCase(12345.1f, 3, "12345")]
    [TestCase(12345.12345f, 3, "12345")]
    [TestCase(1f, 4, "1.000")]
    [TestCase(12f, 4, "12.00")]
    [TestCase(1234f, 4, "1234")]
    [TestCase(12345f, 4, "12345")]
    [TestCase(1.1f, 4, "1.100")]
    [TestCase(1.12f, 4, "1.120")]
    [TestCase(1.12345f, 4, "1.123")]
    [TestCase(12.1f, 4, "12.10")]
    [TestCase(12.12f, 4, "12.12")]
    [TestCase(12.12345f, 4, "12.12")]
    [TestCase(1234.1f, 4, "1234")]
    [TestCase(1234.12345f, 4, "1234")]
    [TestCase(12345.1f, 4, "12345")]
    [TestCase(12345.12345f, 4, "12345")]
    public void Test_ClampTotalDigits(float value, int digits, string expected) =>
        Assert.That(ClampTotalDigits(value, digits), Is.EqualTo(expected));

    private string ClampTotalDigits(float value, int digits)
    {
        var formattedValue = value.ToString($"F{digits - 1}", NumberFormatInfo.InvariantInfo);
        return formattedValue.Length >= digits * 2
            ? formattedValue.Substring(0, formattedValue.Length - digits)
            : formattedValue.Substring(0, digits + 1);
    }
}
