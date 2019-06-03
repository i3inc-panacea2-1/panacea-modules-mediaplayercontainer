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

namespace Panacea.Modules.MediaPlayerContainer
{
    public class MediaPlayerContainer : IMediaPlayerContainer
    {
        private readonly PanaceaServices _core;
        private readonly IPluginLoader _loader;
        private MediaPlayerContainerViewModel _control;
        private MediaResponse _currentResponse;
        public event EventHandler<Exception> Error;
        public event EventHandler<bool> IsSeekableChanged;
        public event EventHandler<float> PositionChanged;
        public event EventHandler IsPausableChanged;
        public event EventHandler Opening;
        public event EventHandler Playing;
        public event EventHandler<string> NowPlaying;
        public event EventHandler Stopped;
        public event EventHandler Paused;
        public event EventHandler Ended;
        public event EventHandler<bool> HasSubtitlesChanged;
        public event EventHandler<TimeSpan> DurationChanged;
        public event EventHandler Click;
        public event EventHandler<bool> HasNextChanged;
        public event EventHandler<bool> HasPreviousChanged;

        public List<IMediaPlayerPlugin> AvailablePlayers { get; private set; }
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
            _control = new MediaPlayerContainerViewModel(this);
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
            HasPreviousChanged?.Invoke(this, e);
        }

        private void Player_HasNextChanged(object sender, bool e)
        {
            HasNextChanged?.Invoke(this, e);
        }

        private void Player_Click(object sender, EventArgs e)
        {
            Click?.Invoke(this, EventArgs.Empty);
        }

        private void Player_DurationChanged(object sender, TimeSpan e)
        {
            DurationChanged?.Invoke(this, e);
        }

        private void Player_HasSubtitlesChanged(object sender, bool e)
        {
            HasSubtitlesChanged?.Invoke(this, e);
        }

        private void Player_IsPausableChanged(object sender, EventArgs e)
        {
            IsPausableChanged?.Invoke(this, e);
        }

        private void Player_PositionChanged(object sender, float e)
        {
            PositionChanged?.Invoke(this, e);
        }

        private void Player_IsSeekableChanged(object sender, bool e)
        {
            IsSeekableChanged?.Invoke(this, e);
        }

        private void Player_Error(object sender, Exception e)
        {
            Refrain();
            _currentResponse?.RaiseError();
            Error?.Invoke(this, e);
        }

        private void Player_Ended(object sender, EventArgs e)
        {
            Refrain();
            _currentResponse?.RaiseEnded();
            Ended?.Invoke(this, e);
        }

        private void Player_Stopped(object sender, EventArgs e)
        {
            Refrain();
            _currentResponse?.RaiseStopped();
            Stopped?.Invoke(this, e);
        }

        private void Player_Paused(object sender, EventArgs e)
        {
            Paused?.Invoke(this, e);
        }

        private void Player_NowPlaying(object sender, string e)
        {
            NowPlaying?.Invoke(this, e);
        }

        private void Player_Playing(object sender, EventArgs e)
        {
            _currentResponse?.RaisePlaying();
            Playing?.Invoke(this, e);
        }

        private void Player_Opening(object sender, EventArgs e)
        {
            HasNextChanged?.Invoke(this, CurrentMediaPlayer.HasNext || CurrentRequest.MediaTraverser != null);
            HasPreviousChanged?.Invoke(this, CurrentMediaPlayer.HasPrevious || CurrentRequest.MediaTraverser != null);
            Opening?.Invoke(this, e);
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

            if (CurrentMediaPlayer != null && players.Contains(CurrentMediaPlayer))
            {
                DetachFromPlayer(CurrentMediaPlayer);
                CurrentMediaPlayer.Stop();
            }
            CurrentRequest = request;
            _currentResponse = new MediaResponse(request);
            AvailablePlayers = players;
            
            if (players.Count == 1)
            {
               
                CurrentMediaPlayer = players.First();
                _control.NowPlayingText = request.Media.Name;
                PlayInternal();
            }
            else
            {
                // show popup and then open
            }
            return _currentResponse;
        }

        private void PlayInternal()
        {
            
            try
            {
                AttachToPlayer(CurrentMediaPlayer);
                Opening?.Invoke(this, EventArgs.Empty);


                CurrentMediaPlayer.Play(CurrentRequest.Media);
                _control.VideoVisible = true;
                switch (CurrentRequest.MediaPlayerPosition)
                {
                    case MediaPlayerPosition.Standalone:
                        if (_core.TryGetUiManager(out IUiManager ui))
                        {
                            ui.Navigate(_control, false);
                        }
                        break;
                    case MediaPlayerPosition.Embedded:
                        CurrentRequest.MediaPlayerHost.Content = _control;
                        break;
                    case MediaPlayerPosition.Notification:
                        if (_core.TryGetUiManager(out IUiManager uii))
                        {
                            _control.VideoVisible = false;
                            uii.Notify(_control);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                _currentResponse?.RaiseError();
                Error?.Invoke(this, ex);
                DetachFromPlayer(CurrentMediaPlayer);
            }

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
