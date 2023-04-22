using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfTimer {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {

        private bool _timerStart = false;
        private int _lastTimer = 5;
        private DispatcherTimer _dispatcherNow;
        private Stopwatch _stopwatch = new Stopwatch();

        public MainWindow() {
            InitializeComponent();
            _dispatcherNow = new DispatcherTimer {
                Interval = TimeSpan.FromMilliseconds(250.0)
            };
            _dispatcherNow.Tick += _dispatcherNow_Tick;
        }

        private void Menu_TopMostTrue_Click(object sender, RoutedEventArgs e) {
            Topmost = true;
        }

        private void Menu_TopMostFalse_Click(object sender, RoutedEventArgs e) {
            Topmost = false;
        }

        private void Menu_TitleShow_Click(object sender, RoutedEventArgs e) {
            WindowStyle = WindowStyle.ToolWindow;
            Height += 25.0;
        }

        private void Menu_TitleHide_Click(object sender, RoutedEventArgs e) {
            WindowStyle = WindowStyle.None;
            Height -= 25.0;
        }

        private void Menu_Exit_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void BTMain_Click(object sender, RoutedEventArgs e) {
            _timerStart = !_timerStart;
            if (_timerStart) {
                _lastTimer = Convert.ToInt32(TBTimer.Text);
                if (_lastTimer > 60) {
                    Width = 220.0;
                } else {
                    Width = 175.0;
                }
                _stopwatch.Start();
                _dispatcherNow.Start();
                BTMain.Content = "停止计时";
                TBTimer.Background = Brushes.Black;
                TBTimer.Foreground = Brushes.White;
                TBTimer.IsReadOnly = true;
            } else {
                TBTimer.IsReadOnly = false;
                _dispatcherNow.Stop();
                _stopwatch.Reset();
                TBTimer.Text = _lastTimer.ToString();
                BTMain.Content = "启动计时";
                TBTimer.Background = Brushes.Black;
                TBTimer.Foreground = Brushes.White;
            }
        }

        private void _dispatcherNow_Tick(object sender, EventArgs e) {
            int numSecond = _lastTimer * 60 - Convert.ToInt32(_stopwatch.ElapsedMilliseconds / 1000);
            TimeSpan timeSpan = new TimeSpan(0, 0, numSecond);
            if (_lastTimer > 60) {
                TBTimer.Text = timeSpan.Hours.ToString("F0") + "时" + timeSpan.Minutes.ToString("F0") + "分" + timeSpan.Seconds.ToString("F0") + "秒";
            } else {
                TBTimer.Text = timeSpan.Minutes.ToString("F0") + "分" + timeSpan.Seconds.ToString("F0") + "秒";
            }
            if (numSecond <= 30 && numSecond >= 0) {
                TBTimer.Background = Brushes.Yellow;
                TBTimer.Foreground = Brushes.Black;
            }
            if (numSecond <= 0) {
                TBTimer.Background = Brushes.Red;
                TBTimer.Foreground = Brushes.White;
            }
        }

        private void TBTimer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (_timerStart) {
                DragMove();
            }
        }

        private void TBTimer_KeyDown(object sender, KeyEventArgs e) {
            if (!_timerStart && e.Key == Key.Enter) {
                BTMain_Click(null, null);
            }
        }
    }
}
