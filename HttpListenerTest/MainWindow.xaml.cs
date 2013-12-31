using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Threading;

namespace HttpListenerTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Timers.Timer _clientTimer;
        private System.Timers.Timer _serverTimer;

        public MainWindow()
        {
            InitializeComponent();

            _clientTimer = new System.Timers.Timer();
            _clientTimer.Interval = 3000;
            _clientTimer.Elapsed += new System.Timers.ElapsedEventHandler(_clientTimer_Elapsed);

            Thread startThread = new Thread(new ThreadStart(StartThread));
            startThread.Name = "StartThread";
            startThread.Start();

            _serverTimer = new System.Timers.Timer();
            _serverTimer.Interval = 1000;
            _serverTimer.Elapsed += new System.Timers.ElapsedEventHandler(_serverTimer_Elapsed);

            Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        void _serverTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new System.Timers.ElapsedEventHandler(SAFE_serverTimer_Elapsed), sender, e);
        }

        void SAFE_serverTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _serverTimer.Stop();

            Server server = new Server();
            server.Start();
        }

        void _clientTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new System.Timers.ElapsedEventHandler(SAFE_clientTimer_Elapsed), sender, e);
        }

        void SAFE_clientTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _clientTimer.Stop();

            Client client = new Client();
            client.ConnectToServer();
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
//            _clientTimer.Start();
//            _serverTimer.Start();
        }

        private void StartThread()
        {
            Server server = new Server();
            server.Start();
        }
    }
}
