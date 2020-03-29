using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

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
        NetworkStream stream;
        Stopwatch timer = Stopwatch.StartNew();
        //private static Mutex mut = new Mutex();
        volatile Boolean stop;

        private double heading, verticalSpeed, groundSpeed, airSpeed, altitude, roll, pitch, altimeter, throttle, aileron, elevator, normalElevator, rudder, normalRudder, latitude, longitude;
        private string location, errorInfo;

        ///INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        ///Constructor
        public Model() { stop = true; }

        public void Connect(string ip, int port)
        {
            try
            {
                tcpClient = new TcpClient();
                tcpClient.Connect(ip, port);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Error: " + e.Message);
                ErrorInfo = "Error: " + e.Message;

            }
        }
        public void Disconnect()
        {
            stop = true;
            if (stream != null)
            {
                stream.Close();
            }
            if (tcpClient != null)
            {
                tcpClient.Close();
            }
        }
        public bool IsConnected() { return tcpClient.Connected; }
        public void Start()
        {
            if (!tcpClient.Connected)
            {
                Console.WriteLine("Error: Server is disconnected");
                ErrorInfo = "Error: Server is disconnected";
                Disconnect();
                return;
            }

            new Thread(delegate ()
            {
                stop = false;
                string value, tmpLongitude, tmpLatitude;
                double value_d;
                while (!stop)
                {
                    try
                    {
                        value = SendCommand(HEADING, false);
                        if (value.Equals("ERR")) { ErrorInfo = "ERR"; }
                        else
                        {
                            value_d = Double.Parse(value);
                            Heading = Math.Round(value_d, 2, MidpointRounding.ToEven);
                        }

                        value = SendCommand(VERTICAL_SPEED, false);
                        if (value.Equals("ERR")) { ErrorInfo = "ERR"; }
                        else
                        {
                            value_d = Double.Parse(value);
                            VerticalSpeed = Math.Round(value_d, 2, MidpointRounding.ToEven);
                        }

                        value = SendCommand(GROUND_SPEED, false);
                        if (value.Equals("ERR")) { ErrorInfo = "ERR"; }
                        else
                        {
                            value_d = Double.Parse(value);
                            GroundSpeed = Math.Round(value_d, 2, MidpointRounding.ToEven);
                        }

                        value = SendCommand(AIR_SPEED, false);
                        if (value.Equals("ERR")) { ErrorInfo = "ERR"; }
                        else
                        {
                            value_d = Double.Parse(value);
                            AirSpeed = Math.Round(value_d, 2, MidpointRounding.ToEven);
                        }

                        value = SendCommand(ALTITUDE, false);
                        if (value.Equals("ERR")) { ErrorInfo = "ERR"; }
                        else
                        {
                            value_d = Double.Parse(value);
                            Altitude = Math.Round(value_d, 2, MidpointRounding.ToEven);
                        }

                        value = SendCommand(ROLL, false);
                        if (value.Equals("ERR")) { ErrorInfo = "ERR"; }
                        else
                        {
                            value_d = Double.Parse(value);
                            Roll = Math.Round(value_d, 2, MidpointRounding.ToEven);
                        }

                        value = SendCommand(PITCH, false);
                        if (value.Equals("ERR")) { ErrorInfo = "ERR"; }
                        else
                        {
                            value_d = Double.Parse(value);
                            Pitch = Math.Round(value_d, 2, MidpointRounding.ToEven);
                        }

                        value = SendCommand(ALTIMETER, false);
                        if (value.Equals("ERR")) { ErrorInfo = "ERR"; }
                        else
                        {
                            value_d = Double.Parse(value);
                            Altimeter = Math.Round(value_d, 2, MidpointRounding.ToEven);
                        }


                        ///get location info
                        tmpLatitude = "0"; tmpLongitude = "0";

                        value = SendCommand(LATITUDE, false);
                        if (value.Equals("ERR")) { ErrorInfo = "ERR"; }
                        else
                        {
                            tmpLatitude = value;
                            value_d = Double.Parse(value);
                            Latitude = Math.Round(value_d, 4, MidpointRounding.ToEven);
                        }

                        value = SendCommand(LONGITUDE, false);
                        if (value.Equals("ERR")) { ErrorInfo = "ERR"; }
                        else
                        {
                            tmpLongitude = value;
                            value_d = Double.Parse(value);
                            Longitude = Math.Round(value_d, 4, MidpointRounding.ToEven);

                            Location = tmpLatitude + "," + tmpLongitude;
                        }


                        Thread.Sleep(250); //read the data in 4Hz
                    }
                    catch (ArgumentNullException e)
                    {
                        Console.WriteLine("ArgumentNullException: {0}", e);
                        ErrorInfo = e.Message;
                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine("SocketException: {0}", e);
                        ErrorInfo = e.Message;
                    }
                }
                System.Diagnostics.Debug.WriteLine("Client thread has been Stopped!");
            }).Start();
        }
        public string SendCommand(string paramPath, bool type)
        {
            return SendCommand(paramPath, false, 0);
        }
        /// <summary>
        /// send get/set command to server
        /// </summary>
        /// <param name="paramPath">path of the parameter</param>
        /// <param name="type">get (false) or set (true)</param>
        /// <param name="paramValue">value of parameter (necessary only if set command)</param>
        /// <returns>the value that returns from server</returns>
        public string SendCommand(string paramPath, bool type, double paramValue)
        {
            string command;

            if (!type) { command = "get " + paramPath + "\n"; }
            else { command = "set " + paramPath + " " + paramValue + "\n"; }

            try
            {
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(command); /// Translate the passed message into ASCII and store it as a Byte array.

                stream = tcpClient.GetStream(); /// Get a client stream for reading and writing.

                stream.Write(data, 0, data.Length); /// Send the message to the connected TcpServer. 

                //Console.WriteLine("Sent: {0}", command);

                data = new Byte[256]; ///  Receive the TcpServer.response. Buffer to store the response bytes.

                String responseData = String.Empty; /// String to store the response ASCII representation.

                                                    /// Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                //Console.WriteLine("Received: {0}", responseData);

                return responseData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ErrorInfo = e.Message;
                Disconnect();
                return "ERR";
            }
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
            set
            {
                if (throttle != value)
                {
                    if (value > 1)
                    {
                        throttle = 1;
                        ErrorInfo = "throttle value is out of bounds";
                    }
                    else if (value < 0)
                    {
                        ErrorInfo = "throttle value is out of bounds";
                        throttle = 0;
                    }
                    else { throttle = value; }

                    if (!stop) { SendCommand(THROTTLE, true, value); }

                    NotifyPropertyChanged("Throttle");
                }
            }
        }
        public double Aileron
        {
            get { return aileron; }
            set
            {
                if (aileron != value)
                {
                    if (value > 1)
                    {
                        ErrorInfo = "aileron value is out of bounds";
                        aileron = 1;
                    }
                    else if (value < -1)
                    {
                        ErrorInfo = "aileron value is out of bounds";
                        aileron = -1;
                    }
                    else { aileron = value; }

                    if (!stop) { SendCommand(AILERON, true, value); }

                    NotifyPropertyChanged("Aileron");
                }
            }
        }
        public double Elevator
        {
            get { return elevator; }
            set
            {
                if (value > 170)
                {
                    ErrorInfo = "elevator value is out of bounds";
                    elevator = 170;
                }
                else if (value < -170)
                {
                    ErrorInfo = "elevator value is out of bounds";
                    elevator = -170;
                }
                else { elevator = value; }

                if (!stop) { SendCommand(ELEVATOR, true, Normalize(value)); }
                NormalElevator = Normalize(value);

                NotifyPropertyChanged("Elevator");
            }
        }
        public double NormalElevator
        {
            get { return normalElevator; }
            set { normalElevator = value; NotifyPropertyChanged("NormalElevator"); }
        }
        public double Rudder
        {
            get { return rudder; }
            set
            {
                if (value > 170)
                {
                    ErrorInfo = "rudder value is out of bounds";
                    rudder = 170;
                }
                else if (value < -170)
                {
                    ErrorInfo = "rudder value is out of bounds";
                    rudder = -170;
                }
                else { rudder = value; }

                if (!stop) { SendCommand(RUDDER, true, Normalize(value)); }
                NormalRudder = Normalize(value);

                NotifyPropertyChanged("Rudder");

            }
        }
        public double NormalRudder
        {
            get { return normalRudder; }
            set { normalRudder = value; NotifyPropertyChanged("NormalRudder"); }
        }
        public double Latitude
        {
            get { return latitude; }
            set
            {
                if (value >= 90)
                {
                    ErrorInfo = "latitude value is out of bounds";
                    latitude = 90;
                }
                else if (value <= -90)
                {
                    ErrorInfo = "latitude value is out of bounds";
                    latitude = -90;
                }
                else { latitude = value; }
                NotifyPropertyChanged("Latitude");
            }
        }
        public double Longitude
        {
            get { return longitude; }
            set
            {
                if (value > 180)
                {
                    ErrorInfo = "longitude value is out of bounds";
                    longitude = 180;
                }
                else if (value < -180)
                {
                    ErrorInfo = "longitude value is out of bounds";
                    longitude = -180;
                }
                else { longitude = value; }
                NotifyPropertyChanged("Longitude");
            }
        }
        public string Location
        {
            get { return location; }
            set { location = value; NotifyPropertyChanged("Location"); }
        }
        public string ErrorInfo
        {
            get { return errorInfo; }
            set
            {
                timer.Restart();

                errorInfo = value;
                NotifyPropertyChanged("ErrorInfo");

                new Thread(delegate ()
                {
                    Thread.Sleep(7000);
                    if (timer.ElapsedMilliseconds >= 7000)
                    {
                        errorInfo = "";
                        NotifyPropertyChanged("ErrorInfo");
                    }

                }).Start();
            }
        }

        private double Normalize(double value)
        {
            double min = -170, max = 170; //range of original joystick is [-170,170]

            //formula to normalize to [-1,1]
            double retVal = 2 * ((value - min) / (max - min)) - 1;

            return retVal;
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
