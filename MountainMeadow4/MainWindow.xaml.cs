using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MountainMeadow4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml - the Login window
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// login window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            //first  check for existing instances of this program
            if (AppSemaphore.PrevInstance)
            {
                if (MessageBox.Show("Do  you really want to start an additional instance " +
                    "of Mountain Meadow? " +
                    "\r\n(It is allowed, although program installation updates will not work " +
                    "if multiple instances are running.)",
                    "Mountain Meadow is already running on this machine",
                    MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    //then terminate
                    this.Close(); //close login window, which should be the only window open
                    return;
                }
            }
        }

        /// <summary>
        /// click login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            ProgramWindow main = new();
            main.Show();
            this.Close();
        }
    }
}