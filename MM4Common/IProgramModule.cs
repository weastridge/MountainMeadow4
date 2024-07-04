
namespace MM4Common
{
    /// <summary>
    /// component to plug into the MM4 program
    /// </summary>
    public interface IProgramModule
    {
        /// <summary>
        /// the panel to show in MM4 Display area if any.
        /// </summary>
        public System.Windows.Controls.Panel DisplayPanel { get; set; }
    }

}
