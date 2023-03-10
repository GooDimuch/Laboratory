using System.Globalization;

namespace UniTests;

public class Tests {
	[SetUp]
	public void Setup() { }

	[Test]
	[TestCase(1f, "1.000")]
	[TestCase(12f, "12.00")]
	[TestCase(1234f, "1234")]
	[TestCase(12345f, "12345")]
	[TestCase(1.1f, "1.100")]
	[TestCase(1.12f, "1.120")]
	[TestCase(1.12345f, "1.123")]
	[TestCase(12.1f, "12.10")]
	[TestCase(12.12f, "12.12")]
	[TestCase(12.12345f, "12.12")]
	[TestCase(1234.1f, "1234")]
	[TestCase(1234.12345f, "1234")]
	[TestCase(12345.1f, "12345")]
	[TestCase(12345.12345f, "12345")]
	[TestCase(123456789.12345f, "123456789")]
	public void Test1(float value, string expected) =>
		Assert.That(ClampTotalDigits(value, 4), Is.EqualTo(expected));

	[Test]
	[TestCase(1f, "1.00")]
	[TestCase(12f, "12.0")]
	[TestCase(1234f, "1234")]
	[TestCase(12345f, "12345")]
	[TestCase(1.1f, "1.10")]
	[TestCase(1.12f, "1.12")]
	[TestCase(1.12345f, "1.12")]
	[TestCase(12.1f, "12.1")]
	[TestCase(12.12f, "12.1")]
	[TestCase(12.12345f, "12.1")]
	[TestCase(1234.1f, "1234")]
	[TestCase(1234.12345f, "1234")]
	[TestCase(12345.1f, "12345")]
	[TestCase(12345.12345f, "12345")]
	[TestCase(123456789.12345f, "123456789")]
	public void Test2(float value, string expected) =>
		Assert.That(ClampTotalDigits(value, 3), Is.EqualTo(expected));

	private string ClampTotalDigits(float value, int digits) {
		var formattedValue = value.ToString($"F{digits - 1}", NumberFormatInfo.InvariantInfo);
		Console.WriteLine(formattedValue);
		return formattedValue.Length >= 8
			? formattedValue.Substring(0, formattedValue.Length - digits)
			: formattedValue.Substring(0, digits + 1);
	}
}