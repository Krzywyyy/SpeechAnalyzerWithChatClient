using SpeechAnalyzer.Conversation;
using System.Threading;
using System.Windows;

namespace ChatClient
{
    public partial class MainWindow : Window
    {
        private readonly string myIp = "127.0.0.1";

        public MainWindow()
        {
            InitializeComponent();
            user.Text = "Michal Bryla\nNATO UNCLASSIFIED\nIP: " + myIp;

            Receiver receiver = new Receiver(this);
            receiver.Receive(myIp, 13000);
            TenSecondsSender();
        }

        private void TenSecondsSender()
        {
            Thread thr = new Thread(() =>
            {
                Thread.Sleep(3000);
                Sender s = new Sender();
                s.Send(myIp, 13000);
                Thread.Sleep(5000);
                s.Disconnect();
            });
            thr.IsBackground = true;
            thr.Start();
        }

        public void Access()
        {
            Dispatcher.Invoke(() =>
            {
                microphone.Visibility = Visibility.Visible;
                denied.Visibility = Visibility.Hidden;
            });
        }


        public void NoAccess()
        {
            Dispatcher.Invoke(() =>
            {
                microphone.Visibility = Visibility.Hidden;
                denied.Visibility = Visibility.Visible;
            });
        }
    }
}
