using DateTimeApp.Model.Base;
using DateTimeApp.ViewModel;
using System.Windows;

namespace DateTimeApp.View
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow(ref Settings settings)
        {
            InitializeComponent();
            this.DataContext = new SettingsViewModel(ref settings);
        }
    }
}
