using HappyValentinesDay.UserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace HappyValentinesDay
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _tokenSource?.Cancel();
            Process.GetCurrentProcess().Kill();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _tokenSource = new CancellationTokenSource();

            await StartHeartAsync(PetalBackground);

            var startTime = new DateTime(2020, 5, 2, 12, 0, 0, 0);
            var span = DateTime.Now - startTime;
            var data = new string[]
            {
                $"经历了 {(int)span.TotalDays} 天",
                $"熬过了 {(int) span.TotalHours} 个小时",
                "宝贝，❤",
                "和你在一起的每时每刻我都倍感珍惜",
                "我想你，绝不仅仅是想你",
                "而是想这辈子都和你在一起",
                "不曾对爱情有过过多的奢求",
                "这辈子有你足矣",
                "一想到能和你共度余生",
                "我就对余生充满了期待",
                "我爱你，直至",
                "天荒地老……"
            };
            await StartTextAsync(data, Tb);
        }
    }

    public partial class MainView : Window
    {
        private CancellationTokenSource _tokenSource = null;
        private async Task StartTextAsync(string[] data, TextBlock textElement)
        {
            for (int i = 0; i < data.Length; i++)
            {
                textElement.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1.2)));
                textElement.Text = data[i];
                textElement.HorizontalAlignment = HorizontalAlignment.Center;
                textElement.VerticalAlignment = VerticalAlignment.Center;
                if (i != data.Length - 1)
                {
                    await Task.Delay(3000);
                    textElement.BeginAnimation(OpacityProperty, new DoubleAnimation(1, 0, TimeSpan.FromSeconds(1.5)));
                    await Task.Delay(1500);
                }
            }
        }

        private async Task StartHeartAsync(Canvas panel)
        {
            Random random = new Random();
            await Task.Factory.StartNew(async () =>
             {
                 for (int j = 0; j < 25; j++)
                 {
                     if (_tokenSource.IsCancellationRequested)
                         return;

                     Thread.Sleep(j * 100);
                     await Dispatcher.InvokeAsync(() =>
                     {
                         int snowCount = random.Next(0, 10);
                         for (int i = 0; i < snowCount; i++)
                         {
                             if (_tokenSource.IsCancellationRequested)
                                 return;

                             #region 创建随机爱心
                             int width = random.Next(10, 40);
                             PetalControl heart = new PetalControl
                             {
                                 Width = width,
                                 Height = width,
                                 RenderTransform = new RotateTransform()
                             };

                             int left = random.Next(0, (int)panel.ActualWidth);
                             Canvas.SetLeft(heart, left);
                             panel.Children.Add(heart);
                             #endregion

                             #region 创建渐变动画
                             int seconds = random.Next(20, 30);
                             heart.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1)));
                             #endregion

                             #region 创建下降动画
                             var fallAnimation = new DoubleAnimationUsingPath()
                             {
                                 Duration = new Duration(new TimeSpan(0, 0, seconds)),
                                 RepeatBehavior = RepeatBehavior.Forever,
                                 PathGeometry = new PathGeometry(new List<PathFigure>() { new PathFigure(new Point(left, 0), new List<PathSegment>() { new LineSegment(new Point(left, panel.ActualHeight), false) }, false) }),
                                 Source = PathAnimationSource.Y
                             };
                             heart.BeginAnimation(Canvas.TopProperty, fallAnimation);
                             #endregion

                             #region 创建旋转动画

                             var angleAnimation = new DoubleAnimation(360, new Duration(new TimeSpan(0, 0, 10)))
                             {
                                 RepeatBehavior = RepeatBehavior.Forever,
                             };
                             heart.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, angleAnimation);
                             #endregion
                         }
                     }, DispatcherPriority.Normal, _tokenSource.Token);
                 }
             }, _tokenSource.Token);
        }
    }
}
