using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartParking.Admin.Helpers
{
    public static class DateTimeHelper
    {
        public static long getUnixTimeStampFromStartOfMonth(int month)
        {
            long ts = 0;
            DateTime date = new DateTime(DateTime.Now.Year, month, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan span = (date - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
            ts = Convert.ToInt64(span.TotalMilliseconds);
            return ts;
        }

        public static long getUnixTimeStampFromEndOfMonth(int month)
        {
            long ts = 0;
            DateTime date = new DateTime(DateTime.Now.Year, month, DateTime.DaysInMonth(DateTime.Now.Year, month), 23, 59, 59, DateTimeKind.Utc);
            TimeSpan span = (date - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
            ts = Convert.ToInt64(span.TotalMilliseconds);
            return ts;
        }


        public static long getUnixTimeStampForStartOfDay(DateTime day)
        {
            long ts = 0;
            DateTime date = new DateTime(day.Year, day.Month, day.Day, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan span = (date - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
            ts = Convert.ToInt64(span.TotalMilliseconds);
            return ts;
        }

        public static long getUnixTimeStampForEndOfDay(DateTime day)
        {
            long ts = 0;
            DateTime date = new DateTime(day.Year, day.Month, day.Day, 23, 59, 59, DateTimeKind.Utc);
            TimeSpan span = (date - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
            ts = Convert.ToInt64(span.TotalMilliseconds);
            return ts;
        }

        public static string getHoursMinsFromUnixTimeStamp(long fromTime, long toTime)
        {
            int hours = 0;
            int mins = 0;
            string totalTime = "";
            int min=0;

            System.DateTime dtFrom = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtFrom = dtFrom.AddMilliseconds(fromTime).ToUniversalTime();

            System.DateTime dtTo = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtTo = dtTo.AddMilliseconds(toTime).ToUniversalTime();

            if (IsValidDateTime(dtFrom, dtTo))
            {
                mins = Convert.ToInt32((dtTo - dtFrom).TotalMinutes);

                if(mins > 60)
                {
                    hours = Convert.ToInt32(mins / 60);
                }
                mins = mins % 60;
                min = Convert.ToInt32(mins);
                if(hours > 0 && mins >0)
                {
                    totalTime = hours + "hr " + min + "min";
                }
                else if(hours > 0 && min == 0 )
                {
                    totalTime = hours + "hr";
                }
                else
                {
                    totalTime = min + "min";
                }
            }


            return totalTime;
        }
        public static double getHoursFromUnixTimeStamp(long fromTime, long toTime)
        {
            double hours = 0;

            System.DateTime dtFrom = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtFrom = dtFrom.AddMilliseconds(fromTime).ToUniversalTime();

            System.DateTime dtTo = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtTo = dtTo.AddMilliseconds(toTime).ToUniversalTime();

            if(IsValidDateTime(dtFrom, dtTo))
            {
                hours = (dtTo - dtFrom).TotalMinutes;
            }
            return hours;
        }

        public static int GetPreviousMonth(int Month)
        {

            DateTime thisMonth = new DateTime(DateTime.Now.Year, Month, 1);
            DateTime previousMonth = thisMonth.AddMonths(-1);
            return previousMonth.Month;
        }

        public static string GetMonthName(int Month)
        {
            DateTime thatMonth = new DateTime(DateTime.Now.Year, Month, 1);
            return thatMonth.ToString("MMMM");
        }

        public static bool IsInHourRange(string hour, long dateFrom, long dateTo)
        {
            //hour = 8:00 AM
            bool yes = false;
            //From 8:20 -- To 8:30
            System.DateTime dtFrom = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtFrom = dtFrom.AddMilliseconds(dateFrom).ToUniversalTime();

            System.DateTime dtTo = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtTo = dtTo.AddMilliseconds(dateTo).ToUniversalTime();
            string dtStr = dtFrom.ToString("yyyy") + dtFrom.ToString("MM") + dtFrom.ToString("dd") + hour;
            DateTime toCheck = DateTime.ParseExact(dtStr, "yyyyMMddH:mm tt", null);
            TimeSpan start = dtFrom.TimeOfDay;
            TimeSpan end = dtTo.TimeOfDay;
            TimeSpan check = toCheck.TimeOfDay;

            if(IsValidDateTime(dtFrom, dtTo))
            {
                if (start.Hours <= check.Hours && end.Hours >= check.Hours)
                {
                    yes = true;
                }
            }
            

            return yes;
        }


        public static bool IsValidDateTime(DateTime dateFrom, DateTime dateTo)
        {
            bool yes = false;
            if (dateFrom != null && dateTo != null)
            {
                if (dateFrom != DateTime.MinValue && dateTo != DateTime.MinValue)
                {
                    if (dateTo >= dateFrom)
                    {
                        yes = true;
                    }
                }
            }
            return yes;
        }

        public static long getUnixTimeStamp(DateTime date)
        {
            long ts = 0;
            DateTime dateDesired = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond, DateTimeKind.Utc);
            TimeSpan span = (dateDesired - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
            ts = Convert.ToInt64(span.TotalMilliseconds);
            return ts;
        }

        public static DateTime getDateTimeFormUnixTimeStamp(long unixTime)
        {
            //System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local);
            
            DateTime dtDateTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, 0),
                  TimeZoneInfo.FindSystemTimeZoneById("Myanmar Standard Time"));
            dtDateTime = dtDateTime.AddMilliseconds(unixTime);
            return dtDateTime;
        }

        public static DateTime GetDateTimeFromDateString(string date)
        {
            DateTime dt = DateTime.Now;
            DateTime tmp = new DateTime();
            if(DateTime.TryParseExact(date, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.AdjustToUniversal, out tmp))
            {
                dt = tmp;
            }
            return dt;
        }

        public static DateTime GetDateWithTimeFromDateString(string date)
        {
            DateTime dt = DateTime.MinValue;
            DateTime tmp = DateTime.MinValue;
            if (DateTime.TryParseExact(date, "MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture,System.Globalization.DateTimeStyles.None, out tmp))
            {
                dt = tmp;
            }
            return dt;
        }
    }
}
