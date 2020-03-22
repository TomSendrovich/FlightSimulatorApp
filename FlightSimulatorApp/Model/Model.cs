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

        private double heading, verticalSpeed, groundSpeed, airSpeed, altitude, roll, pitch, altimeter;

        //INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        //Constructor
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
            const string HEADING = "get /instrumentation/heading-indicator/indicated-heading-deg\n";
            const string VERTICAL_SPEED = "get /instrumentation/gps/indicated-vertical-speed\n";
            const string GROUND_SPEED = "get /instrumentation/gps/indicated-ground-speed-kt\n";
            const string AIR_SPEED = "get /instrumentation/airspeed-indicator/indicated-speed-kt\n";
            const string ALTITUDE = "get /instrumentation/gps/indicated-altitude-ft\n";
            const string ROLL = "get /instrumentation/attitude-indicator/internal-roll-deg\n";
            const string PITCH = "get /instrumentation/attitude-indicator/internal-pitch-deg\n";
            const string ALTIMETER = "get /instrumentation/altimeter/indicated-altitude-ft\n";

            new Thread(delegate ()
            {
                string value;

                //Test - set command
                value = CheckParameter("set /indicated-heading-deg 5\n");
                Heading = Double.Parse(value);

                while (!stop)
                {
                    try
                    {
                        value = CheckParameter(HEADING);
                        Heading = Double.Parse(value);

                        value = CheckParameter(VERTICAL_SPEED);
                        VerticalSpeed = Double.Parse(value);

                        value = CheckParameter(GROUND_SPEED);
                        GroundSpeed = Double.Parse(value);

                        value = CheckParameter(AIR_SPEED);
                        AirSpeed = Double.Parse(value);

                        value = CheckParameter(ALTITUDE);
                        Altitude = Double.Parse(value);

                        value = CheckParameter(ROLL);
                        Roll = Double.Parse(value);

                        value = CheckParameter(PITCH);
                        Pitch = Double.Parse(value);

                        value = CheckParameter(ALTIMETER);
                        Altimeter = Double.Parse(value);

                        Thread.Sleep(250); //read the data in 4Hz
                    }
                    catch (ArgumentNullException e)
                    {
                        Console.WriteLine("ArgumentNullException: {0}", e);
                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine("SocketException: {0}", e);
                    }
                }
            }).Start();
        }

        private string CheckParameter(string msg)
        {
            // Translate the passed message into ASCII and store it as a Byte array.
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(msg);

            // Get a client stream for reading and writing.
            NetworkStream stream = tcpClient.GetStream();

            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);

            Console.WriteLine("Sent: {0}", msg);

            // Receive the TcpServer.response.

            // Buffer to store the response bytes.
            data = new Byte[256];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

            Console.WriteLine("Received: {0}", responseData);

            return responseData;
        }

        //the properties implementation
        public double Heading
        {
            get { return heading; }
            set { heading = value; NotifyPropertyChanged("Heading"); }
        }
        public double VerticalSpeed
        {
            get { return verticalSpeed; }
            set { verticalSpeed = value; NotifyPropertyChanged("VerticalSpeed"); }
        }
        public double GroundSpeed
        {
            get { return groundSpeed; }
            set { groundSpeed = value; NotifyPropertyChanged("GroundSpeed"); }
        }
        public double AirSpeed
        {
            get { return airSpeed; }
            set { airSpeed = value; NotifyPropertyChanged("AirSpeed"); }
        }
        public double Altitude
        {
            get { return altitude; }
            set { altitude = value; NotifyPropertyChanged("Altitude"); }
        }
        public double Roll
        {
            get { return roll; }
            set { roll = value; NotifyPropertyChanged("Roll"); }
        }
        public double Pitch
        {
            get { return pitch; }
            set { pitch = value; NotifyPropertyChanged("Pitch"); }
        }
        public double Altimeter
        {
            get { return altimeter; }
            set { altimeter = value; NotifyPropertyChanged("Altimeter"); }
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
