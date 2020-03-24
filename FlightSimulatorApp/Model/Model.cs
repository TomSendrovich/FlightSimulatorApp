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
        const string HEADING = "/instrumentation/heading-indicator/indicated-heading-deg";
        const string VERTICAL_SPEED = "/instrumentation/gps/indicated-vertical-speed";
        const string GROUND_SPEED = "/instrumentation/gps/indicated-ground-speed-kt";
        const string AIR_SPEED = "/instrumentation/airspeed-indicator/indicated-speed-kt";
        const string ALTITUDE = "/instrumentation/gps/indicated-altitude-ft";
        const string ROLL = "/instrumentation/attitude-indicator/internal-roll-deg";
        const string PITCH = "/instrumentation/attitude-indicator/internal-pitch-deg";
        const string ALTIMETER = "/instrumentation/altimeter/indicated-altitude-ft";
        const string THROTTLE = "/controls/engines/current-engine/throttle";
        const string AILERON = "/controls/flight/aileron";
        const string ELEVATOR = "/controls/flight/elevator";
        const string RUDDER = "/controls/flight/rudder";
        const string LATITUDE = "/position/latitude-deg";
        const string LONGITUDE = "/position/longitude-deg";

        TcpClient tcpClient;
        volatile Boolean stop;

        private double heading, verticalSpeed, groundSpeed, airSpeed, altitude, roll, pitch, altimeter, throttle, aileron, elevator, rudder, latitude, longitude;

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
            new Thread(delegate ()
            {
                string value;
                double value_d;
                while (!stop)
                {
                    try
                    {
                        value = GetParameter(HEADING);
                        value_d = Double.Parse(value);
                        Heading = Math.Round(value_d, 2, MidpointRounding.ToEven);

                        value = GetParameter(VERTICAL_SPEED);
                        value_d = Double.Parse(value);
                        VerticalSpeed = Math.Round(value_d, 2, MidpointRounding.ToEven);

                        value = GetParameter(GROUND_SPEED);
                        value_d = Double.Parse(value);
                        GroundSpeed = Math.Round(value_d, 2, MidpointRounding.ToEven);

                        value = GetParameter(AIR_SPEED);
                        value_d = Double.Parse(value);
                        AirSpeed = Math.Round(value_d, 2, MidpointRounding.ToEven);

                        value = GetParameter(ALTITUDE);
                        value_d = Double.Parse(value);
                        Altitude = Math.Round(value_d, 2, MidpointRounding.ToEven);

                        value = GetParameter(ROLL);
                        value_d = Double.Parse(value);
                        Roll = Math.Round(value_d, 2, MidpointRounding.ToEven);

                        value = GetParameter(PITCH);
                        value_d = Double.Parse(value);
                        Pitch = Math.Round(value_d, 2, MidpointRounding.ToEven);

                        value = GetParameter(ALTIMETER);
                        value_d = Double.Parse(value);
                        Altimeter = Math.Round(value_d, 2, MidpointRounding.ToEven);

                        value = GetParameter(LATITUDE);
                        value_d = Double.Parse(value);
                        Latitude = Math.Round(value_d, 4, MidpointRounding.ToEven);

                        value = GetParameter(LONGITUDE);
                        value_d = Double.Parse(value);
                        Longitude = Math.Round(value_d, 4, MidpointRounding.ToEven);

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

        private string GetParameter(string param)
        {
            // Translate the passed message into ASCII and store it as a Byte array.
            Byte[] data = System.Text.Encoding.ASCII.GetBytes("get " + param + "\n");

            // Get a client stream for reading and writing.
            NetworkStream stream = tcpClient.GetStream();

            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);

            Console.WriteLine("Sent: {0}", param);

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

        #region Properties

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
        public double Throttle
        {
            get { return throttle; }
            set { throttle = value; NotifyPropertyChanged("Throttle"); }
        }
        public double Aileron
        {
            get { return aileron; }
            set { aileron = value; NotifyPropertyChanged("Aileron"); }
        }
        public double Elevator
        {
            get { return elevator; }
            set { elevator = value; NotifyPropertyChanged("Elevator"); }
        }
        public double Rudder
        {
            get { return rudder; }
            set { rudder = value; NotifyPropertyChanged("Rudder"); }
        }
        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; NotifyPropertyChanged("Latitude"); }
        }
        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; NotifyPropertyChanged("Longitude"); }
        }

        public void NotifyPropertyChanged(string PropName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropName));
            }
        }
        #endregion
    }
}
