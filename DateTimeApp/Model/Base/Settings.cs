using System;
using System.Drawing;

namespace DateTimeApp.Model.Base
{
    [Serializable]
    public class Settings
    {
        public Settings() { }

        public bool timeFormat { get; set; }
        public string dateBrush { get; set; }
        public string timeBrush { get; set; }
        public string backgroundBrush { get; set; }
        public bool isAutorun { get; set; }
        public bool nightTheme { get; set; }
        public bool nightThemeToggled { get; set; }
        public bool monthFormat { get; set; }
        public DateTime alarmTime { get; set; }
        public bool isAlarmSet { get; set; }

        public static void Init(ref Settings settings) 
        {
            settings.timeFormat = true;
            settings.dateBrush = ColorTranslator.ToHtml(System.Drawing.Color.Black);
            settings.timeBrush = ColorTranslator.ToHtml(System.Drawing.Color.Black);
            settings.backgroundBrush = ColorTranslator.ToHtml(System.Drawing.Color.White);
            settings.isAutorun = false;
            settings.nightTheme = true;
            settings.nightThemeToggled = false;
            settings.monthFormat = true;
            settings.alarmTime = DateTime.Now;
            settings.isAlarmSet = false;
        }
    }
}
