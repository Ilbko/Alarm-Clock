using DateTimeApp.Model.Base;
using DateTimeApp.ViewModel;
using System;
using System.Windows;

namespace DateTimeApp.View
{
    /// <summary>
    /// Логика взаимодействия для AlarmWindow.xaml
    /// </summary>
    public partial class AlarmWindow : Window
    {
        public AlarmWindow(ref Settings settings)
        {
            InitializeComponent();

            this.StatusLabel.Content = settings.alarmTime < DateTime.Now
                ? "Будильник не заведён"
                : "Будильник заведён на " + settings.alarmTime.ToString();
            this.DataContext = new AlarmViewModel(ref settings, ref this.StatusLabel);
        }
    }
}
