using NSpeex;
using System.ComponentModel.Composition;

namespace SpeechAnalyzer.Conversation
{
    [Export(typeof(INetworkChatCodec))]
    internal class UltraWideBandSpeexCodec : SpeexChatCodec
    {
        public UltraWideBandSpeexCodec()
          : base(BandMode.UltraWide, 32000, "Speex Ultra Wide Band (32kHz)")
        {
        }
    }
}
