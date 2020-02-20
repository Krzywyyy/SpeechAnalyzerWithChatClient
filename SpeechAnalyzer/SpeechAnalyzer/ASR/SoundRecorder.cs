using NAudio.Wave;
using System;

namespace SpeechAnalyzer.ASR
{
    class SoundRecorder
    {
        private IWaveIn recorder;
        private MainWindow mainWindow;

        public SoundRecorder(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public void StartRecording()
        {
            recorder = new WaveInEvent();
            WaveFileWriter writer = new WaveFileWriter("C:\\Users\\dawid\\Documents\\Recordings\\" + Guid.NewGuid() + ".wav", recorder.WaveFormat);

            recorder.DataAvailable += (s, a) =>
            {
                if (mainWindow.Scramble())
                {
                    writer.Write(Scrambler.AESEncryptBytes(a.Buffer), 0, a.BytesRecorded);
                }
                else
                {
                    writer.Write(a.Buffer, 0, a.BytesRecorded);
                }

                //Stopping recording after 30 seconds
                //if (writer.Position > recorder.WaveFormat.AverageBytesPerSecond * 30)
                //{
                //    recorder.StopRecording();
                //}
            };

            recorder.RecordingStopped += (s, a) =>
            {
                writer?.Dispose();
                writer = null;
                recorder.Dispose();
            };

            recorder.StartRecording();
        }

        public void StopRecording()
        {
            recorder.StopRecording();
        }
    }
}
