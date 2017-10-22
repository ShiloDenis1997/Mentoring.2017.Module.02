namespace TimeConverter
{
    public class TimeZone
    {
        public int TimeZoneSign { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }

        public int TotalSeconds
            => (Hours * TimeConstants.SecondsInHour + Minutes * TimeConstants.SecondsInMinute) * TimeZoneSign;
    }
}
