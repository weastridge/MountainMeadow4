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
using System.Windows.Shapes;

namespace MountainMeadow4
{
    /// <summary>
    /// Interaction logic for ProgramWindow.xaml
    /// </summary>
    public partial class ProgramWindow : Window
    {
        /// <summary>
        /// the main program window
        /// </summary>
        public ProgramWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// close whole program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MnuAbout_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Primary Display area is ");
            sb.Append(System.Windows.SystemParameters.PrimaryScreenWidth);
            sb.Append(" x ");
            sb.Append(System.Windows.SystemParameters.PrimaryScreenHeight);
            sb.Append("\r\nwork area size is ");
            sb.Append(System.Windows.SystemParameters.WorkArea.Size.ToString());
            sb.Append("\r\n task bar ht is ");
            sb.Append((System.Windows.SystemParameters.PrimaryScreenHeight - System.Windows.SystemParameters.WorkArea.Height).ToString());
            sb.Append("\r\nMain window size is ");
            sb.Append(this.ActualWidth.ToString());
            sb.Append(", ");
            sb.Append(this.ActualHeight.ToString());
            sb.Append("\r\n");
            sb.Append("BaseDisplay size is ");
            sb.Append(gridDisplay.ActualWidth.ToString());
            sb.Append(", ");
            sb.Append(gridDisplay.ActualHeight.ToString());
            sb.Append("\r\n");
            System.Windows.MessageBox.Show(sb.ToString());
        }
    }
}
