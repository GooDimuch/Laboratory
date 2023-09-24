namespace UniTests;

public class NumberToAbbreviation
{
    [SetUp]
    public void Setup() { }

    [Test]
    [TestCase(500000000d, "500.00M")]
    [TestCase(0.9999999d, "0")]
    [TestCase(12.999999, "12")]
    [TestCase(123.999999, "124")]
    [TestCase(1_000, "1.00K")]
    [TestCase(1_004.999999, "1.00K")]
    [TestCase(1_234.999999, "1.23K")]
    [TestCase(12_345.999999, "12.34K")]
    [TestCase(123_456.999999, "123.45K")]
    [TestCase(1_234_567.999999, "1.23M")]
    [TestCase(12_345_678.999999, "12.34M")]
    [TestCase(123_456_789.999999, "123.45M")]
    [TestCase(1_234_567_890.999999, "1.23B")]
    [TestCase(12_345_678_901.999999, "12.34B")]
    [TestCase(123_456_789_012.999999, "123.45B")]
    [TestCase(1_234_567_890_123.999999, "1.23T")]
    [TestCase(
        12_345_678_901_234_567_890_123_456_789_012_345_678_901_234_567_890_123_456_789_012_345_678_901_234_567_890_123_456_789_012_345_678.999999,
        "12.34Untg")]
    [TestCase(
        12_345_678_901_234_567_890_123_456_789_012_345_678_901_234_567_890_123_456_789_012_345_678_901_234_567_890_123_456_789_012_345_67800.999999,
        "1.23Dutg")]
    [TestCase(
        12_345_678_901_234_567_890_123_456_789_012_345_678_901_234_567_890_123_456_789_012_345_678_901_234_567_890_123_456_789_012_345_678000.999999,
        "Gog+")]
    [TestCase(
        12_345_678_901_234_567_890_123_456_789_012_345_678_901_234_567_890_123_456_789_012_345_678_901_234_567_890_123_456_789_012_345_6780000.999999,
        "Gog+")]
    [TestCase(
        12_345_678_901_234_567_890_123_456_789_012_345_678_901_234_567_890_123_456_789_012_345_678_901_234_567_890_123_456_789_012_345_678_901_234_567_890.999999,
        "Gog+")]
    [TestCase(
        1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890.999999,
        "Gog+")]
    public void Test_ToShortValue(double number, string expected) =>
        Assert.That(number.ToShortValue(), Is.EqualTo(expected));
}

public static class BigNumberExtensionMethods
{
    private static readonly string[] _tabUnits =
    {
        "",
        "K",
        "M",
        "B",
        "T",
        "Qa",
        "Qi",
        "Sx",
        "Sp",
        "Oc",
        "No",
        "Dc",
        "Ud",
        "Dd",
        "Td",
        "Qad",
        "Qid",
        "Sxd",
        "Spd",
        "Ocd",
        "Nod",
        "Vg",
        "Uvg",
        "Dvg",
        "Tvg",
        "Qavg",
        "Qivg",
        "Sxvg",
        "Spvg",
        "Ocvg",
        "Novg",
        "Trg",
        "Untg",
        "Dutg",
    };

    public static string ToShortValue(this long sourceNumber) =>
        ToShortValue((double)sourceNumber);

    public static string ToShortValue(this double sourceNumber)
    {
        var sign = "";
        if (sourceNumber < 0)
        {
            sign = "-";
            sourceNumber = Math.Abs(sourceNumber);
        }
        var exponentIndex = GetExponentIndex(sourceNumber, out float visibleNumber);

        switch (exponentIndex)
        {
            case { } when exponentIndex == 0:
                return $"{sign}{Math.Floor(visibleNumber).ToString("F0").Replace(',', '.')}";
            case { } when exponentIndex == _tabUnits.Length - 1 && visibleNumber > 10:
                return $"Gog+";
            case { } when exponentIndex > _tabUnits.Length - 1:
                return $"Gog+";
            default:
                return
                    $"{sign}{(Math.Floor(visibleNumber * 100) / 100).ToString("F2").Replace(',', '.')}{_tabUnits[exponentIndex]}";
        }
    }

    public static string ToShortValue(this decimal sourceNumber)
    {
        var sign = "";
        if (sourceNumber < 0)
        {
            sign = "-";
            sourceNumber = Math.Abs(sourceNumber);
        }
        var exponentIndex = GetExponentIndex(sourceNumber, out float visibleNumber);

        switch (exponentIndex)
        {
            case { } when exponentIndex == 0:
                return $"{sign}{Math.Floor(visibleNumber).ToString("F0").Replace(',', '.')}";
            case { } when exponentIndex == _tabUnits.Length - 1 && visibleNumber > 10:
                return $"Gog+";
            case { } when exponentIndex > _tabUnits.Length - 1:
                return $"Gog+";
            default:
                return
                    $"{sign}{(Math.Floor(visibleNumber * 100) / 100).ToString("F2").Replace(',', '.')}{_tabUnits[exponentIndex]}";
        }
    }

    public static int GetExponentIndex(double number, out float visibleNumber)
    {
        var exponentIndex = 0;
        while (number / 1000 >= 1)
        {
            number /= 1000;
            exponentIndex++;
        }
        visibleNumber = (float)number;
        return exponentIndex;
    }

    public static int GetExponentIndex(decimal number, out float visibleNumber)
    {
        var exponentIndex = 0;
        while (number / 1000 >= 1)
        {
            number /= 1000;
            exponentIndex++;
        }
        visibleNumber = (float)number;
        return exponentIndex;
    }
}
