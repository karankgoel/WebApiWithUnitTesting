using System;
using System.Diagnostics.CodeAnalysis;

namespace eBroker.Shared
{
    [ExcludeFromCodeCoverage]
    public static class Utilities
    {
        public static bool IsMarketOpen()
        {
            var currentTime = DateTime.Now.TimeOfDay;
            var openingTime = new DateTime(2021, 12, 20, 9, 0, 0).TimeOfDay;
            var closingTime = new DateTime(2021, 12, 20, 15, 0, 0).TimeOfDay;

            if (currentTime >= openingTime && currentTime < closingTime && 
                DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
            {
                return true;
            }
            return false;
        }
    }
}
