using DateTimeApp.Model;
using DateTimeApp.Model.Base;
using DateTimeApp.View;
using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace DateTimeApp.ViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private Settings settings;
        public SettingsViewModel(ref Settings settings) => this.settings = settings;

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
            set 
            { 
                this.settings.isAutorun = value;
                if (this.settings.isAutorun)
                    Logic.AddAutorun();
                else
                    Logic.RemoveAutorun();

                OnPropertyChanged("IsAutorun"); 
            }
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
            set { this.settings.monthFormat = value; OnPropertyChanged("MonthFormat"); }
        }


        private RelayCommand<string> colorCommand;

        public RelayCommand<string> ColorCommand
        {
            get 
            {
                return colorCommand ?? new RelayCommand<string>(act => 
                {
                    PropertyInfo propertyInfo = this.settings.GetType().GetProperty(act);

                    string propValue = propertyInfo.GetValue(this.settings).ToString();

                    propertyInfo.SetValue(this.settings, Logic.ChangeColor(propValue));

                    OnPropertyChanged(new StringBuilder(act[0] + 32).Insert(0, act.Remove(0)).ToString());
                    //switch (act)
                    //{
                    //    case "date":
                    //        {
                    //            Logic.ChangeColor(ref this.settings.dateBrush);
                    //            break;
                    //        }
                    //    case "time":
                    //        {
                    //            Logic.ChangeColor(ref this.settings.timeBrush);
                    //            break;
                    //        }
                    //    case "background":
                    //        {
                    //            Logic.ChangeColor(ref this.settings.backgroundBrush);
                    //            break;
                    //        }
                    //}
                    //foreach (PropertyInfo info in typeof(Settings).GetProperties())
                    //{
                    //    MessageBox.Show(info.Name);
                    //}
                });
            }
        }

        private RelayCommand alarmCommand;
        public RelayCommand AlarmCommand
        {
            get
            {
                return alarmCommand ?? new RelayCommand(() => new AlarmWindow(ref this.settings).ShowDialog());
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
