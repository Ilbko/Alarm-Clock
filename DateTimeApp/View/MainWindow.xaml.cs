using DateTimeApp.ViewModel;
using System.Windows;
using System.ComponentModel;

namespace DateTimeApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TimeViewModel timeViewModel;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this.timeViewModel = new TimeViewModel(ref this.MainGrid);
            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e) => this.timeViewModel.SaveSettings();
    }
}
