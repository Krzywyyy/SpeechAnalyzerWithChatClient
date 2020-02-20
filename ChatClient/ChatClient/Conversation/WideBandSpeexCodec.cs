using NSpeex;
using System.Composition;


namespace SpeechAnalyzer.Conversation
{
    [Export(typeof(INetworkChatCodec))]
    internal class WideBandSpeexCodec : SpeexChatCodec
    {
        public WideBandSpeexCodec()
          : base(BandMode.Wide, 16000, "Speex Wide Band (16kHz)")
        {
        }
    }
}
