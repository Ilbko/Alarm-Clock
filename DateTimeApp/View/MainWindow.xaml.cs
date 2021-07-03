using DateTimeApp.ViewModel;
using System.Windows;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System;
using DateTimeApp.Model;

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
            
            string path = Path.GetTempPath() + @"\AlarmLog" + DateTime.Now.ToString("yyyy-MM-dd;HH-mm-ss") + ".log";

            TextWriterTraceListener text = new TextWriterTraceListener(path);
            Trace.Listeners.Add(text);
            Trace.AutoFlush = true;
            Logic.Log("Был установлен текстовый слушатель трассировки.", -1);

            this.DataContext = this.timeViewModel = new TimeViewModel(ref this.MainGrid);
            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e) 
        {
            this.timeViewModel.SaveSettings();
            Logic.Log("Программа была закрыта.", 0);
        }
    }
}
