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

        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            vm.Disconnect();
            UpdateUI(false);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            vm.Disconnect();
            UpdateUI(false);
        }

        internal void Connect(string ip, int port)
        {
            try
            {
                vm.Connect(ip, port);
                if (vm.IsConnected())
                {
                    System.Diagnostics.Debug.WriteLine("Client is connected successfully!");
                    vm.Start();
                    UpdateUI(true);
                }
                else
                {
                    UpdateUI(false);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void dashboard_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public void UpdateUI(bool isConnected)
        {
            if (isConnected)
            {
                latitudeValue.Visibility = Visibility.Visible;
                latitudeTitle.Visibility = Visibility.Visible;
                longitudeValue.Visibility = Visibility.Visible;
                longitudeTitle.Visibility = Visibility.Visible;
                mapCanvas.Visibility = Visibility.Visible;
            }
            else
            {
                latitudeValue.Visibility = Visibility.Hidden;
                latitudeTitle.Visibility = Visibility.Hidden;
                longitudeValue.Visibility = Visibility.Hidden;
                longitudeTitle.Visibility = Visibility.Hidden;
                mapCanvas.Visibility = Visibility.Hidden;
            }
        }
    }

}
