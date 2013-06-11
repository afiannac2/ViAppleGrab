using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ViAppleGrab;
using System.Drawing.Printing;
using TargetAnalysis.Properties;

namespace TargetAnalysis
{
    public struct Apple
    {
        public Point point;
        public bool rotten;
    }

    public partial class TargetAnalysis : Form
    {
        static int vertDemarcT = 380 / 5;
        static int horDemarcT = 560 / 8;

        static int vertDemarcA = 380 / 5;
        static int horDemarcA = 560 / 8;

        Bitmap graphT = new Bitmap(640, 480);
        Bitmap graphA = new Bitmap(640, 480);

        List<Apple> pointsT;
        List<Apple> pointsA;

        int rottenT = 0;
        int rottenA = 0;

        bool DisplayOrders = true;
        bool DisplayOrders2 = true;

        public TargetAnalysis()
        {
            InitializeComponent();

            cmbTypeOfTargets.SelectedIndex = 1;
        }

        private void DisplayGraphs()
        {
            rottenA = 0;
            rottenT = 0;

            pointsT = new List<Apple>();
            pointsA = new List<Apple>();

            InitializeGraphs();

            ReadTargetFiles();

            DrawApples(ref graphT, false, ref pointsT);
            DrawApples(ref graphA, true, ref pointsA);

            AddStatsToGraphs();

            pbTogetherGraph.Image = ProportionallyResizeBitmap(graphT, 480, 360);
            pbAlternatingGraph.Image = ProportionallyResizeBitmap(graphA, 480, 360);
        }

        private void InitializeGraphs()
        {
            for (int i = 0; i < 640; i++)
                for (int j = 0; j < 480; j++)
                {
                    if (i == 0 || i == 639 || j == 0 || j == 479)
                    {
                        graphT.SetPixel(i, j, Color.Black);
                        graphA.SetPixel(i, j, Color.Black);
                    }
                    else
                    {
                        graphT.SetPixel(i, j, Color.White);
                        graphA.SetPixel(i, j, Color.White);
                    }
                }
        }

        private void ReadTargetFiles()
        {
            StreamReader readerT = new StreamReader(Properties.Settings.Default.TOGETHER_FILE);
            StreamReader readerA = new StreamReader(Properties.Settings.Default.ALTERNATING_FILE);

            
            //Throw out the first line of each file
            readerT.ReadLine();
            readerA.ReadLine();

            string[] parts;
            bool r;

            string text = readerT.ReadLine();

            while (text != null)
            {
                parts = text.Split(new char[] { ' ' });

                r = Boolean.Parse(parts[2]);

                pointsT.Add(new Apple { point = new Point(Int32.Parse(parts[0]), Int32.Parse(parts[1])), rotten = r });

                if (r)
                    rottenT++;

                text = readerT.ReadLine();
            }

            tbNumTargets.Text = pointsT.Count.ToString();

            text = readerA.ReadLine();

            while (text != null)
            {
                parts = text.Split(new char[] { ' ' });

                r = Boolean.Parse(parts[2]);

                pointsA.Add(new Apple { point = new Point(Int32.Parse(parts[0]), Int32.Parse(parts[1])), rotten = r });

                if (r)
                    rottenA++;

                text = readerA.ReadLine();
            }

            tbNumTargets2.Text = pointsA.Count.ToString();

            readerT.Close();
            readerA.Close();

            if (pointsT.Count == 20)
            {
                vertDemarcT = 380 / 5;
                horDemarcT = 560 / 4;
            }
            else if (pointsT.Count == 32)
            {
                vertDemarcT = 380 / 4;
                horDemarcT = 560 / 8;
            }

            if (pointsA.Count == 20)
            {
                vertDemarcA = 380 / 5;
                horDemarcA = 560 / 4;
            }
            else if (pointsA.Count == 32)
            {
                vertDemarcA = 380 / 4;
                horDemarcA = 560 / 8;
            }

            //Draw the borders and gridlines
            if (cmbTypeOfTargets.SelectedItem.ToString() != "Warmup")
                DrawGrids();

            DrawBorders();
        }

