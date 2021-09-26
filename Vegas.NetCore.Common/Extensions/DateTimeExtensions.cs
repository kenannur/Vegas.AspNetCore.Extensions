using System;
using System.Globalization;

namespace Vegas.NetCore.Common.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Returns for example 15 Ağu 2020 or 15 Aug 2020 depending on culture.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToAbbreviatedDateString(this DateTime dateTime)
            => $"{dateTime.Day} {DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(dateTime.Month)} {dateTime.Year}";

        /// <summary>
        /// Returns for example 15 Ağu 2020 or 15 Aug 2020 depending on culture.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToAbbreviatedDateString(this DateTime? dateTime, string defaultValue = "")
            => dateTime.HasValue ? ToAbbreviatedDateString(dateTime.Value) : defaultValue;

        /// <summary>
        /// Returns HH:mm:ss formatted string
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToTimeString(this DateTime dateTime)
            => dateTime.ToString("HH:mm:ss");

        /// <summary>
        /// Returns HH:mm:ss formatted string
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToTimeString(this DateTime? dateTime, string defaultValue = "")
            => dateTime.HasValue ? ToTimeString(dateTime.Value) : defaultValue;

        /// <summary>
        /// Returns for example Ağustos 2020 or August 2020 depending on culture
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToMonthYearString(this DateTime dateTime)
            => $"{DateTimeFormatInfo.CurrentInfo.GetMonthName(dateTime.Month)} {dateTime.Year}";

        /// <summary>
        /// Returns for example Ağustos 2020 or August 2020 depending on culture
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToMonthYearString(this DateTime? dateTime, string defaultValue = "")
            => dateTime.HasValue ? ToMonthYearString(dateTime.Value) : defaultValue;

        /// <summary>
        /// Returns for example Ağu 2020 or Aug 2020 depending on culture
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToAbbreviatedMonthYearString(this DateTime dateTime)
            => $"{DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(dateTime.Month)} {dateTime.Year}";

        /// <summary>
        /// Returns for example Ağu 2020 or Aug 2020 depending on culture
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToAbbreviatedMonthYearString(this DateTime? dateTime, string defaultValue = "")
            => dateTime.HasValue ? ToAbbreviatedMonthYearString(dateTime.Value) : defaultValue;
    }
}
