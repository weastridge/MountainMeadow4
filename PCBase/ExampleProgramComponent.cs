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
    public class ExampleProgramComponent:IProgramComponent

    {
        #region fields
        /// <summary>
        /// the display which hosts a panel, 
        /// or null if none
        /// </summary>
        public UserControl? Display { get; set; }

        /// <summary>
        /// the database methods
        /// </summary>
        public IMM4Data? MM4Data { get; set; }

        /// <summary>
        /// the state of the main program
        /// </summary>
        public IMM4State? MM4State { get; set; }

        /// <summary>
        /// the user this component is currently set for
        /// </summary>
        public UserBrief? CurrentUser { get; set; }

        /// <summary>
        /// the patient this component is currently set for
        /// </summary>
        public PatientExpanded? CurrentPatient { get; set; }
        #endregion fields


        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="mm4Data"></param>
        public ExampleProgramComponent(IMM4Data mm4Data) 
        {
            Display = new BasePCDisplay(this);
            MM4Data = mm4Data;
        }
        #endregion constructor

        public void OnCallBeforeShow(string[] parameters)
        {
            throw new NotImplementedException();
        }

        public void OnCallWhenShown()
        {
            throw new NotImplementedException();
        }

        public bool OkToChangePages(ref string[] parameters)
        {
            throw new NotImplementedException();
        }

        public bool OkToChangePatients(IProgramPiece currentPieceShowingInDisplay)
        {
            throw new NotImplementedException();
        }

        public bool OkToLogout(IProgramPiece currentPieceShowingInDisplay)
        {
            throw new NotImplementedException();
        }

        public void ForcedLogout(IProgramPiece currentPieceShowingInDisplay)
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void Reload()
        {
            throw new NotImplementedException();
        }
    }

}
