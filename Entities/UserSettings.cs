using CSharpNotion.Api.General;

namespace CSharpNotion.Entities
{
    public class UserSettings
    {
        public string Id { get; init; }
        public int Version { get; protected set; }
        public CookieConsent? CookieConsent { get; protected set; }
        public string? Locale { get; protected set; }
        public string? Persona { get; protected set; }
        public string? PreferredLocale { get; protected set; }
        public string? PreferredLocaleOrigin { get; protected set; }
        public bool SeenDatabaseGroupIntro { get; protected set; }
        public bool SeenFileAttachmentIntro { get; protected set; }
        public bool SeenViewsIntroModal { get; protected set; }
        public bool ShowAppDownload { get; protected set; }
        public long SignupTime { get; protected set; }
        public string? Source { get; protected set; }
        public int StartDayOfWeek { get; protected set; }
        public string? TimeZone { get; protected set; }
        public string? Type { get; protected set; }
        public bool UsedAndroidApp { get; protected set; }
        public bool UsedDesktopWebApp { get; protected set; }
        public bool UsedWindowsApp { get; protected set; }

        internal UserSettings(RecordMapUserSettings userValue)
        {
            Id = userValue.Id ?? throw new ArgumentNullException();
            Version = userValue.Version;

            if (userValue.Settings is null) throw new ArgumentNullException();
            CookieConsent = userValue.Settings.CookieConsent;
            Locale = userValue.Settings.Locale;
            Persona = userValue.Settings.Persona;
            PreferredLocale = userValue.Settings.PreferredLocale;
            PreferredLocaleOrigin = userValue.Settings.PreferredLocaleOrigin;
            SeenDatabaseGroupIntro = userValue.Settings.SeenDatabaseGroupIntro;
            SeenFileAttachmentIntro = userValue.Settings.SeenFileAttachmentIntro;
            SeenViewsIntroModal = userValue.Settings.SeenViewsIntroModal;
            ShowAppDownload = userValue.Settings.ShowAppDownload;
            SignupTime = userValue.Settings.SignupTime;
            Source = userValue.Settings.Source;
            StartDayOfWeek = userValue.Settings.StartDayOfWeek;
            TimeZone = userValue.Settings.TimeZone;
            Type = userValue.Settings.Type;
            UsedAndroidApp = userValue.Settings.UsedAndroidApp;
            UsedDesktopWebApp = userValue.Settings.UsedDesktopWebApp;
            UsedWindowsApp = userValue.Settings.UsedWindowsApp;
        }
    }
}