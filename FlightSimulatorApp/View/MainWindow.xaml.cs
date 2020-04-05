using System;
using System.Windows;

namespace FlightSimulatorApp.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel vm;
        public MainWindow()
        {
            vm = new ViewModel(new Model());
            DataContext = vm;

            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            vm.Disconnect();
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectWindow window = new ConnectWindow();
            window.ShowDialog();
        }

        private void DisconnectButton_Click(object sender, RoutedEventArgs e) { vm.Disconnect(); }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) { vm.Disconnect(); }

        internal void Connect(string ip, int port)
        {
            try
            {
                vm.Connect(ip, port);
                if (vm.IsConnected())
                {
                    System.Diagnostics.Debug.WriteLine("Client is connected successfully!");
                    vm.Start();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
