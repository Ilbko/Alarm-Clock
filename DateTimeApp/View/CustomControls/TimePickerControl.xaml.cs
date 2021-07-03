using System.Windows;
using System.Windows.Controls;

namespace DateTimeApp.View
{
    /// <summary>
    /// Логика взаимодействия для TimePickerControl.xaml
    /// </summary>
    public partial class TimePickerControl : UserControl
    {
        public static DependencyProperty HourProperty;
        public int MyHourProperty
        {
            get { return (int)GetValue(HourProperty); }
            set { SetValue(HourProperty, value); }
        }

        public static DependencyProperty MinuteProperty;
        public int MyMinuteProperty
        {
            get { return (int)GetValue(MinuteProperty); }
            set { SetValue(MinuteProperty, value); }
        }
        public TimePickerControl()
        {
            InitializeComponent();
            for (int i = 0; i < 24; i++)
                this.HourComboBox.Items.Add(i);
            for (int i = 0; i < 60; i++)
                this.MinuteComboBox.Items.Add(i);
        }
        static TimePickerControl()
        {
            HourProperty = DependencyProperty.Register("MyHourProperty", typeof(int), typeof(TimePickerControl),
                new FrameworkPropertyMetadata(0, new PropertyChangedCallback(HourPropertyChanged)));
            MinuteProperty = DependencyProperty.Register("MyMinuteProperty", typeof(int), typeof(TimePickerControl),
                new FrameworkPropertyMetadata(0, new PropertyChangedCallback(MinutePropertyChanged)));
        }

        private static void HourPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TimePickerControl))
                return;

            TimePickerControl control = (TimePickerControl)d;
            
            if (e.Property == HourProperty)
            {
                control.HourComboBox.SelectedIndex = (int)e.NewValue;
            }
        }
        private static void MinutePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TimePickerControl))
                return;

            TimePickerControl control = (TimePickerControl)d;
            
            if (e.Property == MinuteProperty)
            {
                control.MinuteComboBox.SelectedIndex = (int)e.NewValue;
            }
        }

        private void HourComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MyHourProperty = (sender as ComboBox).SelectedIndex;
        }

        private void MinuteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MyMinuteProperty = (sender as ComboBox).SelectedIndex;
        }
    }
}
