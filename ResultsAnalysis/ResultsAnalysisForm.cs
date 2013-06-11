using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using Ionic.Zip;

namespace ResultsAnalysis
{
    public partial class ResultsAnalysisForm : Form
    {
        FolderBrowserDialog fbd;
        DirectoryInfo dir;
        ResultsCollection results = new ResultsCollection();

        public ResultsAnalysisForm()
        {
            InitializeComponent();

            fbd = new FolderBrowserDialog();
            fbd.Description = "Select the folder containing the ViAppleGrabResults";
            fbd.SelectedPath = @"C:\Users\afiannac2\SkyDrive\Graduate\Results_ViAppleGrab_082112";
            fbd.ShowNewFolderButton = false;

            if (fbd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                Application.Exit();
            }

            using (Stream writer = new FileStream(@"C:\Users\Alex\SkyDrive\Graduate\Results_ViAppleGrab_082112\Images\blank.jpg", FileMode.Create))
            {
                targetGraph.SaveImage(writer, ChartImageFormat.Png);
                writer.Close();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ResultsAnalysisForm_Load(object sender, EventArgs e)
        {
            avgTimePerPixelLabel1.DataBindings[0].Format += new ConvertEventHandler(AvgTimePerPixel_Format);
            avgTimePerTargetLabel1.DataBindings[0].Format += new ConvertEventHandler(AvgTimePerTarget_Format);

            lblAvgXTime.DataBindings[0].Format += new ConvertEventHandler(AvgXYTimePerPixel_Format);
            lblAvgYTime.DataBindings[0].Format += new ConvertEventHandler(AvgXYTimePerPixel_Format);

            PercStartedInXRange.DataBindings[0].Format += new ConvertEventHandler(Percent_Format);
            PercStartedInYRange.DataBindings[0].Format += new ConvertEventHandler(Percent_Format);

            lblAvgTimeOutOfBounds.DataBindings[0].Format += new ConvertEventHandler(AvgOutOfBoundsTime_Format);
            lblAvgTimeOutOfBoundsL.DataBindings[0].Format += new ConvertEventHandler(AvgOutOfBoundsTime_Format);
            lblAvgTimeOutOfBoundsR.DataBindings[0].Format += new ConvertEventHandler(AvgOutOfBoundsTime_Format);

            dir = new DirectoryInfo(fbd.SelectedPath);

            results.AddFromResultsFiles(dir.GetFiles("*.xml", SearchOption.TopDirectoryOnly));

            resultsCollectionBindingSource.DataSource = results;

            UpdateGameBinding();

            _calculateStudyWideStats();
        }

        void Percent_Format(object sender, ConvertEventArgs e)
        {
            e.Value = String.Format("{0:F2} %", (double)e.Value * 100);
        }

        void AvgTimePerPixel_Format(object sender, ConvertEventArgs e)
        {
            e.Value = String.Format("{0:F4}   ms / pixel", (double)e.Value * 1000);
        }

        void AvgXYTimePerPixel_Format(object sender, ConvertEventArgs e)
        {
            e.Value = String.Format("{0:F4}   sec / target", (double)e.Value / 1000);
        }

        void AvgTimePerTarget_Format(object sender, ConvertEventArgs e)
        {
            e.Value = String.Format("{0:F4}   sec / target", e.Value);
        }

        void AvgOutOfBoundsTime_Format(object sender, ConvertEventArgs e)
        {
            e.Value = String.Format("{0:F4}   ms / target", e.Value);
        }

        private void _calculateStudyWideStats()
        {
            double sumDist_A = 0;
            int distCount_A = 0;
            int sumScore_A = 0;
            int scoreCount_A = 0;
            double sumAvgP_A = 0.0d;
            int sumAvgPCount_A = 0;
            double sumAvg_A = 0.0d;
            int sumAvgCount_A = 0;
            List<double> timesA = new List<double>();

            double sumDist_B = 0;
            int distCount_B = 0;
            int sumScore_B = 0;
            int scoreCount_B = 0;
            double sumAvgP_B = 0.0d;
            int sumAvgPCount_B = 0;
            double sumAvg_B = 0.0d;
            int sumAvgCount_B = 0;
            List<double> timesB = new List<double>();

            foreach (Results r in results)
            {
                foreach (Game g in r.Games)
                {
                    if (!g.IsWarmup)
                    {
                        if (r.User.TestGroup == "A")
                        {
                            lvGroupAUsers.Items.Add(r.User.Name + " - " + r.User.Age.ToString());

                            g.AppendTimesPerPixel(ref timesA);

                            sumDist_A += g.AvgInitDistance;
                            distCount_A++;

                            sumScore_A += g.Score;
                            scoreCount_A++;

                            sumAvg_A += g.AvgTimePerTarget;
                            sumAvgCount_A++;

                            sumAvgP_A += g.AvgTimePerPixel;
                            sumAvgPCount_A++;
                        }
                        else
                        {
                            lvGroupBUsers.Items.Add(r.User.Name + " - " + r.User.Age.ToString());

                            g.AppendTimesPerPixel(ref timesB);

                            sumDist_B += g.AvgInitDistance;
                            distCount_B++;

                            sumScore_B += g.Score;
                            scoreCount_B++;

                            sumAvg_B += g.AvgTimePerTarget;
                            sumAvgCount_B++;

                            sumAvgP_B += g.AvgTimePerPixel;
                            sumAvgPCount_B++;
                        }
                    }
                }
            }
            
            lblAvgScoreA.Text = (sumScore_A / scoreCount_A).ToString() + "  points";
            lblAvgInitDistA.Text = (sumDist_A / distCount_A).ToString("F4") + "  pixels";
            lblAvgTimePerTargetA.Text = (sumAvg_A / sumAvgCount_A).ToString("F4") + "   sec / target";
            lblAvgTimePerPixelA.Text = (sumAvgP_A / sumAvgPCount_A * 1000).ToString("F4") + "   ms / pixel";
            lblStdDevA.Text = StdDev(ref timesA).ToString("F4");

            lblAvgScoreB.Text = (sumScore_B / scoreCount_B).ToString() + "  points";
            lblAvgInitDistB.Text = (sumDist_B / distCount_B).ToString("F4") + "  pixels";
            lblAvgTimePerTargetB.Text = (sumAvg_B / sumAvgCount_B).ToString("F4") + "   sec / target";
            lblAvgTimePerPixelB.Text = (sumAvgP_B / sumAvgPCount_B * 1000).ToString("F4") + "   ms / pixel";
            lblStdDevB.Text = StdDev(ref timesB).ToString("F4");
        }

        private double Mean(ref List<double> nums)
        {
            double mu = 0;

            for (int i = 0; i < nums.Count; i++)
            {
                mu += nums[i];
            }

            return mu / nums.Count;
        }

        private double StdDev(ref List<double> nums)
        {
            double mean = Mean(ref nums);
            double sum = 0;

            for (int i = 0; i < nums.Count; i++)
            {
                //Square the difference from the mean
                sum += (nums[i] - mean) * (nums[i] - mean);
            }

            //Std Dev is the sqrt of the sum of the squares of the differences from the mean divided by the number of items
            return Math.Sqrt(sum / nums.Count);
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            UpdateGameBinding();
            UpdateGraph();
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            UpdateGameBinding();
            UpdateGraph();
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            UpdateGameBinding();
            UpdateGraph();
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            UpdateGameBinding();
            UpdateGraph();
        }

        private void UpdateGameBinding()
        {
            gameBindingSource.DataSource = ((Results)resultsCollectionBindingSource.Current).Games;
            gameBindingSource.MoveLast();
        }

        private void btnViewGraphs_Click(object sender, EventArgs e)
        {
            positionsDataGridView.Visible = false;
            positionsDataGridView.Enabled = false;
            pnlGraphs.Visible = true;
        }

        private void btnViewRawData_Click(object sender, EventArgs e)
        {
            positionsDataGridView.Visible = true;
            positionsDataGridView.Enabled = true;
            pnlGraphs.Visible = false;
        }

        private void pnlGraphs_VisibleChanged(object sender, EventArgs e)
        {
            UpdateGraph();
        }

        private void targetsBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            UpdateGraph();
        }

        private void UpdateGraph()
        {
            if (pnlGraphs.Visible)
            {
                Target t = (Target)targetsBindingSource.Current;

                if (t != null)
                {
                    //Reset the graph
                    targetGraph.Series["Series1"].Points.Clear();
                    targetGraph.Series["Series2"].Points.Clear();
                    targetGraph.Series["Target"].Points.Clear();

                    //Set the title
                    targetGraph.Titles[0].Text = nameLabel1.Text
                        + ((isWarmupCheckBox.Checked) ? " - Warmup" : " - User Study")
                        + " - Target #" + (t.ID + 1).ToString() ;
                    
                    //Turn black position markers on or off
                    int size = 0;
                    if (cbShowEachPosition.Checked)
                    {
                        size = 3;
                    }

                    //Turn smooth lines on or off
                    if (!cbSmoothLines.Checked)
                    {
                        targetGraph.Series["Series1"].ChartType = SeriesChartType.Line;
                        targetGraph.Series["Series2"].ChartType = SeriesChartType.Line;
                    }
                    else
                    {
                        targetGraph.Series["Series1"].ChartType = SeriesChartType.Spline;
                        targetGraph.Series["Series2"].ChartType = SeriesChartType.Spline;
                    }

                    //Mark the right controller's positions
                    if (t.RightPositions)
                    {
                        targetGraph.Series["Series1"].IsVisibleInLegend = true;

                        foreach (TargetPosition tp in t.Positions)
                        {
                            targetGraph.Series["Series1"].Points.AddXY(tp.right.X, 480 - tp.right.Y);
                            targetGraph.Series["Series1"].MarkerSize = size;
                        }

                        targetGraph.Series["Series1"].Points[0].MarkerColor = Color.Green;
                        targetGraph.Series["Series1"].Points[0].MarkerSize = 8;
                        targetGraph.Series["Series1"].Points[0].MarkerStyle = MarkerStyle.Circle;

                        targetGraph.Series["Series1"].Points[t.Positions.Count - 1].MarkerColor = Color.Red;
                        targetGraph.Series["Series1"].Points[t.Positions.Count - 1].MarkerSize = 8;
                        targetGraph.Series["Series1"].Points[t.Positions.Count - 1].MarkerStyle = MarkerStyle.Circle;
                    }
                    else
                    {
                        targetGraph.Series["Series1"].IsVisibleInLegend = false;
                    }

                    //Mark the left controller's positions
                    if (t.LeftPositions)
                    {
                        targetGraph.Series["Series2"].IsVisibleInLegend = true;

                        foreach (TargetPosition tp in t.Positions)
                        {
                            targetGraph.Series["Series2"].Points.AddXY(tp.left.X, 480 - tp.left.Y);
                            targetGraph.Series["Series2"].MarkerSize = size;
                        }

                        targetGraph.Series["Series2"].Points[0].MarkerColor = Color.Green;
                        targetGraph.Series["Series2"].Points[0].MarkerSize = 8;
                        targetGraph.Series["Series2"].Points[0].MarkerStyle = MarkerStyle.Circle;

                        targetGraph.Series["Series2"].Points[t.Positions.Count - 1].MarkerColor = Color.Red;
                        targetGraph.Series["Series2"].Points[t.Positions.Count - 1].MarkerSize = 8;
                        targetGraph.Series["Series2"].Points[t.Positions.Count - 1].MarkerStyle = MarkerStyle.Circle;
                    }
                    else
                    {
                        targetGraph.Series["Series2"].IsVisibleInLegend = false;
                    }

                    //Mark the target's box (100 x 80 box surrounding the target's location)
                    targetGraph.Series["Target"].Points.AddXY(t.Location.X - 40, 480 - t.Location.Y - 50);
                    targetGraph.Series["Target"].Points.AddXY(t.Location.X - 40, 480 - t.Location.Y + 50);
                    targetGraph.Series["Target"].Points.AddXY(t.Location.X + 40, 480 - t.Location.Y + 50);
                    targetGraph.Series["Target"].Points.AddXY(t.Location.X + 40, 480 - t.Location.Y - 50);
                    targetGraph.Series["Target"].Points.AddXY(t.Location.X - 40, 480 - t.Location.Y - 50);
                }
            }
        }

        private void btnNextTarget_Click(object sender, EventArgs e)
        {
            targetsBindingSource.MoveNext();
        }

        private void btnPrevTarget_Click(object sender, EventArgs e)
        {
            targetsBindingSource.MovePrevious();
        }

        private void cbShowEachPosition_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGraph();
        }

