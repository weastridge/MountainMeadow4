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

namespace PCBase
{
    /// <summary>
    /// Interaction logic for BaseDisplay.xaml
    /// </summary>
    public partial class BasePCDisplay : UserControl
    {
        public BasePCDisplay()
        {
            InitializeComponent();
        }

        private void ButtonLoadUsers_Click(object sender, RoutedEventArgs e)
        {
            TextBoxUsers.Text = "Wesley \r\n Joy \r\n alpha \r\n bravo \r\n charley \r\n delta \r\n echo \r\n foxtrot \r\n golf \r\n hotel \r\n indigo \r\n juliet \r\n kilo \r\n lima\r\n mike";
        }
    }
}
