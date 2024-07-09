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
    /// base template for a MM4 program component
    /// </summary>
    public class BaseProgramComponent:IProgramComponent

    {
        /// <summary>
        /// the display which hosts a panel, (nullable)
        /// </summary>
        public UserControl? Display { get; set; }

        public BaseProgramComponent() 
        {
            Display = new BasePCDisplay();
        }
    }

}
