namespace CSharpNotion.Entities
{
    public class DatePropertyValue
    {
        public DateTime StartDate { get; protected set; }
        public DateTime? EndDate { get; protected set; }
        public bool HaveTime { get; protected set; }
        public string? TimeZone { get; set; }
        public DatePropertyValueReminder? Reminder { get; set; }

        public DatePropertyValue(DateOnly startDate)
        {
            HaveTime = false;
            StartDate = startDate.ToDateTime(new TimeOnly());
        }

        public DatePropertyValue(DateOnly startDate, DateOnly endDate) : this(startDate)
        {
            EndDate = endDate.ToDateTime(new TimeOnly());
        }

        public DatePropertyValue(DateTime startDate)
        {
            HaveTime = true;
            StartDate = startDate;
        }

        public DatePropertyValue(DateTime startDate, DateTime endDate) : this(startDate)
        {
            EndDate = endDate;
        }

        public override string ToString()
        {
            string s = StartDate.ToString();
            if (EndDate is not null) s += $" -> {EndDate}";
            if (TimeZone is not null) s += $" ({TimeZone})";
            if (HaveTime) s += ", with time";
            else s += ", no time";
            if (Reminder is not null) s += ", with reminder";
            else s += ", no reminder";
            return s;
        }
    }

    public class DatePropertyValueReminder
    {
        public string Unit { get; set; }
        public int Value { get; set; }
        public TimeOnly? Time { get; set; }

        public DatePropertyValueReminder(string unit, int value, TimeOnly? time = null)
        {
            Unit = unit;
            Value = value;
            Time = time;
        }
    }
}