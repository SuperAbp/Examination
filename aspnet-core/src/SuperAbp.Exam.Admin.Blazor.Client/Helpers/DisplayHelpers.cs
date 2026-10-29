using System;
using System.Globalization;

namespace SuperAbp.Exam.Admin.Blazor.Client.Helpers;

public static class DisplayHelpers
{
    private static readonly string[] SimplifiedNumbers =
    [
        "一", "二", "三", "四", "五", "六", "七", "八", "九", "十"
    ];

    public static string CharLabel(int index)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index);
        return ((char)('A' + index)).ToString();
    }

    public static string SimplifiedOrdinal(int number)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(number, 1);
        return number <= SimplifiedNumbers.Length
            ? SimplifiedNumbers[number - 1]
            : number.ToString(CultureInfo.InvariantCulture);
    }
}

