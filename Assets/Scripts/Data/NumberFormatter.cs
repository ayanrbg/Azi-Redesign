using System.Globalization;

public static class NumberFormatter
{
    public static string ToShortString(double value, int decimals = 1)
    {
        if (value < 1_000)
            return value.ToString("0", CultureInfo.InvariantCulture);

        if (value < 1_000_000)
            return Format(value, 1_000, "K", decimals);

        if (value < 1_000_000_000)
            return Format(value, 1_000_000, "M", decimals);

        if (value < 1_000_000_000_000)
            return Format(value, 1_000_000_000, "B", decimals);

        return Format(value, 1_000_000_000_000, "T", decimals);
    }

    private static string Format(double value, double divider, string suffix, int decimals)
    {
        double shortValue = value / divider;

        string format = decimals > 0
            ? $"0.{new string('#', decimals)}"
            : "0";

        return shortValue.ToString(format, CultureInfo.InvariantCulture) + suffix;
    }
}