        public void DrawLine(Point start, Point end, Color color, ref Bitmap graph)
        {
            if (end.X < start.X || end.Y < start.Y)
            {
                Point temp = end;
                end = start;
                start = temp;
            }

            for(int i = 0; i <= end.X - start.X; i++)
            {
                for(int j = 0; j <= end.Y - start.Y; j++)
                {
                    graph.SetPixel(start.X + i, start.Y + j, color);
                }
            }
        }

        public void DrawDottedLine(Point start, Point end, Color color, ref Bitmap graph)
        {
            int dottedSwitch = 0;
            Color c1 = color;
            Color c2 = Color.White;
            Color tempC;

            if (end.X < start.X || end.Y < start.Y)
            {
                Point temp = end;
                end = start;
                start = temp;
            }

            for (int i = 0; i <= end.X - start.X; i++)
            {
                for (int j = 0; j <= end.Y - start.Y; j++)
                {
                    //Every four pixels, turn the color on or off
                    if (dottedSwitch % 4 == 0)
                    {
                        tempC = c1;
                        c1 = c2;
                        c2 = tempC;
                    }

                    graph.SetPixel(start.X + i, start.Y + j, c1);
                }
            }
        }

        public void DrawBorders()
        {
            //Alternating specific
            //Left Box
            Point p1 = new Point(40, 50);
            Point p2 = new Point(40, 430);
            DrawLine(p1, p2, Color.Blue, ref graphA);

            p1 = new Point((40 + 600) / 2 - 1, 50);
            p2 = new Point((40 + 600) / 2 - 1, 430);
            DrawLine(p1, p2, Color.Blue, ref graphA);

            p1 = new Point(40, 50);
            p2 = new Point((40 + 600) / 2 - 1, 50);
            DrawLine(p1, p2, Color.Blue, ref graphA);

            p1 = new Point(40, 430);
            p2 = new Point((40 + 600) / 2 - 1, 430);
            DrawLine(p1, p2, Color.Blue, ref graphA);

            //Separator
            p1 = new Point((40 + 600) / 2, 50);
            p2 = new Point((40 + 600) / 2, 430);
            DrawLine(p1, p2, Color.White, ref graphA);

            //Right Box
            p1 = new Point((40 + 600) / 2 + 1, 50);
            p2 = new Point((40 + 600) / 2 + 1, 430);
            DrawLine(p1, p2, Color.Blue, ref graphA);

            p1 = new Point(600, 50);
            p2 = new Point(600, 430);
            DrawLine(p1, p2, Color.Blue, ref graphA);

            p1 = new Point((40 + 600) / 2 + 1, 50);
            p2 = new Point(600, 50);
            DrawLine(p1, p2, Color.Blue, ref graphA);

            p1 = new Point((40 + 600) / 2 + 1, 430);
            p2 = new Point(600, 430);
            DrawLine(p1, p2, Color.Blue, ref graphA);
            
            //Together specific
            p1 = new Point(40, 50);
            p2 = new Point(40, 430);
            DrawLine(p1, p2, Color.Blue, ref graphT);

            p1 = new Point(600, 50);
            p2 = new Point(600, 430);
            DrawLine(p1, p2, Color.Blue, ref graphT);

            p1 = new Point(40, 50);
            p2 = new Point(600, 50);
            DrawLine(p1, p2, Color.Blue, ref graphT);

            p1 = new Point(40, 430);
            p2 = new Point(600, 430);
            DrawLine(p1, p2, Color.Blue, ref graphT);
        }

