namespace Base.Extensions
{
    /// <summary>
    /// Provides extension methods for DateTime operations
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Checks if a DateTime falls within a specified range
        /// </summary>
        /// <param name="date">The date to check</param>
        /// <param name="startDate">The start of the range</param>
        /// <param name="endDate">The end of the range</param>
        /// <returns>True if the date falls within the range; otherwise, false</returns>
        public static bool IsBetween(this DateTime date, DateTime startDate, DateTime endDate)
        {
            return date >= startDate && date <= endDate;
        }

        /// <summary>
        /// Gets the first day of the month for a given date
        /// </summary>
        /// <param name="date">The date</param>
        /// <returns>The first day of the month</returns>
        public static DateTime FirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// Gets the last day of the month for a given date
        /// </summary>
        /// <param name="date">The date</param>
        /// <returns>The last day of the month</returns>
        public static DateTime LastDayOfMonth(this DateTime date)
        {
            return date.FirstDayOfMonth().AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Gets the first day of the week for a given date
        /// </summary>
        /// <param name="date">The date</param>
        /// <param name="startOfWeek">The day considered as start of week (defaults to Sunday)</param>
        /// <returns>The first day of the week</returns>
        public static DateTime FirstDayOfWeek(this DateTime date, DayOfWeek startOfWeek = DayOfWeek.Sunday)
        {
            int diff = (7 + (date.DayOfWeek - startOfWeek)) % 7;
            return date.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Gets the last day of the week for a given date
        /// </summary>
        /// <param name="date">The date</param>
        /// <param name="startOfWeek">The day considered as start of week (defaults to Sunday)</param>
        /// <returns>The last day of the week</returns>
        public static DateTime LastDayOfWeek(this DateTime date, DayOfWeek startOfWeek = DayOfWeek.Sunday)
        {
            return date.FirstDayOfWeek(startOfWeek).AddDays(6);
        }

        /// <summary>
        /// Gets the age in years from a birth date
        /// </summary>
        /// <param name="birthDate">The birth date</param>
        /// <returns>The age in years</returns>
        public static int GetAge(this DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }

        /// <summary>
        /// Checks if a DateTime represents a weekend day
        /// </summary>
        /// <param name="date">The date to check</param>
        /// <returns>True if the date falls on a weekend; otherwise, false</returns>
        public static bool IsWeekend(this DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// Checks if a DateTime represents a weekday
        /// </summary>
        /// <param name="date">The date to check</param>
        /// <returns>True if the date falls on a weekday; otherwise, false</returns>
        public static bool IsWeekday(this DateTime date)
        {
            return !date.IsWeekend();
        }

        /// <summary>
        /// Gets the next occurrence of a specific day of week
        /// </summary>
        /// <param name="date">The starting date</param>
        /// <param name="dayOfWeek">The day of week to find</param>
        /// <returns>The next occurrence of the specified day</returns>
        public static DateTime NextDayOfWeek(this DateTime date, DayOfWeek dayOfWeek)
        {
            int daysToAdd = ((int)dayOfWeek - (int)date.DayOfWeek + 7) % 7;
            return date.AddDays(daysToAdd == 0 ? 7 : daysToAdd);
        }

        /// <summary>
        /// Gets the previous occurrence of a specific day of week
        /// </summary>
        /// <param name="date">The starting date</param>
        /// <param name="dayOfWeek">The day of week to find</param>
        /// <returns>The previous occurrence of the specified day</returns>
        public static DateTime PreviousDayOfWeek(this DateTime date, DayOfWeek dayOfWeek)
        {
            int daysToSubtract = ((int)date.DayOfWeek - (int)dayOfWeek + 7) % 7;
            return date.AddDays(daysToSubtract == 0 ? -7 : -daysToSubtract);
        }

        /// <summary>
        /// Converts a DateTime to Unix timestamp (seconds since Unix epoch)
        /// </summary>
        /// <param name="date">The date to convert</param>
        /// <returns>The Unix timestamp</returns>
        public static long ToUnixTimestamp(this DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(date.ToUniversalTime() - epoch).TotalSeconds;
        }

        /// <summary>
        /// Creates a DateTime from a Unix timestamp
        /// </summary>
        /// <param name="timestamp">The Unix timestamp (seconds since epoch)</param>
        /// <returns>The corresponding DateTime</returns>
        public static DateTime FromUnixTimestamp(this long timestamp)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(timestamp);
        }

        /// <summary>
        /// Gets the start of the day (midnight)
        /// </summary>
        /// <param name="date">The date</param>
        /// <returns>The start of the day</returns>
        public static DateTime StartOfDay(this DateTime date)
        {
            return date.Date;
        }

        /// <summary>
        /// Gets the end of the day (23:59:59.999)
        /// </summary>
        /// <param name="date">The date</param>
        /// <returns>The end of the day</returns>
        public static DateTime EndOfDay(this DateTime date)
        {
            return date.Date.AddDays(1).AddTicks(-1);
        }

        /// <summary>
        /// Gets the quarter number (1-4) for a given date
        /// </summary>
        /// <param name="date">The date</param>
        /// <returns>The quarter number (1-4)</returns>
        public static int GetQuarter(this DateTime date)
        {
            return (date.Month - 1) / 3 + 1;
        }

        /// <summary>
        /// Gets the start date of the quarter for a given date
        /// </summary>
        /// <param name="date">The date</param>
        /// <returns>The first day of the quarter</returns>
        public static DateTime StartOfQuarter(this DateTime date)
        {
            var quarter = date.GetQuarter();
            return new DateTime(date.Year, (quarter - 1) * 3 + 1, 1);
        }

        /// <summary>
        /// Gets the end date of the quarter for a given date
        /// </summary>
        /// <param name="date">The date</param>
        /// <returns>The last day of the quarter</returns>
        public static DateTime EndOfQuarter(this DateTime date)
        {
            return date.StartOfQuarter().AddMonths(3).AddDays(-1);
        }
    }
}
