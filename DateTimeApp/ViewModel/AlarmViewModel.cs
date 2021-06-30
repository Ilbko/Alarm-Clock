using DateTimeApp.Model.Base;
using DateTimeApp.View.ViewModel;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace DateTimeApp.ViewModel
{
    public class AlarmViewModel : INotifyPropertyChanged
    {
        private Settings settings;
        private Label statusLabel;

        private DateTime pickedDate;
        public DateTime PickedDate
        {
            get { return pickedDate; }
            set { pickedDate = value; OnPropertyChanged("PickedDate"); }
        }

        private int pickedHour;
        public int PickedHour
        {
            get { return pickedHour; }
            set { pickedHour = value; OnPropertyChanged("PickedHour"); }
        }

        private int pickedMinute;
        public int PickedMinute
        {
            get { return pickedMinute; }
            set { pickedMinute = value; OnPropertyChanged("PickedMinute"); }
        }

        private RelayCommand windCommand;
        public RelayCommand WindCommand
        {
            get
            {
                return windCommand ?? new RelayCommand(act => 
                {
                    this.settings.alarmTime = new DateTime(pickedDate.Year, pickedDate.Month, pickedDate.Day, pickedHour, pickedMinute, 0);

                    if (settings.alarmTime < DateTime.Now)
                    {
                        this.statusLabel.Content = "Будильник не заведён";
                        this.settings.isAlarmSet = false;
                    }
                    else
                    {
                        this.statusLabel.Content = "Будильник заведён на " + settings.alarmTime.ToString();
                        this.settings.isAlarmSet = true;
                    }

                });
            }
        }


        private RelayCommand resetCommand;
        public RelayCommand ResetCommand
        {
            get
            {
                return resetCommand ?? new RelayCommand(act => 
                {
                    DateTime fullCurrentDate = this.settings.alarmTime = DateTime.Now;
                    this.PickedDate = fullCurrentDate.Date;
                    this.PickedHour = fullCurrentDate.Hour;
                    this.PickedMinute = fullCurrentDate.Minute;

                    this.statusLabel.Content = "Будильник не заведён";
                    this.settings.isAlarmSet = false;
                });
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = " ")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        public AlarmViewModel(ref Settings settings, ref Label statusLabel) 
        { 
            this.settings = settings;
            this.statusLabel = statusLabel;
            this.PickedDate = this.settings.alarmTime.Date;
            this.PickedHour = this.settings.alarmTime.Hour;
            this.PickedMinute = this.settings.alarmTime.Minute;
        }
    }
}
