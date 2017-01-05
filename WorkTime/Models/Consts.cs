using System;

namespace WorkTime.Models
{
    public class Consts
    {
        public const string TIME_SPAN_TEMPLATE = @"hh\:mm\:ss";
        public const string WORK_DURATION_TIME_MSG_TEMPLATE = "Work duration is: {0}";
        public const string END_TIME_MSG_TEMPLATE = "End time is: {0}";
        public const int TOTAL_HOURS_IN_DAY = 9;
        public const int TOTAL_MINUTES_IN_DAY = 0;
        public static readonly DateTime DEFAULT_DATE_TIME = new DateTime(1970, 1, 1);
    }
}
