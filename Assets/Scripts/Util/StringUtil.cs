using System;
using System.Globalization;
using UnityEngine;

public class StringUtil : MonoBehaviour
{
    public static string Invariant(FormattableString formattable)
    {
        return formattable.ToString(CultureInfo.InvariantCulture);
    }

    public static string ConvertToMono(string stringToConvert, float space)
    {
        string spacer = space.ToString(CultureInfo.InvariantCulture);

        string text = stringToConvert.Replace(".", $"</mspace>.<mspace={spacer}em>").Replace(":", $"</mspace>:<mspace={spacer}em>");
        string convertedString = $"<mspace={spacer}em>{text}</mspace>";

        return convertedString;
    }
}