        private void DrawGrids()
        {
            int h = 380 / vertDemarcT;
            int l = 560 / horDemarcT;
            int h2 = 380 / vertDemarcA;
            int l2 = 560 / horDemarcA;

            tbNumTargetsH.Text = h.ToString();
            tbNumTargetsL.Text = l.ToString();
            if (h == 5)
            {
                if (l == 4)
                    rbT3.Select();
                else
                    rbT1.Select();
            }
            else
                rbT2.Select();

            tbNumTargetsH2.Text = h2.ToString();
            tbNumTargetsL2.Text = l2.ToString();
            if (h2 == 5)
            {
                if (l2 == 4)
                    rbA3.Select();
                else
                    rbA1.Select();
            }
            else
                rbA2.Select();

            Point p1, p2;

            //Together
            //Horizontal
            for (int i = 1; i < h; i++)
            {
                p1 = new Point(40 + 1, 50 + vertDemarcT * i);
                p2 = new Point(600 - 1, 50 + vertDemarcT * i);

                DrawDottedLine(p1, p2, Color.Gray, ref graphT);
            }

            //Vertical
            for (int i = 1; i < l; i++)
            {
                p1 = new Point(40 + horDemarcT * i, 50 + 1);
                p2 = new Point(40 + horDemarcT * i, 430 - 1);

                DrawDottedLine(p1, p2, Color.Gray, ref graphT);
            }

            //Alternating
            //Horizontal
            for (int i = 1; i < h2; i++)
            {
                p1 = new Point(40 + 1, 50 + vertDemarcA * i);
                p2 = new Point(600 - 1, 50 + vertDemarcA * i);

                DrawDottedLine(p1, p2, Color.Gray, ref graphA);
            }

            //Vertical
            for (int i = 1; i < l2; i++)
            {
                p1 = new Point(40 + horDemarcA * i, 50 + 1);
                p2 = new Point(40 + horDemarcA * i, 430 - 1);

                DrawDottedLine(p1, p2, Color.Gray, ref graphA);
            }
        }

