
namespace MM4Common
{
    /// <summary>
    /// component to plug into the MM4 program
    /// </summary>
    public interface IProgramComponent
    {
        /// <summary>
        /// hosts a panel to show in MM4 Display area if any. (nullable)
        /// </summary>
        public System.Windows.Controls.UserControl? Display { get; set; }
        /// <summary>
        /// the database methods
        /// </summary>
        public IMM4Data? MM4Data { get; set; }
    }

}
