using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SpeechAnalyzer.Conversation
{
    class Sender
    {
        private WaveInEvent waveIn;
        private UdpClient udpSender;
        private INetworkChatCodec selectedCodec;
        private volatile bool connected;
        private IPAddress serverip;
        private int serveraudioPort;
        private List<string> deviceList;

        public Sender()
        {
            deviceList = new List<string>();
            PopulateInputDevicesList();
            selectedCodec = new UltraWideBandSpeexCodec();
        }

        private void PopulateInputDevicesList()
        {
            for (int devNumber = 0; devNumber < WaveIn.DeviceCount; ++devNumber)
                deviceList.Add(WaveIn.GetCapabilities(devNumber).ProductName);
        }

        public bool Send(string receiver_ip, int port)
        {
            try
            {
                serverip = IPAddress.Parse(receiver_ip);
                serveraudioPort = port;
                Task.Factory.StartNew(() => ListenAudio());
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void ListenAudio()
        {
            udpSender = new UdpClient();
            udpSender.Connect(serverip, serveraudioPort);
            waveIn = new WaveInEvent();
            waveIn.BufferMilliseconds = 50;
            waveIn.DeviceNumber = 0;
            waveIn.WaveFormat = selectedCodec.RecordFormat;
            waveIn.DataAvailable += new EventHandler<WaveInEventArgs>(waveIn_DataAvailable);
            waveIn.StartRecording();
            connected = true;
        }

        public void Disconnect()
        {
            if (!connected)
                return;
            connected = false;
            waveIn.DataAvailable -= new EventHandler<WaveInEventArgs>(waveIn_DataAvailable);
            waveIn.StopRecording();
            udpSender.Close();
            waveIn.Dispose();
            selectedCodec.Dispose();
        }

        private void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            byte[] dgram = selectedCodec.Encode(e.Buffer, 0, e.BytesRecorded);
            udpSender.Send(dgram, dgram.Length);
        }
    }
}