        private void DrawApples(ref Bitmap graph, bool IsA, ref List<Apple> points)
        {
            Point p;
            Color c;

            //Graphics stuff...
            Graphics g = Graphics.FromImage(graph); 
            StringFormat strFor = new StringFormat();
            strFor.LineAlignment = StringAlignment.Far;
            strFor.Alignment = StringAlignment.Center;
            RectangleF rec;
            System.Drawing.Font f = new Font("Tahoma", 10);
            string text;

            for (int i = 0; i < points.Count; i++)
            {
                p = points[i].point;

                //Surround the point
                graph.SetPixel(p.X - 2, p.Y, Color.Black);
                graph.SetPixel(p.X - 2, p.Y - 1, Color.Black);
                graph.SetPixel(p.X - 2, p.Y - 2, Color.Black);
                graph.SetPixel(p.X - 2, p.Y + 1, Color.Black);
                graph.SetPixel(p.X - 2, p.Y + 2, Color.Black);

                graph.SetPixel(p.X - 1, p.Y - 2, Color.Black);
                graph.SetPixel(p.X - 1, p.Y + 2, Color.Black);

                graph.SetPixel(p.X, p.Y - 2, Color.Black);
                graph.SetPixel(p.X, p.Y + 2, Color.Black);

                graph.SetPixel(p.X + 1, p.Y - 2, Color.Black);
                graph.SetPixel(p.X + 1, p.Y + 2, Color.Black);

                graph.SetPixel(p.X + 2, p.Y, Color.Black);
                graph.SetPixel(p.X + 2, p.Y - 1, Color.Black);
                graph.SetPixel(p.X + 2, p.Y - 2, Color.Black);
                graph.SetPixel(p.X + 2, p.Y + 1, Color.Black);
                graph.SetPixel(p.X + 2, p.Y + 2, Color.Black);

                //Fill in the point based on rotten or not rotten
                c = (points[i].rotten) ? Color.Red : Color.White;
                graph.SetPixel(p.X - 1, p.Y, c);
                graph.SetPixel(p.X - 1, p.Y - 1, c);
                graph.SetPixel(p.X - 1, p.Y + 1, c);
                graph.SetPixel(p.X + 1, p.Y, c);
                graph.SetPixel(p.X + 1, p.Y - 1, c);
                graph.SetPixel(p.X + 1, p.Y + 1, c);
                graph.SetPixel(p.X, p.Y, c);
                graph.SetPixel(p.X, p.Y - 1, c);
                graph.SetPixel(p.X, p.Y + 1, c);

                text = (i + 1).ToString();
                rec = new RectangleF(p.X - horDemarcT / 2, p.Y, horDemarcT, vertDemarcT / 4);
                g.DrawString(text, f, Brushes.Black, rec, strFor);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Bitmap output = new Bitmap(660, 480 * 2 + 30);
            Graphics g = Graphics.FromImage(output);
            g.FillRectangle(Brushes.LightGray, new Rectangle(0, 0, 660, 480 * 2 + 30));
            g.DrawImage(graphT, new Point(10, 10));
            g.DrawImage(graphA, new Point(10, 500));  

            PrintDocument pd = new PrintDocument();
            pd.OriginAtMargins = true;

            PageSettings ps = new PageSettings();
            ps.Margins.Left = 500;
            ps.Margins.Top = 500;
            ps.Margins.Bottom = 500;
            ps.Margins.Right = 500;

            pd.DefaultPageSettings = ps;
            
            pd.PrintPage += delegate(object o, PrintPageEventArgs ev)
            {
                ev.Graphics.DrawImage(output, new RectangleF(0, 0, ps.PrintableArea.Width, ps.PrintableArea.Height));
                //ev.Graphics.DrawImage(output, new Point(70, 40));
                //ev.Graphics.DrawImage(graphA, new Point(70, 540));
            };

            PrintDialog dialog = new PrintDialog();

            dialog.AllowSomePages = false;
            dialog.ShowHelp = true;
            dialog.Document = pd;

            DialogResult result = dialog.ShowDialog();

            // If the result is OK then print the document.
            if (result == DialogResult.OK)
            {
                pd.Print();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Bitmap output = new Bitmap(660, 480 * 2 + 30);
            Graphics g = Graphics.FromImage(output);
            g.FillRectangle(Brushes.LightGray, new Rectangle(0, 0, 660, 480 * 2 + 30));
            g.DrawImage(graphT, new Point(10, 10));
            g.DrawImage(graphA, new Point(10, 500));       

            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = "JPEG Image(*.JPG)|*.JPG";

            if (cmbTypeOfTargets.SelectedItem.ToString() == "Warmup")
                dialog.FileName = "Warmup.jpg";
            else
                dialog.FileName = "Targets.jpg";

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (Stream writer = new FileStream(dialog.FileName, FileMode.Create))
                {
                    output.Save(writer, System.Drawing.Imaging.ImageFormat.Jpeg);
                    writer.Close();
                }
            }
        }

        public void AddStatsToGraphs()
        {
            //Setup the output settings
            Graphics gT = Graphics.FromImage(graphT);
            Graphics gA = Graphics.FromImage(graphA);

            gT.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            gA.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            StringFormat strFor = new StringFormat();
            strFor.LineAlignment = StringAlignment.Center;
            strFor.Alignment = StringAlignment.Center;

            RectangleF rec = new RectangleF(0, 430, 640, 50);

            System.Drawing.Font f = new Font("Tahoma", 10);

            //Draw the text at the bottom
            string text = "Targets: " + pointsT.Count.ToString() + " | Number Rotten: " + rottenT.ToString() + " | Order of Appearance is Printed Below Target Location";
            gT.DrawString(text, f, Brushes.Black, rec, strFor);
            text = "Targets: " + pointsA.Count.ToString() + " | Number Rotten: " + rottenA.ToString() + " | Order of Appearance is Printed Below Target Location";
            gA.DrawString(text, f, Brushes.Black, rec, strFor);

            //Draw the text at the top
            f = new Font("Tahoma", 20);
            rec = new RectangleF(0, 0, 640, 50);

            gT.DrawString("TOGETHER MODE", f, Brushes.Black, rec, strFor);
            gA.DrawString("ALTERNATING MODE", f, Brushes.Black, rec, strFor);

            //Draw string on the side
            f = new Font("Tahoma", 10);
            rec = new RectangleF(600, 0, 40, 480);
            strFor.FormatFlags = StringFormatFlags.DirectionVertical;

            gT.DrawString("Border Region", f, Brushes.Black, rec, strFor);
            gA.DrawString("Border Region", f, Brushes.Black, rec, strFor);
        }

        public static Bitmap ProportionallyResizeBitmap(Bitmap sourceBitmap, int maxWidth, int maxHeight)
        {
            // original dimensions  
            int width = sourceBitmap.Width;
            int height = sourceBitmap.Height;

            // Find the longest and shortest dimentions  
            int longestDimension = (width > height) ? width : height;
            int shortestDimension = (width < height) ? width : height;

            double factor = ((double)longestDimension) / (double)shortestDimension;

            // Set width as max  
            double newWidth = maxWidth;
            double newHeight = maxWidth / factor;

            //If height is actually greater, then we reset it to use height instead of width  
            if (width < height)
            {
                newWidth = maxHeight / factor;
                newHeight = maxHeight;
            }

            // Create new Bitmap at new dimensions based on original bitmap  
            Bitmap resizedBitmap = new Bitmap((int)newWidth, (int)newHeight);
            using (Graphics g = Graphics.FromImage((System.Drawing.Image)resizedBitmap))
                g.DrawImage(sourceBitmap, 0, 0, (int)newWidth, (int)newHeight);
            return resizedBitmap;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNewTargets_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the current targets and make new ones?",
                "Warning!", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                int numTargets, numTargetsH, numTargetsL;

                if (!cbCenteredTargets.Checked)
                {
                    if (Int32.TryParse(tbNumTargets.Text, out numTargets))
                    {
                        Cursor = Cursors.WaitCursor;

                        GenerateNewTargets(numTargets, false);

                        DisplayGraphs();
                        Cursor = Cursors.Arrow;

                        return;
                    }
                }
                else
                {
                    if (rbT1.Checked)
                    {
                        numTargetsH = 5;
                        numTargetsL = 8;
                    }
                    else if (rbT2.Checked)
                    {
                        numTargetsH = 4;
                        numTargetsL = 8;
                    }
                    else
                    {
                        numTargetsH = 5;
                        numTargetsL = 4;
                    }

                    Cursor = Cursors.WaitCursor;

                    GenerateCenteredTargets(false, numTargetsL, numTargetsH);

                    DisplayGraphs();
                    Cursor = Cursors.Arrow;

                    return;
                }

                MessageBox.Show("Enter an integer value(s) for the number of targets!");
            }
        }

