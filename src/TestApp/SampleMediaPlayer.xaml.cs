using Panacea.Modularity;
using Panacea.Modularity.Media;
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

namespace TestApp
{
    /// <summary>
    /// Interaction logic for SampleMediaPlayer.xaml
    /// </summary>
    public partial class SampleMediaPlayer : UserControl, IMediaPlayerPlugin
    {
        const int DURATION = 10;
        DispatcherTimer _timer = new DispatcherTimer(DispatcherPriority.Render);
        public SampleMediaPlayer()
        {
            InitializeComponent();
            _timer.Tick += _timer_Tick;
            _timer.Interval = TimeSpan.FromMilliseconds(500);
        }

        event EventHandler<bool> IMediaPlayer.IsPausableChanged
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("dur");
            Position+=1f/DURATION / 2;
            if(Position >= 1)
            {
                _timer.Stop();
                Position = 0;
                Ended?.Invoke(this, EventArgs.Empty);
                Text.Text = "Ended";
            }
            else
            {
                PositionChanged?.Invoke(this, Position);
            }
        }

        public FrameworkElement VideoControl => this;

        public bool IsSeekable { get; private set; }

        public float Position { get; set; }

        public bool IsPlaying { get; private set; }

        public bool HasNext => false;

        public bool HasPrevious => false;

        public bool IsPausable => true;

        public bool HasSubtitles => true;

        public TimeSpan Duration => TimeSpan.FromSeconds(30);

        public event EventHandler Click;
        public event EventHandler<bool> IsSeekableChanged;
        public event EventHandler<float> PositionChanged;
        public event EventHandler IsPausableChanged;
        public event EventHandler Opening;
        public event EventHandler Playing;
        public event EventHandler<string> NowPlaying;
        public event EventHandler Stopped;
        public event EventHandler Paused;
        public event EventHandler Ended;
        public event EventHandler<Exception> Error;
        public event EventHandler<bool> HasSubtitlesChanged;
        public event EventHandler<TimeSpan> DurationChanged;
        public event EventHandler<bool> HasNextChanged;
        public event EventHandler<bool> HasPreviousChanged;

        public bool CanPlayChannel(object channel)
        {
            return true;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool HasMoreChapters()
        {
            throw new NotImplementedException();
        }

        public void Next()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            Text.Text = "Paused";
            Paused?.Invoke(this, null);
            IsPlaying = false;
            _timer.Stop();
        }

        public void Play(MediaItem channel)
        {
            Stop();
            Position = 0;
            IsSeekable = false;
            IsSeekableChanged?.Invoke(this, false);
            Text.Text = "Playing " + channel.Name;
            Opening?.Invoke(this, null);
            Playing?.Invoke(this, null);
            IsPlaying = true;
            DurationChanged?.Invoke(this, TimeSpan.FromSeconds(DURATION));
            _timer.Start();
            NowPlaying?.Invoke(this, channel.Name);
        }

        public void Play()
        {
            Text.Text = "Resuming";
            Playing?.Invoke(this, null);
            IsPlaying = true;
            _timer.Start();
        }

        public void Previous()
        {
            throw new NotImplementedException();
        }

        public void SetSubtitles(bool on)
        {
            throw new NotImplementedException();
        }

        public Task Shutdown()
        {
            return Task.CompletedTask;
        }

        public void Stop()
        {
            Text.Text = "Stopped";
            IsPlaying = false;
            Stopped?.Invoke(this, null);
        }

        Task IPlugin.BeginInit()
        {
            return Task.CompletedTask;
        }

        Task IPlugin.EndInit()
        {
            return Task.CompletedTask;
        }

        private void Grid_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Click?.Invoke(this, EventArgs.Empty);
        }
    }
}
