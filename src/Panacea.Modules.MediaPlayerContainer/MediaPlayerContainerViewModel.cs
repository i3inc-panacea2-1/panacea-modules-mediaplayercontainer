using Panacea.Controls;
using Panacea.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Panacea.Modules.MediaPlayerContainer
{

    [View(typeof(MediaPlayerContainerControl))]
    public class MediaPlayerContainerViewModel : ViewModelBase
    {
        private readonly MediaPlayerContainer _container;
        Window _fullscreenWindow;
        private TimeSpan _totalTime;

        public MediaPlayerContainerViewModel(MediaPlayerContainer container)
        {
            _container = container;
            _container.Opening += _container_Opening;
            _container.Error += _container_Error;
            _container.Playing += _container_Playing;
            _container.Paused += _container_Paused;
            _container.Stopped += _container_Stopped;
            _container.DurationChanged += _container_DurationChanged;
            _container.PositionChanged += _container_PositionChanged;
            _container.Ended += _container_Ended;
            _container.Click += _container_Click;
            _container.HasNextChanged += _container_HasNextChanged;
            _container.HasPreviousChanged += _container_HasPreviousChanged;
            _container.NowPlaying += _container_NowPlaying;
            _container.HasSubtitlesChanged += _container_HasSubtitlesChanged;
            _container.IsSeekableChanged += _container_IsSeekableChanged;
            PauseCommand = new RelayCommand((args) =>
            {
                if (_container.IsPlaying)
                    _container.Pause();
                else _container.Play();
            });

            StopCommand = new RelayCommand(
                (args) => _container.Stop());

            PreviousCommand = new RelayCommand((args) => _container.Previous(), (args) => _container.CurrentMediaPlayer?.HasPrevious == true);

            NextCommand = new RelayCommand((args) => _container.Next(), (args) => _container.CurrentMediaPlayer?.HasNext == true);

            FullscreenCommand = new RelayCommand(args =>
            {
                container.GoFullscreen();
            });

            PipCommand = new RelayCommand(args =>
            {
                container.GoToPip();
            });

            
        }

        private void _container_IsSeekableChanged(object sender, bool e)
        {
            IsSeekable = e;
        }

        private void _container_HasSubtitlesChanged(object sender, bool e)
        {
            HasClosedCaptions = e;
            _container.CurrentMediaPlayer.SetSubtitles(ClosedCaptionsEnabled);
        }

        public override Type GetViewType()
        {
            var t = base.GetViewType();
            return t;
        }

        bool _closedCaptionsEnabled;
        public bool ClosedCaptionsEnabled
        {
            get => _closedCaptionsEnabled;
            set
            {
                _closedCaptionsEnabled = value;
                _container.SetSubtitles(value);
                OnPropertyChanged();
            }
        }


        bool _isSeekable;
        public bool IsSeekable
        {
            get => _isSeekable;
            set
            {
                _isSeekable = value;
                OnPropertyChanged();
            }
        }

        bool _hasSubtitles;
        public bool HasClosedCaptions
        {
            get => _hasSubtitles;
            set
            {
                _hasSubtitles = value;
                OnPropertyChanged();
            }
        }

        bool _isPlaying;
        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                _isPlaying = value;
                OnPropertyChanged();
            }
        }

        bool _isVideoVisible;
        public bool VideoVisible
        {
            get => _isVideoVisible;
            set
            {
                _isVideoVisible = value;
                OnPropertyChanged();
            }
        }

        bool _areControlsVisible;
        public bool AreControlsVisible
        {
            get => _areControlsVisible;
            set
            {
                _areControlsVisible = value;
                OnPropertyChanged();
            }
        }

        bool _previousButtonVisible;
        public bool PreviousButtonVisible
        {
            get => _previousButtonVisible;
            set
            {
                _previousButtonVisible = value;
                OnPropertyChanged();
            }
        }

        bool _nextButtonVisible;
        public bool NextButtonVisible
        {
            get => _nextButtonVisible;
            set
            {
                _nextButtonVisible = value;
                OnPropertyChanged();
            }
        }

        bool _pauseButtonVisible;
        public bool PauseButtonVisible
        {
            get => _pauseButtonVisible;
            set
            {
                _pauseButtonVisible = value;
                OnPropertyChanged();
            }
        }

        bool _stopButtonVisible;
        public bool StopButtonVisible
        {
            get => _stopButtonVisible;
            set
            {
                _stopButtonVisible = value;
                OnPropertyChanged();
            }
        }

        bool _switchPlayerButtonVisible;
        public bool SwitchPlayerButtonVisible
        {
            get => _switchPlayerButtonVisible;
            set
            {
                _switchPlayerButtonVisible = value;
                OnPropertyChanged();
            }
        }

        string _nowPlayingText;
        public string NowPlayingText
        {
            get => _nowPlayingText;
            set
            {
                _nowPlayingText = value;
                OnPropertyChanged();
            }
        }

        string _currentTimeText;
        public string CurrentTimeText
        {
            get => _currentTimeText;
            set
            {
                _currentTimeText = value;
                OnPropertyChanged();
            }
        }

        string _totalTimeText;
        public string TotalTimeText
        {
            get => _totalTimeText;
            set
            {
                _totalTimeText = value;
                OnPropertyChanged();
            }
        }

        bool _dragging = false;

        CancellationTokenSource _cts;
        double _seekbarValue;
        public double SeekbarValue
        {
            get => _seekbarValue;
            set
            {
                _seekbarValue = value;
                _dragging = true;
                _cts?.Cancel();
                var cts = new CancellationTokenSource();
                _cts = cts;
                Task.Delay(TimeSpan.FromMilliseconds(500)).ContinueWith(task =>
                {
                    if (cts.IsCancellationRequested) return;
                    _container.Position = (float)(_seekbarValue/100.0);
                    _dragging = false;
                });

                OnPropertyChanged();
            }
        }

        string _pauseButtonIcon;
        public string PauseButtonIcon
        {
            get => _pauseButtonIcon;
            set
            {
                _pauseButtonIcon = value;
                OnPropertyChanged();
            }
        }

        FrameworkElement _currentVideoControl;
        public FrameworkElement CurrentVideoControl
        {
            get => _currentVideoControl;
            set
            {
                var grid = new Grid();
                grid.Children.Add(value);
                _currentVideoControl = grid;
                OnPropertyChanged();
            }
        }

        private void _container_NowPlaying(object sender, string e)
        {
            NowPlayingText = e;
        }

        private void _container_HasPreviousChanged(object sender, bool e)
        {
            PreviousButtonVisible = e;
        }

        private void _container_HasNextChanged(object sender, bool e)
        {
            NextButtonVisible = e;
        }

        private void _container_Click(object sender, EventArgs e)
        {
            if (_fullscreenWindow != null)
            {
                _fullscreenWindow.Close();
            }
        }

        private void _container_Ended(object sender, EventArgs e)
        {
            IsPlaying = false;
            _fullscreenWindow?.Close();
        }

        private void _container_PositionChanged(object sender, float e)
        {
            CurrentTimeText = TimeSpan.FromMilliseconds(_totalTime.TotalMilliseconds * e).ToString("hh\\:mm\\:ss");
            if (!_dragging)
            {
                _seekbarValue = e * 100.0;
                OnPropertyChanged("SeekbarValue");
            }
        }

        private void _container_DurationChanged(object sender, TimeSpan e)
        {
            _totalTime = e;
            TotalTimeText = e.ToString("hh\\:mm\\:ss");

        }

        private void _container_Stopped(object sender, EventArgs e)
        {
            IsPlaying = false;
            PauseButtonVisible = StopButtonVisible = false;
            _fullscreenWindow?.Close();
        }

        private void _container_Paused(object sender, EventArgs e)
        {
            PauseButtonIcon = "play_arrow";
        }

        private void _container_Opening(object sender, EventArgs e)
        {
            SeekbarValue = 0.0;
            IsPlaying = true;
            SwitchPlayerButtonVisible = _container.AvailablePlayers.Count > 1;
        }

        private void _container_Playing(object sender, EventArgs e)
        {
            IsPlaying = true;
            _container.CurrentMediaPlayer.VideoControl.RemoveChild();
            CurrentVideoControl = _container.CurrentMediaPlayer.VideoControl;
            PauseButtonIcon = "pause";
            PauseButtonVisible = StopButtonVisible = true;
          
        }

        private void _container_Error(object sender, Exception e)
        {
            IsPlaying = false;
        }


        private void W_Closed(object sender, EventArgs e)
        {
            _fullscreenWindow.Closed -= W_Closed;
            _fullscreenWindow = null;
            _container.CurrentMediaPlayer.VideoControl.RemoveChild();
            CurrentVideoControl = _container.CurrentMediaPlayer.VideoControl;
        }
        public override void Deactivate()
        {
            Deactivated?.Invoke(this, null);
        }

        public override void Activate()
        {
            Activated?.Invoke(this, null);
        }

        public event EventHandler Deactivated;
        public event EventHandler Activated;

        public RelayCommand PreviousCommand { get; }

        public RelayCommand NextCommand { get; }

        public RelayCommand StopCommand { get; }

        public RelayCommand PauseCommand { get; }

        public RelayCommand FullscreenCommand { get; }

        public RelayCommand PipCommand { get; }


    }

}
