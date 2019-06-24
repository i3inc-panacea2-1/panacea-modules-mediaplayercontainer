using Panacea.Modularity.Media;
using Panacea.Modularity.MediaPlayerContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panacea.Modules.MediaPlayerContainer
{
    public class MediaResponse : IMediaResponse
    {
        public MediaResponse(MediaRequest request)
        {
            Request = request;
        }
        public MediaRequest Request { get; private set; }

        public event EventHandler Playing;
        internal void RaisePlaying()
        {
            Playing?.Invoke(this, null);
        }

        public event EventHandler Stopped;
        internal void OnStopped()
        {
            Stopped?.Invoke(this, null);
        }

        public event EventHandler Ended;
        internal void OnEnded()
        {
            Ended?.Invoke(this, null);
        }

        public event EventHandler<Exception> Error;
        internal void OnError(Exception ex)
        {
            Error?.Invoke(this, ex);
        }
        public bool IsSeekable { get; set; }

        public float Position { get; set; }

        public bool IsPlaying => throw new NotImplementedException();

        public bool HasNext { get; set; }

        public bool HasPrevious { get; set; }

        public bool IsPausable { get; set; }

        public TimeSpan Duration { get; set; }

        public bool HasSubtitles { get; set; }

        public event EventHandler<bool> IsSeekableChanged;
        public event EventHandler<float> PositionChanged;
        public event EventHandler<bool> HasNextChanged;
        public event EventHandler<bool> HasPreviousChanged;
        public event EventHandler<bool> IsPausableChanged;
        public event EventHandler<TimeSpan> DurationChanged;
        public event EventHandler<bool> HasSubtitlesChanged;
        public event EventHandler Opening;
        public event EventHandler<string> NowPlaying;
        public event EventHandler Paused;


        public void Next()
        {
            throw new NotImplementedException();
        }

        public void Previous()
        {
            throw new NotImplementedException();
        }

        public void Play()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void SetSubtitles(bool on)
        {
            throw new NotImplementedException();
        }

        internal void OnHasPreviousChanged(bool e)
        {
            HasPreviousChanged?.Invoke(this, e);
        }
        internal void OnHasNextChanged(bool e)
        {
            HasNextChanged?.Invoke(this, e);
        }
        internal void OnDurationChanged(TimeSpan e)
        {
            DurationChanged?.Invoke(this, e);
        }
        internal void OnHasSubtitlesChanged(bool e)
        {
            HasSubtitlesChanged?.Invoke(this, e);
        }
        internal void OnIsPausableChanged(bool e)
        {
            IsPausableChanged?.Invoke(this, e);
        }

        internal void OnPositionChanged(float e)
        {
            PositionChanged?.Invoke(this, e);
        }

        internal void OnIsSeekableChanged(bool e)
        {
            IsSeekableChanged?.Invoke(this, e);
        }

        internal void OnPaused(EventArgs e)
        {
            Paused?.Invoke(this, e);
        }

        internal void OnNowPlaying(string e)
        {
            NowPlaying?.Invoke(this, e);
        }

        internal void OnPlaying(EventArgs e)
        {
            Playing?.Invoke(this, e);
        }

        internal void OnOpening(EventArgs e)
        {
            Opening?.Invoke(this, e);
        }
    }
}
