using HappyValentinesDay.UserControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HappyValentinesDay
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }


     

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await StartHeart(PetalBackground);
            String data = "我一直在等待你的出现^" +
    "谢谢你选择了我^" +
    "此生不换^" +
    "执子之手，与子偕老^" +
    "携手到永远……";
            await StartTextAsync(data.Split('^'), Tb);
        }
    }

    public partial class MainView : Window
    {
        private async Task StartTextAsync(string[] data, TextBlock textElement)
        {
            for (int i = 0; i < data.Length; i++)
            {
                textElement.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1.2)));
                textElement.Text = data[i];
                if (i != data.Length - 1)
                {
                    await Task.Delay(3000);
                    textElement.BeginAnimation(OpacityProperty, new DoubleAnimation(1, 0, TimeSpan.FromSeconds(1.5)));
                    await Task.Delay(1500);
                }
            }
        }

        private async Task StartHeart(Canvas panel)
        {
            Random random = new Random();
            await Task.Factory.StartNew(async () =>
             {
                 for (int j = 0; j < 25; j++)
                 {
                     Thread.Sleep(j * 100);
                     await Dispatcher.InvokeAsync(() =>
                     {
                         int snowCount = random.Next(0, 10);
                         for (int i = 0; i < snowCount; i++)
                         {
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
                     });
                 }
             });
        }
    }
}
