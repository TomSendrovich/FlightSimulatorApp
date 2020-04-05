using System.ComponentModel;
using System.Windows.Media;
using System.Windows;

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
        string Heading { get; set; }
        string VerticalSpeed { get; set; }
        string GroundSpeed { get; set; }
        string AirSpeed { get; set; }
        string Altitude { get; set; }
        string Roll { get; set; }
        string Pitch { get; set; }
        string Altimeter { get; set; }
        string Throttle { get; set; }
        string Aileron { get; set; }
        string Elevator { get; set; }
        string NormalElevator { get; set; }
        string Rudder { get; set; }
        string NormalRudder { get; set; }
        string Latitude { get; set; }
        string Longitude { get; set; }
        string Angle { get; set; }
        string Location { get; set; }
        string ErrorInfo { get; set; }
        string ConnectionStatus { get; set; }
        SolidColorBrush ConnectionColor { get; set; }
        Visibility Vis { get; set; }
    }
}
