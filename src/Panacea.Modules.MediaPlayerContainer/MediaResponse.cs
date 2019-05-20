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
        internal void RaiseStopped()
        {
            Stopped?.Invoke(this, null);
        }

        public event EventHandler Ended;
        internal void RaiseEnded()
        {
            Ended?.Invoke(this, null);
        }

        public event EventHandler Error;
        internal void RaiseError()
        {
            Error?.Invoke(this, null);
        }
    }
}
