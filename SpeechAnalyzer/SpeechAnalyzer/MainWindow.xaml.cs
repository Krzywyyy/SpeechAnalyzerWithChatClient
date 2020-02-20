using Ozeki.Media;
using Ozeki.Media.MediaHandlers;
using SpeechAnalyzer.ASR;
using SpeechAnalyzer.Conversation;
using System;
using System.Threading;
using System.Windows;

namespace SpeechAnalyzer
{
    public partial class MainWindow : Window
    {
        Thread conversationListenerThread;

        private SoundRecorder soundRecorder;

        private string classification = "NATO UNCLASSIFIED";
        private bool scrambling;

        private static Microphone microphone;
        private static Speaker speaker;
        private static MediaConnector connector;
        private static AudioQualityEnhancer audioProcessor;

        private readonly string secretUserIp = "9.241.156.82";
        private readonly string unclassifiedUserIp = "9.241.155.7";

        private Sender secretSender;
        private Sender unclassifiedSender;

        private bool secretReceiving;
        private bool unclassifiedReceiving;

        public MainWindow()
        {
            InitializeComponent();
            ReduceNoiseFromMicrophone();
            ConnectUsers();
            soundRecorder = new SoundRecorder(this);
        }

        private void SendToUnclassified()
        {
            if (!secretReceiving)
            {
                secretSender = new Sender();
                secretSender.Send(secretUserIp, 13000);
                secretReceiving = true;
            }
            if (!unclassifiedReceiving)
            {
                unclassifiedSender = new Sender();
                unclassifiedSender.Send(unclassifiedUserIp, 13000);
                unclassifiedReceiving = true;
            }
        }

        private void SendToZero()
        {
            if (secretReceiving)
            {
                secretSender.Disconnect();
                secretReceiving = false;
            }
            if (unclassifiedReceiving)
            {
                unclassifiedSender.Disconnect();
                unclassifiedReceiving = false;
            }
        }

        private void SendToSecretButNoUnclassified()
        {
            if (!secretReceiving)
            {
                secretSender = new Sender();
                secretSender.Send(secretUserIp, 13000);
                secretReceiving = true;
            }
            if (unclassifiedReceiving)
            {
                unclassifiedSender.Disconnect();
                unclassifiedReceiving = false;
            }
        }

        private void SendToSecret()
        {
            if (!secretReceiving)
            {
                secretSender = new Sender();
                secretSender.Send(secretUserIp, 13000);
                secretReceiving = true;
            }
            if (unclassifiedReceiving)
            {
                unclassifiedSender.Disconnect();
                unclassifiedReceiving = false;
            }
        }
        public bool Scramble()
        {
            return scrambling;
        }

        private void ConnectUsers()
        {
            firstConnectedUser.Text = "Rafal Huk\nNATO SECRET\nIP: " + secretUserIp;
            secondConnectedUser.Text = "Michal Bryla\nNATO UNCLASSIFIED\nIP: " + unclassifiedUserIp;
        }

        private void ReduceNoiseFromMicrophone()
        {
            microphone = Microphone.GetDefaultDevice();
            speaker = Speaker.GetDefaultDevice();
            connector = new MediaConnector();
            audioProcessor = new AudioQualityEnhancer();

            audioProcessor.NoiseReductionLevel = NoiseReductionLevel.High;
            audioProcessor.SetEchoSource(speaker);

            connector.Connect(microphone, audioProcessor);
            connector.Connect(audioProcessor, speaker);

            microphone.Start();
        }

        public void ChangeConversationClassification(string classification)
        {
            switch (classification)
            {
                case "Zero":
                    this.classification = "COSMIC TOP SECRET";
                    SendToZero();
                    break;
                case "Sheila":
                    this.classification = "NATO SECRET";
                    SendToSecret();
                    break;
                case "Dog":
                    this.classification = "NATO CONFIDENTIAL";
                    SendToSecretButNoUnclassified();
                    break;
                case "Happy":
                    this.classification = "NATO RESTRICTED";
                    SendToSecretButNoUnclassified();
                    break;
                case "House":
                    this.classification = "NATO UNCLASSIFIED";
                    SendToUnclassified();
                    break;
            }
            UpdateText();
        }

        public void ChangeConversationScramblingState(bool scrambling)
        {
            this.scrambling = scrambling;
            UpdateText();
        }

        private void UpdateText()
        {
            Dispatcher.Invoke(() =>
            {
                currentClassification.Content = "Current classification: " + classification;
                scramblingLabel.Content = "Scrambling: " + scrambling;
        });
        }

        private void StartConversationListenerThread()
        {
            conversationListenerThread = new Thread(StartConversationListener);
            conversationListenerThread.IsBackground = true;
            conversationListenerThread.Start();
        }

        private void StartConversationListener()
        {
            ConversationListener conversationListener = new ConversationListener(this);
        }

        private void StartConversation(object sender, RoutedEventArgs e)
        {
            recordingGif.Visibility = Visibility.Visible;
            noRecording.Visibility = Visibility.Hidden;

            UpdateText();
            soundRecorder.StartRecording();
            SendToUnclassified();
            if (conversationListenerThread == null)
            {
                StartConversationListenerThread();
            }
            Dispatcher.Invoke(() =>
            {
                startConversationButton.Visibility = Visibility.Hidden;
                stopConversationButton.Visibility = Visibility.Visible;
                classificationButton.Visibility = Visibility.Visible;
            });
        }

        private void StopConversation(object sender, RoutedEventArgs e)
        {
            recordingGif.Visibility = Visibility.Hidden;
            noRecording.Visibility = Visibility.Visible;
            soundRecorder.StopRecording();
            SendToZero();
            if (conversationListenerThread != null)
            {
                conversationListenerThread.Interrupt();
            }
            Dispatcher.Invoke(() =>
            {
                startConversationButton.Visibility = Visibility.Visible;
                stopConversationButton.Visibility = Visibility.Hidden;
                classificationButton.Visibility = Visibility.Hidden;
            });
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
