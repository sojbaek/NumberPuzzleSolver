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
