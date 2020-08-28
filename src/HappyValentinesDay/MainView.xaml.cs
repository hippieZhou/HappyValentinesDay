using HappyValentinesDay.UserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace HappyValentinesDay
{
    public partial class MainView : Window
    {
        private readonly MediaPlayer _mp;
        public MainView()
        {
            InitializeComponent();
            _mp = new MediaPlayer();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _tokenSource?.Cancel();
            Process.GetCurrentProcess().Kill();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PlayRandomMusic(0);

            _tokenSource = new CancellationTokenSource();

            await StartHeartAsync(PetalBackground);

            var startTime = new DateTime(2020, 5, 2, 12, 0, 0, 0);
            var span = DateTime.Now - startTime;
            var data = new string[]
            {
                "亲爱的老婆大人，❤",
                "七夕快乐哟哟哟 ~ ~",
                $"经历了 {(int)span.TotalDays} 天",
                $"熬过了 {(int) span.TotalHours} 个小时",
                "和你在一起的每时每刻我都倍感珍惜",
                "我想你，绝不仅仅是想你",
                "而是想这辈子都和你在一起",
                "不曾对爱情有过太多的奢求",
                "这辈子有你足矣",
                "一想到能和你共度余生",
                "我就对余生充满了期待",
                "Start with you, no end……",
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
                             int width = random.Next(10, 100);
                             PetalControl heart = new PetalControl
                             {
                                 Cursor = Cursors.Hand,
                                 Width = width,
                                 Height = width,
                                 RenderTransform = new RotateTransform()
                             };
                             heart.MouseDoubleClick += (sender, e) => 
                             {
                                 var index = random.Next(0, MusicList.Count() - 1);
                                 PlayRandomMusic(index);
                             };

                             int left = random.Next(0, (int)panel.ActualWidth);
                             Canvas.SetLeft(heart, left);
                             panel.Children.Add(heart);
                             #endregion

                             #region 创建渐变动画
                             int seconds = random.Next(30, 60);
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

        private static IEnumerable<string> MusicList = new List<string>
        {
            "http://218.205.239.34/MIGUM2.0/v1.0/content/sub/listenSong.do?toneFlag=LQ&netType=00&copyrightId=0&contentId=600910000009613592&resourceType=2&channel=0",
            "http://218.205.239.34/MIGUM2.0/v1.0/content/sub/listenSong.do?toneFlag=LQ&netType=00&copyrightId=0&contentId=600908000000398656&resourceType=2&channel=0",
            "http://218.205.239.34/MIGUM2.0/v1.0/content/sub/listenSong.do?toneFlag=LQ&netType=00&copyrightId=0&contentId=600902000002560500&resourceType=2&channel=0",
            "http://218.205.239.34/MIGUM2.0/v1.0/content/sub/listenSong.do?toneFlag=LQ&netType=00&copyrightId=0&contentId=600914000002097907&resourceType=2&channel=0",
            "http://218.205.239.34/MIGUM2.0/v1.0/content/sub/listenSong.do?toneFlag=LQ&netType=00&copyrightId=0&contentId=600902000006889066&resourceType=2&channel=0",
            "http://218.205.239.34/MIGUM2.0/v1.0/content/sub/listenSong.do?toneFlag=LQ&netType=00&copyrightId=0&contentId=600902000009218893&resourceType=2&channel=0",
            "http://218.205.239.34/MIGUM2.0/v1.0/content/sub/listenSong.do?toneFlag=HQ&netType=00&copyrightId=0&contentId=600913000009207857&resourceType=2&channel=0",
            "http://218.205.239.34/MIGUM2.0/v1.0/content/sub/listenSong.do?toneFlag=LQ&netType=00&copyrightId=0&contentId=600910000005536461&resourceType=2&channel=0",
            "http://218.205.239.34/MIGUM2.0/v1.0/content/sub/listenSong.do?toneFlag=LQ&netType=00&copyrightId=0&contentId=600908000008814792&resourceType=2&channel=0",
            "http://218.205.239.34/MIGUM2.0/v1.0/content/sub/listenSong.do?toneFlag=LQ&netType=00&copyrightId=0&contentId=600913000006669361&resourceType=2&channel=0",
            "http://218.205.239.34/MIGUM2.0/v1.0/content/sub/listenSong.do?toneFlag=LQ&netType=00&copyrightId=0&contentId=600908000008738879&resourceType=2&channel=0",
            "http://218.205.239.34/MIGUM2.0/v1.0/content/sub/listenSong.do?toneFlag=LQ&netType=00&copyrightId=0&contentId=600913000004733076&resourceType=2&channel=0",
            "http://218.205.239.34/MIGUM2.0/v1.0/content/sub/listenSong.do?toneFlag=LQ&netType=00&copyrightId=0&contentId=600907000004656284&resourceType=2&channel=0",
            "http://218.205.239.34/MIGUM2.0/v1.0/content/sub/listenSong.do?toneFlag=SQ&netType=00&copyrightId=0&contentId=600916000005730738&resourceType=E&channel=0",
            "http://freetyst.nf.migu.cn/public/product7th/productB16/2020/07/1418/2019%E5%B9%B412%E6%9C%8818%E6%97%A508%E7%82%B910%E5%88%86%E6%89%B9%E9%87%8F%E9%A1%B9%E7%9B%AE%E5%92%AA%E5%92%95Believe179%E9%A6%96-6/%E5%85%A8%E6%9B%B2%E8%AF%95%E5%90%AC/Mp3_64_22_16/6993160VZDN184529.mp3?channelid=03&k=d320f7c6b58589ff&t=1598364024&msisdn=0fbc4fe7-89a5-4a19-920d-35ba05251a87",
        };
        private void PlayRandomMusic(int index)
        {
            _mp.Close();

            var music = MusicList.ElementAt(index);
            var source = new Uri(music);
            _mp.Open(source);
            _mp.MediaEnded += (_s, _e) => { _mp.Position = TimeSpan.FromSeconds(0); _mp.Play(); };
            _mp.Play();
        }
    }
}
