using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightSimulatorApp.View.Controls
{
    /// <summary>
    /// Interaction logic for Joystick.xaml
    /// </summary>
    public partial class Joystick : UserControl
    {
        //data members
        private Point firstPoint = new Point();
        bool mousePressed;

        public Joystick()
        {
            InitializeComponent();
        }


        private void centerKnob_Completed(Object sender, EventArgs e)
        {
            // Not developed yet.
            throw new NotImplementedException();
        }

        private void Knob_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                firstPoint = e.GetPosition(this);
                mousePressed = true;
                
            }
        }

        private void Knob_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                double x = e.GetPosition(this).X - firstPoint.X;
                double y = e.GetPosition(this).Y - firstPoint.Y;
                if (Math.Sqrt(x * x + y * y) < Base.Width / 2)
                {
                    knobPosition.X = x;
                    knobPosition.Y = y;
                }
            }
        }

        private void Knob_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mousePressed = false;
            knobPosition.X = 0;
            knobPosition.Y = 0;
        }

        //helping method
        private double length(double x, double y, double x1, double y1)
        {
            return Math.Sqrt((x1 - x) * (x1 - x) + (y1 - y) * (y1 - y));
        }
    }


}
