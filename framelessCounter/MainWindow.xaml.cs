using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Media.Effects;


namespace framelessCounter
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private enum  Corner
        {
            Lefttop,
            Leftbottom,
            RightTop,
            RightBottom
        }

        private Corner corner = Corner.RightBottom;
        private int targetTime = 20;
        public bool isplaying;
        public DispatcherTimer timer = new DispatcherTimer();
        private DateTime startTime;
        private bool onlyOnce = true;
        private bool settingOn;


        private int hiddenOptions = 0;

        public MainWindow()
        {
            InitializeComponent();
            //ExtraSettingPage.Visibility = Visibility.Hidden;
            clockText.Content = "20 : 00";
            SettingPage.Visibility = Visibility.Hidden;
            Slider.Value = (20/6.0);
            Mover();
        }

        ~MainWindow()
        {
            timer.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan diff = currentTime.Subtract(startTime);
            
            clockText.Foreground = new SolidColorBrush((((targetTime - 1) - diff.Minutes) < 5)?Colors.Red: Color.FromRgb(138, 234, 146));
            clockText.Content = (((targetTime-1) - diff.Minutes)>=0
                                    ? ((targetTime - 1) - diff.Minutes)>=10
                                        ?((targetTime - 1) - diff.Minutes).ToString() 
                                        : "0"+((targetTime - 1) - diff.Minutes)
                                    : "+"+(diff.Minutes- (targetTime))) 
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
            settingOn = false;
            SettingPaneController();
            Window window = (Window)sender;
            window.Topmost = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click_play(object sender, RoutedEventArgs e)
        {
            ((Path)((Canvas)pausePlayButton.Content).Children[0]).Fill = new SolidColorBrush(Colors.DimGray);
            if (!isplaying)
            {
                //now starting 
                startTime = DateTime.Now;
                if (onlyOnce)
                {
                    timer.Interval = TimeSpan.FromSeconds(0.1);
                    timer.Tick += Timer_Tick;
                    onlyOnce = false;
                }
                timer.Start();
            }
            isplaying = !isplaying;
        }
        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            ((Path)((Canvas)pausePlayButton.Content).Children[0]).Fill = new SolidColorBrush(Colors.White);
            isplaying = false;
            startTime = DateTime.Now;
            timer.Stop();
            clockText.Content = ((targetTime < 10) ? "0" + targetTime : targetTime.ToString()) + " : 00";
        }

        void SettingPaneController()
        {
            SettingPage.Visibility = !settingOn ? Visibility.Hidden : Visibility.Visible;
        }

        private void settingButton_Click(object sender, RoutedEventArgs e)
        {
            settingOn = !settingOn;
            SettingPaneController();
            hiddenOptions++;
            if (hiddenOptions >= 7)
            {
                ExtraSettingPage.Visibility = (ExtraSettingPage.Visibility == Visibility.Visible)
                    ? Visibility.Hidden
                    : Visibility.Visible;
                hiddenOptions = 0;
            }
        }

        void Mover()
        {
            if (corner == Corner.Lefttop || corner == Corner.RightTop)//위일때
            {
                Bottomarrow.Visibility = Visibility.Hidden;
                Toparrow.Visibility = Visibility.Visible;
                SettingPage.Margin = new Thickness(15,121,15,15);
                SettingPage.VerticalAlignment = VerticalAlignment.Top;
                SettingPage.HorizontalAlignment =
                    (corner == Corner.Lefttop) ? HorizontalAlignment.Left : HorizontalAlignment.Right;
            }
            else
            {
                Toparrow.Visibility = Visibility.Hidden;
                Bottomarrow.Visibility = Visibility.Visible;
                SettingPage.Margin = new Thickness(15, 15, 15, 121);
                SettingPage.VerticalAlignment = VerticalAlignment.Bottom;
                SettingPage.HorizontalAlignment =
                    (corner == Corner.Leftbottom) ? HorizontalAlignment.Left : HorizontalAlignment.Right;
            }
            Leftbottombutton.Background = new SolidColorBrush((corner==Corner.Leftbottom)? Color.FromRgb(138, 234, 146) : Colors.White);
            Rightbottombutton.Background = new SolidColorBrush((corner == Corner.RightBottom) ? Color.FromRgb(138, 234, 146) : Colors.White);
            Lefttopbutton.Background = new SolidColorBrush((corner == Corner.Lefttop) ? Color.FromRgb(138, 234, 146) : Colors.White);
            Righttopbutton.Background = new SolidColorBrush((corner == Corner.RightTop) ? Color.FromRgb(138, 234, 146) : Colors.White);

            MainPage.HorizontalAlignment = (corner == Corner.Leftbottom || corner == Corner.Lefttop)
                ? HorizontalAlignment.Left
                : HorizontalAlignment.Right;
            MainPage.VerticalAlignment = (corner == Corner.Lefttop || corner == Corner.RightTop)
                ? VerticalAlignment.Top
                : VerticalAlignment.Bottom;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            targetTime = (int)(e.NewValue*59/10.0)+1;
            targetTimeLabel.Content = targetTime;
            DateTime currentTime = DateTime.Now;
            TimeSpan diff = currentTime.Subtract(startTime);
            
            if (onlyOnce)
                clockText.Content = ((targetTime<10)?"0"+targetTime:targetTime.ToString())+ " : 00";
            else
            {
                clockText.Foreground = new SolidColorBrush((((targetTime - 1) - diff.Minutes) < 5) ? Colors.Red : Color.FromRgb(138, 234, 146));
            }
        }

        private void Lefttopbutton_Click(object sender, RoutedEventArgs e)
        {
            corner = Corner.Lefttop;
            Mover();
        }

        private void Leftbottombutton_Click(object sender, RoutedEventArgs e)
        {
            corner = Corner.Leftbottom;
            Mover();
        }

        private void Righttopbutton_Click(object sender, RoutedEventArgs e)
        {
            corner = Corner.RightTop;
            Mover();
        }

        private void Rightbottombutton_Click(object sender, RoutedEventArgs e)
        {
            corner = Corner.RightBottom;
            Mover();
        }

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
        }
        private void settingButton_LostFocus(object sender, RoutedEventArgs e)
        {
            hiddenOptions = 0;
        }
    }
}
