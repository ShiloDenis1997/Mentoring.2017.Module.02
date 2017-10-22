namespace TimeConverter
{
    public class UtcTime
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public TimeZone TimeZone { get; set; }

        public int TotalLocalSeconds
            => Hours * TimeConstants.SecondsInHour + Minutes * TimeConstants.SecondsInMinute + Seconds;
    }
}
