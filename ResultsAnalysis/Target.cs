using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;
using ViAppleGrab;
using System.Windows.Forms.DataVisualization.Charting;

namespace ResultsAnalysis
{
    class Target
    {
        public int ID { get; private set; }

        public bool IsRotten { get; private set; }

        public bool CollectedByRight { get; private set; }

        public Point Location { get; private set; }

        public double InitDistance { get; private set; }

        public double InitXDist { get; private set; }

        public double InitYDist { get; private set; }

        public DateTime TimeCreated { get; private set; }

        public ControllerIndex Controller { get; private set; }

        public double ScanningTime { get; private set; }

        public double RecognitionTime { get; private set; }

        public double TotalAliveTime { get; private set; }

        public double TotalOutOfBoundsTimeLeft { get; private set; }

        public double TotalOutOfBoundsTimeRight { get; private set; }

        public double TimeToFindX { get; set; }

        public double TimeToFindY { get; set; }

        public bool StartedInX { get; set; }

        public bool StartedInY { get; set; }

        public List<TargetPosition> Positions { get; private set; }

        public bool RightPositions { get; private set; }

        public bool LeftPositions { get; private set; }

        public Target(XmlNode targetDataNode)
        {
            ID = Int32.Parse(targetDataNode.Attributes["ID"].Value);
            IsRotten = Boolean.Parse(targetDataNode.Attributes["IsRotten"].Value);
            Location = new Point(
                Int32.Parse(targetDataNode.Attributes["X"].Value),
                Int32.Parse(targetDataNode.Attributes["Y"].Value)
                );
            TimeCreated = DateTime.Parse(targetDataNode.Attributes["Time"].Value);
            Controller = (ControllerIndex)Enum.Parse(typeof(ControllerIndex), targetDataNode.Attributes["Controller"].Value);
            ScanningTime = Double.Parse(targetDataNode.SelectSingleNode("ScanningTime").InnerText);
            RecognitionTime = Double.Parse(targetDataNode.SelectSingleNode("RecognitionTime").InnerText);
            TotalAliveTime = Double.Parse(targetDataNode.SelectSingleNode("TotalAliveTime").InnerText);

            _parseLocations(targetDataNode.SelectSingleNode("RightController"),
                targetDataNode.SelectSingleNode("LeftController"));

            CollectedByRight = Positions[Positions.Count - 1].rTargetState == TargetState.Collecting;

            _setAnalytics();
        }

        private void _setAnalytics()
        {
            Point p1;
            Point p2 = Location;
            
            int index = (ID == 0) ? 1 : 0;

            if (CollectedByRight)
            {
                p1 = Positions[index].right;
            }
            else
            {
                p1 = Positions[index].left;
            }

            InitDistance = Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y));
            InitXDist = Math.Abs(p2.X - p1.X);
            InitYDist = Math.Abs(p2.Y - p1.Y);

            TotalOutOfBoundsTimeLeft = 0;
            TotalOutOfBoundsTimeRight = 0;
            
            foreach (TargetPosition tp in Positions)
            {
                if(tp.IsLeftOutOfBounds)
                    TotalOutOfBoundsTimeLeft += tp.ElapsedTime;

                if (tp.IsRightOutOfBounds)
                    TotalOutOfBoundsTimeRight += tp.ElapsedTime;
            }
        }

        private void _parseLocations(XmlNode right, XmlNode left)
        {
            int num = 0;
            Positions = new List<TargetPosition>();
            XmlNodeList r = null, l = null;

            if (right != null && left != null)
            {
                r = right.SelectNodes("Position");
                l = left.SelectNodes("Position");
                num = r.Count;

                for (int i = 0; i < num; i++)
                {
                    Positions.Add(new TargetPosition(r[i], l[i]));
                }

                RightPositions = true;
                LeftPositions = true;
            }
            else if (right != null)
            {
                r = right.SelectNodes("Position");
                num = r.Count;

                for (int i = 0; i < num; i++)
                {
                    Positions.Add(new TargetPosition(r[i], null));
                }

                RightPositions = true; 
                LeftPositions = false;
            }
            else
            {
                l = left.SelectNodes("Position");
                num = l.Count;

                for (int i = 0; i < num; i++)
                {
                    Positions.Add(new TargetPosition(null, l[i]));
                }

                RightPositions = false;
                LeftPositions = true;
            }
        }
    }
}
