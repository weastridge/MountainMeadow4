using System;
using System.Collections.Generic;
using System.Text;
using MM4Common;
using System.Windows.Controls;

namespace MM4Common
{

    /// <summary>
    /// interface for a piece to plug in to program
    /// </summary>
    public interface IProgramPiece
    {
        /// <summary>
        /// the piece's reference to connection-tier object
        /// </summary>
        /// <returns></returns>
        IMM4Data MM4Data { get; set; }
        /// <summary>
        /// the piece's reference to the state object
        /// </summary>
        /// <returns></returns>
        IMM4State MM3State { get; set; }
        /// <summary>
        /// current user using program piece
        /// </summary>
        UserBrief CurrentUser { get; set; }
        /// <summary>
        /// current patient program piece pertains to
        /// </summary>
        PatientExpanded CurrentPatient { get; set; }

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
        void OnCallAfterShow();
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
        /// called after MainForm is created,  this 
        /// should assign ChildControlToShow if any any exists, and
        /// can assign menu items
        /// </summary>
        void Initialize();
        /// <summary>
        /// requests reloading the piece from scratch as when
        /// user wants to refresh the control's data
        /// </summary>
        void Reload();


        // SuggestionsForConnectingPiece is not longer used.
        // we now use the PluginSuggestions custom attribute
        // added above the class definition for that.
        #region SuggestionsForConnectingPiece

        /*
        /// <summary>
        /// To be shown to administrator when assigning pieces
        /// </summary>
        /// <returns></returns>
        string Description { get; set; }


        /// <summary>
        /// Set of user permissions, all of which the current user must
        /// have in order to access this piece
        /// </summary>
        UserPermissions PermissionsRequiredSuggested { get; set; }

        /// <summary>
        /// label to show on navigation bar or menu
        /// </summary>
        /// <returns></returns>
        string LabelSuggested { get; set; }
        /// <summary>
        /// Indicates if label is to be included in navigation bar items
        /// </summary>
        bool ShowInNavBarSuggested { get; set; }
        /// <summary>
        /// color desired to show label in the navigation bar
        /// </summary>
        /// <returns></returns>
        Color LabelColorSuggested { get; set; }
        /// <summary>
        /// the letter desired to call piece when combined with Ctrl key
        /// if is in Nav Bar
        /// </summary>
        /// <returns></returns>
        char ShortcutLetterSuggested { get; set; }
        */
        #endregion  SuggestionsForConnectingPiece
    }


    /// <summary>
    /// supports reference to a windows control 
    /// which a program piece may show within the main
    /// form of the program
    /// </summary>
    public interface IPPDisplay
    {
        /// <summary>
        /// reference to the control to be displayed in the 
        /// program area of the main form, typically a user control
        /// </summary>
        /// <returns></returns>
        Control Display { get; set; }
    }

    ///////////// <summary>
    ///////////// supports a program piece generating reports to be included in
    ///////////// the main program's Reports section
    ///////////// </summary>
    //////////public interface IPPReports
    //////////{
    //////////    /// <summary>
    //////////    /// An array of objects that generate a data report
    //////////    /// </summary>
    //////////    MM4Common.MM3Printing.MM3ReportSection[] ReportSections { get; }
    //////////}


    ///////////// <summary>
    ///////////// supports a program piece handling a MM3Message sent from the Piecer
    ///////////// </summary>
    //////////public interface IPPMessageHandler
    //////////{
    //////////    /// <summary>
    //////////    /// receives and processes message from other pieces
    //////////    /// </summary>
    //////////    /// <param name="sender">what object called this method; 
    //////////    /// not necessarily the originator of the message</param>
    //////////    /// <param name="message"></param>
    //////////    void ReceiveMessage(object sender, MM3Message message);
    //////////}
}
