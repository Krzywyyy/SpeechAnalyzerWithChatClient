using NSpeex;
using System.Composition;

namespace SpeechAnalyzer.Conversation
{
    [Export(typeof(INetworkChatCodec))]
    internal class NarrowBandSpeexCodec : SpeexChatCodec
    {
        public NarrowBandSpeexCodec()
          : base(BandMode.Narrow, 8000, "Speex Narrow Band")
        {
        }
    }
}
