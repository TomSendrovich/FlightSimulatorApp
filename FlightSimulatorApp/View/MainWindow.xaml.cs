using FlightSimulatorApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            TcpClient tcpClient = new TcpClient();
            var model = new Model(tcpClient);
            //model.connect("127.0.0.1", 5402);
            //model.start();

            vm = new ViewModel(model);
            DataContext = vm;

            InitializeComponent();
            
        }
    }
}
