using NAudio.Wave;
using NSpeex;
using System;
using System.Diagnostics;

namespace SpeechAnalyzer.Conversation
{
    internal class SpeexChatCodec : INetworkChatCodec, IDisposable
    {
        private readonly WaveFormat recordingFormat;
        private readonly SpeexDecoder decoder;
        private readonly SpeexEncoder encoder;
        private readonly WaveBuffer encoderInputBuffer;
        private readonly string description;

        public SpeexChatCodec(BandMode bandMode, int sampleRate, string description)
        {
            this.decoder = new SpeexDecoder(bandMode, true);
            this.encoder = new SpeexEncoder(bandMode);
            this.recordingFormat = new WaveFormat(sampleRate, 16, 1);
            this.description = description;
            this.encoderInputBuffer = new WaveBuffer(this.recordingFormat.AverageBytesPerSecond);
        }

        public string Name
        {
            get
            {
                return this.description;
            }
        }

        public int BitsPerSecond
        {
            get
            {
                return -1;
            }
        }

        public WaveFormat RecordFormat
        {
            get
            {
                return this.recordingFormat;
            }
        }

        public byte[] Encode(byte[] data, int offset, int length)
        {
            this.FeedSamplesIntoEncoderInputBuffer(data, offset, length);
            int shortBufferCount = this.encoderInputBuffer.ShortBufferCount;
            if ((uint)(shortBufferCount % this.encoder.FrameSize) > 0U)
                shortBufferCount -= shortBufferCount % this.encoder.FrameSize;
            byte[] outData = new byte[length];
            int length1 = this.encoder.Encode(this.encoderInputBuffer.ShortBuffer, 0, shortBufferCount, outData, 0, length);
            byte[] numArray = new byte[length1];
            Array.Copy((Array)outData, 0, (Array)numArray, 0, length1);
            this.ShiftLeftoverSamplesDown(shortBufferCount);
            Debug.WriteLine(string.Format("NSpeex: In {0} bytes, encoded {1} bytes [enc frame size = {2}]", (object)length, (object)length1, (object)this.encoder.FrameSize));
            return numArray;
        }

        private void ShiftLeftoverSamplesDown(int samplesEncoded)
        {
            int num = this.encoderInputBuffer.ShortBufferCount - samplesEncoded;
            Array.Copy((Array)this.encoderInputBuffer.ByteBuffer, samplesEncoded * 2, (Array)this.encoderInputBuffer.ByteBuffer, 0, num * 2);
            this.encoderInputBuffer.ShortBufferCount = num;
        }

        private void FeedSamplesIntoEncoderInputBuffer(byte[] data, int offset, int length)
        {
            Array.Copy((Array)data, offset, (Array)this.encoderInputBuffer.ByteBuffer, this.encoderInputBuffer.ByteBufferCount, length);
            this.encoderInputBuffer.ByteBufferCount += length;
        }

        public byte[] Decode(byte[] data, int offset, int length)
        {
            byte[] bufferToBoundTo = new byte[length * 320];
            WaveBuffer waveBuffer = new WaveBuffer(bufferToBoundTo);
            int length1 = this.decoder.Decode(data, offset, length, waveBuffer.ShortBuffer, 0, false) * 2;
            byte[] numArray = new byte[length1];
            Array.Copy((Array)bufferToBoundTo, 0, (Array)numArray, 0, length1);
            Debug.WriteLine(string.Format("NSpeex: In {0} bytes, decoded {1} bytes [dec frame size = {2}]", (object)length, (object)length1, (object)this.decoder.FrameSize));
            return numArray;
        }

        public void Dispose()
        {
        }

        public bool IsAvailable
        {
            get
            {
                return true;
            }
        }
    }
}
