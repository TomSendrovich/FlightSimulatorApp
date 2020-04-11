using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace FlightSimulatorApp
{
    class Model : IModel
    {
        const string HEADING = "/indicated-heading-deg";
        const string VERTICAL_SPEED = "/gps_indicated-vertical-speed";
        const string GROUND_SPEED = "/gps_indicated-ground-speed-kt";
        const string AIR_SPEED = "/airspeed-indicator_indicated-speed-kt";
        const string ALTITUDE = "/gps_indicated-altitude-ft";
        const string ROLL = "/attitude-indicator_internal-roll-deg";
        const string PITCH = "/attitude-indicator_internal-pitch-deg";
        const string ALTIMETER = "/altimeter_indicated-altitude-ft";
        const string THROTTLE = "/controls/engines/current-engine/throttle";
        const string AILERON = "/controls/flight/aileron";
        const string ELEVATOR = "/controls/flight/elevator";
        const string RUDDER = "/controls/flight/rudder";
        const string LATITUDE = "/position/latitude-deg";
        const string LONGITUDE = "/position/longitude-deg";

        TcpClient tcpClient;
        NetworkStream stream;
        Stopwatch timer = Stopwatch.StartNew();
        volatile Boolean stop, calcAngle = true;
        Visibility visibility;

        private string heading, verticalSpeed, groundSpeed, airSpeed, altitude, roll, pitch, altimeter, throttle, aileron, elevator, normalElevator, rudder, normalRudder, latitude, longitude, angle;
        private double originalLat, originalLong, prevLat, prevLong;
        private string location, errorInfo, connectionStatus;
        SolidColorBrush connectionColor;

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

                ConnectionStatus = "Connected";
                ConnectionColor = new SolidColorBrush(Colors.Green);
                connectionColor.Freeze();
                Vis = Visibility.Visible;
            }
            catch (SocketException e)
            {
                Console.WriteLine("Error: " + e.Message);
                ErrorInfo = "Error: Server cannot connect " + ip + ":" + port;

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
            ConnectionStatus = "Disconnected";
            ConnectionColor = new SolidColorBrush(Colors.Red);
            connectionColor.Freeze();
            Vis = Visibility.Hidden;
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
                string tmpLongitude, tmpLatitude;
                while (!stop)
                {
                    try
                    {
                        //Heading
                        Heading = SendCommand(HEADING, false);

                        //VerticalSpeed
                        VerticalSpeed = SendCommand(VERTICAL_SPEED, false);

                        //GroundSpeed
                        GroundSpeed = SendCommand(GROUND_SPEED, false);

                        //AirSpeed
                        AirSpeed = SendCommand(AIR_SPEED, false);

                        //Altitude
                        Altitude = SendCommand(ALTITUDE, false);

                        //Roll
                        Roll = SendCommand(ROLL, false);

                        //Pitch
                        Pitch = SendCommand(PITCH, false);

                        //Altimeter
                        Altimeter = SendCommand(ALTIMETER, false);


                        ///get location info
                        tmpLatitude = "0"; tmpLongitude = "0";
                        prevLat = originalLat; prevLong = originalLong;

                        //Latitude
                        Latitude = SendCommand(LATITUDE, false);
                        tmpLatitude = Latitude;
                        if (!Latitude.Contains("ERR"))
                        {
                            originalLat = Double.Parse(Latitude);
                        }

                        //Longitude
                        Longitude = SendCommand(LONGITUDE, false);
                        tmpLongitude = Longitude;
                        if (!Longitude.Contains("ERR"))
                        {
                            originalLong = Double.Parse(Longitude);
                        }

                        //Location
                        if (originalLat > 90)
                        {
                            originalLat = 90;
                            tmpLatitude = "90";
                        }
                        else if (originalLat < -90)
                        {
                            originalLat = -90;
                            tmpLatitude = "-90";
                        }
                        if (originalLong > 180)
                        {
                            originalLong = 180;
                            tmpLongitude = "180";
                        }
                        else if (originalLong < -180)
                        {
                            originalLong = -180;
                            tmpLongitude = "-180";
                        }
                        Location = tmpLatitude + "," + tmpLongitude;

                        //Angle
                        if (calcAngle)
                        {
                            Angle = AngleFromCoordinate(prevLat, prevLong, originalLat, originalLong).ToString();
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
            return SendCommand(paramPath, false, "0");
        }
        /// <summary>
        /// send get/set command to server
        /// </summary>
        /// <param name="paramPath">path of the parameter</param>
        /// <param name="type">get (false) or set (true)</param>
        /// <param name="paramValue">value of parameter (necessary only if set command)</param>
        /// <returns>the value that returns from server</returns>
        public string SendCommand(string paramPath, bool type, string paramValue)
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
                ErrorInfo = "Error: Connection was closed";
                Disconnect();
                return "ERR";
            }
        }

        #region Properties

        public string Heading
        {
            get { return heading; }
            set { heading = value; NotifyPropertyChanged("Heading"); }
        }
        public string VerticalSpeed
        {
            get { return verticalSpeed; }
            set { verticalSpeed = value; NotifyPropertyChanged("VerticalSpeed"); }
        }
        public string GroundSpeed
        {
            get { return groundSpeed; }
            set { groundSpeed = value; NotifyPropertyChanged("GroundSpeed"); }
        }
        public string AirSpeed
        {
            get { return airSpeed; }
            set { airSpeed = value; NotifyPropertyChanged("AirSpeed"); }
        }
        public string Altitude
        {
            get { return altitude; }
            set { altitude = value; NotifyPropertyChanged("Altitude"); }
        }
        public string Roll
        {
            get { return roll; }
            set { roll = value; NotifyPropertyChanged("Roll"); }
        }
        public string Pitch
        {
            get { return pitch; }
            set { pitch = value; NotifyPropertyChanged("Pitch"); }
        }
        public string Altimeter
        {
            get { return altimeter; }
            set { altimeter = value; NotifyPropertyChanged("Altimeter"); }
        }
        public string Throttle
        {
            get { return throttle; }
            set
            {
                if (throttle != value)
                {
                    if (Double.Parse(value) > 1)
                    {
                        throttle = "1";
                        ErrorInfo = "throttle value is out of bounds";
                    }
                    else if (Double.Parse(value) < 0)
                    {
                        throttle = "0";
                        ErrorInfo = "throttle value is out of bounds";
                    }
                    else { throttle = value; }

                    if (!stop) { SendCommand(THROTTLE, true, value); }

                    NotifyPropertyChanged("Throttle");
                }
            }
        }
        public string Aileron
        {
            get { return aileron; }
            set
            {
                if (aileron != value)
                {
                    if (Double.Parse(value) > 1)
                    {
                        ErrorInfo = "aileron value is out of bounds";
                        aileron = "1";
                    }
                    else if (Double.Parse(value) < -1)
                    {
                        ErrorInfo = "aileron value is out of bounds";
                        aileron = "-1";
                    }
                    else { aileron = value; }

                    if (!stop) { SendCommand(AILERON, true, value); }

                    NotifyPropertyChanged("Aileron");
                }
            }
        }
        public string Elevator
        {
            get { return elevator; }
            set
            {
                if (Double.Parse(value) > 170)
                {
                    elevator = "170";
                    ErrorInfo = "elevator value is out of bounds";
                }
                else if (Double.Parse(value) < -170)
                {
                    elevator = "-170";
                    ErrorInfo = "elevator value is out of bounds";
                }
                else { elevator = value; }

                if (!stop) { SendCommand(ELEVATOR, true, Normalize(value)); }
                NormalElevator = Normalize(value);

                NotifyPropertyChanged("Elevator");
            }
        }
        public string NormalElevator
        {
            get { return normalElevator; }
            set { normalElevator = value; NotifyPropertyChanged("NormalElevator"); }
        }
        public string Rudder
        {
            get { return rudder; }
            set
            {
                if (Double.Parse(value) > 170)
                {
                    ErrorInfo = "rudder value is out of bounds";
                    rudder = "170";
                }
                else if (Double.Parse(value) < -170)
                {
                    ErrorInfo = "rudder value is out of bounds";
                    rudder = "-170";
                }
                else { rudder = value; }

                if (!stop) { SendCommand(RUDDER, true, Normalize(value)); }
                NormalRudder = Normalize(value);

                NotifyPropertyChanged("Rudder");

            }
        }
        public string NormalRudder
        {
            get { return normalRudder; }
            set { normalRudder = value; NotifyPropertyChanged("NormalRudder"); }
        }
        public string Latitude
        {
            get { return latitude; }
            set
            {
                if (value.Contains("ERR"))
                {
                    latitude = value; calcAngle = false;
                }
                else if (Double.Parse(value) >= 90)
                {
                    ErrorInfo = "latitude value is out of bounds";
                    latitude = "90";
                    calcAngle = false;
                }
                else if (Double.Parse(value) <= -90)
                {
                    ErrorInfo = "latitude value is out of bounds";
                    latitude = "-90";
                    calcAngle = false;
                }
                else { latitude = value; calcAngle = true; }
                NotifyPropertyChanged("Latitude");
            }
        }
        public string Longitude
        {
            get { return longitude; }
            set
            {
                if (value.Contains("ERR"))
                {
                    longitude = value; calcAngle = false;
                }
                else if (Double.Parse(value) > 180)
                {
                    ErrorInfo = "longitude value is out of bounds";
                    longitude = "180";
                    calcAngle = false;
                }
                else if (Double.Parse(value) < -180)
                {
                    ErrorInfo = "longitude value is out of bounds";
                    longitude = "-180";
                    calcAngle = false;
                }
                else { longitude = value; calcAngle = true; }
                NotifyPropertyChanged("Longitude");
            }
        }
        public string Location
        {
            get { return location; }
            set { location = value; NotifyPropertyChanged("Location"); }
        }
        public string Angle
        {
            get { return angle; }
            set
            {
                angle = value;
                NotifyPropertyChanged("Angle");
                Console.WriteLine("Angle: " + value);
            }
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
        public string ConnectionStatus
        {
            get { return connectionStatus; }
            set { connectionStatus = value; NotifyPropertyChanged("ConnectionStatus"); }

        }
        public SolidColorBrush ConnectionColor
        {
            get { return connectionColor; }
            set { connectionColor = value; NotifyPropertyChanged("ConnectionColor"); }

        }
        public Visibility Vis
        {
            get { return visibility; }
            set { visibility = value; NotifyPropertyChanged("Vis"); }

        }

        private string Normalize(string value)
        {

            double min = -170, max = 170; //range of original joystick is [-170,170]

            //formula to normalize to [-1,1]
            double retVal = 2 * ((Double.Parse(value) - min) / (max - min)) - 1;

            return retVal.ToString();
        }

        private double AngleFromCoordinate(double lat1, double long1, double lat2, double long2)
        {
            double dLon = (long2 - long1);

            double x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1)
                    * Math.Cos(lat2) * Math.Cos(dLon);
            double y = Math.Sin(dLon) * Math.Cos(lat2);

            double radians = (Math.Atan2(y, x) + Math.PI * 2) % (Math.PI * 2);
            double degrees = radians * 180 / Math.PI;

            return degrees;
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
