using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ViAppleGrab;
using System.Drawing;

namespace ResultsAnalysis
{
    class Game
    {
        public bool IsWarmup { get; private set; }

        public ControlType TypeOfControl { get; private set; }

        public GameType TypeOfGame { get; private set; }

        public DateTime TimeStarted { get; private set; }

        public DateTime TimeEnded { get; private set; }

        public int Score { get; private set; }

        public int TargetCount { get; private set; }

        public List<Target> Targets { get; private set; }

        public double AvgTimePerTarget { get; private set; }

        public double AvgXTimePerTarget { get; private set; }

        public double AvgYTimePerTarget { get; private set; }

        public double PercentStartInRangeX { get; private set; }

        public double PercentStartInRangeY { get; private set; }

        public double AvgTimePerPixel { get; private set; }

        public double AvgTimeOutOfBoundsLeft { get; private set; }

        public double AvgTimeOutOfBoundsRight { get; private set; }

        public double AvgTimeOutOfBounds { get; private set; }

        public double AvgTimeLeft_GroupA { get; private set; }

        public double AvgTimeRight_GroupA { get; private set; }

        public double AvgTimeLeft_GroupB { get; private set; }

        public double AvgTimeRight_GroupB { get; private set; }

        public double AvgInitDistance { get; private set; }

        public double AvgInitXDist { get; private set; }

        public double AvgInitYDist { get; private set; }

        public int AvgNumberOfMisses { get; private set; }

        public Game(XmlNode gameNode, XmlNode targetNode)
        {
            IsWarmup = Boolean.Parse(gameNode.Attributes["IsWarmup"].Value);

            TypeOfControl = (ControlType)Enum.Parse(typeof(ControlType), gameNode.Attributes["ControlType"].Value);

            TypeOfGame = (GameType)Enum.Parse(typeof(GameType), gameNode.Attributes["GameType"].Value);

            TimeStarted = DateTime.Parse(gameNode.SelectSingleNode("TimeStarted").InnerText);

            TimeEnded = DateTime.Parse(gameNode.SelectSingleNode("TimeEnded").InnerText);

            Score = Int32.Parse(gameNode.SelectSingleNode("Score").InnerText);

            _parseTargets(targetNode);

            _calculateStats();
        }

        private void _calculateStats()
        {
            double temp;
            double sum = 0.0d;
            double sumP = 0.0d;
            double sumD = 0.0d;
            double sumDx = 0.0d;
            double sumDy = 0.0d;
            double sumRight = 0.0d;
            double sumLeft = 0.0d;

            bool lastControllerWasRight = false;

            AvgTimeOutOfBounds = 0.0d;
            AvgTimeOutOfBoundsLeft = 0.0d;
            AvgTimeOutOfBoundsRight = 0.0d;

            AvgXTimePerTarget = 0; //temp
            AvgYTimePerTarget = 0; //temp

            PercentStartInRangeX = 0;
            PercentStartInRangeY = 0;

            //Loop across all of the targets calculating average time to target
            foreach (Target t in Targets)
            {
                temp = t.ScanningTime + t.RecognitionTime;
                sum += temp;
                sumP += temp / t.InitDistance;
                sumD += t.InitDistance;
                sumDx += t.InitXDist;
                sumDy += t.InitYDist;

                if (TypeOfControl == ControlType.Alternating)
                {
                    AvgTimeOutOfBounds += t.TotalOutOfBoundsTimeLeft + t.TotalOutOfBoundsTimeRight;
                    AvgTimeOutOfBoundsLeft += t.TotalOutOfBoundsTimeLeft;
                    AvgTimeOutOfBoundsRight += t.TotalOutOfBoundsTimeRight;

                    if (t.CollectedByRight)
                    {
                        sumRight += temp;
                    }
                    else
                    {
                        sumLeft += temp;
                    }
                }
                else
                {
                    AvgTimeOutOfBoundsLeft += t.TotalOutOfBoundsTimeLeft;
                    AvgTimeOutOfBoundsRight += t.TotalOutOfBoundsTimeRight;
                }

                //Check X/Y bounds


                CalculateXYStats(t, lastControllerWasRight);

                AvgXTimePerTarget += t.TimeToFindX;
                AvgYTimePerTarget += t.TimeToFindY;

                PercentStartInRangeX += (t.StartedInX) ? 1 : 0;
                PercentStartInRangeY += (t.StartedInY) ? 1 : 0;

                if (TypeOfControl == ControlType.Alternating)
                    lastControllerWasRight = !lastControllerWasRight;
            }
            
            //Calculate the out of bounds time
            if (TypeOfControl == ControlType.Alternating)
            {
                AvgTimeOutOfBounds /= TargetCount;
                AvgTimeOutOfBoundsLeft /= (TargetCount / 2);
                AvgTimeOutOfBoundsRight /= (TargetCount / 2);
                sumRight /= (TargetCount / 2);
                sumLeft /= (TargetCount / 2);
                AvgTimeLeft_GroupA = sumLeft;
                AvgTimeRight_GroupA = sumRight;
            }
            else
            {
                AvgTimeOutOfBoundsLeft /= TargetCount;
                AvgTimeOutOfBoundsRight /= TargetCount;
                AvgTimeLeft_GroupA = 0;
                AvgTimeRight_GroupA = 0;
            }

            AvgTimePerTarget = sum / TargetCount;
            AvgTimePerPixel = sumP / TargetCount;
            AvgInitDistance = sumD / TargetCount;
            AvgInitXDist = sumDx / TargetCount;
            AvgInitYDist = sumDy / TargetCount;

            AvgXTimePerTarget /= TargetCount;
            AvgYTimePerTarget /= TargetCount;

            PercentStartInRangeX /= TargetCount;
            PercentStartInRangeY /= TargetCount;
        }

