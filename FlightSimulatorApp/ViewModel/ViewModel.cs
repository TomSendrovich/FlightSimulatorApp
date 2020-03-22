using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        #region Public Properties
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

        #endregion
    }
}
