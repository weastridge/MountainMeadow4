using MM4Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MM4Common
{
    /// <summary>
    /// interface for the main class that keeps state
    /// information including current user and patient
    /// </summary>
    public interface IMM4State
    {
        /// <summary>
        /// copyright and license info
        /// </summary>
        string CopyrightInfo { get; }

        /// <summary>
        /// true indicates the user specified a server name rather than using
        /// the primary server designated in the local config file
        /// </summary>
        bool UsingCustomServerName { get; set; }

        /// <summary>
        /// get or set the current user
        /// </summary>
        MM4Common.UserExpanded CurrentUser { get; set; }

        //todo:  put in configs

        ///////////// <summary>
        ///////////// user config settings for current user from database
        ///////////// </summary>
        //////////IConfigForUser ConfigForUser { get; }

        ///////////// <summary>
        ///////////// config settings for local client application from config file
        ///////////// </summary>
        //////////IConfigForClient ConfigForClient { get; }

        ///////////// <summary>
        ///////////// global configuration for program from database
        ///////////// </summary>
        //////////IConfigForGlobal ConfigForGlobal { get; }

        /// <summary>
        /// get or set the patient chart currently open
        /// </summary>
        MM4Common.PatientExpanded CurrentPatient { get; set; }

        /// <summary>
        /// reload information about the current patient
        /// </summary>
        void RefreshCurrentPatient();

        /// <summary>
        /// get a list of patients who were recently current
        /// </summary>
        List<PatientExpanded> PriorPatients { get; }

        /////////// <summary>
        /////////// Merge a new MenuStrip item into top level or dropdown menu items
        /////////// unless it already exists, and associate it with the ProgramPieceInfo
        /////////// if any given (optional)
        /////////// </summary>
        /////////// <param name="captions">path of menu item captions
        /////////// e.g. File, New, Folder, etc.</param>
        /////////// <param name="menuStrip"></param>
        /////////// <param name="onClick"></param>
        /////////// <param name="shortcutKey">shortcut key or 
        /////////// Keys.None if none.  Acceptible keys are
        /////////// Keys.Control|Keys.F for find and
        /////////// Keys.Control|Keys.P for print</param>
        /////////// <returns>reference to the new item or existing item if not 
        /////////// replaced</returns>
        ////////System.Windows.Forms.ToolStripMenuItem MergeItemsIntoMenuStrip(
        ////////    string[] captions,
        ////////    EventHandler onClick,
        ////////    System.Windows.Forms.Keys shortcutKey);

        /// <summary>
        /// the name of this program, e.g. Mountain Meadow 3
        /// </summary>
        /// <returns></returns>
        string ProgramName();

        /// <summary>
        /// used to keep track of hibernation
        /// </summary>
        Microsoft.Win32.PowerModes CurrentPowerMode { get; set; }

        ///////////// <summary>
        ///////////// remove indicated item
        ///////////// </summary>
        ///////////// <param name="item"></param>
        //////////void RemoveToolStripMenuItem(
        //////////    System.Windows.Forms.ToolStripItem item);

        /// <summary>
        /// calls program piece of given name if found or else return false
        /// </summary>
        /// <param name="programPieceClassName">Namespace.ClassName</param>
        /// <returns></returns>
        bool CallProgramPiece(string programPieceClassName);

        /// <summary>
        /// calls program piece of given name if found or else return false
        /// </summary>
        /// <param name="programPieceClassName">Namespace.ClassName</param>
        /// <param name="parameters">optional array of params to send new piece</param>
        /// <returns></returns>
        bool CallProgramPiece(string programPieceClassName, string[] parameters);

        /// <summary>
        /// call program piece and return true if success
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="parameters">optional array of parameters or null if none</param>
        /// <returns></returns>
        bool CallProgramPiece(ProgramPieceInfo pieceInfo, string[] parameters);

        /// <summary>
        /// list of program pieces that are loaded, both from intrinsic
        /// pieces and external assemblies (Systems.Collections.Generic.List)
        /// </summary>
        List<ProgramPieceInfo> ProgramPieceInfos { get; }

        /// <summary>
        /// info on the program piece that is currently showing in the display
        /// </summary>
        ProgramPieceInfo CurrentProgramPieceShowing
        {
            get;
        }

        /// <summary>
        /// call Reload() on program piece currently showing its
        /// display
        /// </summary>
        void ReloadCurrentlyShowingProgramPiece();

        /// <summary>
        /// append parameter string to array of parameters, as for calling program pieces
        /// for example
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="newParameter"></param>
        void AppendParameter(
            ref string[] parameters, string newParameter);

        ///////////// <summary>
        ///////////// Send message to one or all of program pieces and MM3.MainClass
        ///////////// </summary>
        ///////////// <param name="sender">object calling this method, 
        ///////////// which might or might not be same as originator of message</param>
        ///////////// <param name="message"></param>
        //////////void SendMessage(object sender, MM4Message message);

        ///////////// <summary>
        ///////////// array of sections to offer user for creating reports
        ///////////// </summary>
        //////////MM4Common.MM3Printing.MM3ReportSection[] ReportSections { get; set; }

        ///////////// <summary>
        ///////////// add given section to the array of available sections, trying to 
        ///////////// honor its index position relative to other chunks
        ///////////// </summary>
        ///////////// <param name="section"></param>
        ///////////// <param name="labelOfPiece">helpful to track source of blank sections errors</param>
        //////////void InsertReportSection(MM3Common.MM3Printing.MM3ReportSection section,
        //////////    string labelOfPiece);

        ///////////// <summary>
        ///////////// add given sections to array of available sections,
        ///////////// in the order given by OrderIndex
        ///////////// </summary>
        ///////////// <param name="sections"></param>
        ///////////// <param name="labelOfPiece">helpful to track source of blank sections errors</param>
        //////////void InsertReportSections(MM3Common.MM3Printing.MM3ReportSection[] sections,
        //////////    string labelOfPiece);

        /// <summary>
        /// reset the MainClass timer that counts time until automatic logout
        /// occurs back to zero (start counting over again)
        /// </summary>
        void ResetTimer();

        /// <summary>
        /// sends logout request and logs out if ok,
        /// (calls MainForm.LogoutMenuItem_Click())
        /// </summary>
        void LogoutRequest();

        /// <summary>
        /// makes name of mm3 keys file known to program pieces, although
        /// the dll file itself won't give up the keys unless the program
        /// piece is signed either by same author as keys file or else by
        /// EastRidges 
        /// </summary>
        string MM3KeysFileName { get; }

        /// <summary>
        /// return true if full social security number is 
        /// used in patient demographics
        /// </summary>
        /// <returns></returns>
        bool UsingFullSSN();

        /// <summary>
        /// current size of the Main Form
        /// </summary>
        System.Drawing.Size CurrentMainFormSize { get; }

        /// <summary>
        /// category of current size of Main Form (not used nor set!)
        /// </summary>
        MainFormSizeCat CurrentMainFormSizeCat { get; set; }
        

        ///////////// <summary>
        ///////////// Current default font for program,  set by MainForm, 
        ///////////// normally use size 9.75.  (vs 8.25, 9, 9.75, 11.25, 12, 14.25)
        ///////////// </summary>
        //////////System.Drawing.Font CurrentDefaultFont { get; set; }

        /// <summary>
        /// current size of the display area for program pieces
        /// within the main form
        /// </summary>
        System.Drawing.Size CurrentMainFormDisplayAreaSize { get; }

        /// <summary>
        /// list of versions of Adobe Acrobat pdf reader that
        /// are installed on this machine, e.g. '11.0'
        /// </summary>
        List<string> InstalledAdobeAcrobatVersions { get; }
    }
}