        public void CalculateXYStats(Target t, bool lastControllerWasRight)
        {
            t.StartedInX = false;
            bool FoundX = false;
            double xSearchTime = 0.0d;
            double xReserve = 0.0d;

            t.StartedInY = false;
            bool FoundY = false;
            double ySearchTime = 0.0d;
            double yReserve = 0.0d;

            //Determine if the controller started in either x or y range of the target
            if (TypeOfControl == ControlType.Alternating)
            {
                if (isInTargetX(t, t.Positions[0].right))
                {
                    t.StartedInX = true;
                    FoundX = true;
                }

                if (isInTargetY(t, t.Positions[0].right))
                {
                    t.StartedInY = true;
                    FoundY = true;
                }

                foreach (TargetPosition tp in t.Positions)
                {
                    if (!lastControllerWasRight)
                    {
                        if (FoundX)
                        {
                            if (isInTargetX(t, tp.right))
                                xReserve += tp.ElapsedTime;
                            else
                            {
                                xSearchTime += xReserve + tp.ElapsedTime;
                                xReserve = 0;
                                FoundX = false;
                            }
                        }
                        else
                        {
                            if (isInTargetX(t, tp.right))
                            {
                                FoundX = true;
                                xReserve += tp.ElapsedTime;
                            }
                            else
                                xSearchTime += tp.ElapsedTime;
                        }

                        if (FoundY)
                        {
                            if (isInTargetY(t, tp.right))
                                yReserve += tp.ElapsedTime;
                            else
                            {
                                ySearchTime += yReserve + tp.ElapsedTime;
                                yReserve = 0;
                                FoundY = false;
                            }
                        }
                        else
                        {
                            if (isInTargetY(t, tp.right))
                            {
                                FoundY = true;
                                yReserve += tp.ElapsedTime;
                            }
                            else
                                ySearchTime += tp.ElapsedTime;
                        }
                    }
                    else
                    {
                        if (FoundX)
                        {
                            if (isInTargetX(t, tp.left))
                                xReserve += tp.ElapsedTime;
                            else
                            {
                                xSearchTime += xReserve + tp.ElapsedTime;
                                xReserve = 0;
                                FoundX = false;
                            }
                        }
                        else
                        {
                            if (isInTargetX(t, tp.left))
                            {
                                FoundX = true;
                                xReserve += tp.ElapsedTime;
                            }
                            else
                                xSearchTime += tp.ElapsedTime;
                        }

                        if (FoundY)
                        {
                            if (isInTargetY(t, tp.left))
                                yReserve += tp.ElapsedTime;
                            else
                            {
                                ySearchTime += yReserve + tp.ElapsedTime;
                                yReserve = 0;
                                FoundY = false;
                            }
                        }
                        else
                        {
                            if (isInTargetY(t, tp.left))
                            {
                                FoundY = true;
                                yReserve += tp.ElapsedTime;
                            }
                            else
                                ySearchTime += tp.ElapsedTime;
                        }
                    }
                }
            }
            else
            {
                if (isInTargetX(t, t.Positions[0].right))
                {
                    t.StartedInX = true;
                    FoundX = true;
                }

                if (isInTargetY(t, t.Positions[0].left))
                {
                    t.StartedInY = true;
                    FoundY = true;
                }

                foreach (TargetPosition tp in t.Positions)
                {
                    if (FoundX)
                    {
                        if (isInTargetX(t, tp.right))
                            xReserve += tp.ElapsedTime;
                        else
                        {
                            xSearchTime += xReserve + tp.ElapsedTime;
                            xReserve = 0;
                            FoundX = false;
                        }
                    }
                    else
                    {
                        if (isInTargetX(t, tp.right))
                        {
                            FoundX = true;
                            xReserve += tp.ElapsedTime;
                        }
                        else
                            xSearchTime += tp.ElapsedTime;
                    }

                    if (FoundY)
                    {
                        if (isInTargetY(t, tp.left))
                            yReserve += tp.ElapsedTime;
                        else
                        {
                            ySearchTime += yReserve + tp.ElapsedTime;
                            yReserve = 0;
                            FoundY = false;
                        }
                    }
                    else
                    {
                        if (isInTargetY(t, tp.left))
                        {
                            FoundY = true;
                            yReserve += tp.ElapsedTime;
                        }
                        else
                            ySearchTime += tp.ElapsedTime;
                    }
                }
            }

            t.TimeToFindX = xSearchTime;
            t.TimeToFindY = ySearchTime;
        }

        public bool isInTargetX(Target t, Point p)
        {
            if (p.X >= (t.Location.X - 40) && p.X <= (t.Location.X + 40))
                return true;
            else
                return false;
        }

        public bool isInTargetY(Target t, Point p)
        {
            if (p.Y >= (t.Location.Y - 50) && p.Y <= (t.Location.Y + 50))
                return true;
            else
                return false;
        }

        public void AppendDistances(ref List<double> distList)
        {
            foreach (Target t in Targets)
            {
                distList.Add(t.InitDistance);
            }
        }

        public void AppendTimesPerPixel(ref List<double> l)
        {
            foreach (Target t in Targets)
            {
                l.Add((t.ScanningTime + t.RecognitionTime) / t.InitDistance * 1000);
            }
        }

        private void _parseTargets(XmlNode targetNode)
        {
            XmlNodeList nodes = targetNode.SelectNodes("TargetData");

            Targets = new List<Target>();

            foreach(XmlNode n in nodes)
            {
                Targets.Add(new Target(n));
            }

            TargetCount = Targets.Count;
        }
    }
}
