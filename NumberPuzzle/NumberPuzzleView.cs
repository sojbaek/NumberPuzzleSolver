using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NumberPuzzle
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:NumberPuzzle"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:NumberPuzzle;assembly=NumberPuzzle"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    ///         
    /// 

    public class LineSegmentWithCircles : Shape
    {
        // DependencyProperty - Progress
        private static FrameworkPropertyMetadata progressMetadata =
                new FrameworkPropertyMetadata(
                    0.0,     // Default value
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    null,    // Property changed callback
                    new CoerceValueCallback(CoerceProgress));   // Coerce value callback

        // DependencyProperty - Radius
        private static FrameworkPropertyMetadata radiusMetadata =
                new FrameworkPropertyMetadata(
                    90.0,     // Default value
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    null,    // Property changed callback
                    null);   // Coerce value callback


        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register("Progress", typeof(Double), typeof(LineSegmentWithCircles), progressMetadata);
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register("Radius", typeof(Double), typeof(LineSegmentWithCircles), radiusMetadata);
        public static readonly DependencyProperty FromProperty = DependencyProperty.Register("From", typeof(Point), typeof(LineSegmentWithCircles));
        public static readonly DependencyProperty ToProperty = DependencyProperty.Register("To", typeof(Point), typeof(LineSegmentWithCircles));

        private static object CoerceProgress(DependencyObject depObj, object baseVal)
        {
            double prog = (double)baseVal;
            prog = (prog < 0.0) ? 0.0 : ((prog > 1.0) ? 1.0 : prog);
            return prog;
        }

        public double Progress
        {
            get { return (double)this.GetValue(ProgressProperty); }
            set { this.SetValue(ProgressProperty, value);
                //Console.WriteLine("!Progress={0}", value);
            }
        }

        public double Radius
        {
            get { return (double)this.GetValue(RadiusProperty); }
            set { this.SetValue(RadiusProperty, value);
                //Console.WriteLine("!Radius={0}", value);
            }
        }
        public Point From
        {
            get { return (Point)this.GetValue(FromProperty); }
            set { this.SetValue(FromProperty, value); }
        }
        public Point To
        {
            get { return (Point)this.GetValue(ToProperty); }
            set { this.SetValue(ToProperty, value); }
        }
       
        public LineSegmentWithCircles()
        {
        }
       
        
        protected override Geometry DefiningGeometry
        {
            get
            {
                double theta;
                Point dest;
                
                if (Progress != 1.0)
                    dest = Vector.Add(Vector.Multiply(Progress, Point.Subtract(To, From)), From);
                else
                    dest = To;

                double x1 = From.X;
                double x2 = dest.X;
                double y1 = From.Y;
                double y2 = dest.Y;
                double r = Radius;
                                
                if (x2==x1)
                {
                    theta = (y2 >= y1) ? Math.PI / 2.0 : -Math.PI/2.0;
                } else
                {
                    theta = Math.Atan((y2 - y1) / (x2 - x1));
                }
                if (x2 < x1)
                    theta += Math.PI;

                Point p1 = new Point(x1 + r * Math.Cos(theta + Math.PI / 2.0),
                   y1 + r * Math.Sin(theta + Math.PI / 2.0));

                Point p2 = new Point(x1 + r * Math.Cos(theta + 1.5*Math.PI),
                   y1 + r * Math.Sin(theta +1.5* Math.PI));

                Point p4 = new Point(x2 + r * Math.Cos(theta + 0.5* Math.PI),
                y2 + r * Math.Sin(theta + 0.5 * Math.PI));

                Point p3 = new Point(x2 + r * Math.Cos(theta - 0.5 * Math.PI),
                y2 + r * Math.Sin(theta - 0.5 * Math.PI));


                List<PathSegment> segments = new List<PathSegment>();

                double inc = Math.PI / 10.0;

                segments.Add(new LineSegment(p1, true));
                for (double dt = inc; dt < Math.PI; dt += inc)
                {
                    segments.Add(new LineSegment(new Point(x1 + r * Math.Cos(theta + Math.PI / 2.0 + dt),
            y1 + r * Math.Sin(theta + Math.PI / 2.0 + dt)), true));
                }
                segments.Add(new LineSegment(p2, true));
                segments.Add(new LineSegment(p3, true));
                for (double dt = inc; dt < Math.PI; dt += inc)
                {
                    segments.Add(new LineSegment(new Point(x2 + r * Math.Cos(theta - Math.PI / 2.0 + dt),
            y2 + r * Math.Sin(theta - Math.PI / 2.0 + dt)), true));
                }
                segments.Add(new LineSegment(p4, true));
                
                                
                List<PathFigure> figures = new List<PathFigure>(1);
                PathFigure pf = new PathFigure(p1, segments, true);
                pf.IsFilled = true;
                figures.Add(pf);
                Geometry g = new PathGeometry(figures, FillRule.EvenOdd, null);
                return g;
            }
        }

    }


    public class NumberPuzzleView : Canvas
    {
        private delegate void AnimationDelegate();

        static NumberPuzzleView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumberPuzzleView), new FrameworkPropertyMetadata(typeof(NumberPuzzleView)));
        }
        
        
        public NumberPuzzleView()
        {
            //mySolidColorBrush = new SolidColorBrush();
            // mySolidColorBrush.Color = Color.FromArgb(255, 255, 255, 0);
            rands = new Random();
        }


        #region Declarations
        //private List<Points> _points;
        //private Spirograph _graph;
        TextBlock[,] numberGrid;
        //SolidColorBrush[] colorBrushes;
        public double fsize = 0.0;
        public double hmargin = 50.0;
        public double vmargin = 20.0;
        
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the softness of the graph's line.
        /// </summary>
        ///// <value>The drawing line softness.</value>
        //public double Softness { get; set; }
        ///// <summary>
        ///// Gets or sets the pen.
        ///// </summary>

        private NumberMaze doc;
        public NumberMaze Doc
        {
            get { return doc; }
            set { doc = value;
                draw();
                ///   Register this to doc as a notifier
            }
        }

        private bool withAnimation;
        public bool WithAnimation
        {
            get { return withAnimation; }
            set
            {
                withAnimation = value;
            }
        }



        private Timer timer;
        private Random rands;
        private int tick;
        private int step;
        private int nexttick;

        public double Velocity = 100;  // 1000 millisec
        
        #endregion


        #region Methods

        private void startAnimation()
        {
            tick = 0; step = 0; nexttick = 1;
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }

            timer = new System.Timers.Timer();
            timer.Interval = Velocity;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Elapsed);
            timer.Start();
            
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {   
                if (step >= doc.Goal)
                {
                    timer.Stop();
                } else
                {
                    if (tick >= nexttick)
                    {
                        step++;
                        AnimationDelegate animationMethod = new AnimationDelegate(this.AnimateSolutionThreaded);
                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, animationMethod);
                        nexttick += measureBlockLength(step);                        
                    }
                    tick++;
                }
            }
            catch (TargetInvocationException) { }
        }

        private void AnimateSolutionThreaded()
        {
            if (doc != null)
            {
                if (doc.solved)
                {
           //         Console.WriteLine("tick={0}, step={1}, nexttick={2}", tick, step, nexttick);
                    InsertAnimatedRanges(step);                    
                }
            } else
            {
                timer.Stop();
            }            
        }

        private void InsertAnimatedRanges(int step)
        {
            // Create a line segment from the to and from points
            if (step > 0 && step <= doc.Goal)
            {
                LineSegmentWithCircles lsc = CreateNumberGroups(step);
                Storyboard sb = new Storyboard();

                this.Children.Add(lsc);

                DoubleAnimation myAnimation = new DoubleAnimation();
                int numblocks = measureBlockLength(step);

                Duration duration = new Duration(TimeSpan.FromMilliseconds(Velocity * numblocks));
                myAnimation.Duration = duration;

                Storyboard myStoryboard = new Storyboard();

                myStoryboard.Children.Add(myAnimation);
                myStoryboard.Duration = duration;
                Storyboard.SetTarget(myAnimation, lsc);
                myAnimation.From = 0.0;

                if (numblocks <= 1)
                {
                    myAnimation.To = fsize / 2.0;
                    Storyboard.SetTargetProperty(myAnimation, new PropertyPath("(Radius)"));
                } else
                {
                    myAnimation.To = 1.0;
                    Storyboard.SetTargetProperty(myAnimation, new PropertyPath("(Progress)"));
                }
                myStoryboard.Begin();


            }
        }

        private int measureBlockLength(int step)
        {
            int ic1 = doc.solution[step][0] / doc.cmax;
            int ir1 = doc.solution[step][0] % doc.cmax;
            int ic2 = doc.solution[step][1] / doc.cmax;
            int ir2 = doc.solution[step][1] % doc.cmax;
            int numblocks = Math.Max(Math.Abs(ic1 - ic2), Math.Abs(ir1 - ir2)) + 1;
            return numblocks;
        }

        private LineSegmentWithCircles ER(Point p1, Point p2)
        {
            LineSegmentWithCircles myER = new LineSegmentWithCircles();

            myER.Radius = 10.0;
            myER.From = p1;
            myER.To = p2;
            myER.Stroke = Brushes.Black;
            myER.StrokeThickness = 2;
            return myER;

        }

        private void drawNumbers()
        {
            if (doc == null) return;
            
            if (fsize == 0)
                return;
            int nrow = doc.Maze.GetLength(0);
            int ncol = doc.Maze.GetLength(1);

            SolidColorBrush grayBrush = new SolidColorBrush(Colors.Gray);
            SolidColorBrush blackBrush = new SolidColorBrush(Colors.Black);

            Size ts = new Size(fsize, fsize);

            if (fsize >0)
            {
                Label startArrowLabel = new Label();
                startArrowLabel.Content = "↑Start";
                startArrowLabel.FontSize = 25;
                Canvas.SetTop(startArrowLabel, doc.StartRow * fsize + vmargin);
                Canvas.SetLeft(startArrowLabel,(doc.StartCol-1)* fsize + hmargin);
                this.Children.Add(startArrowLabel);

                numberGrid = new TextBlock[nrow, ncol];
                for (int ii = 0; ii < nrow; ii++)
                {
                    for (int jj = 0; jj < ncol; jj++)
                    {
                        TextBlock number = new TextBlock();
                        number.RenderSize = ts;
                        number.FontSize = fsize;
                        if (doc.solidx != null) {
                            if (doc.solidx[ii, jj] == 0) {
                                number.Foreground = grayBrush; 
                            } else
                            {
                                number.Foreground = blackBrush;
                                number.ToolTip = doc.solidx[ii, jj].ToString();
                            }
                                
                        }                          
                        double fs1 = number.FontSize;
                        double fs2 = number.FontSize * 10.0;
                        number.Text = doc.Maze[ii, jj].ToString();
                        Canvas.SetTop(number, ii * fsize + vmargin);
                        Canvas.SetLeft(number, jj * fsize + hmargin);
                        this.Children.Add(number);

                        numberGrid[ii, jj] = number;
                    }
                }
            }
            
        }

        
        public void drawStaticSolution()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
                
            if (doc.solved && fsize > 0.0 && withAnimation==false)
            {
                             
                for (int ii = 1; ii <= doc.Goal; ii++)
                {
                    LineSegmentWithCircles mer1 = CreateNumberGroups(ii);
                    this.Children.Add(mer1);
                }
            }
            drawNumbers();
        }

        private LineSegmentWithCircles CreateNumberGroups(int ii)
        {
            LineSegmentWithCircles lsc;

            int ir1, ir2, ic1, ic2;
            double x1, x2, y1, y2;

            SolidColorBrush colorBrushes = new SolidColorBrush(Color.FromRgb((byte)rands.Next(1, 255), (byte)rands.Next(1, 255), (byte)rands.Next(1, 255))); ;
            ic1 = doc.solution[ii][0] / doc.cmax;
            ir1 = doc.solution[ii][0] % doc.cmax;
            ic2 = doc.solution[ii][1] / doc.cmax;
            ir2 = doc.solution[ii][1] % doc.cmax;
            x1 = ic1 * fsize + hmargin + fsize * .25;
            y1 = ir1 * fsize + vmargin + fsize * .75;
            x2 = ic2 * fsize + hmargin + fsize * .25;
            y2 = ir2 * fsize + vmargin + fsize * .75;
            
            lsc = ER(new Point(x1, y1), new Point(x2, y2));
            lsc.Radius = fsize / 2.0;
            lsc.Fill = colorBrushes;
            lsc.Progress = 1.0;
            lsc.Opacity = 0.5;
            lsc.ToolTip = String.Format("{0}", ii);
            return lsc;
        }

        public void draw()
        {
            this.Children.Clear();


            if (withAnimation)
            {
                drawNumbers();
                startAnimation();
            } else
            {
                drawStaticSolution();
            }            
        }
        #endregion

        public void sizeChanged()
        {
            Console.WriteLine("numberMazeCanvas_SizeChanged..");
            if (doc == null) return;

            int nrow = doc.Maze.GetLength(0);
            int ncol = doc.Maze.GetLength(1);
            if (ncol != 0)
            {
                double fsize1 = (this.ActualWidth - hmargin * 2.0) / ncol;
                double fsize2 = (this.ActualHeight - vmargin * 2.0) / nrow;
                fsize = (fsize1>fsize2) ? fsize2: fsize1;
            }
            draw();
        }
        
    }
}
