using System;
using System.Globalization;

public static class MoneyFormatter
{
    private static readonly string[] Suffixes =
    {
        "",     // 10^0
        "k",    // 10^3
        "M",    // 10^6
        "B",    // 10^9
        "T"     // 10^12
    };

    /// <summary>
    /// Форматирует число в компактный денежный формат:
    /// 309 -> "309"
    /// 3000 -> "3k"
    /// 10300 -> "10.3k"
    /// 100030 -> "100.03k"
    /// 5740000 -> "5.74M"
    /// 37230000000000 -> "37.23T"
    /// </summary>
    public static string Format(double value, int decimals = 2)
    {
        if (value < 0)
            return "-" + Format(Math.Abs(value), decimals);

        if (value < 1000)
            return value.ToString("0", CultureInfo.InvariantCulture);

        int suffixIndex = 0;

        while (value >= 1000 && suffixIndex < Suffixes.Length - 1)
        {
            value /= 1000;
            suffixIndex++;
        }

        string format =
            value >= 100
                ? "0"
                : value >= 10
                    ? "0.#"
                    : "0." + new string('#', decimals);

        return value.ToString(format, CultureInfo.InvariantCulture) + Suffixes[suffixIndex];
    }

    // Удобные перегрузки
    public static string Format(int value) => Format((double)value);
    public static string Format(long value) => Format((double)value);
    public static string Format(float value) => Format((double)value);
}
