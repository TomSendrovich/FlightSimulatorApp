using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FlightSimulatorApp
{
    interface IModel : INotifyPropertyChanged
    {
        //connection to the simulator
        void Connect(string ip, int port);
        void Disconnect();
        void Start();
        bool IsConnected();

        //properties
        double Heading { get; set; }
        double VerticalSpeed { get; set; }
        double GroundSpeed { get; set; }
        double AirSpeed { get; set; }
        double Altitude { get; set; }
        double Roll { get; set; }
        double Pitch { get; set; }
        double Altimeter { get; set; }
        double Throttle { get; set; }
        double Aileron { get; set; }
        double Elevator { get; set; }
        double NormalElevator { get; set; }
        double Rudder { get; set; }
        double NormalRudder { get; set; }
        double Latitude { get; set; }
        double Longitude { get; set; }
        double Angle { get; set; }
        string Location { get; set; }
        string ErrorInfo { get; set; }
    }
}
