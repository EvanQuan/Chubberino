﻿namespace Chubberino.Common.Extensions;

public static class TimeSpanExtensions
{
    public static String Format(this TimeSpan source)
    {
        StringBuilder builder = new();

        if (source.Hours > 0)
        {
            builder.Append(source.Hours);
            builder.Append(" hour");

            if (source.Hours != 1)
            {
                builder.Append('s');
            }
        }

        if (source.Minutes > 0)
        {
            if (source.Hours > 0)
            {
                if (source.Seconds == 0)
                {
                    builder.Append(" and ");
                }
                else
                {

                    builder.Append(", ");
                }
            }

            builder.Append(source.Minutes);
            builder.Append(" minute");

            if (source.Minutes != 1)
            {
                builder.Append('s');
            }
        }

        if (source.Seconds > 0 || builder.Length == 0)
        {
            if (builder.Length > 0)
            {
                builder.Append(" and ");
            }

            builder.Append(source.Seconds);
            builder.Append(" second");

            if (source.Seconds != 1)
            {
                builder.Append('s');
            }
        }

        return builder.ToString();
    }
}
