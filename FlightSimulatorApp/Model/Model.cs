using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

namespace FlightSimulatorApp
{
    class Model : IModel
    {
        TcpClient tcpClient;
        volatile Boolean stop;

        private double heading;

        //INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public Model(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            stop = false;
        }

        public void connect(string ip, int port)
        {
            tcpClient.Connect(ip, port);
        }

        public void disconnect()
        {
            stop = true;
            tcpClient.Close();
        }

        public void start()
        {
            new Thread(delegate ()
            {
                while (!stop)
                {
                    string message = "get /indicated-heading-deg";

                    // Translate the passed message into ASCII and store it as a Byte array.
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                    // Get a client stream for reading and writing.
                    NetworkStream stream = tcpClient.GetStream();

                    // Send the message to the connected TcpServer. 
                    stream.Write(data, 0, data.Length);

                    Console.WriteLine("Sent: {0}", message);

                    // Receive the TcpServer.response.

                    // Buffer to store the response bytes.
                    data = new Byte[256];

                    // String to store the response ASCII representation.
                    String responseData = String.Empty;

                    // Read the first batch of the TcpServer response bytes.
                    Int32 bytes = stream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    Console.WriteLine("Received: {0}", responseData);

                    Heading = Double.Parse(responseData);

                    Thread.Sleep(250); //read the data in 4Hz
                }
            }).Start();
        }

        //the properties implementation
        public double Heading
        {
            get { return heading; }
            set { heading = value; NotifyPropertyChanged("Heading"); }
        }

        public void NotifyPropertyChanged(string PropName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropName));
            }
        }
    }
}
