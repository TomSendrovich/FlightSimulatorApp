﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Image Airplane
        {
            get { return this.airplane; }
        }
        public double VM_Heading
        {
            get { return model.Heading; }
        }
        public double VM_VerticalSpeed
        {
            get { return model.VerticalSpeed; }
        }
        public double VM_GroundSpeed
        {
            get { return model.GroundSpeed; }
        }
        public double VM_AirSpeed
        {
            get { return model.AirSpeed; }
        }
        public double VM_Altitude
        {
            get { return model.Altitude; }
        }
        public double VM_Roll
        {
            get { return model.Roll; }
        }
        public double VM_Pitch
        {
            get { return model.Pitch; }
        }
        public double VM_Altimeter
        {
            get { return model.Altimeter; }
        }
        public double VM_Throttle
        {
            get { return model.Throttle; }
            set { model.Throttle = value; }
        }
        public double VM_Aileron
        {
            get { return model.Aileron; }
            set { model.Aileron = value; }
        }
        public double VM_Elevator
        {
            get { return model.Elevator; }
            set { model.Elevator = value; }
        }
        public double VM_NormalElevator
        {
            get { return model.NormalElevator; }
        }
        public double VM_Rudder
        {
            get { return model.Rudder; }
            set { model.Rudder = value; }
        }
        public double VM_NormalRudder
        {
            get { return model.NormalRudder; }
        }
        public double VM_Latitude
        {
            get { return model.Latitude; }
        }
        public double VM_Longitude
        {
            get { return model.Longitude; }
        }
        public double VM_Angle
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
    }
    #endregion
}

