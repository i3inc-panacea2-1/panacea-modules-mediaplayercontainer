﻿using Panacea.Controls;
using Panacea.Core;
using Panacea.Modularity.AudioManager;
using Panacea.Modularity.MediaPlayerContainer;
using Panacea.Modularity.UiManager;
using Panacea.Multilinguality;
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
    public class MediaPlayerContainerViewModel : PopupViewModelBase<object>
    {
        private readonly PanaceaServices _core;
        private readonly MediaPlayerContainer _container;
        Window _fullscreenWindow;
        private TimeSpan _totalTime;

        public MediaPlayerContainerViewModel(MediaPlayerContainer container, PanaceaServices core)
        {
            _core = core;
            _container = container;
            _container.ResponseChanged += _container_ResponseChanged;
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
            VolumeUpCommand = new RelayCommand(args =>
            {
                if (core.TryGetAudioManager(out IAudioManager audio))
                {
                    var v = audio.SpeakersVolume;
                    audio.SpeakersVolume = RoundBy5Up(v) + 5;
                }
            },
            args =>
            {
                if (core.TryGetAudioManager(out IAudioManager audio))
                {
                    return audio.SpeakersVolume < 100;
                }
                return false;
            });

            VolumeDownCommand = new RelayCommand(args =>
            {
                if (core.TryGetAudioManager(out IAudioManager audio))
                {
                    var v = audio.SpeakersVolume;
                    audio.SpeakersVolume = RoundBy5Down(v) - 5;
                }
            },
            args =>
            {
                if (core.TryGetAudioManager(out IAudioManager audio))
                {
                    return audio.SpeakersVolume > 0;
                }
                return false;
            });
        }

        public override void Close()
        {
            _container.Stop();
            //if(_core.TryGetUiManager(out IUiManager ui))
            //{
            //    ui.HidePopup(this);
            //}
        }

        int RoundBy5Down(int v)
        {
            return (int)(v / 10 * 10.0 + Math.Ceiling(v % 10 / 5.0) * 5);
        }

        int RoundBy5Up(int v)
        {
            return (int)(v / 10 * 10.0 + Math.Floor(v % 10 / 5.0) * 5);
        }

        IMediaResponse _mediaResponse;
        private void _container_ResponseChanged(object sender, IMediaResponse e)
        {
            if (_mediaResponse != null)
            {
                DetachFromResponse(_mediaResponse);
            }
            if (e != null)
            {
                _mediaResponse = e;
                AttachToMediaResponse(e);
            }
        }

        void AttachToMediaResponse(IMediaResponse response)
        {
            response.Opening += _container_Opening;
            response.Error += _container_Error;
            response.Playing += _container_Playing;
            response.Paused += _container_Paused;
            response.Stopped += _container_Stopped;
            response.DurationChanged += _container_DurationChanged;
            response.PositionChanged += _container_PositionChanged;
            response.Ended += _container_Ended;
            //response.Click += _container_Click;
            response.HasNextChanged += _container_HasNextChanged;
            response.HasPreviousChanged += _container_HasPreviousChanged;
            response.NowPlaying += _container_NowPlaying;
            response.HasSubtitlesChanged += _container_HasSubtitlesChanged;
            response.IsSeekableChanged += _container_IsSeekableChanged;
        }
        void DetachFromResponse(IMediaResponse response)
        {
            response.Opening -= _container_Opening;
            response.Error -= _container_Error;
            response.Playing -= _container_Playing;
            response.Paused -= _container_Paused;
            response.Stopped -= _container_Stopped;
            response.DurationChanged -= _container_DurationChanged;
            response.PositionChanged -= _container_PositionChanged;
            response.Ended -= _container_Ended;
            //response.Click += _container_Click;
            response.HasNextChanged -= _container_HasNextChanged;
            response.HasPreviousChanged -= _container_HasPreviousChanged;
            response.NowPlaying -= _container_NowPlaying;
            response.HasSubtitlesChanged -= _container_HasSubtitlesChanged;
            response.IsSeekableChanged -= _container_IsSeekableChanged;
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
                    _container.Position = (float)(_seekbarValue / 100.0);
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

        //private void _container_Click(object sender, EventArgs e)
        //{
        //    if (_fullscreenWindow != null)
        //    {
        //        _fullscreenWindow.Close();
        //    }
        //}

        private void _container_Ended(object sender, EventArgs e)
        {
            _core.Logger.Info(this, "Ended");
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
            _core.Logger.Info(this, "Stopped");
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
            _core.Logger.Info(this, "Opening");
            SeekbarValue = 0.0;
            SwitchPlayerButtonVisible = _container.AvailablePlayers.Count > 1;
        }

        private void _container_Playing(object sender, EventArgs e)
        {
            _core.Logger.Info(this, "Playing");
            IsPlaying = true;
            _container.CurrentMediaPlayer.VideoControl.RemoveChild();
            CurrentVideoControl = _container.CurrentMediaPlayer.VideoControl;
            PauseButtonIcon = "pause";
            PauseButtonVisible = StopButtonVisible = true;

        }

        private void _container_Error(object sender, Exception e)
        {
            _core.Logger.Info(this, "Error");
            IsPlaying = false;
            if(_core.TryGetUiManager(out IUiManager ui))
            {
                ui.Toast(new Translator("MediaPlayerContainer").Translate("Media failed to play"));
            }
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

        bool _fullscreenVisible;
        public bool FullscreenVisible
        {
            get => _fullscreenVisible;
            set
            {
                _fullscreenVisible = value;
                OnPropertyChanged();
            }
        }

        public event EventHandler Deactivated;
        public event EventHandler Activated;

        public RelayCommand PreviousCommand { get; }

        public RelayCommand NextCommand { get; }

        public RelayCommand StopCommand { get; }

        public RelayCommand PauseCommand { get; }

        public RelayCommand FullscreenCommand { get; }

        public RelayCommand PipCommand { get; }

        public RelayCommand VolumeUpCommand { get; }


        public RelayCommand VolumeDownCommand { get; }


    }

}
