using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;
using ViAppleGrab;

namespace ResultsAnalysis
{
    class TargetPosition
    {
        public Point right { get; private set; }

        public Point left { get; private set; }

        public int ID { get; private set; }

        public bool IsLeftOutOfBounds { get; private set; }

        public bool IsRightOutOfBounds { get; private set; }

        public double ElapsedTime { get; private set; }

        public TargetState rTargetState { get; private set; }

        public InputButtonState rTriggerState { get; private set; }

        public TargetState lTargetState { get; private set; }

        public InputButtonState lTriggerState { get; private set; }

        public TargetPosition(XmlNode r, XmlNode l)
        {
            if (r != null)
            {
                right = new Point(
                    Int32.Parse(r.Attributes["X"].Value),
                    Int32.Parse(r.Attributes["Y"].Value)
                    );
                rTargetState = (TargetState)Enum.Parse(typeof(TargetState), r.Attributes["TargetState"].Value);
                rTriggerState = (InputButtonState)Enum.Parse(typeof(InputButtonState), r.Attributes["TriggerState"].Value);

                IsRightOutOfBounds = CheckBounds(right);

                ID = Int32.Parse(r.Attributes["ID"].Value);
                ElapsedTime = Double.Parse(r.Attributes["ElapsedTime"].Value);
            }
            else
            {
                right = Point.Empty;

                IsRightOutOfBounds = false;

                rTargetState = TargetState.Inactive;
                rTriggerState = InputButtonState.NotPressed;
            }

            if (l != null)
            {
                left = new Point(
                    Int32.Parse(l.Attributes["X"].Value),
                    Int32.Parse(l.Attributes["Y"].Value)
                    );
                lTargetState = (TargetState)Enum.Parse(typeof(TargetState), l.Attributes["TargetState"].Value);
                lTriggerState = (InputButtonState)Enum.Parse(typeof(InputButtonState), l.Attributes["TriggerState"].Value);
                
                IsLeftOutOfBounds = CheckBounds(left);

                ID = Int32.Parse(l.Attributes["ID"].Value);
                ElapsedTime = Double.Parse(l.Attributes["ElapsedTime"].Value);
            }
            else
            {
                left = Point.Empty;

                IsLeftOutOfBounds = false;

                lTargetState = TargetState.Inactive;
                lTriggerState = InputButtonState.NotPressed;
            }
        }

        private bool CheckBounds(Point p)
        {
            if (p.X > 600 || p.X < 40 || p.Y > 430 || p.Y < 50)
                return true;
            else
                return false;
        }
    }
}