        private void cbSmoothLines_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGraph();
        }

        private void bindingNavigatorMoveNextItem1_Click(object sender, EventArgs e)
        {
            UpdateGraph();
        }

        private void bindingNavigatorMovePreviousItem1_Click(object sender, EventArgs e)
        {
            UpdateGraph();
        }

        private void bindingNavigatorMoveLastItem1_Click(object sender, EventArgs e)
        {
            UpdateGraph();
        }

        private void bindingNavigatorMoveFirstItem1_Click(object sender, EventArgs e)
        {
            UpdateGraph();
        }

        private void exportGraphImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            f.Description = "Select the folder containing the ViAppleGrabResults";
            f.ShowNewFolderButton = true;
            f.SelectedPath = @"C:\Users\Alex\SkyDrive\Graduate\Results_ViAppleGrab_082112\Images";
            string fname, gameName;
            string[] temp;

            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;

                string dir = f.SelectedPath;

                resultsCollectionBindingSource.MoveFirst();
                UpdateGameBinding();

                for (int i = 0; i < results.Count; i++)
                {
                    gameBindingSource.MoveFirst();

                    Directory.CreateDirectory(dir + @"\" + results[i].User.Name);

                    for (int j = 0; j < results[i].Games.Length; j++)
                    {
                        gameName = results[i].Games[j].IsWarmup ? "Warmup" : "UserStudy";

                        targetsBindingSource.MoveFirst();

                        for (int k = 0; k < results[i].Games[j].Targets.Count; k++)
                        {
                            UpdateGraph();

                            temp = targetGraph.Titles[0].Text.Split(new char[] { '-', ','});

                            fname = temp[0].Trim() + temp[1].Trim() + "_" + gameName + "_" + (k + 1).ToString() + ".png";

                            using (Stream writer = new FileStream(dir + @"\" + results[i].User.Name + @"\" + fname, FileMode.Create))
                            {
                                targetGraph.SaveImage(writer, ChartImageFormat.Png);
                                writer.Close();
                            }

                            targetsBindingSource.MoveNext();
                        }

                        gameBindingSource.MoveNext();
                    }

                    resultsCollectionBindingSource.MoveNext();
                    UpdateGameBinding();
                }

                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(dir);

                    zip.Save(fbd.SelectedPath + @"\Images.zip");
                }

                Cursor = Cursors.Arrow;
            }
        }

