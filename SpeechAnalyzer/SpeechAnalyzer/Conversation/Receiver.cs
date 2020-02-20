using NAudio.Wave;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SpeechAnalyzer.Conversation
{
    public class Receiver
    {
        private UdpClient listenerAudio;
        private IPEndPoint myEndPoint;
        private IWavePlayer waveOut;
        private BufferedWaveProvider waveProvider;
        private INetworkChatCodec selectedCodec;

        public Receiver()
        {
            selectedCodec = new NarrowBandSpeexCodec();
        }

        public bool Receive(string receiver_ip, int port)
        {
            try
            {
                myEndPoint = new IPEndPoint(IPAddress.Parse(receiver_ip), port);
                listenerAudio = new UdpClient();
                listenerAudio.Client.Bind(myEndPoint);
                waveOut = new WaveOut();
                waveProvider = new BufferedWaveProvider(selectedCodec.RecordFormat);
                waveOut.Init(waveProvider);
                waveOut.Play();
                Task.Factory.StartNew(() => ListenerA());
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void ListenerA()
        {
            while (true)
            {
                try
                {
                    byte[] data = listenerAudio.Receive(ref myEndPoint);
                    byte[] buffer = selectedCodec.Decode(data, 0, data.Length);
                    Receiver receiver = this;
                    receiver.waveProvider.AddSamples(buffer, 0, buffer.Length);
                }
                catch (SocketException)
                {
                }
            }
        }
    }
}
