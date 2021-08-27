using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Framework
{
    public class DateTimeHelper
    {        
        private string fileName = "DateTimeHelper.cs";

        #region "Singleton"

        private static readonly DateTimeHelper instance = new DateTimeHelper();

        private DateTimeHelper() { }

        public static DateTimeHelper Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        public static string GetFormatedDate(DateTime dateTime)
        {
            return string.Format("{0:MM/dd/yyyy}", dateTime);
        }

        public static string GetFormatedTime(DateTime dateTime)
        {
            return string.Format("{0:T}", dateTime);
        }

        public string GetFormatedDateTime(DateTime dateTime)
        {
            return $"{dateTime:s}Z";
        }

        public static string GetUtcFormatedDate(DateTime dateTime)
        {
            return string.Format("{0:yyyy-MM-ddTHH:mm:ss}", dateTime);
        }

        public long DateTimeToMilliseconds(DateTime dateTime, Entities.OffsetType offsetType, int offset)
        {
            long milliseconds = 0;

            var firstJan1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); //It is a base time to calculate the milliseconds

            if (offsetType == Entities.OffsetType.Day)
            {
                milliseconds = (long)(dateTime.AddDays(-offset) - firstJan1970).TotalMilliseconds;
            }
            else if (offsetType == Entities.OffsetType.Hour)
            {
                milliseconds = (long)(dateTime.AddHours(-offset) - firstJan1970).TotalMilliseconds;
            }
            else if (offsetType == Entities.OffsetType.Minute)
            {
                milliseconds = (long)(dateTime.AddMinutes(-offset) - firstJan1970).TotalMilliseconds;
            }

            return milliseconds;
        }

        public string DateTimeFromMilliseconds(string dateTimeinMilliseconds)
        {
            var firstJan1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); //It is a base time to calculate the milliseconds
            double ticks = double.Parse(dateTimeinMilliseconds);
            TimeSpan time = TimeSpan.FromMilliseconds(ticks);
            DateTime startdate = firstJan1970 + time;

            return GetFormatedDateTime(startdate);
        }

        public string GetStartDateTime(DateTime dateTime, Entities.OffsetType offsetType, int offset)
        {
            string startDateTime = string.Empty;

            if (offsetType == Entities.OffsetType.Day)
            {
                startDateTime = $"{dateTime.AddDays(-offset):s}Z";
            }
            else if (offsetType == Entities.OffsetType.Hour)
            {
                startDateTime = $"{dateTime.AddHours(-offset):s}Z";
            }
            else if (offsetType == Entities.OffsetType.Minute)
            {
                startDateTime = $"{dateTime.AddMinutes(-offset):s}Z";
            }

            return startDateTime;
        }
    }
}
