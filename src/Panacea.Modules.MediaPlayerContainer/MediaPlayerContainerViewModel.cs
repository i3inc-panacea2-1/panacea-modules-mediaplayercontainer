using Panacea.Controls;
using Panacea.Modularity.MediaPlayerContainer;
using Panacea.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            PauseCommand = new RelayCommand((args) =>
            {
                if (_container.IsPlaying)
                    _container.Pause();
                else _container.Play();
            });

            StopCommand = new RelayCommand(
                (args) => _container.Stop());

            PreviousCommand = new RelayCommand((args) => _container.Previous(), (args)=>_container.CurrentMediaPlayer?.HasPrevious == true);

            NextCommand = new RelayCommand((args) => _container.Next(), (args) => _container.CurrentMediaPlayer?.HasNext == true);
        }

        public override Type GetViewType()
        {
            var t = base.GetViewType();
            return t;
        }

        Visibility _previousButtonVisibility;
        public Visibility PreviousButtonVisibility
        {
            get => _previousButtonVisibility;
            set
            {
                _previousButtonVisibility = value;
                OnPropertyChanged();
            }
        }

        Visibility _nextButtonVisibility;
        public Visibility NextButtonVisibility
        {
            get => _nextButtonVisibility;
            set
            {
                _nextButtonVisibility = value;
                OnPropertyChanged();
            }
        }

        Visibility _pauseButtonVisibility;
        public Visibility PauseButtonVisibility
        {
            get => _pauseButtonVisibility;
            set
            {
                _pauseButtonVisibility = value;
                OnPropertyChanged();
            }
        }

        Visibility _stopButtonVisibility;
        public Visibility StopButtonVisibility
        {
            get => _stopButtonVisibility;
            set
            {
                _stopButtonVisibility = value;
                OnPropertyChanged();
            }
        }

        Visibility _switchPlayerButtonVisibility;
        public Visibility SwitchPlayerButtonVisibility
        {
            get => _switchPlayerButtonVisibility;
            set
            {
                _switchPlayerButtonVisibility = value;
                OnPropertyChanged();
            }
        }

        Visibility _contentGridVisibility = Visibility.Collapsed;
        public Visibility ContentGridVisibility
        {
            get => _contentGridVisibility;
            set
            {
                _contentGridVisibility = value;
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

        double _seekbarValue;
        public double SeekbarValue
        {
            get => _seekbarValue;
            set
            {
                _seekbarValue = value;
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

        UIElement _currentVideoControl;
        public UIElement CurrentVideoControl
        {
            get => _currentVideoControl;
            set
            {
                _currentVideoControl = value;
                OnPropertyChanged();
            }
        }

        private void _container_NowPlaying(object sender, string e)
        {
            NowPlayingText = e;
        }

        private void _container_HasPreviousChanged(object sender, bool e)
        {
            PreviousButtonVisibility = e ? Visibility.Visible : Visibility.Collapsed;
        }

        private void _container_HasNextChanged(object sender, bool e)
        {
            NextButtonVisibility = e ? Visibility.Visible : Visibility.Collapsed;
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
            ContentGridVisibility = Visibility.Collapsed;
            _fullscreenWindow?.Close();
        }

        private void _container_PositionChanged(object sender, float e)
        {
            CurrentTimeText = TimeSpan.FromMilliseconds(_totalTime.TotalMilliseconds * e).ToString("hh\\:mm\\:ss");
            SeekbarValue = e * 100;
        }

        private void _container_DurationChanged(object sender, TimeSpan e)
        {
            _totalTime = e;
            TotalTimeText = e.ToString("hh\\:mm\\:ss");

        }

        private void _container_Stopped(object sender, EventArgs e)
        {
            PauseButtonVisibility = StopButtonVisibility = Visibility.Collapsed;
            ContentGridVisibility = Visibility.Collapsed;
            _fullscreenWindow?.Close();
        }

        private void _container_Paused(object sender, EventArgs e)
        {
            PauseButtonIcon = "play_arrow";
        }

        private void _container_Opening(object sender, EventArgs e)
        {
            ContentGridVisibility = Visibility.Visible;
            SwitchPlayerButtonVisibility = _container.AvailablePlayers.Count > 1 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void _container_Playing(object sender, EventArgs e)
        {
            _container_PositionChanged(this, _container.CurrentMediaPlayer.Position);
            RemoveChild(_container.CurrentMediaPlayer.VideoControl);
            CurrentVideoControl = _container.CurrentMediaPlayer.VideoControl;
            PauseButtonIcon = "pause";
            PauseButtonVisibility = StopButtonVisibility = Visibility.Visible;
        }

        private void _container_Error(object sender, Exception e)
        {

        }

        public static void RemoveChild(FrameworkElement element)
        {
            var objs = new List<DependencyObject>()
            {
                 element.Parent,
                 VisualTreeHelper.GetParent(element)
            };
            foreach (var dobjs in objs.Where(o => o != null))
            {
                var panel = dobjs as Panel;
                if (panel != null)
                {
                    panel.Children.Remove(element);
                    return;
                }

                var decorator = dobjs as Decorator;
                if (decorator != null)
                {
                    if (decorator.Child == element)
                    {
                        decorator.Child = null;
                    }
                    return;
                }

                var contentPresenter = dobjs as ContentPresenter;
                if (contentPresenter != null)
                {
                    if (contentPresenter.Content == element)
                    {
                        contentPresenter.Content = null;
                    }
                    return;
                }

                var contentControl = dobjs as ContentControl;
                if (contentControl != null)
                {
                    if (contentControl.Content == element)
                    {
                        contentControl.Content = null;
                    }
                }
            }
        }



        private void FullscreenButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveChild(_container.CurrentMediaPlayer.VideoControl);
            _fullscreenWindow = new Window()
            {
                ShowInTaskbar = false,
                WindowStyle = WindowStyle.None,
                ResizeMode = ResizeMode.NoResize,
                WindowState = WindowState.Maximized,
                Content = _container.CurrentMediaPlayer.VideoControl
            };
            _fullscreenWindow.Closed += W_Closed;
            _fullscreenWindow.Show();
        }

        private void W_Closed(object sender, EventArgs e)
        {
            _fullscreenWindow.Closed -= W_Closed;
            _fullscreenWindow = null;
            RemoveChild(_container.CurrentMediaPlayer.VideoControl);
            CurrentVideoControl = _container.CurrentMediaPlayer.VideoControl;
        }

        public RelayCommand PreviousCommand { get; }

        public RelayCommand NextCommand { get; }

        public RelayCommand StopCommand { get; }

        public RelayCommand PauseCommand { get; }


    }

}
