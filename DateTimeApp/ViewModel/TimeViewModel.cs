using DateTimeApp.Model;
using DateTimeApp.Model.Base;
using DateTimeApp.View;
using DateTimeApp.View.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Media;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Serialization;

namespace DateTimeApp.ViewModel
{
    public class TimeViewModel : INotifyPropertyChanged
    {
        private Settings settings;
        private Grid mainGrid;
        private XmlSerializer settingsSerializer = new XmlSerializer(typeof(Settings));
        private string settingsPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\AlarmSettings.xml";
        private Lazy<SoundPlayer> soundPlayer = new Lazy<SoundPlayer>(() => new SoundPlayer(Properties.Resources.Alarm));

        internal void SaveSettings()
        {
            using (FileStream stream = new FileStream(this.settingsPath, FileMode.OpenOrCreate))
            {
                this.settingsSerializer.Serialize(stream, this.settings);
                Logic.Log("Настройки были сериализированы.", 3);
            }
        }

        private Timer timer = new Timer() { Interval = 30000 };
        private List<string> themes = new List<string>() { "DayTheme", "NightTheme" };

        private string timeString;
        public string TimeString
        {
            get { return timeString; }
            set { timeString = value; OnPropertyChanged("TimeString"); }
        }

        private string dateString;
        public string DateString
        {
            get { return dateString; }
            set { dateString = value; OnPropertyChanged("DateString"); }
        }

        private RelayCommand settingCommand;
        public RelayCommand SettingCommand
        {
            get
            {
                return settingCommand ?? new RelayCommand(act =>
                {
                    new SettingWindow(ref settings).ShowDialog();
                    OnPropertyChanged(string.Empty);
                    this.UpdateTime();
                });
            }
        }

        private void UpdateTime()
        {
            if (DateTime.Now >= this.settings.alarmTime && this.settings.isAlarmSet)
            {
                Logic.Log("Сработал будильник.", 0);
                this.settings.isAlarmSet = false;
                this.soundPlayer.Value.PlayLooping();
                if (MessageBox.Show(this.settings.alarmTime.ToShortTimeString(), "Будильник", MessageBoxButton.OK, MessageBoxImage.Exclamation) == MessageBoxResult.OK)
                    this.soundPlayer.Value.Stop();
            }    

            if (this.settings.monthFormat)
                this.DateString = DateTime.Now.ToShortDateString();
            else
                this.DateString = $"{DateTime.Now.Day} {(MonthEnum)DateTime.Now.Month} {DateTime.Now.Year}";

            if (this.settings.timeFormat)
                this.TimeString = DateTime.Now.ToShortTimeString();
            else
                this.TimeString = DateTime.Now.ToString("hh:mm tt", CultureInfo.InvariantCulture);

            this.SetTheme();
        }

        private void SetDayTheme()
        {
            if (Application.Current.Resources.Source.ToString() == @"View/Themes/" + themes[1] + ".xaml")
            {
                Application.Current.Resources.Clear();

                BindingOperations.SetBinding(this.mainGrid, Grid.BackgroundProperty, new Binding()
                {
                    Source = this,
                    Path = new PropertyPath("BackgroundBrush"),
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

                Application.Current.Resources.Source = new Uri("View/Themes/" + themes[0] + ".xaml", UriKind.RelativeOrAbsolute);
                Logic.Log("Была установлена дневная тема.", 0);
            }
        }

        private void SetNightTheme()
        {
            if (Application.Current.Resources.Source.ToString() == @"View/Themes/" + themes[0] + ".xaml")
            {
                Application.Current.Resources.Clear();

                BindingOperations.ClearBinding(this.mainGrid, Grid.BackgroundProperty);

                //ResourceDictionary resource = (ResourceDictionary)Application.LoadComponent(new Uri("View/Themes/" + themes[1] + ".xaml", UriKind.Relative));
                //Application.Current.Resources.MergedDictionaries.Add(resource);
                Application.Current.Resources.Source = new Uri("View/Themes/" + themes[1] + ".xaml", UriKind.RelativeOrAbsolute);
                Logic.Log("Была установлена ночная тема.", 0);
            }
        }

        private void SetTheme()
        {
            if (this.settings.nightTheme)
            {
                if (DateTime.Parse(this.TimeString).Hour > 18 || DateTime.Parse(this.TimeString).Hour < 8)
                {
                    this.SetNightTheme();
                }
                else
                {
                    this.SetDayTheme();
                }
            }
            else
            {
                if (this.settings.nightThemeToggled)
                {
                    this.SetNightTheme();
                }
                else
                {
                    this.SetDayTheme();
                }
            }                 
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e) => this.UpdateTime();

        public bool TimeFormat
        {
            get { return this.settings.timeFormat; }
            set { this.settings.timeFormat = value; OnPropertyChanged("TimeFormat"); }
        }

        public string DateBrush
        {
            get { return this.settings.dateBrush; }
            set { this.settings.dateBrush = value; OnPropertyChanged("DateBrush"); }
        }

        public string TimeBrush
        {
            get { return this.settings.timeBrush; }
            set { this.settings.timeBrush = value; OnPropertyChanged("TimeBrush"); }
        }

        public string BackgroundBrush
        {
            get { return this.settings.backgroundBrush; }
            set { this.settings.backgroundBrush = value; OnPropertyChanged("BackgroundBrush"); }
        }

        public bool IsAutorun
        {
            get { return this.settings.isAutorun; }
            set { this.settings.isAutorun = value; OnPropertyChanged("IsAutorun"); }
        }

        public bool NightTheme
        {
            get { return this.settings.nightTheme; }
            set { this.settings.nightTheme = value; OnPropertyChanged("NightTheme"); }
        }

        public bool NightThemeToggled
        {
            get { return this.settings.nightThemeToggled; }
            set { this.settings.nightThemeToggled = value; OnPropertyChanged("NightThemeToggled"); }
        }

        public bool MonthFormat
        {
            get { return this.settings.monthFormat; }
            set { this.settings.nightTheme = value; OnPropertyChanged("MonthFormat"); }
        }

        private void InitSettings(bool isError)
        {
            if (isError)
            {
                Logic.Log("Ошибка при десериализации файла настроек.", 2);
                File.Delete(this.settingsPath);
                Logic.Log("Файл настроек был удалён.", 1);
            }

            if (!File.Exists(this.settingsPath))
            {
                this.settings = new Settings();

                Settings.Init(ref this.settings);

                using (FileStream stream = new FileStream(this.settingsPath, FileMode.OpenOrCreate))
                {
                    this.settingsSerializer.Serialize(stream, this.settings);
                    Logic.Log("Создан файл настроек.", 0);
                }
            }
        }

        private void GetSettings()
        {
            using (FileStream stream = new FileStream(this.settingsPath, FileMode.OpenOrCreate))
            {
                try
                {
                    this.settings = (Settings)this.settingsSerializer.Deserialize(stream);
                    Logic.Log("Файл настроек успешно десериализирован.", 3);
                }
                catch (System.Exception e)
                {
                    MessageBox.Show(e.Message + "\nФайл настроек будет пересоздан.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    stream.Close();
                    this.InitSettings(isError: true);
                }
            }
        }

        public TimeViewModel(ref Grid mainGrid)
        {
            this.mainGrid = mainGrid;

            this.InitSettings(isError: false);

            this.GetSettings();          

            Application.Current.Resources.Source = new Uri("View/Themes/" + themes[1] + ".xaml", UriKind.RelativeOrAbsolute);

            this.UpdateTime();

            this.timer.Elapsed += Timer_Elapsed;
            this.timer.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
