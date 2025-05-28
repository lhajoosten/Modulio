namespace Modulio.Application.Extensions
{
    /// <summary>
    /// Extension methods for dates
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Gets the age in years based on a birth date
        /// </summary>
        /// <param name="birthDate">The birth date</param>
        /// <returns>The age in years</returns>
        public static int GetAge(this DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;

            if (birthDate.Date > today.AddYears(-age))
                age--;

            return age;
        }

        /// <summary>
        /// Checks if a date is in the future
        /// </summary>
        /// <param name="date">The date to check</param>
        /// <returns>True if the date is in the future, false otherwise</returns>
        public static bool IsFuture(this DateTime date)
        {
            return date > DateTime.Now;
        }

        /// <summary>
        /// Checks if a date is in the past
        /// </summary>
        /// <param name="date">The date to check</param>
        /// <returns>True if the date is in the past, false otherwise</returns>
        public static bool IsPast(this DateTime date)
        {
            return date < DateTime.Now;
        }

        /// <summary>
        /// Gets the start of a day (00:00:00)
        /// </summary>
        /// <param name="date">The date</param>
        /// <returns>The date at the start of the day</returns>
        public static DateTime StartOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, date.Kind);
        }

        /// <summary>
        /// Gets the end of a day (23:59:59.999)
        /// </summary>
        /// <param name="date">The date</param>
        /// <returns>The date at the end of the day</returns>
        public static DateTime EndOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999, date.Kind);
        }
    }
}
