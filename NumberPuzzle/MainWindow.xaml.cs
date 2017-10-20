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
using NumberPuzzle;

namespace NumberPuzzle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //tbOutput.Text = "Call me Ishmael. Some years ago- never mind how long precisely- having " +
            //    "little or no money in my purse, and nothing particular to interest me on shore, I thought " +
            //    "I would sail about a little and see the watery part of the world. It is a way I have of driving off the spleen and regulating the circulation. " +
            //    "Whenever I find myself growing grim  the mouth; whenever it is a damp, drizzly November in my soul; whenever I find myself involuntarily pausing" +
            //    "before coffin warehouses, and bringing up aboutthe rear of every funeral I meet; and especially whenever my hypos get such an upper hand of me, that it " +
            //    "requires a strong moral principle to prevent me from deliberately stepping into the street, and methodically knocking people's hats off- then, " +
            //    "I account it high time to get to sea as soon as I can. This is my substitute for pistol and ball. With a philosophical flourish Cato throws himself " +
            //    "upon his sword; I quietly take to the ship. There is nothing surprising in this. If they but knew it, almost all men in their degree, some time or other, " +
            //    "cherish very nearly the same feelings towards the ocean with me.\n" + "There now is your insular city of the Manhattoes, belted round by wharves as Indian " +
            //    "isles by coral reefs-commerce surrounds it with her surf.Right and left, the streets take you waterward.Its extreme downtown is the battery, where that noble mole" +
            //    " is washed by waves, and cooled by breezes, which a few hours previous were out of sight of land. Look at the crowds of water-gazers there.";

            NumberMaze np = new NumberMaze();
            numberMazeCanvas.Doc = np;
           

        }

        private void numberMazeCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Console.WriteLine("numberMazeCanvas_SizeChanged..");
            numberMazeCanvas.sizeChanged();
        }

        private void drawButton_Click(object sender, RoutedEventArgs e)
        {
            NumberMaze np = numberMazeCanvas.Doc;
            np.Solve();
            numberMazeCanvas.Velocity = velocitySlider.Value;
            if (np.solved)
            {
                //StringBuilder stringBuilder = new StringBuilder();
                //for (int ii = 1; ii < np.Goal; ii++)
                //{
                //    StringBuilder s = new StringBuilder();
                //    foreach (var nn in np.solution[ii])
                //    {
                //        int irow = nn / np.cmax;
                //        int icol = nn % np.cmax;
                //        s.Append(String.Format("({0},{1}) ", irow, icol));
                //    }
                //    stringBuilder.AppendLine(s.ToString());
                //}
                //tbOutput.Text = stringBuilder.ToString();

                numberMazeCanvas.draw();
            }
            else
            {
                tbOutput.Text = "Solution not found.";
            }
        }

        private void animateCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            numberMazeCanvas.WithAnimation = true;
        }

        private void animateCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            numberMazeCanvas.WithAnimation = false;
        }
    }
}
