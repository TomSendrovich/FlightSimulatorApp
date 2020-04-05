using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FlightSimulatorApp.View.Controls
{
    /// <summary>
    /// Interaction logic for Joystick.xaml
    /// </summary>
    public partial class Joystick : UserControl
    {
        //data members
        private Point firstPoint = new Point();
        //bool mousePressed;

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
                //mousePressed = true;

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

                    //Console.WriteLine("x: " + x + ", y: " + y);
                }
            }
        }

        private void Knob_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //mousePressed = false;
            knobPosition.X = 0;
            knobPosition.Y = 0;
        }

        //helping methods

        /* private double Length(double x, double y, double x1, double y1)
         {
             return Math.Sqrt((x1 - x) * (x1 - x) + (y1 - y) * (y1 - y));
         }*/
    }
}
