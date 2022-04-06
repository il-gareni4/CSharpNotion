using CSharpNotion.Api.General;
using CSharpNotion.Utilities;

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

        internal BlockDateInformation ToBlockDateInformation()
        {
            return new BlockDateInformation()
            {
                StartDate = NotionUtils.FormatToNotionDate(StartDate),
                StartTime = HaveTime ? NotionUtils.FormatToNotionTime(StartDate) : null,
                EndDate = EndDate is not null ? NotionUtils.FormatToNotionDate(EndDate.Value) : null,
                EndTime = EndDate is not null && HaveTime ? NotionUtils.FormatToNotionTime(EndDate.Value) : null,
                Reminder = Reminder is not null ? Reminder.ToBlockDateInformationReminder() : null,
                TimeZone = TimeZone,
                Type = HaveTime ? (EndDate is not null ? "datetimerange" : "datetime") : (EndDate is not null ? "daterange" : "date")
            };
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

        internal BlockDateInformationReminder ToBlockDateInformationReminder()
        {
            return new BlockDateInformationReminder()
            {
                Time = Time is not null ? NotionUtils.FormatToNotionTime(Time.Value) : null,
                Value = Value,
                Unit = Unit
            };
        }
    }
}