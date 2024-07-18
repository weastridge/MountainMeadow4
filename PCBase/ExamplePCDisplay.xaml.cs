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
using MM4Common;

namespace PCBase
{
    /// <summary>
    /// Interaction logic for BaseDisplay.xaml
    /// </summary>
    public partial class BasePCDisplay : UserControl
    {
        /// <summary>
        /// reference to the Program Component that hosts this
        /// </summary>
        public IProgramComponent PCRef { get; set; }

        public BasePCDisplay(IProgramComponent pcRef)
        {
            InitializeComponent();
            PCRef = pcRef;
        }

        private void ButtonLoadUsers_Click(object sender, RoutedEventArgs e)
        {
            UserBrief[]? users = PCRef.MM4Data!.SelectUserBriefs(false, //only if active
                    UserPermissions.None, //min conditions
                    false); //only if not guest
            StringBuilder sb = new();
            foreach (var user in users)
            {
                sb.Append(user.DisplayName);
                sb.Append(Environment.NewLine);
            }
            TextBoxUsers.Text = sb.ToString();
                //"Wesley \r\n Joy \r\n alpha \r\n bravo \r\n charley \r\n delta \r\n echo \r\n foxtrot \r\n golf \r\n hotel \r\n indigo \r\n juliet \r\n kilo \r\n lima\r\n mike";
        }
    }
}
