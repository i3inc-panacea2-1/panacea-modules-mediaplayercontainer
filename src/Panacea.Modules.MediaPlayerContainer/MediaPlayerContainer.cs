using Panacea.Core;
using Panacea.Modularity;
using Panacea.Modularity.Media;
using Panacea.Modularity.MediaPlayerContainer;
using Panacea.Modularity.UiManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Panacea.Modules.MediaPlayerContainer
{
    public class MediaPlayerContainer : IMediaPlayerContainer
    {
        private readonly PanaceaServices _core;
        private readonly IPluginLoader _loader;
        private MediaPlayerContainerViewModel _control;
        private MediaResponse _currentResponse;
        public MediaResponse CurrentResponse {
            get => this._currentResponse;
            set {
                ResponseChanged?.Invoke(this, value);
                this._currentResponse = value;
            }
        }
        Window _fullscreenWindow;
        public List<IMediaPlayerPlugin> AvailablePlayers { get; private set; }
        public event EventHandler<IMediaResponse> ResponseChanged ;
        internal void OnError(Exception ex)
        {
            CurrentResponse?.OnError(ex);
        }
        public MediaPlayerContainer(PanaceaServices core)
        {
            _core = core;
            _loader = core.PluginLoader;
            _loader.PluginLoaded += _loader_PluginLoaded;
            _loader.PluginUnloaded += _loader_PluginUnloaded;
        }

        private void CreateMediaControl()
        {
            if (_control != null) return;
            _control = new MediaPlayerContainerViewModel(this, _core);
            _control.Deactivated += _control_Deactivated;
            _control.Activated += _control_Activated;
        }
        private void _control_Activated(object sender, EventArgs e)
        {
            _transitioning = false;
        }
        private void _control_Deactivated(object sender, EventArgs e)
        {
            if (_transitioning) return;
            if (_pip?.IsVisible == true) return;
            Stop();
        }

        private void _loader_PluginUnloaded(object sender, IPlugin e)
        {
            var player = e as IMediaPlayerPlugin;
            if (e == null) return;
        }

        private void _loader_PluginLoaded(object sender, IPlugin e)
        {
            var player = e as IMediaPlayerPlugin;
            if (e == null) return;
        }

        private void AttachToPlayer(IMediaPlayerPlugin player)
        {
            player.Opening += Player_Opening;
            player.Playing += Player_Playing;
            player.NowPlaying += Player_NowPlaying;
            player.Paused += Player_Paused;
            player.Stopped += Player_Stopped;
            player.Ended += Player_Ended;
            player.Error += Player_Error;
            player.IsSeekableChanged += Player_IsSeekableChanged;
            player.PositionChanged += Player_PositionChanged;
            player.IsPausableChanged += Player_IsPausableChanged;
            player.HasSubtitlesChanged += Player_HasSubtitlesChanged;
            player.DurationChanged += Player_DurationChanged;
            player.Click += Player_Click;
            player.HasNextChanged += Player_HasNextChanged;
            player.HasPreviousChanged += Player_HasPreviousChanged;
        }



        private void DetachFromPlayer(IMediaPlayerPlugin player)
        {
            player.Opening -= Player_Opening;
            player.Playing -= Player_Playing;
            player.NowPlaying -= Player_NowPlaying;
            player.Paused -= Player_Paused;
            player.Stopped -= Player_Stopped;
            player.Ended -= Player_Ended;
            player.Error -= Player_Error;
            player.IsSeekableChanged -= Player_IsSeekableChanged;
            player.PositionChanged -= Player_PositionChanged;
            player.IsPausableChanged -= Player_IsPausableChanged;
            player.HasSubtitlesChanged -= Player_HasSubtitlesChanged;
            player.DurationChanged -= Player_DurationChanged;
            player.Click -= Player_Click;
            player.HasNextChanged -= Player_HasNextChanged;
            player.HasPreviousChanged -= Player_HasPreviousChanged;
        }

        private void Player_HasPreviousChanged(object sender, bool e)
        {
            CurrentResponse.HasPrevious = e;
            CurrentResponse.OnHasPreviousChanged(e);
        }
        private void Player_HasNextChanged(object sender, bool e)
        {
            _currentResponse.HasNext = e;
            _currentResponse.OnHasNextChanged(e);
        }
        private void Player_Click(object sender, EventArgs e)
        {
            if (_pip?.IsVisible == true) return;
            _fullscreenWindow?.Close();
            EmbedPlayer();
            _control.AreControlsVisible = CurrentRequest.ShowControls;
            _control.VideoVisible = CurrentRequest.ShowVideo;
        }
        private void Player_DurationChanged(object sender, TimeSpan e)
        {
            CurrentResponse.Duration = e;
            CurrentResponse.OnDurationChanged(e);
        }
        private void Player_HasSubtitlesChanged(object sender, bool e)
        {
            CurrentResponse.HasSubtitles = e;
            CurrentResponse.OnHasSubtitlesChanged(e);
        }
        private void Player_IsPausableChanged(object sender, bool e)
        {
            _currentResponse.IsPausable = e;
            _currentResponse.OnIsPausableChanged(e);
        }

        private void Player_PositionChanged(object sender, float e)
        {
            CurrentResponse.Position = e;
            CurrentResponse.OnPositionChanged(e);
        }

        private void Player_IsSeekableChanged(object sender, bool e)
        {
            CurrentResponse.IsSeekable = e;
            CurrentResponse.OnIsSeekableChanged(e);
        }

        private void Player_Error(object sender, Exception e)
        {
            Refrain();
            CurrentResponse?.OnError(e);
            RemoveChild();
            if (_core.TryGetUiManager(out IUiManager ui))
            {
                if (ui.CurrentPage == _control)
                {
                    ui.GoBack();
                }
            }
        }

        private void Player_Ended(object sender, EventArgs e)
        {
            Refrain();
            CurrentResponse?.OnEnded();
            RemoveChild();
            if (_core.TryGetUiManager(out IUiManager ui))
            {
                if (ui.CurrentPage == _control)
                {
                    ui.GoBack();
                }
            }
        }

        private void Player_Stopped(object sender, EventArgs e)
        {
            Refrain();
            CurrentResponse?.OnStopped();
            if(_core.TryGetUiManager(out IUiManager ui))
            {
                ui.HidePopup(_control);
                if(ui.CurrentPage == _control)
                {
                    ui.GoBack();
                }
            }
            RemoveChild();
        }

        private void Player_Paused(object sender, EventArgs e)
        {
            CurrentResponse.OnPaused(e);
        }

        private void Player_NowPlaying(object sender, string e)
        {
            CurrentResponse.OnNowPlaying(e);
        }

        private void Player_Playing(object sender, EventArgs e)
        {
            CurrentResponse?.OnPlaying(e);
        }

        private void Player_Opening(object sender, EventArgs e)
        {
            bool hasNext = CurrentMediaPlayer.HasNext || CurrentRequest.MediaTraverser != null;
            Player_HasNextChanged(sender, hasNext);
            bool hasPrevious = CurrentMediaPlayer.HasPrevious || CurrentRequest.MediaTraverser != null;
            Player_HasPreviousChanged(sender, hasPrevious);
            CurrentResponse.OnOpening(e);
        }

        public MediaRequest CurrentRequest { get; private set; }

        public object CurrentOwner { get; private set; }

        public IMediaPlayerPlugin CurrentMediaPlayer { get; private set; }

        public bool IsSeekable => CurrentMediaPlayer.IsSeekable;

        public float Position
        {
            get => CurrentMediaPlayer.Position;
            set => CurrentMediaPlayer.Position = value;
        }

        public bool IsPlaying => CurrentMediaPlayer.IsPlaying;

        public bool HasNext => CurrentMediaPlayer.HasNext;

        public bool HasPrevious => CurrentMediaPlayer.HasPrevious;

        public bool IsPausable => CurrentMediaPlayer.IsPausable;

        public bool HasSubtitles => CurrentMediaPlayer.HasSubtitles;

        public TimeSpan Duration => CurrentMediaPlayer.Duration;

        public void Pause()
        {
            CurrentMediaPlayer?.Pause();
        }

        public void Play()
        {
            CurrentMediaPlayer?.Play();
        }

        public IMediaResponse Play(MediaRequest request)
        {
            CreateMediaControl();
            var players = _loader.GetPlugins<IMediaPlayerPlugin>()
                .Where(p => p.CanPlayChannel(request.Media))
                .ToList();

            if (CurrentMediaPlayer != null)
            {
                CurrentMediaPlayer.Stop();
                DetachFromPlayer(CurrentMediaPlayer);
                
            }
            CurrentRequest = request;
            CurrentResponse = new MediaResponse(request, this);
            AvailablePlayers = players;


            if (players.Count == 1)
            {
                CurrentMediaPlayer = players.First();
                Player_HasSubtitlesChanged(this, false);
                _control.AreControlsVisible = request.ShowControls;
                _control.VideoVisible = request.ShowVideo;
                _control.NowPlayingText = request.Media.Name;
                Player_IsSeekableChanged(this, false);
                Player_PositionChanged(this, 0f);
                Player_DurationChanged(this, TimeSpan.FromSeconds(0));
                PlayInternal();
            }
            else
            {
                // show popup and then open
            }
            return CurrentResponse;
        }

        private void PlayInternal()
        {

            try
            {
                AttachToPlayer(CurrentMediaPlayer);
                CurrentMediaPlayer.Play(CurrentRequest.Media);
                EmbedPlayer();
            }
            catch (Exception ex)
            {
                CurrentResponse?.OnError(ex);
                DetachFromPlayer(CurrentMediaPlayer);
            }

        }

        public void GoFullscreen()
        {
            _transitioning = true;
            RemoveChild();
            _fullscreenWindow = new FullscreenWindow()
            {
                WindowState = WindowState.Maximized,
                WindowStyle = WindowStyle.None,
                ResizeMode = ResizeMode.NoResize,
                ShowInTaskbar = false,
                Content = _control.View,
                Topmost = true
            };
            _control.AreControlsVisible = false;
            _fullscreenWindow.Show();
         }

        bool _transitioning = false;
        void EmbedPlayer()
        {
            _transitioning = true;
            
            _pip?.Close();
            switch (CurrentRequest.MediaPlayerPosition)
            {
                case MediaPlayerPosition.Standalone:
                    if (_core.TryGetUiManager(out IUiManager ui))
                    {
                        
                        ui.Navigate(_control, false);
                    }
                    break;
                case MediaPlayerPosition.Embedded:
                    CurrentRequest.MediaPlayerHost.Unloaded += MediaPlayerHost_Unloaded;
                    CurrentRequest.MediaPlayerHost.Content = _control.View;
                    break;
                case MediaPlayerPosition.Notification:
                    if (_core.TryGetUiManager(out IUiManager uii))
                    {
                        _control.VideoVisible = false;
                        uii.Notify(_control);
                    }
                    break;
                case MediaPlayerPosition.Popup:
                    if (_core.TryGetUiManager(out IUiManager uii2))
                    {
                        uii2.ShowPopup(_control, "", PopupType.Empty, true, false);
                    }
                    break;
            }
           
        }

        

        void RemoveChild()
        {
            if (_core.TryGetUiManager(out IUiManager ui) && ui.CurrentPage == _control)
            {
                ui.GoBack();
            }
            else
            {
                _control.View.RemoveChild();
            }
        }

        PipWindow _pip;
        internal void GoToPip()
        {
            _transitioning = true;
            RemoveChild();
            _pip = new PipWindow();
            _control.AreControlsVisible = false;
            _pip.viewer.Children.Add(_control.View);
            _pip.Width = 520;
            _pip.Height = 360;
            _pip.WindowState = WindowState.Normal;
            _pip.Show();
            _transitioning = false;
        }

        private void MediaPlayerHost_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            CurrentRequest.MediaPlayerHost.Unloaded -= MediaPlayerHost_Unloaded;
            if (_pip?.IsVisible == true) return;
            Stop();
        }

        void Refrain()
        {
            if (_core.TryGetUiManager(out IUiManager ui))
            {
                ui.Refrain(_control);
            }
        }

        public void Stop()
        {
            CurrentMediaPlayer?.Stop();
        }

        public void Next()
        {
            if (CurrentRequest.MediaTraverser == null)
            {
                CurrentMediaPlayer?.Next();
            }
            else
            {
                CurrentRequest.MediaTraverser.Next();
            }
        }

        public void Previous()
        {
            if (CurrentRequest.MediaTraverser == null)
            {
                CurrentMediaPlayer?.Previous();
            }
            else
            {
                CurrentRequest.MediaTraverser.Previous();
            }
        }

        public void SetSubtitles(bool on)
        {
            CurrentMediaPlayer?.SetSubtitles(on);
        }

        public Task BeginInit()
        {
            return Task.CompletedTask;
        }

        public Task EndInit()
        {
            return Task.CompletedTask;
        }

        public Task Shutdown()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}
