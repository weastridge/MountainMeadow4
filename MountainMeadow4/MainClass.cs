using MM4Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountainMeadow4
{
    /// <summary>
    /// main class for MountainMeadow4
    /// </summary>
    internal static class MainClass
    {
        /// <summary>
        /// the database methods
        /// </summary>
        internal static IMM4Data? MM4DataClass;

        /// <summary>
        /// constructor
        /// </summary>
        static MainClass() 
        { 
            MM4DataClass = new MM4DataSql.DataClass();
        }

    }
}
