
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

        /// <summary>
        /// the piece's reference to the state object
        /// </summary>
        /// <returns></returns>
        IMM4State? MM4State { get; set; }

        /// <summary>
        /// current user using program piece
        /// </summary>
        UserBrief? CurrentUser { get; set; }

        /// <summary>
        /// current patient program piece pertains to
        /// </summary>
        PatientExpanded? CurrentPatient { get; set; }

        /// <summary>
        /// called each time a 
        /// piece is selected by the user 
        /// (before showing the child form if any)
        /// and typically decides
        /// whether or not to reload data
        /// </summary>
        /// <param name="parameters">optional strings to pass to piece</param>
        void OnCallBeforeShow(string[] parameters);

        /// <summary>
        /// called immediately after showing child
        /// form (if any) and typically assigns
        /// focus to a control on the page.
        /// </summary>
        void OnCallWhenShown();

        /// <summary>
        /// called on the piece whose user control is showing
        /// before changing to another program piece's user control
        /// (display), cancellable
        /// </summary>
        /// <param name="parameters">optional array of strings to pass to new piece</param>
        /// <returns></returns>
        bool OkToChangePages(ref string[] parameters);

        /// <summary>
        /// called before changing patients,
        /// cancellable
        /// </summary>
        /// <param name="currentPieceShowingInDisplay">allows piece to know if it
        /// is the one showing when this method is called</param>
        /// <returns></returns>
        bool OkToChangePatients(IProgramPiece currentPieceShowingInDisplay);

        /// <summary>
        /// called when user tries to logout or
        /// close program, cancellable
        /// </summary>
        /// <param name="currentPieceShowingInDisplay">allows piece to know what piece is
        /// showing when this method is called (and particularly if it is itself)</param>
        /// <returns></returns>
        bool OkToLogout(IProgramPiece currentPieceShowingInDisplay);

        /// <summary>
        /// Warning: no dialog boxes or errors allowed.
        /// Called before automatic timeout logouts or
        /// before system shutdown.  Not cancelable, user
        /// is not in attendance and errors could prevent
        /// normal shutdown.
        /// </summary>
        /// <param name="currentPieceShowingInDisplay">allows piece to know if it
        /// is the piece showing when this method is called</param>
        void ForcedLogout(IProgramPiece currentPieceShowingInDisplay);

        /// <summary>
        /// called after Program Component is created,  this 
        /// should assign ChildControlToShow if any any exists, and
        /// can assign persistent menu items if desired
        /// </summary>
        void Initialize();

        /// <summary>
        /// requests reloading the component from scratch as when
        /// user wants to refresh the control's data
        /// </summary>
        void Reload();
    }

}