        private void btnNewTargets2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the current targets and make new ones?",
                "Warning!", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                int numTargets, numTargetsH, numTargetsL;

                if (!cbCenteredTargets2.Checked)
                {
                    if (Int32.TryParse(tbNumTargets2.Text, out numTargets))
                    {
                        Cursor = Cursors.WaitCursor;

                        GenerateNewTargets(numTargets, true);

                        DisplayGraphs();
                        Cursor = Cursors.Arrow;

                        return;
                    }
                }
                else
                {
                    if (rbA1.Checked)
                    {
                        numTargetsH = 5;
                        numTargetsL = 8;
                    }
                    else if (rbA2.Checked)
                    {
                        numTargetsH = 4;
                        numTargetsL = 8;
                    }
                    else
                    {
                        numTargetsH = 5;
                        numTargetsL = 4;
                    }

                    Cursor = Cursors.WaitCursor;

                    GenerateCenteredTargets(true, numTargetsL, numTargetsH);

                    DisplayGraphs();
                    Cursor = Cursors.Arrow;

                    return;
                }

                MessageBox.Show("Enter an integer value for the number of targets!");
            }
        }

        public static void GenerateCenteredTargets(bool IsAlternating, int L, int H)
        {
            Random rand = new Random();
            int index, totalTargets;

            Point[] _targetLocations = new Point[40];
            bool[] _rottenState = new bool[40];

            //Set the horizontal demarcation and vertical demarcation
            if (560 % L == 0 && 380 % H == 0 && L % 2 == 0)
            {
                if (!IsAlternating)
                {
                    vertDemarcT = 380 / H;
                    horDemarcT = 560 / L;
                }
                else
                {
                    vertDemarcA = 380 / H;
                    horDemarcA = 560 / L;
                }

                totalTargets = L * H;
            }
            else
            {
                return;
            }

            for (int i = 0; i < L; i++)
            {
                for (int j = 0; j < H; j++)
                {
                    _targetLocations[i * H + j] = new Point(40 + i * horDemarcT + horDemarcT / 2, 50 + j * vertDemarcT + vertDemarcT / 2);

                    //33.3% chance of an apple being rotten
                    _rottenState[i * H + j] = ((int)rand.Next(0, 3) == 2) ? true : false;
                }
            }

            if (IsAlternating)
            {
                Point[] left = new Point[totalTargets / 2];
                bool[] lRot = new bool[totalTargets / 2];
                List<int> lMarked = new List<int>();

                Point[] right = new Point[totalTargets / 2];
                bool[] rRot = new bool[totalTargets / 2];
                List<int> rMarked = new List<int>();

                for (int i = 0; i < totalTargets / 2; i++)
                {
                    left[i] = _targetLocations[i];
                    lRot[i] = _rottenState[i];
                    right[i] = _targetLocations[i + totalTargets / 2];
                    rRot[i] = _rottenState[i + totalTargets / 2];
                }

                _targetLocations = new Point[totalTargets];
                _rottenState = new bool[totalTargets];

                for (int i = 0; i < totalTargets; i++)
                {
                    index = rand.Next(totalTargets / 2);

                    if (i % 2 == 0) //Right hand first
                    {
                        while (rMarked.Contains(index)) //Go to the nearest index not yet used
                        {
                            index = (index + 1) % (totalTargets / 2);
                        }

                        rMarked.Add(index);

                        _targetLocations[i] = right[index];
                        _rottenState[i] = rRot[index];
                    }
                    else //Left hand second
                    {
                        while (lMarked.Contains(index))
                        {
                            index = (index + 1) % (totalTargets / 2);
                        }

                        lMarked.Add(index);

                        _targetLocations[i] = left[index];
                        _rottenState[i] = lRot[index];
                    }
                }

                using (TextWriter writer = new StreamWriter(Properties.Settings.Default.ALTERNATING_FILE))
                {
                    writer.WriteLine("Alternating");

                    for (int i = 0; i < totalTargets; i++)
                        writer.WriteLine(_targetLocations[i].X.ToString() + " "
                            + _targetLocations[i].Y.ToString() + " "
                            + _rottenState[i].ToString());

                    writer.Close();
                }
            }
            else
            {
                Point[] temp = new Point[totalTargets];
                bool[] rot = new bool[totalTargets];
                List<int> marked = new List<int>();

                for (int i = 0; i < totalTargets; i++)
                {
                    index = rand.Next(totalTargets);

                    while (marked.Contains(index)) //Go to the nearest index not yet used
                    {
                        index = (index + 1) % totalTargets;
                    }

                    marked.Add(index);

                    temp[i] = _targetLocations[index];
                    rot[i] = _rottenState[index];
                }

                using (TextWriter writer = new StreamWriter(Properties.Settings.Default.TOGETHER_FILE))
                {
                    writer.WriteLine("Together");

                    for (int i = 0; i < totalTargets; i++)
                        writer.WriteLine(temp[i].X.ToString() + " "
                            + temp[i].Y.ToString() + " "
                            + rot[i].ToString());

                    writer.Close();
                }
            }
        }

        public static void GenerateNewTargets(int _totalTargets, bool IsAlternating)
        {
            Random rand = new Random();
            int mid = 640 / 2;
            int minX = 40;
            int minY = 50;
            int maxX = 640 - minX;
            int maxY = 480 - minY;

            Point[] _targetLocations = new Point[_totalTargets];
            bool[] _rottenState = new bool[_totalTargets];

            if (IsAlternating)
            {
                for (int i = 0; i < _totalTargets; i++)
                {
                    if (i % 2 == 0) //Start with the right hand by default
                    {
                        _targetLocations[i] = new Point((int)rand.Next(mid, maxX), (int)rand.Next(minY, maxY));
                    }
                    else //then the left hand
                    {
                        _targetLocations[i] = new Point((int)rand.Next(minX, mid), (int)rand.Next(minY, maxY));
                    }

                    //33.3% chance of an apple being rotten
                    _rottenState[i] = ((int)rand.Next(0, 3) == 2) ? true : false;
                }

                using (TextWriter writer = new StreamWriter(Properties.Settings.Default.ALTERNATING_FILE))
                {
                    writer.WriteLine("Alternating");

                    for (int i = 0; i < _totalTargets; i++)
                        writer.WriteLine(_targetLocations[i].X.ToString() + " "
                            + _targetLocations[i].Y.ToString() + " "
                            + _rottenState[i].ToString());

                    writer.Close();
                }
            }
            else
            {
                for (int i = 0; i < _totalTargets; i++)
                {
                    _targetLocations[i] = new Point((int)rand.Next(minX, maxX), (int)rand.Next(minY, maxY));

                    //33.3% chance of an apple being rotten
                    _rottenState[i] = ((int)rand.Next(0, 3) == 2) ? true : false;
                }
                using (TextWriter writer = new StreamWriter(Properties.Settings.Default.TOGETHER_FILE))
                {
                    writer.WriteLine("Together");

                    for (int i = 0; i < _totalTargets; i++)
                        writer.WriteLine(_targetLocations[i].X.ToString() + " "
                            + _targetLocations[i].Y.ToString() + " "
                            + _rottenState[i].ToString());

                    writer.Close();
                }
            }
        }

        private void cbCenteredTargets_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCenteredTargets.Checked)
            {
                gbCenteredDimensions.Visible = true;

                tbNumTargets.Enabled = false;
                tbNumTargets.Visible = false;

                tbNumTargetsH.Visible = true;
                tbNumTargetsL.Visible = true;

                lblNumTargetsCentered.Visible = true;
            }
            else
            {
                gbCenteredDimensions.Visible = false;

                tbNumTargets.Enabled = true;
                tbNumTargets.Visible = true;

                tbNumTargetsH.Visible = false;
                tbNumTargetsL.Visible = false;

                lblNumTargetsCentered.Visible = false;
            }
        }

        private void cbCenteredTargets2_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCenteredTargets2.Checked)
            {
                gbCenteredDimensions2.Visible = true;

                tbNumTargets2.Enabled = false;
                tbNumTargets2.Visible = false;

                tbNumTargetsH2.Visible = true;
                tbNumTargetsL2.Visible = true;

                lblNumTargetsCentered2.Visible = true;
            }
            else
            {
                gbCenteredDimensions2.Visible = false;

                tbNumTargets2.Enabled = true;
                tbNumTargets2.Visible = true;

                tbNumTargetsH2.Visible = false;
                tbNumTargetsL2.Visible = false;

                lblNumTargetsCentered2.Visible = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            string ext = "../../../ViAppleGrab/ViAppleGrab/bin/x86/Debug/";

            if (cmbTypeOfTargets.SelectedItem.ToString() == "Warmup")
            {
                Settings.Default.ALTERNATING_FILE = ext + "Warmup_Alternating.txt";
                Settings.Default.TOGETHER_FILE = ext + "Warmup_Together.txt";

                cbCenteredTargets.Checked = false;
                cbCenteredTargets2.Checked = false;
            }
            else
            {
                Settings.Default.ALTERNATING_FILE = ext + "Targets_Alternating.txt";
                Settings.Default.TOGETHER_FILE = ext + "Targets_Together.txt";

                cbCenteredTargets.Checked = true;
                cbCenteredTargets2.Checked = true;
            }

            DisplayGraphs();
            Cursor = Cursors.Arrow;
        }

        private void cbDisplayWithOrders_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDisplayWithOrders.Checked)
            {
                if (!DisplayOrders)
                {
                    Cursor = Cursors.WaitCursor;
                    DisplayOrders = true;
                    DisplayGraphs();
                    Cursor = Cursors.Arrow;
                }
            }
            else
            {
                if (DisplayOrders)
                {
                    Cursor = Cursors.WaitCursor;
                    DisplayOrders = false;
                    DisplayGraphs();
                    Cursor = Cursors.Arrow;
                }
            }
        }

        private void cbDisplayWithOrders2_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDisplayWithOrders2.Checked)
            {
                if (!DisplayOrders2)
                {
                    Cursor = Cursors.WaitCursor;
                    DisplayOrders2 = true;
                    DisplayGraphs();
                    Cursor = Cursors.Arrow;
                }
            }
            else
            {
                if (DisplayOrders2)
                {
                    Cursor = Cursors.WaitCursor;
                    DisplayOrders2 = false;
                    DisplayGraphs();
                    Cursor = Cursors.Arrow;
                }
            }
        }
    }
}
