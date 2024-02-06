using System;
using UnityEngine;
using System.Globalization;

public class TimerUtil : MonoBehaviour
{
    public static string TimeToString(float timeInSeconds)
    {
        timeInSeconds = Mathf.Abs(timeInSeconds);
        int minutes = (int)(timeInSeconds / 60);
        double secondsRest = Math.Round((timeInSeconds % 60), 3);
        return StringUtil.ConvertToMono(StringUtil.Invariant($"{minutes:00}:{secondsRest:00.000}"), 0.6f);
    }
}