        private void exportFittsLawDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            //Initialize the streams
            string dir = @"C:\Users\Alex\SkyDrive\Graduate\Results_ViAppleGrab_082112\ResultsStuff\";
            Stream streamA = new FileStream(dir + @"GroupA_Fitts.csv", FileMode.Create);
            Stream streamB = new FileStream(dir + @"GroupB_Fitts.csv", FileMode.Create);
            TextWriter writerA = new StreamWriter(streamA);
            TextWriter writerB = new StreamWriter(streamB);

            //Write the headers in the csv files
            writerA.WriteLine("User,Target ID,Distance in Pixels,Time");
            writerB.WriteLine("User,Target ID,Distance in Pixels,Time");

            //Write the file data
            foreach (Results r in results)
            {
                Game g = (r.GameCount == 1) ? r.Games[0] : r.Games[1];

                if (g.TypeOfControl == ViAppleGrab.ControlType.Alternating) //Group A
                {
                    foreach (Target t in g.Targets)
                    {
                        writerA.WriteLine(r.User.LastName + "," + t.ID.ToString() + "," + t.InitDistance.ToString() + "," + t.ScanningTime.ToString());
                    }
                }
                else //Group B
                {
                    foreach (Target t in g.Targets)
                    {
                        writerB.WriteLine(r.User.LastName + "," + t.ID.ToString() + "," + t.InitDistance.ToString() + "," + t.ScanningTime.ToString());
                    }
                }
            }

            writerA.Close();
            writerB.Close();
            streamA.Close();
            streamB.Close();

            Cursor = Cursors.Arrow;
        }

    }
}
