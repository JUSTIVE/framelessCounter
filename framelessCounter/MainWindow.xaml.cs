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
        private const int targetTime = 20;
        public bool isplaying = false;
        public DispatcherTimer Timer = new DispatcherTimer();
        private DateTime startTime;
        private bool onlyOnce = true;
        public MainWindow()
        {
            InitializeComponent();
            clockText.Content = "20 : 00";
        }

        ~MainWindow()
        {
            Timer.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan diff = currentTime.Subtract(startTime);
            if (((targetTime - 1) - diff.Minutes) < 0)
                clockText.Foreground = new SolidColorBrush(Colors.Red);
            clockText.Content = (((targetTime-1) - diff.Minutes)>=0
                                    ? ((targetTime - 1) - diff.Minutes).ToString() 
                                    :"+"+(diff.Minutes- (targetTime + 1))) 
                                + " : " + 
                                (((targetTime - 1) - diff.Minutes) < 0 
                                    ? diff.Seconds<10
                                      ? "0" + diff.Seconds
                                      : diff.Seconds.ToString()
                                    : (59- diff.Seconds)<10
                                        ? "0"+ (59 - diff.Seconds) 
                                        : (59 - diff.Seconds).ToString());
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click_play(object sender, RoutedEventArgs e)
        {

            if (!isplaying)
            {
                //now starting 
                startTime = DateTime.Now;
                if (onlyOnce)
                {
                    Timer.Interval = TimeSpan.FromSeconds(0.5);
                    Timer.Tick += new EventHandler(Timer_Tick);
                    onlyOnce = false;
                }
                Timer.Start();
            }
            isplaying = !isplaying;
        }
        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            startTime = DateTime.Now;
            Timer.Stop();
            clockText.Content = "20 : 00";
        }
    }
}
