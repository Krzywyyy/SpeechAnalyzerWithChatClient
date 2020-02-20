using ChatClient;
using NAudio.Wave;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
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
        private MainWindow mainWindow;

        public Receiver(MainWindow mainWindow)
        {
            selectedCodec = new NarrowBandSpeexCodec();
            this.mainWindow = mainWindow;
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
                mainWindow.NoAccess();
                return false;
            }
            return true;
        }

        private Thread thread;

        private void ListenerA()
        {
            while (true)
            {
                mainWindow.Access();
                thread = new Thread(() =>
                {
                    try
                    {
                        Thread.Sleep(1000);
                        mainWindow.NoAccess();
                    }
                    catch (ThreadInterruptedException)
                    {
                        Console.WriteLine("Interrupted");
                    }
                });
                thread.IsBackground = true;
                thread.Start();
                try
                {
                    byte[] data = listenerAudio.Receive(ref myEndPoint);
                    Console.WriteLine(data.Length);
                    if (thread != null)
                    {
                        thread.Interrupt();
                    }
                    byte[] buffer = selectedCodec.Decode(data, 0, data.Length);
                    Receiver receiver = this;
                    receiver.waveProvider.AddSamples(buffer, 0, buffer.Length);
                }
                catch (SocketException)
                {
                    Console.WriteLine("Listener exception");
                }
            }
        }
    }
}
