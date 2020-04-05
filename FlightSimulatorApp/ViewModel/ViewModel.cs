using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows;

namespace FlightSimulatorApp
{
    class ViewModel : INotifyPropertyChanged
    {
        private IModel model;
        public ViewModel(IModel model)
        {
            this.model = model;
            model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        public void Disconnect()
        {
            model.Disconnect();
        }
        public void Connect(string ip, int port)
        {
            model.Connect(ip, port);
        }
        public void Start()
        {
            model.Start();
        }
        public bool IsConnected()
        {
            return model.IsConnected();
        }

        #region Public Properties
        public string VM_Heading
        {
            get
            {
                if (model.Heading != null && !model.Heading.Contains("ERR"))
                {
                    double d_value = Double.Parse(model.Heading);
                    d_value = Math.Round(d_value, 2, MidpointRounding.ToEven);
                    return d_value.ToString();
                }
                else
                {
                    return model.Heading;
                }
            }
        }
        public string VM_VerticalSpeed
        {
            get
            {
                if (model.VerticalSpeed != null && !model.VerticalSpeed.Contains("ERR"))
                {
                    double d_value = Double.Parse(model.VerticalSpeed);
                    d_value = Math.Round(d_value, 2, MidpointRounding.ToEven);
                    return d_value.ToString();
                }
                else
                {
                    return model.VerticalSpeed;
                }
            }
        }
        public string VM_GroundSpeed
        {
            get
            {
                if (model.GroundSpeed != null && !model.GroundSpeed.Contains("ERR"))
                {
                    double d_value = Double.Parse(model.GroundSpeed);
                    d_value = Math.Round(d_value, 2, MidpointRounding.ToEven);
                    return d_value.ToString();
                }
                else
                {
                    return model.GroundSpeed;
                }
            }
        }
        public string VM_AirSpeed
        {
            get
            {
                if (model.AirSpeed != null && !model.AirSpeed.Contains("ERR"))
                {
                    double d_value = Double.Parse(model.AirSpeed);
                    d_value = Math.Round(d_value, 2, MidpointRounding.ToEven);
                    return d_value.ToString();
                }
                else
                {
                    return model.AirSpeed;
                }
            }
        }
        public string VM_Altitude
        {
            get
            {
                if (model.Altitude != null && !model.Altitude.Contains("ERR"))
                {
                    double d_value = Double.Parse(model.Altitude);
                    d_value = Math.Round(d_value, 2, MidpointRounding.ToEven);
                    return d_value.ToString();
                }
                else
                {
                    return model.Altitude;
                }
            }
        }
        public string VM_Roll
        {
            get
            {
                if (model.Roll != null && !model.Roll.Contains("ERR"))
                {
                    double d_value = Double.Parse(model.Roll);
                    d_value = Math.Round(d_value, 2, MidpointRounding.ToEven);
                    return d_value.ToString();
                }
                else
                {
                    return model.Roll;
                }
            }
        }
        public string VM_Pitch
        {
            get
            {
                if (model.Pitch != null && !model.Pitch.Contains("ERR"))
                {
                    double d_value = Double.Parse(model.Pitch);
                    d_value = Math.Round(d_value, 2, MidpointRounding.ToEven);
                    return d_value.ToString();
                }
                else
                {
                    return model.Pitch;
                }
            }
        }
        public string VM_Altimeter
        {
            get
            {
                if (model.Altimeter != null && !model.Altimeter.Contains("ERR"))
                {
                    double d_value = Double.Parse(model.Altimeter);
                    d_value = Math.Round(d_value, 2, MidpointRounding.ToEven);
                    return d_value.ToString();
                }
                else
                {
                    return model.Altimeter;
                }
            }
        }
        public string VM_Throttle
        {
            get
            {
                if (model.Throttle != null && !model.Throttle.Contains("ERR"))
                {
                    double d_value = Double.Parse(model.Throttle);
                    d_value = Math.Round(d_value, 2, MidpointRounding.ToEven);
                    return d_value.ToString();
                }
                else
                {
                    return model.Throttle;
                }
            }
            set { model.Throttle = value; }
        }
        public string VM_Aileron
        {
            get
            {
                if (model.Aileron != null && !model.Aileron.Contains("ERR"))
                {
                    double d_value = Double.Parse(model.Aileron);
                    d_value = Math.Round(d_value, 2, MidpointRounding.ToEven);
                    return d_value.ToString();
                }
                else
                {
                    return model.Aileron;
                }
            }
            set { model.Aileron = value; }
        }
        public string VM_Elevator
        {
            get
            {
                if (model.Elevator != null && !model.Elevator.Contains("ERR"))
                {
                    double d_value = Double.Parse(model.Elevator);
                    d_value = Math.Round(d_value, 2, MidpointRounding.ToEven);
                    return d_value.ToString();
                }
                else
                {
                    return model.Elevator;
                }
            }
            set { model.Elevator = value; }
        }
        public string VM_NormalElevator
        {
            get
            {
                if (model.NormalElevator != null && !model.NormalElevator.Contains("ERR"))
                {
                    double d_value = Double.Parse(model.NormalElevator);
                    d_value = Math.Round(d_value, 2, MidpointRounding.ToEven);
                    return d_value.ToString();
                }
                else
                {
                    return model.NormalElevator;
                }
            }
        }
        public string VM_Rudder
        {
            get
            {
                if (model.Rudder != null && !model.Rudder.Contains("ERR"))
                {
                    double d_value = Double.Parse(model.Rudder);
                    d_value = Math.Round(d_value, 2, MidpointRounding.ToEven);
                    return d_value.ToString();
                }
                else
                {
                    return model.Rudder;
                }
            }
            set { model.Rudder = value; }
        }
        public string VM_NormalRudder
        {
            get
            {
                if (model.NormalRudder != null && !model.NormalRudder.Contains("ERR"))
                {
                    double d_value = Double.Parse(model.NormalRudder);
                    d_value = Math.Round(d_value, 2, MidpointRounding.ToEven);
                    return d_value.ToString();
                }
                else
                {
                    return model.NormalRudder;
                }
            }
        }
        public string VM_Latitude
        {
            get
            {
                if (model.Latitude != null && !model.Latitude.Contains("ERR"))
                {
                    double d_value = Double.Parse(model.Latitude);
                    d_value = Math.Round(d_value, 4, MidpointRounding.ToEven);
                    return d_value.ToString();
                }
                else
                {
                    return model.Latitude;
                }
            }
        }
        public string VM_Longitude
        {
            get
            {
                if (model.Longitude != null && !model.Longitude.Contains("ERR"))
                {
                    double d_value = Double.Parse(model.Longitude);
                    d_value = Math.Round(d_value, 4, MidpointRounding.ToEven);
                    return d_value.ToString();
                }
                else
                {
                    return model.Longitude;
                }
            }
        }
        public string VM_Angle
        {
            get { return model.Angle; }
        }
        public string VM_Location
        {
            get { return model.Location; }
        }
        public string VM_ErrorInfo
        {
            get { return model.ErrorInfo; }
        }
        public string VM_ConnectionStatus
        {
            get { return model.ConnectionStatus; }
        }
        public SolidColorBrush VM_ConnectionColor
        {
            get { return model.ConnectionColor; }
        }
        public Visibility VM_Vis
        {
            get { return model.Vis; }
        }
    }
    #endregion
}

