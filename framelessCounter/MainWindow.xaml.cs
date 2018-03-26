using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace framelessCounter
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public DispatcherTimer Timer = new DispatcherTimer();
        DateTime startTime = DateTime.Now;
        public MainWindow()
        {
            InitializeComponent();
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan diff = currentTime.Subtract(startTime);
            if ((1 - diff.Minutes) >= 0)
            {
            }
            else { 
                clockText.Foreground = new SolidColorBrush(Colors.Red);
            }
            clockText.Content = ((1 - diff.Minutes)>=0? (1 - diff.Minutes).ToString() :"+"+(diff.Minutes-2)) + " : " + ((1 - diff.Minutes) >= 0 ? (60 - diff.Seconds):diff.Seconds);
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }
        
    }
}
