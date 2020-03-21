using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FlightSimulatorApp.Model
{
    interface IModel : INotifyPropertyChanged
    {
        //connection to the simulator
        void connect(string ip, int port);
        void disconnect();
        void start();

        //properties (all parameters from simulator?)
        double Heading { get; set; }
    }
}
