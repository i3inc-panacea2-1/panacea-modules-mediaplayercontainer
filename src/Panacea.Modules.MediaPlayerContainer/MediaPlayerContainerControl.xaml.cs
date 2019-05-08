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

namespace Panacea.Modules.MediaPlayerContainer
{
    /// <summary>
    /// Interaction logic for MediaPlayerContainerControl.xaml
    /// </summary>
    public partial class MediaPlayerContainerControl : UserControl
    {
        private readonly MediaPlayerContainer _container;
        Window _fullscreenWindow;
        private TimeSpan _totalTime;
        internal MediaPlayerContainerControl(MediaPlayerContainer container)
        {
            _container = container;
            InitializeComponent();
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
            ContentGrid.Visibility = Visibility.Collapsed;
        }

        private void _container_NowPlaying(object sender, string e)
        {
            nowplaying.Text = e;
        }

        private void _container_HasPreviousChanged(object sender, bool e)
        {
            PreviousButton.Visibility = e ? Visibility.Visible : Visibility.Collapsed;
        }

        private void _container_HasNextChanged(object sender, bool e)
        {
            NextButton.Visibility = e ? Visibility.Visible : Visibility.Collapsed;
        }

        private void _container_Click(object sender, EventArgs e)
        {
            if(_fullscreenWindow != null)
            {
                _fullscreenWindow.Close();
            }
        }

        private void _container_Ended(object sender, EventArgs e)
        {
            ContentGrid.Visibility = Visibility.Collapsed;
            _fullscreenWindow?.Close();
        }

        private void _container_PositionChanged(object sender, float e)
        {
            currentTime.Text = TimeSpan.FromMilliseconds(_totalTime.TotalMilliseconds * e).ToString("hh\\:mm\\:ss");
            slider.Value = e * 100;
        }

        private void _container_DurationChanged(object sender, TimeSpan e)
        {
            _totalTime = e;
            totalTime.Text = e.ToString("hh\\:mm\\:ss");
            
        }

        private void _container_Stopped(object sender, EventArgs e)
        {
            PauseButton.Visibility = StopButton.Visibility = Visibility.Collapsed;
            ContentGrid.Visibility = Visibility.Collapsed;
            _fullscreenWindow?.Close();
        }

        private void _container_Paused(object sender, EventArgs e)
        {
            PauseButtonIcon.Icon = "play_arrow";
        }

        private void _container_Opening(object sender, EventArgs e)
        {
            ContentGrid.Visibility = Visibility.Visible;
            SwitchPlayerButton.Visibility = _container.AvailablePlayers.Count > 1 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void _container_Playing(object sender, EventArgs e)
        {
            _container_PositionChanged(this, _container.CurrentMediaPlayer.Position);
            RemoveChild(_container.CurrentMediaPlayer.VideoControl);
            VideoContainer.Child = _container.CurrentMediaPlayer.VideoControl;
            PauseButtonIcon.Icon = "pause";
            PauseButton.Visibility = StopButton.Visibility = Visibility.Visible;
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

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_container.IsPlaying)
                _container.Pause();
            else _container.Play();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _container.Stop();
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
            VideoContainer.Child = _container.CurrentMediaPlayer.VideoControl;
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            _container.Previous();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            _container.Next();
        }
    }
}
