using System;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;
using MM4Common;

namespace MM4Common
{
    //***************************************************************
    // This file contains various classes, structs used by
    //  the program and by plugin pieces
    // 
    //***************************************************************

    #region structs
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //Section one: Structs
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    /// <summary>
    /// for logging all accesses to patient charts
    /// records user who accessed, patient being accessed, and the time
    /// </summary>
    /// <remarks>
    /// constructor with default values of int.MinValue for patient 
    /// and user and DateTime.MinValue for TimeIn and TimeOut
    /// </remarks>
    public struct LogOfChartAccess(int userID, DateTime dateTimeIn, DateTime dateTimeOut, int patientID)
    {
        /// <summary>
        /// id of user doing the accessing
        /// </summary>
        public int UserID = userID;
        /// <summary>
        /// time when chart access begins
        /// </summary>
        public DateTime DateTimeIn = dateTimeIn;
        /// <summary>
        /// time when chart access ends or datetime.minvalue for null
        /// </summary>
        public DateTime DateTimeOut = dateTimeOut;
        /// <summary>
        /// ID of patient being accessed
        /// </summary>
        public int PatientID = patientID;
    }




    #endregion structs

    #region classes

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //Section two: Classes
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    /// <summary>
    /// Main Class for MM3Common useful tasks
    /// </summary>
    public static class MainCommonClass
    {
        /// <summary>
        /// get description of an enum value if it was defined with the 
        /// attribute [Description("My Description of attribute")],
        /// otherwise just get the name value of the enum; 
        /// for Description attribute 
        /// from http://weblogs.asp.net/stefansedich/archive/2008/03/12/enum-with-string-values-in-c.aspx
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static String GetEnumDescription(Enum e)
        {
            if (e == null)
            {
                return string.Empty;
            }
            FieldInfo? fieldInfo = e.GetType().GetField(e.ToString());
            if (fieldInfo == null)
            {
                return e.ToString();
            }
            DescriptionAttribute[] enumAttributes =
                (DescriptionAttribute[])fieldInfo.GetCustomAttributes(
                typeof(DescriptionAttribute), false);
            if (enumAttributes.Length > 0)
            {
                return enumAttributes[0].Description;
            }
            else
            {
                return e.ToString();
            }
        }
    }


    /// <summary>
    /// provides a string representation of gender type
    /// </summary>
    [Serializable]
    public static class Gender
    {
        /// <summary>
        /// returns F,M,O or ?
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string LetterRepresentation(GenderType type)
        {
            if (type == GenderType.Female)
                return "F";
            else if (type == GenderType.Male)
                return "M";
            else if (type == GenderType.Other)
                return "O";
            else
                return "?";
        }
        /// <summary>
        /// returns Male, Female, Other or Unknown
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string WordRepresentation(GenderType type)
        {
            if (type == GenderType.Female)
                return "Female";
            else if (type == GenderType.Male)
                return "Male";
            else if (type == GenderType.Other)
                return "Other";
            else
                return "Unknown";
        }

        /// <summary>
        /// returns male or female if recognized,
        /// or else Unknown
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static GenderType Get(string text)
        {
            GenderType result = GenderType.Unknown;
            switch (text.ToLower())
            {
                case "f":
                    result = GenderType.Female;
                    break;
                case "female":
                    result = GenderType.Female;
                    break;
                case "m":
                    result = GenderType.Male;
                    break;
                case "male":
                    result = GenderType.Male;
                    break;
            }
            return result;
        }

    }
    /// <summary>
    /// the information stored in database specifying the piece
    /// to be loaded by client program
    /// </summary>
    [Serializable]
    public class ProgramPieceInfo
    {
        /// <summary>
        /// id in database
        /// </summary>
        public int ProgramPieceInfoID = int.MinValue;
        /// <summary>
        /// permissions, any one of which a user must have in order to 
        /// access this program piece
        /// </summary>
        public UserPermissions PermissionsRequired = UserPermissions.None;
        /// <summary>
        /// the long form of the strong name of assembly
        /// containing the piece, or MainProgram if intrinsic
        /// to the main assembly, MM3.dll
        /// </summary>
        public string AssemblyRef = "";
        /// <summary>
        /// Namespace.ClassName of the program piece
        /// (which must inherit from IProgramPiece)
        /// </summary>
        public string PieceClassName = "";
        /// <summary>
        /// label to show in nav bar or
        /// empty string if not to show
        /// </summary>
        public string Label = "";
        /// <summary>
        /// indicates if label should be shown in the navigation bar
        /// </summary>
        public bool ShowInNavBar = false;
        /// <summary>
        /// color to show in navigation bar
        /// </summary>
        public System.Drawing.Color LabelColor = System.Drawing.Color.Black;
        /// <summary>
        /// letter to invoke piece when combined with Ctrl,
        /// or Char.MinValue for no shortcut
        /// </summary>
        public char ShortcutLetter = char.MinValue;
        /// <summary>
        /// index to order pieces in navigation bar
        /// </summary>
        public int OrderInList = int.MinValue;
        /// <summary>
        /// reference to the piece object itself
        /// </summary>
        public IProgramPiece? Piece = null;
        /// <summary>
        /// enforce loading only module of same version as administrator designated.
        /// If false, still must be same 
        /// </summary>
        public bool EnforceVersion = true;
        /// <summary>
        /// this might not be stored in database but can be set when
        /// loading a new piece
        /// </summary>
        public string Description = string.Empty;

        /// <summary>
        /// gets the assembly qualified type name of the class (for use with Type.CreateInstance)
        /// </summary>
        public string AssemblyQualifiedClassType
        {
            get { return System.Reflection.Assembly.CreateQualifiedName(AssemblyRef, PieceClassName); }
        }
    }

    /*
    /// <summary>
    /// class representing a report on certain data.  Programmer
    /// should set Label and GetReport() before it is called
    /// </summary>
    public class ProgramPieceReporter
    {
        string label = "";
        /// <summary>
        /// short title of report to show in listboxes, etc.
        /// </summary>
        string Label
        {
            get { return label; }
            set { label = value; }
        }

        /// <summary>
        /// generate object to generate reports with given label to 
        /// identify it, e.g. 'Medications'
        /// </summary>
        /// <param name="label"></param>
        public ProgramPieceReporter(string label)
        {
            this.label = label;
        }

        /// <summary>
        /// returns a report such as
        ///  for patient chart printout or data export functions
        /// </summary>
        /// <param name="startDate">beginning date of report</param>
        /// <param name="endDate">ending date of report</param>
        /// <param name="privacyStatus">privacy restrictions</param>
        /// <param name="includeXOuts">true to include records that have been x'd out if applies,
        /// ignored if not applicable</param>
        /// <param name="onlyIfActive">true to omit records marked inactive if applies,
        /// ignored if not applicable</param>
        /// <param name="parameters">formatted like myparam="myvalue" and separated by newlines</param>
        /// <param name="format">returns the type of formatting the report is in</param>
        /// <returns></returns>
        public delegate string GetReportTextDelegate(
            DateTime startDate,
            DateTime endDate,
            PrivacyFlags privacyStatus,
            bool includeXOuts,
            bool onlyIfActive,
            string parameters,
            out TextFormat format);

        /// <summary>
        /// get report
        /// </summary>
        public GetReportTextDelegate GetReportText;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate">beginning date of report</param>
        /// <param name="endDate">ending date of report</param>
        /// <param name="privacyStatus">privacy restrictions</param>
        /// <param name="includeXOuts">true to include records that have been x'd out if applies,
        /// ignored if not applicable</param>
        /// <param name="onlyIfActive">true to omit records marked inactive if applies,
        /// ignored if not applicable</param>
        /// <param name="parameters">formatted like myparam="myvalue" and separated by newlines</param>
        /// <param name="grfx">the graphics object to draw on</param>
        /// <param name="rectangleToDrawIn">the area on the graphics object to draw on</param>
        /// <param name="startAtBeginning">if true start drawing the beginning of report; otherwise
        /// start where last call left off.</param>
        /// <returns></returns>
        public delegate bool DrawReportDelegate(
            DateTime startDate,
            DateTime endDate,
            PrivacyFlags privacyStatus,
            bool includeXOuts,
            bool onlyIfActive,
            string parameters,
            System.Drawing.Graphics grfx,
            System.Drawing.Rectangle rectangleToDrawIn,
            bool startAtBeginning);

        /// <summary>
        /// draw report in given rectangle, and return true if has 
        /// more data to draw. 
        /// </summary>
        public DrawReportDelegate DrawReport;

        /// <summary>
        /// label of report
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return label;
        }
    }
    */

    /// <summary>
    /// for logging miscellaneous events using integers
    /// that have meanings specific to each programming company
    /// </summary>
    /// <remarks>
    /// constructor with default values of int.MinValue for ID's 
    /// and DateTime.MinValue for DateAndTime
    /// </remarks>
    /// <param name="companyUID"></param>
    public struct LogOfEvent(int companyUID)
    {
        /// <summary>
        /// 1 is reserved for EastRidges (MM3)
        /// </summary>
        public int CompanyUID = companyUID;// = int.MinValue;
        /// <summary>
        /// main category of event (e.g. backups)
        /// </summary>
        public int ID1 = int.MinValue;//1 = int.MinValue;
        /// <summary>
        /// first subcategory (e.g. DifferentialBackup)
        /// </summary>
        public int ID2 = int.MinValue;// = int.MinValue;
        /// <summary>
        /// second subcategory, typically used for result,
        /// (e.g. 0=success, 1=failed)
        /// </summary>
        public int ID3 = int.MinValue;// = int.MinValue;
        /// <summary>
        /// third subcategory, could be details of a failure
        /// </summary>
        public int ID4 = int.MinValue;// = int.MinValue;
        /// <summary>
        /// fourth subcategory, could be a link ID
        /// </summary>
        public int ID5 = int.MinValue;// = int.MinValue;
        /// <summary>
        /// date and time of the event
        /// </summary>
        public DateTime DateAndTime = DateTime.MinValue;// = DateTime.MinValue;
    }
    /// <summary>
    /// Specifies what kind of user is logging in
    /// </summary>
    public enum LoginTypeOfUser : int
    {
        /// <summary>
        /// not specified
        /// </summary>
        Unspecified = 0,
        /// <summary>
        /// regular user
        /// </summary>
        RegularUser = 1,
        /// <summary>
        /// guest user (can't login themselves)
        /// </summary>
        GuestUser = 2,
        /// <summary>
        /// web user
        /// </summary>
        WebUser = 3
    }
    /// <summary>
    /// entry of log of user logging in to system
    /// </summary>
    public class LogOfLogin
    {
        /// <summary>
        /// id in database
        /// </summary>
        public int LogOfLoginsID = int.MinValue;
        /// <summary>
        /// user who logged in or int.minvalue if none specified
        /// </summary>
        public int UserID = int.MinValue;
        /// <summary>
        /// web user who logged in or int.minvalue if not specified
        /// </summary>
        public int WebUserID = int.MinValue;
        /// <summary>
        /// not used right now
        /// </summary>
        public LoginTypeOfUser TypeOfUser = LoginTypeOfUser.Unspecified;
        /// <summary>
        /// name of user logging
        /// </summary>
        public string User = string.Empty;
        /// <summary>
        /// when user logged in
        /// </summary>
        public DateTime DateTimeIn = DateTime.MinValue;
        /// <summary>
        /// when user logged out
        /// </summary>
        public DateTime DateTimeOut = DateTime.MinValue;
        /// <summary>
        /// true if login attempt was successful
        /// </summary>
        public bool LoginSuccessful = false;
        /// <summary>
        /// name of machine user logged in from
        /// </summary>
        public string ClientMachineName = string.Empty;
        /// <summary>
        /// ip address user logged in from, or zeros if not specified,
        /// as reported by client machine 
        /// </summary>
        public byte[] ClientIP = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
        /// <summary>
        /// ip address user logged in from as queried from database
        /// </summary>
        public byte[] ClientIPPerDB = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
        /// <summary>
        /// if applicable, the UserID of user who logged in and passed their
        /// connection to this guest (appliicable when User is a guest user)
        /// </summary>
        public int GuestOf = int.MinValue;
    }

    /// <summary>
    /// logs changes to tables that aren't evident elsewhere;  For instance
    /// notes don't need to be included because they are x-d out but never 
    /// altered, but changes to the Users table could be logged here.
    /// </summary>
    [Serializable]
    public class LogOfTableUpdatesEntry
    {
        /// <summary>
        /// the identity column in the changes log
        /// </summary>
        public int LogOfTableUpdatesID = int.MinValue;
        /// <summary>
        /// the name of the table that was changed,
        /// max length 500 chars
        /// </summary>
        public string TableName = "";
        /// <summary>
        /// date the change was made
        /// </summary>
        public DateTime DateOfChange = DateTime.MinValue;
        /// <summary>
        /// userID of user who altered the table
        /// </summary>
        public int WhoMadeChangeID = int.MinValue;
        /// <summary>
        /// descripton of the change, including the old data,
        /// max length 4000 chars, e.g. name of object serialized if any
        /// </summary>
        public string Details = "";
        /// <summary>
        /// optional binary data, as in serialization of object
        /// max length 8000 bytes
        /// </summary>
        public byte[]? BinaryData = null;
        /// <summary>
        /// the identity column value of the row that was changed
        /// </summary>
        public int OldTableRowID = int.MinValue;
        /// <summary>
        /// derived from WhoMadeChangeID
        /// </summary>
        public string WhoMadeChange = string.Empty;

        /// <summary>
        /// empty entry
        /// </summary>
        public LogOfTableUpdatesEntry()
        {
            //need to populate
        }
        /// <summary>
        /// make log of table update
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dateOfChange"></param>
        /// <param name="whoMadeChangeID"></param>
        /// <param name="details"></param>
        /// <param name="binaryData"></param>
        /// <param name="oldTableRowID"></param>
        public LogOfTableUpdatesEntry(
            string tableName,
            DateTime dateOfChange,
            int whoMadeChangeID,
            string details,
            byte[] binaryData,
            int oldTableRowID)
        {
            //validate and assign values
            if (tableName.Length > 500)
            {
                throw new Exception("Table name exceeds length acceptible for logging this change.");
            }
            else if (details.Length > 3000)
            {
                throw new Exception(
                    "Details exceed acceptible length for logging this change.");
            }
            else
            {
                TableName = tableName;
                DateOfChange = dateOfChange;
                WhoMadeChangeID = whoMadeChangeID;
                Details = details;
                BinaryData = binaryData;
                OldTableRowID = oldTableRowID;
            }
        }
    }


    /// <summary>
    /// Information about a person's name
    /// </summary>
    [Serializable]
    public class PersonName : ICloneable
    {
        /// <summary>
        /// id in database
        /// </summary>
        public int PersonNameID = int.MinValue;
        /// <summary>
        /// link to the PatientID, UserID, ExternalActorID
        /// </summary>
        public int LinkID = int.MinValue;
        /// <summary>
        /// wve interprets this as Mr. or Ms., etc.
        /// Max 10 char, or empty if none
        /// </summary>
        public string Title = "";
        /// <summary>
        /// e.g. First Name in American names
        /// max 50 char
        /// </summary>
        public string GivenName = "";
        /// <summary>
        /// middle names, or given names
        /// other than the main GivenName, 
        /// which are separated by spaces
        /// if multiple.
        /// </summary>
        public string MiddleNames = "";
        /// <summary>
        /// e.g. Last Name in American names
        /// </summary>
        public string FamilyName = "";
        /// <summary>
        /// such as Jr, III
        /// max 10 char
        /// </summary>
        public string Suffix = "";
        /// <summary>
        /// nickname or preferred given name
        /// or empty string if none
        /// </summary>
        public string NickName = "";
        /// <summary>
        /// e.g. current name, birth name, or alias
        /// </summary>
        public PersonNameType Type;
        /// <summary>
        /// id to x-out table if crossed out, or
        /// int.MinValue if not
        /// </summary>
        public int XOutID = int.MinValue;
        /// <summary>
        /// when this name was entered into database
        /// or DateTime.MinValue if undefined
        /// </summary>
        public DateTime WhenEntered = DateTime.MinValue;
        /// <summary>
        /// id of who entered this into database
        /// or int.MinValue if undefined
        /// </summary>
        public int WhoEnteredID = int.MinValue;
        /// <summary>
        /// derived from Users
        /// </summary>
        public string WhoEntered = string.Empty;
        /// <summary>
        /// returns title, given name, middle, family name and sufffix
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string CreateDisplayName(PersonName name)
        {
            StringBuilder sb = new ();
            _ = sb.Append(name.Title.Trim());
            if (name.Title.Length > 0)
                _ = sb.Append(' ');
            _ = sb.Append(name.GivenName.Trim());
            if (name.GivenName.Length > 0)
                _ = sb.Append(' ');
            _ = sb.Append(name.MiddleNames.Trim());
            if (name.MiddleNames.Length > 0)
                _ = sb.Append(' ');
            _ = sb.Append(name.FamilyName.Trim());
            if (name.Suffix.Length > 0)
            {
                _ = sb.Append(", ");
                _ = sb.Append(name.Suffix.Trim());
            }
            return sb.ToString();
        }

        /// <summary>
        /// returns true if has at least a first and last name
        /// and no commas
        /// </summary>
        /// <param name="allowInitials">VIIS won't allow single initials</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool Validate(bool allowInitials, out string errMsg)
        {
            int minLength;
            if (allowInitials)
                minLength = 1;
            else
                minLength = 2;
            bool result = true; //unless problem found

            StringBuilder sb = new ();
            //first name
            if ((GivenName == null) || (GivenName.Trim() == string.Empty))
            {
                result = false;
                _ = sb.Append("Missing first name.\r\n");
            }
            else if (GivenName.Length < minLength)
            {
                result = false;
                _ = sb.Append("First name is too short.\r\n");
            }
            //family name
            if ((FamilyName == null) || (FamilyName.Trim() == string.Empty))
            {
                result = false;
                _ = sb.Append("Missing family name.\r\n");
            }
            else if (FamilyName.Length < minLength)
            {
                result = false;
                _ = sb.Append("Family name is too short.\r\n");
            }
            //commas
            if ((GivenName != null) && (GivenName.Contains(',')))
            {
                result = false;
                _ = sb.Append("Given name contains a comma.\r\n");
            }
            if ((FamilyName != null) && (FamilyName.Contains(',')))
            {
                result = false;
                _ = sb.Append("Family name contains a comma.\r\n");
            }
            errMsg = sb.ToString();
            return result;
        }

        /// <summary>
        /// returns the display name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return CreateDisplayName(this);
        }



        #region ICloneable Members

        /// <summary>
        /// returns a clone of the Person name
        /// which is deep since only consists of strings
        /// and numbers
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion ICloneable Members
    }



    /// <summary>
    /// information about a person, to be included
    /// in Patient and User etc
    /// </summary>
    [Serializable]
    public abstract class Person
    {
        //Note:  full implementions of Person should include the 
        // PersonName objects  that are stored in Names table

        /// <summary>
        /// generally matches CurrentName, max 50 char
        /// </summary>
        public string DisplayName = "";
        /// <summary>
        /// for letters, e.g. Dr. Wesley
        /// </summary>
        public string Salutation = "";
        /// <summary>
        /// birthdate or DateTime.MinValue if not applicable or not known
        /// </summary>
        public DateTime DateOfBirth = DateTime.MinValue;
        /// <summary>
        /// date of death or DateTime.MinValue of not specified or not applicable
        /// </summary>
        public DateTime DateOfDeath = DateTime.MinValue;
        /// <summary>
        /// birth sex (can specify how displayed using Gender object)
        /// </summary>
        public GenderType Gender = GenderType.Unknown;
        /// <summary>
        /// DEPRECIATED:  race major category (depreciated because
        /// we are required to register multiple races now)
        /// </summary>
        public RaceEnumDepreciated RaceDepreciated = RaceEnumDepreciated.Unspecified;
        /// <summary>
        /// one or more Race Hierichical Codes preceded with 
        /// vertical bar character
        /// </summary>
        public string RacesCodified = string.Empty;
        /// <summary>
        /// one or more Ethnicity Hierarchical codes preceded 
        /// with a vertical bar character
        /// </summary>
        public string EthnicitiesCodified = string.Empty;
        /// <summary>
        /// ethnicity major category, e.g. hispanic or not,
        /// depreciated in favor of EthnicitiiesCodified now
        /// to allow for multiple ethnicities
        /// </summary>
        public EthnicityEnumDepreciated EthnicityDepreciated = EthnicityEnumDepreciated.Unspecified;
        /// <summary>
        /// preferred language
        /// </summary>
        public CodifiedLanguage Language = CodifiedLanguage.Unspecified;
        /// <summary>
        /// sexual orientation
        /// </summary>
        public CodifiedSexualOrientation? SexualOrientation = CodifiedSexualOrientation.
            Get(CodifiedSexualOrientation.Selections.Unspecified);
        /// <summary>
        /// gender identtity
        /// </summary>
        public CodifiedGenderIdentity GenderIdentity = CodifiedGenderIdentity.Unspecified;
    }

    /// <summary>
    /// anyone who uses this program,
    /// excludes PersonNames and password field
    /// for quick access from db
    /// </summary>
    [Serializable]
    public class UserBrief : Person
    {

        /// <summary>
        /// identity - note zero and negatives reserved
        /// </summary>
        public int UserID = int.MinValue;
        /// <summary>
        /// max 10 char (use DisplayName for long name)
        /// </summary>
        public string ShortName = "";
        /// <summary>
        /// 50 char max
        /// </summary>
        public string Login = "";
        /// <summary>
        /// Encrypted password, ONLY used for saving new passwords, and NOT 
        /// normally retrieved from database; 500 char max
        /// </summary>
        public string? Password = null;
        /// <summary>
        /// up to 32 bit flags of user's authorizations
        /// cast in database as int32
        /// </summary>
        public UserPermissions Permissions = 0;
        /// <summary>
        /// data to be excluded from user's view
        /// cast in database as 32 bit int
        /// </summary>
        public PrivacyFlags PrivacyExclusions = 0;
        /// <summary>
        /// date user last updated their password
        /// </summary>
        public DateTime DateLastUpdatedPwd =
            DateTime.MinValue;
        /// <summary>
        /// Comments regarding the user, max 500 char
        /// </summary>
        public string Comment = "";

        /// <summary>
        /// personal National Provider Identifier, if any
        /// or int.MinValue if none
        /// </summary>
        public int NPI = int.MinValue;

        /// <summary>
        /// true for user acccounts that are just guest accounts for
        /// patients or quality assessment workers and temporary viewers
        /// and false for employees
        /// </summary>
        public bool IsGuest = false;

        private System.Collections.Specialized.StringDictionary? _otherIDs = null;
        /// <summary>
        /// collection of other ID's such as NPI, etc
        /// </summary>
        public System.Collections.Specialized.StringDictionary OtherIDs
        {
            get
            {
                _otherIDs ??= [];  //if is null make a new empty array
                return _otherIDs;
            }
            set { _otherIDs = value; }
        }
        /// <summary>
        /// returns other ID's serialized as XML
        /// e.g. (ID)NPI="123" XID="abc"(/ID)
        /// </summary>
        /// <returns></returns>
        public string GetOtherIDsAsXML()
        {
            XmlDocument xDoc = new ();
            _ = xDoc.AppendChild(xDoc.CreateElement("ID"));
            if ((_otherIDs != null) && (xDoc.DocumentElement != null))
            {
                foreach (string s in _otherIDs.Keys)
                {
                    XmlAttribute xa = xDoc.CreateAttribute(s);
                    xa.Value = _otherIDs[s];
                    _ = xDoc.DocumentElement.Attributes.Append(xa);
                }
            }
            return xDoc.OuterXml;
        }

        /// <summary>
        /// get value of OtherID's for given key
        /// if exists
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string? GetOtherID(string key)
        {
            return OtherIDs[key];
        }


        /// <summary>
        /// set the OtherIDs dictionary with values given in attributes
        /// of the xmlElement called ID
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public bool SetOtherIDs(string xmlString)
        {
            bool success = false; //unless successful
            try
            {
                _otherIDs = [];
                if ((xmlString != null) && (xmlString.Trim() != string.Empty))
                {
                    XmlDocument xDoc = new();
                    try
                    {
                        xDoc.LoadXml(xmlString);
                    }
                    catch
                    {
                        //just leave the collection empty
                    }

                    if ((xDoc == null) ||
                        (xDoc.DocumentElement == null) ||
                        (!xDoc.DocumentElement.Name.Trim().Equals("ID", StringComparison.CurrentCultureIgnoreCase)))
                    {
                        //we'll just go on with no other id's
                        //throw new Exception("Programming error:  trying to read xml " + "representation of user's ID's but didn't find ID document element.");
                    }
                    else
                    {
                        //set new values
                        foreach (XmlAttribute attr in xDoc.DocumentElement.Attributes)
                        {
                            _otherIDs.Add(attr.Name, attr.Value);
                        }
                        //if got here were successful
                        success = true;
                    }
                }//from if xmlString isn't empty
            }
            catch (Exception er)
            {
                throw new Exception("Programming Error reading the xml representation of " +
                    "user's other ID's...", er);
            }
            return success;
        }


        private string? _drFirstUserName = null;
        /// <summary>
        /// usename for DrFirst eRx program or null if not defined
        /// </summary>
        public string? DrFirstUsername
        {
            get
            {
                if (_drFirstUserName == null)
                {
                    //try to find username in other id's
                    if (this.OtherIDs.ContainsKey("DrFirstUsername"))
                    {
                        _drFirstUserName = this.OtherIDs["DrFirstUsername"];
                    }
                }
                //either way, return the value
                return _drFirstUserName;
            }
        }


        /// <summary>
        /// true if user is active
        /// </summary>
        public bool IsActive = false;
        /// <summary>
        /// id used by DrFirst Rcopia
        /// </summary>
        public long RcopiaID = long.MinValue;


        /// <summary>
        /// User object representing the system itself
        /// Example use:  Something was saved by: 'System,' instead
        /// of a real person; id=0
        /// </summary>
        /// <returns></returns>
        public static UserBrief System()
        {
            UserBrief result = new()
            {
                Comment = "Represents the system (program) itself, " +
                "rather than a real person.",
                DisplayName = "System",
                Permissions = UserPermissions.None,
                ShortName = "System",
                UserID = 0 //0 reserved for system
            };
            return result;
        }

        /// <summary>
        /// for example, when selecting a user, this
        /// represents nobody; id=-1
        /// </summary>
        /// <returns></returns>
        public static UserBrief Nobody()
        {
            UserBrief result = new()
            {
                Comment = "Represents no user",
                DisplayName = "Nobody",
                Permissions = UserPermissions.None,
                ShortName = "Nobody",
                UserID = -1 // -1 = nobody
            };
            return result;
        }

        /// <summary>
        /// represents all users; e.g. when choosing whose 
        /// results to show, you can choose all users;  id=-2
        /// </summary>
        /// <returns></returns>
        public static UserBrief AllUsers()
        {
            UserBrief result = new()
            {
                Comment = "Represents all users",
                DisplayName = "All users",
                Permissions = UserPermissions.None,
                PrivacyExclusions = (PrivacyFlags.DataNotForPatient | PrivacyFlags.DataSensitive),
                ShortName = "All users",
                UserID = -2
            };
            return result;
        }

        /// <summary>
        /// generic representation of a provider not using this system;
        /// E.g. a prescription may be prscribed by an "Outside provider" ; id = -3
        /// </summary>
        /// <returns></returns>
        public static UserBrief OutsideProvider()
        {
            UserBrief result = new()
            {
                Comment = "Represents an outside provider not otherwise specified",
                DisplayName = "Outside provider",
                Permissions = UserPermissions.None,
                PrivacyExclusions = PrivacyFlags.DataSensitive,
                ShortName = "Outside provider",
                UserID = -3
            };
            return result;
        }

        /// <summary>
        /// represents some error condition when looking for a user; 
        /// id = -4
        /// </summary>
        /// <returns></returns>
        public static UserBrief Error()
        {
            UserBrief result = new()
            {
                Comment = "Represents an error condition in representing a user",
                DisplayName = "Error",
                Permissions = UserPermissions.None,
                ShortName = "Error",
                UserID = -4
            };
            return result;
        }

        /// <summary>
        /// represents the patient rather than a real user
        /// </summary>
        /// <returns></returns>
        public static UserBrief ThePatient()
        {
            UserBrief result = new()
            {
                Comment = "Represents the patient themself rather than a real user",
                DisplayName = "ThePatient",
                Permissions = UserPermissions.None,
                PrivacyExclusions = PrivacyFlags.DataNotForPatient,
                ShortName = "Patient",
                UserID = -5
            };
            return result;
        }


        /// <summary>
        /// returns short name, in parentheses if inactive
        /// and more if is guest user
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            _ = sb.Append(IsActive ? string.Empty : "(");
            _ = sb.Append(ShortName);
            _ = sb.Append(IsActive ? string.Empty : ")");
            if (IsGuest)
            {
                _ = sb.Append(": guest user: ");
                _ = sb.Append(DisplayName);
                _ = sb.Append(": ");
                _ = sb.Append(Comment);
            }
            return sb.ToString();
        }
        // System.Serialization.BinaryFormatter is obsolete now and Microsoft recommends
        // not using it because of safety issues if someone tries to BinaryFormat
        // malicious code...  If Serialization is needed, consider XmlSerializer or DataContractSerializer
        // instead.  see https://learn.microsoft.com/en-us/dotnet/standard/serialization/binaryformatter-security-guide#preferred-alternatives  7/10/24 wve
        ///////////// <summary>
        ///////////// serialize this object to byte array
        ///////////// </summary>
        ///////////// <returns></returns>
        //////////public byte[] Serialize()
        //////////{
        //////////    byte[] result = null;
        //////////    using (MemoryStream stream = new MemoryStream())
        //////////    {
        //////////        BinaryFormatter formatter = new BinaryFormatter();
        //////////        formatter.Serialize(stream, this);
        //////////        result = stream.ToArray();
        //////////    }
        //////////    return result;
        //////////}

        ///////////// <summary>
        ///////////// deserialize from byte array
        ///////////// </summary>
        ///////////// <param name="serialized"></param>
        ///////////// <returns></returns>
        //////////public static UserBrief Deserialize(byte[] serialized)
        //////////{
        //////////    UserBrief result = null;
        //////////    using (MemoryStream stream = new MemoryStream(serialized))
        //////////    {
        //////////        BinaryFormatter formatter = new BinaryFormatter();
        //////////        result = (UserBrief)formatter.Deserialize(stream);
        //////////    }
        //////////    return result;
        //////////}
    }

    /// <summary>
    /// anyone who uses this program
    /// comprehensive version including PersonNames
    /// but not password
    /// </summary>
    [Serializable]
    public class UserExpanded : UserBrief
    {
        /// <summary>
        /// default constructor makes blank user
        /// </summary>
        public UserExpanded()
        {
        }

        /// <summary>
        /// assigns fields from the userBrief given
        /// </summary>
        /// <param name="userBriefModel"></param>
        public UserExpanded(UserBrief userBriefModel)
        {
            ApplyFields(userBriefModel);
        }

        /// <summary>
        /// assigns fields from the userBrief given
        /// and assigns given names
        /// </summary>
        /// <param name="userBriefModel"></param>
        /// <param name="names"></param>
        /// <param name="commAdds"></param>
        public UserExpanded(UserBrief userBriefModel,
            PersonName[] names,
            CommunicationAddress[] commAdds)
        {
            ApplyFields(userBriefModel);
            this.Names = names;
            this.CommunicationsAddresses = commAdds;
        }

        private void ApplyFields(UserBrief userBriefModel)
        {
            this.Comment = userBriefModel.Comment;
            this.DateLastUpdatedPwd = userBriefModel.DateLastUpdatedPwd;
            this.DateOfBirth = userBriefModel.DateOfBirth;
            this.DisplayName = userBriefModel.DisplayName;
            this.Gender = userBriefModel.Gender;
            this.IsActive = userBriefModel.IsActive;
            this.Login = userBriefModel.Login;
            this.OtherIDs = userBriefModel.OtherIDs;
            this.Password = null;
            this.Permissions = userBriefModel.Permissions;
            this.PrivacyExclusions = userBriefModel.PrivacyExclusions;
            this.Salutation = userBriefModel.Salutation;
            this.ShortName = userBriefModel.ShortName;
            this.UserID = userBriefModel.UserID;
            this.NPI = userBriefModel.NPI;
        }

        /// <summary>
        /// the names assigned to the user
        /// </summary>
        public PersonName[]? Names;

        /// <summary>
        /// addresses other than mail
        /// </summary>
        public CommunicationAddress[]? CommunicationsAddresses;

        /// <summary>
        /// returns the first name of PersonNametype.CurrentName
        /// if any, otherwise the first name found, or null if none
        /// </summary>
        public PersonName? PreferredName
        {
            get
            {
                if (Names != null)
                {
                    foreach (PersonName name in Names)
                    {
                        if (name.Type == PersonNameType.CurrentName)
                        {
                            return name;
                        }
                    }
                    //if got here didn't find one
                    if (Names.Length > 0)
                    {
                        return Names[0];
                    }
                    //if got here still didnt find one
                    return null;
                }
                else
                {
                    return null;
                }
            }
        }
    }



    /// <summary>
    /// brief form useful for quick lists of patients
    /// excludes the PersonName objects 
    /// </summary>
    [Serializable]
    public class PatientBrief : Person
    {
        /// <summary>
        /// id in database
        /// </summary>
        public int PatientID = int.MinValue;
        /// <summary>
        /// office chart number (text) up to 50 char
        /// </summary>
        public string ChartNum = "";
        /// <summary>
        /// social security number
        /// </summary>
        public int SSN = int.MinValue;
        /// <summary>
        /// last 4 digits of ssn
        /// </summary>
        public int SSNLast4 = int.MinValue;
        /// <summary>
        /// userID of primary doc (in this practice)
        /// </summary>
        public int PrimaryDocID = int.MinValue;
        /// <summary>
        /// max 500 char
        /// </summary>
        public string Comment = "";
        /// <summary>
        /// set to true if has similar name to other patient
        /// </summary>
        public bool SimilarNamesAlert;
        /// <summary>
        /// userID of last user editing
        /// </summary>
        public int WhoLastEditedID = int.MinValue;
        /// <summary>
        /// derived from Users table
        /// </summary>
        public string WhoLastEdited = string.Empty;
        /// <summary>
        /// datetime of last editing
        /// or DateTime.MinValue if none
        /// </summary>
        public DateTime WhenLastEdited = DateTime.MinValue;
        /// <summary>
        /// patient is active vs inactive
        /// </summary>
        public bool IsActive = true;
        /// <summary>
        /// patient is dead vs alive
        /// </summary>
        public bool IsDeceased;
        /// <summary>
        /// defaults to true
        /// </summary>
        public bool CareSparkYesNo = true;
        /// <summary>
        /// ID used by DrFirst Rcopia
        /// </summary>
        public long RcopiaID = long.MinValue;
        /// <summary>
        /// flags for subgroups of patients useful in queries,
        /// lower members of which are defined in PatientBrief.SubGroups
        /// e.g. ACO if member of Accountable Care Organization
        /// </summary>
        public int SubGroupFlags = (int)SubGroups.Unspecified;
        /// <summary>
        /// Medicare ID or empty string if none
        /// </summary>
        public string MedicareID = string.Empty;
        /// <summary>
        /// not saved in database, just useful for tagging
        /// information to a patient temporarily
        /// </summary>
        public object? TemporaryTag = null;
        /// <summary>
        /// returns display name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return DisplayName;
        }
        /// <summary>
        /// show details about this patient data in short
        /// one line form
        /// </summary>
        /// <returns></returns>
        public string ShowDetailsShort()
        {
            StringBuilder sb = new();
            _ = sb.Append("Patient ");
            _ = sb.Append(DisplayName);
            _ = sb.Append(" last edited ");
            _ = sb.Append(WhenLastEdited.ToShortDateString());
            _ = sb.Append(" by ");
            _ = sb.Append(WhoLastEdited);
            _ = sb.Append(" {id=");
            _ = sb.Append(PatientID);
            _ = sb.Append('}');
            return sb.ToString();
        }

        ///////////// <summary>
        ///////////// get properties related to this patient
        ///////////// or null if none defined
        ///////////// </summary>
        ///////////// <param name="mm3Data"></param>
        ///////////// <returns></returns>
        //////////public PatientProperties GetProperties(IMM4Data mm4Data)
        //////////{
        //////////    PatientProperties result = null;
        //////////    PatientData[] data = mm4Data.ProgramData.SelectPatientData(
        //////////        Enum.GetName(typeof(PatientData.PatientDataKnownTypes),
        //////////        PatientData.PatientDataKnownTypes.PatProps),
        //////////        this.PatientID);
        //////////    if (data.Length > 0)
        //////////    {
        //////////        string propsXml = data[0].DataText;
        //////////        result = new PatientProperties(propsXml);
        //////////    }
        //////////    return result;
        //////////}

        /// <summary>
        /// return smoking status as a string
        /// (call GetProperties() to find props)
        /// </summary>
        /// <param name="props">call GetProperties() with mm3Data to get props</param>
        /// <returns></returns>
        public static string GetSmokingStatus(PatientProperties props)
        {
            StringBuilder sb = new();
            if (props == null)
            {
                props = new PatientProperties();
                _ = sb.Append('~'); //to indicate undefined
            }
            _ = sb.Append(MainCommonClass.GetEnumDescription(props.SmokingStatus));
            return sb.ToString();
        }
        /// <summary>
        /// get smokeless tobacco status as a string
        /// </summary>
        /// <param name="props"></param>
        /// <returns></returns>
        public static string GetSmokelessTobaccoStatus(PatientProperties props)
        {
            StringBuilder sb = new();
            if (props == null)
            {
                props = new PatientProperties();
                _ = sb.Append('~'); //to indicate undefined
            }
            _ = sb.Append(MainCommonClass.GetEnumDescription(props.SmokelessTobaccoStatus));
            return sb.ToString();
        }

        /// <summary>
        /// get alcoholUse as a string
        /// </summary>
        /// <param name="props"></param>
        /// <returns></returns>
        public static string GetAlcoholUseStatus(PatientProperties props)
        {
            StringBuilder sb = new();
            if (props == null)
            {
                props = new PatientProperties();
                _ = sb.Append('~'); //to indicate undefined
            }
            _ = sb.Append(MainCommonClass.GetEnumDescription(props.AlcoholStatus));
            return sb.ToString();
        }

        /// <summary>
        /// get other substance use as a string
        /// </summary>
        /// <param name="props"></param>
        /// <returns></returns>
        public static string GetSubstanceUseStatus(PatientProperties props)
        {
            StringBuilder sb = new();
            if (props == null)
            {
                props = new PatientProperties();
                _ = sb.Append('~'); //to indicate undefined
            }
            _ = sb.Append(MainCommonClass.GetEnumDescription(props.SubstanceUseStatus));
            return sb.ToString();
        }

        //please note that the default test patient is always the 
        // patient with PatientID = 0 and is normally
        // named Aaron Aardvark.


        /// <summary>
        /// a designation for no patient
        /// (whose patientID will be given as -1)
        /// </summary>
        /// <returns></returns>
        public static PatientBrief Nobody()
        {
            PatientBrief result = new()
            {
                PatientID = -1,
                DisplayName = "<Nobody>"
            };
            return result;
        }

        /// <summary>
        /// a designation for all patients
        /// (whose patientID will be given as -2)
        /// </summary>
        /// <returns></returns>
        public static PatientBrief All()
        {
            PatientBrief result = new()
            {
                PatientID = -2,
                DisplayName = "<All>"
            };
            return result;
        }
        /// <summary>
        /// a designation for unknown patients
        /// (whose PatientID will be -3)
        /// </summary>
        /// <returns></returns>
        public static PatientBrief Unknown()
        {
            PatientBrief result = new()
            {
                PatientID = -3,
                DisplayName = "<Unkn>"
            };
            return result;
        }

        ///////////// <summary>
        ///////////// array of users who can be assigned as PrimaryDoc
        ///////////// </summary>
        //////////private UserBrief[] _eligibleProviders = null;
        ///////////// <summary>
        ///////////// return the userID of patient's primary provider, or determine who it 
        ///////////// should be if not already assigned;  optionally can assign it
        ///////////// Returns int.MinValue if can't assign one at all
        ///////////// </summary>
        ///////////// <param name="changeInDatabase">if true will change the assigned
        ///////////// primary provider in the database </param>
        ///////////// <returns></returns>
        ///////////// <param name="mm3Data">reference to data class</param>
        ///////////// <param name="mm3State">reference to state class</param>
        ///////////// <param name="evenIfAlreadyAssigned"></param>
        //////////public int DeterminePrimaryProvider(IMM4Data mm4Data,
        //////////    IMM3State mm3State,
        //////////    bool changeInDatabase,
        //////////    bool evenIfAlreadyAssigned)
        //////////{
        //////////    int result = int.MinValue;
        //////////    //determine eligible providers
        //////////    if (_eligibleProviders == null)
        //////////    {
        //////////        _eligibleProviders = mm4Data.ActorsData.SelectUserBriefs(true,//only if active
        //////////        UserPermissions.CanBePrimProv);
        //////////    }
        //////////    //assign a primary provider to this patient if possible, and 
        //////////    // if not already assigned to a currently active primary provider
        //////////    if ((this.PrimaryDocID > 0) &&
        //////////        (isEligiblePrimaryProvider(this.PrimaryDocID, _eligibleProviders)) &&
        //////////        (!evenIfAlreadyAssigned))
        //////////    {
        //////////        //easy; it's already assigned
        //////////        result = this.PrimaryDocID;
        //////////    }
        //////////    else //need to assign
        //////////    {
        //////////        System.Collections.Generic.SortedList<ProviderServiceCounter, UserBrief> counter =
        //////////            new SortedList<ProviderServiceCounter, UserBrief>();
        //////////        for (int j = 0; j < _eligibleProviders.Length; j++)
        //////////        {
        //////////            counter.Add(new ProviderServiceCounter(_eligibleProviders[j].UserID),
        //////////                _eligibleProviders[j]);
        //////////        }
        //////////        //count visits
        //////////        ChartNotes.ChartNoteBrief[] visits = mm4Data.DocumentsData.SelectChartNoteBriefs(
        //////////            this.PatientID,
        //////////            PrivacyFlags.None,
        //////////            int.MinValue,//all cats
        //////////            DateTime.Now,
        //////////            DateTime.Now.AddYears(-1)); //visits for past year
        //////////        for (int i = 0; i < visits.Length; i++)
        //////////        {
        //////////            foreach (System.Collections.Generic.KeyValuePair<ProviderServiceCounter, UserBrief>
        //////////                pair in counter)
        //////////            {
        //////////                if (visits[i].AuthorID == (pair.Key.ProviderUserID))
        //////////                {
        //////////                    pair.Key.ChartNotesCount++;
        //////////                    if (visits[i].DateOfEncounter > pair.Key.WhenLastChartNote)
        //////////                    {
        //////////                        pair.Key.WhenLastChartNote = visits[i].DateOfEncounter;
        //////////                    }
        //////////                }//from if is that provider's note
        //////////            }//from foeach provider counter object
        //////////        }//from for each visit
        //////////        if (counter.Count > 0)
        //////////        {
        //////////            result = counter.Keys[0].ProviderUserID;
        //////////        }
        //////////        if (changeInDatabase)
        //////////        {
        //////////            //save the change
        //////////            this.PrimaryDocID = result;
        //////////            mm4Data.ActorsData.UpdatePatient(this,
        //////////                mm3State.CurrentUser.UserID);
        //////////        }
        //////////    }//from if need to assign
        //////////    return result;
        //////////}

        /// <summary>
        /// return true if userID belongs to a provider who can 
        /// be assigned as a primary provider
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="eligibles"></param>
        /// <returns></returns>
#pragma warning disable IDE0051 // Remove unused private members
        private static bool IsEligiblePrimaryProvider(int userID, UserBrief[] eligibles)
#pragma warning restore IDE0051 // Remove unused private members
        {
            for (int i = 0; i < eligibles.Length; i++)
            {
                if (userID == eligibles[i].UserID)
                {
                    return true;
                }
            }
            //if got here didn't find it
            return false;
        }

        /// <summary>
        /// useful marker in determining who a patient's primary clinician is
        /// Gives priority to provider with most chart notes, or most recent one
        /// if tie, or flips a coin if no notes
        /// </summary>
        /// <remarks>
        /// useful marker in determining who a patient's primary clnician is
        /// </remarks>
        /// <param name="userID">of the clinician</param>
        public class ProviderServiceCounter(int userID) : IComparable<ProviderServiceCounter>
        {
            /// <summary>
            /// userid of clinician
            /// </summary>
            public int ProviderUserID = userID;
            /// <summary>
            /// how many chart notes are counted
            /// </summary>
            public int ChartNotesCount = 0;
            /// <summary>
            /// date of most recent chart note authored by clinician
            /// for the patient
            /// </summary>
            public DateTime WhenLastChartNote = DateTime.MinValue;

            /// <summary>
            /// returns greater than zero if this has more chart notes than the
            /// other, or in case of a tie, the
            /// most recent note, or flips a coin to assign one if can't tell, as
            /// in patient without any chart notes.
            /// </summary>
            /// <param name="other"></param>
            /// <returns>1 if other is null</returns>
#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
            public int CompareTo(ProviderServiceCounter other)
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
            {
                if (other == null)
                    return 1;
                else
                {
                    if (this.ChartNotesCount != other.ChartNotesCount)
                    {
                        return this.ChartNotesCount.CompareTo(other.ChartNotesCount);
                    }
                    else
                    {
                        if (this.WhenLastChartNote > other.WhenLastChartNote)
                        {
                            return 1;
                        }
                        else if (this.WhenLastChartNote < other.WhenLastChartNote)
                        {
                            return -1;
                        }
                        else
                        {
                            //flip a coin
                            if (DateTime.Now.Millisecond % 2 == 1)
                            {
                                return 1;
                            }
                            else
                            {
                                return -1;
                            }
                        }
                    }//from if chart note counts equal
                }//from other not null
            }//from compare to
        }

        /// <summary>
        /// flags for
        /// subgroups of patients useful for queries
        /// </summary>
        [Flags]
        public enum SubGroups : int
        {
            /// <summary>
            /// not specified
            /// </summary>
            Unspecified = 0,
            /// <summary>
            /// assigned to the Accountable Care Organization
            /// (not sure this is ever used since it changes so 
            /// much we usually read the list given by CMS instead)
            /// </summary>
            ACO = 1
        }
    }

    /// <summary>
    /// information about a patient, retrieved every time a chart is opened
    /// includes the PersonName objects
    /// </summary>
    [Serializable]
    public class PatientExpanded : PatientBrief, ICloneable
    {
        //(notice if we add members need to edit the Clone() method)


        /// <summary>
        /// new Patient Expanded; see alternate constructor
        /// to create one based on PatientBrief base object
        /// </summary>
        public PatientExpanded()
        {
            Names = [];
            CommunicationsAddresses = [];
            PostalAddresses = [];
            Races = [];
            Ethnicities = [];
        }

        /// <summary>
        /// creates a PatientExpanded based on the PatientBrief source,
        /// to which you must add the members specific to PatientExpanded
        /// </summary>
        /// <param name="basePatientBrief"></param>
        public PatientExpanded(PatientBrief basePatientBrief)
        {
            Names = [];
            CommunicationsAddresses = [];
            PostalAddresses = [];
            Races = [];
            Ethnicities = [];

            if (basePatientBrief != null)
            {
                //Person members
                DisplayName = basePatientBrief.DisplayName;
                Salutation = basePatientBrief.Salutation;
                DateOfBirth = basePatientBrief.DateOfBirth;
                Gender = basePatientBrief.Gender;
                //PatientBrief members
                PatientID = basePatientBrief.PatientID;
                ChartNum = basePatientBrief.ChartNum;
                SSN = basePatientBrief.SSN;
                SSNLast4 = basePatientBrief.SSNLast4;
                PrimaryDocID = basePatientBrief.PrimaryDocID;
                Comment = basePatientBrief.Comment;
                SimilarNamesAlert = basePatientBrief.SimilarNamesAlert;
                WhoLastEditedID = basePatientBrief.WhoLastEditedID;
                WhoLastEdited = basePatientBrief.WhoLastEdited;
                WhenLastEdited = basePatientBrief.WhenLastEdited;
                IsActive = basePatientBrief.IsActive;
                IsDeceased = basePatientBrief.IsDeceased;
                CareSparkYesNo = basePatientBrief.CareSparkYesNo;
                RaceDepreciated = basePatientBrief.RaceDepreciated;
                EthnicityDepreciated = basePatientBrief.EthnicityDepreciated;
                RcopiaID = basePatientBrief.RcopiaID;
                SubGroupFlags = basePatientBrief.SubGroupFlags;
                MedicareID = basePatientBrief.MedicareID;
                GenderIdentity = basePatientBrief.GenderIdentity;
                SexualOrientation = basePatientBrief.SexualOrientation;
                RacesCodified = basePatientBrief.RacesCodified;
                EthnicitiesCodified = basePatientBrief.EthnicitiesCodified;
                Language = basePatientBrief.Language;
                DateOfDeath = basePatientBrief.DateOfDeath;
            }
        }
        /// <summary>
        /// array of names this patient has
        /// </summary>
        public PersonName[] Names;

        /// <summary>
        /// returns the first name of PersonNametype.CurrentName
        /// if any, otherwise the first name found, or null if none
        /// </summary>
        public PersonName? PreferredName
        {
            get
            {
                foreach (PersonName name in Names)
                {
                    if (name.Type == PersonNameType.CurrentName)
                    {
                        return name;
                    }
                }
                //if got here didn't find one
                if (Names.Length > 0)
                {
                    return Names[0];
                }
                //if got here still didnt find one
                return null;
            }
        }

        /// <summary>
        /// mailing addresses
        /// </summary>
        public PostalAddress[] PostalAddresses;

        /// <summary>
        /// returns primary postal address if one is so designated, or
        /// first address found if not, 
        /// or null if none
        /// </summary>
        public PostalAddress? PreferredPostalAddress
        {
            get
            {
                foreach (PostalAddress addr in PostalAddresses)
                {
                    if (addr.Priority == AddressPriority.Primary)
                    {
                        return addr;
                    }
                }
                //if got here none primary
                if (PostalAddresses.Length > 0)
                {
                    return PostalAddresses[0];
                }
                //if got here none
                return null;
            }
        }

        /// <summary>
        /// addresses other than mail
        /// </summary>
        public CommunicationAddress[] CommunicationsAddresses;
        /// <summary>
        /// the first PostalAddress given, in one line,
        /// or empty string if none
        /// </summary>
        /// <returns></returns>
        public string FullAddress()
        {
            StringBuilder sb = new();
            if (PostalAddresses.Length > 0)
            {
                _ = sb.Append(PostalAddresses[0].Line1);
                _ = sb.Append(", ");
                _ = sb.Append(PostalAddresses[0].Line2);
                _ = sb.Append(", ");
                _ = sb.Append(PostalAddresses[0].City);
                _ = sb.Append(", ");
                _ = sb.Append(PostalAddresses[0].State);
            }
            return sb.ToString();
        }

        /// <summary>
        /// one or more races, derived from Races table
        /// </summary>
        public List<CodifiedRace> Races;
        /// <summary>
        /// one or more ethnicities derived from Ethnicities table
        /// </summary>
        public List<CodifiedEthnicity> Ethnicities;

        ///////////// <summary>
        ///////////// get other ID's etc
        ///////////// </summary>
        //////////protected PatientData[] _getIDs = null;
        ///////////// <summary>
        ///////////// get patient other id's , 
        ///////////// optionally from local cache if available,
        ///////////// otherwise contacts database to retrieve it if 
        ///////////// mm3DataRef not null.  Returns null if cache
        ///////////// empty and mm3DataRef is null
        ///////////// </summary>
        ///////////// <param name="mm3DataRef">database reference or 
        ///////////// null if you know it's in the cache</param>
        ///////////// <param name="forceRefresh">load data regardless of cache</param>
        ///////////// <returns></returns>
        //////////public PatientData[] GetIDs(IMM3Data mm3DataRef, bool forceRefresh)
        //////////{
        //////////    if (forceRefresh || (_getIDs == null))
        //////////    {
        //////////        if (mm3DataRef != null)
        //////////        {
        //////////            _getIDs = mm3DataRef.ProgramData.SelectPatientData(
        //////////                PatientData.PatientDataKnownTypes.ID.ToString(),
        //////////                this.PatientID);
        //////////        }
        //////////        else
        //////////        {
        //////////            //data ref null and we needed refresh so null
        //////////            _getIDs = null;
        //////////        }
        //////////    }
        //////////    return _getIDs;
        //////////}
        ///////////// <summary>
        ///////////// gets ID with given type, from cache or database;
        ///////////// Returns null if not found or if we needed
        ///////////// database but mm3DataRef was null
        ///////////// </summary>
        ///////////// <param name="idType">case sensitive eg Msha, Wlmnt, 
        /////////////  GW or Tiger, see PatientExpanded</param>
        ///////////// <param name="mm3DataRef">ref to db or null if you know
        ///////////// the id is already in the cache</param>
        ///////////// <param name="forceRefresh"></param>
        ///////////// <returns></returns>
        //////////public string GetID(string idType, IMM3Data mm3DataRef, bool forceRefresh)
        //////////{
        //////////    string result = null;
        //////////    //get all id's whether from cache or database (if mm3DataRef not null)
        //////////    PatientData[] allIDs = GetIDs(mm3DataRef, forceRefresh);
        //////////    if (allIDs != null)
        //////////    {
        //////////        for (int i = 0; i < allIDs.Length; i++)
        //////////        {
        //////////            if (allIDs[i].DataKey == idType)
        //////////            {
        //////////                result = allIDs[i].DataText;
        //////////            }
        //////////        }
        //////////    }
        //////////    return result;
        //////////}


        /// <summary>
        /// returns a deep clone of this object
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            //make clone of PatientBrief members
            PatientExpanded result = new(this)
            {
                //and add the PatientExpanded-specific members
                Names = new PersonName[Names.Length]
            };
            for (int i = 0; i < Names.Length; i++)
            {
                result.Names[i] = (PersonName)Names[i].Clone();
            }
            result.PostalAddresses = new PostalAddress[PostalAddresses.Length];
            for (int i = 0; i < PostalAddresses.Length; i++)
            {
                result.PostalAddresses[i] = (PostalAddress)PostalAddresses[i].Clone();
            }
            result.CommunicationsAddresses = new CommunicationAddress[CommunicationsAddresses.Length];
            for (int i = 0; i < CommunicationsAddresses.Length; i++)
            {
                result.CommunicationsAddresses[i] = (CommunicationAddress)CommunicationsAddresses[i].Clone();
            }
            result.Races = [];
            for (int i = 0; i < Races.Count; i++)
            {
                result.Races.Add(this.Races[i]);
            }
            result.Ethnicities = [];
            for (int i = 0; i < Ethnicities.Count; i++)
            {
                result.Ethnicities.Add(this.Ethnicities[i]);
            }
            return result;
        }

        /// <summary>
        /// more detailed, multi-line summary of patient
        /// </summary>
        /// <returns></returns>
        public string ToLongString()
        {
            StringBuilder sb = new();
            _ = sb.Append("Name: ");
            _ = sb.Append(DisplayName);
            _ = sb.Append(Environment.NewLine);
            _ = sb.Append("DOB: ");
            _ = sb.Append(DateOfBirth.ToShortDateString());
            _ = sb.Append("       Gender: ");
            _ = sb.Append(MM4Common.Gender.WordRepresentation(this.Gender));
            _ = sb.Append(Environment.NewLine);
            _ = sb.Append("Chart Number: ");
            _ = sb.Append(ChartNum);
            _ = sb.Append(Environment.NewLine);
            _ = sb.Append("SSN: ");
            if (SSN > -1)
            {
                _ = sb.Append(string.Format("{0:000000000}", SSN));
            }
            else
            {
                _ = sb.Append("unknown");
            }
            _ = sb.Append(Environment.NewLine);
            _ = sb.Append("SSN Last 4 digits: ");
            _ = sb.Append(SSNLast4 > -1 ?
                string.Format("{0:0000}", SSNLast4) :
                "unkn");
            _ = sb.Append(Environment.NewLine);
            _ = sb.Append("Address: ");
            _ = sb.Append(FullAddress());
            _ = sb.Append(Environment.NewLine);
            _ = sb.Append("Comment: ");
            _ = sb.Append(Environment.NewLine);
            _ = sb.Append(Comment);
            return sb.ToString();
        }
    }

    /// <summary>
    /// for people OR institutions other than patients
    /// or users; e.g. referral docs, labs, etc.
    /// Brief form excludes Person field and arrays.
    /// 0 Always refers to local source (this program)
    /// </summary>
    [Serializable]
    public class ExternalActorBrief : Person
    {
        /// <summary>
        /// id in table;  0 reserved for local program
        /// </summary>
        public int ExternalActorID = int.MinValue;
        /// <summary>
        /// null if not an organization
        /// </summary>
        public string? OrganizationName = null;
        /// <summary>
        /// null if not an IS entity
        /// </summary>
        public string? InformationSystemName = null;
        /// <summary>
        /// null if not an IS entity
        /// </summary>
        public string? InformationSystemType = null;
        /// <summary>
        /// null if not an IS entity
        /// </summary>
        public string? InformationSystemVersion = null;

    }
    /// <summary>
    /// external actor expanded
    /// </summary>
    [Serializable]
    public class ExternalActorExpanded : ExternalActorBrief
    {
        /// <summary>
        /// null if not a person
        /// </summary>
        public PersonName[]? Names = null;
        /// <summary>
        /// delimit multiple relations with vertical bars
        /// </summary>
        public string? Relations;
        /// <summary>
        /// delimit multiple specialties with vertical bars
        /// </summary>
        public string? Specialties;
        /// <summary>
        /// postal addresses
        /// </summary>
        public PostalAddress[]? PostalAddresses;
        /// <summary>
        /// non-postal numbers and addresses
        /// </summary>
        public CommunicationAddress[]? CommunicationAddresses;
    }

    /// <summary>
    /// street or postal address
    /// </summary>
    [Serializable]
    public class PostalAddress : ICloneable
    {
        /// <summary>
        /// id in table
        /// </summary>
        public int AddressID = int.MinValue;
        /// <summary>
        /// link to whose address this is
        /// </summary>
        public int LinkID;
        /// <summary>
        /// first line
        /// </summary>
        public string Line1 = "";
        /// <summary>
        /// optional second line
        /// </summary>
        public string Line2 = "";
        /// <summary>
        /// city
        /// </summary>
        public string City = "";
        /// <summary>
        /// optional county within the state, or empty string
        /// </summary>
        public string County = "";
        /// <summary>
        /// state
        /// </summary>
        public string State = "";
        /// <summary>
        /// optional country, or empty string if US
        /// </summary>
        public string Country = "";
        /// <summary>
        /// zip code etc
        /// </summary>
        public string PostalCode = "";
        /// <summary>
        /// eg primary, secondary, least preferred
        /// </summary>
        public AddressPriority Priority = AddressPriority.Primary;
        /// <summary>
        /// eg home, work
        /// </summary>
        public PostalAddressType Type = PostalAddressType.Other;
        /// <summary>
        /// is active
        /// </summary>
        public bool IsActive = true;
        /// <summary>
        /// comment
        /// </summary>
        public string Comment = "";
        /// <summary>
        /// the text of the address
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            _ = sb.Append(Line1);
            _ = sb.Append(' ');
            if ((Line2 != null) && (Line2.Length > 0))
            {
                _ = sb.Append("\r\n");
                _ = sb.Append(Line2);
                _ = sb.Append(' ');
            }
            _ = sb.Append("\r\n");
            _ = sb.Append(City);
            _ = sb.Append(' ');
            if ((County != null) && (County.Length > 0))
            {
                _ = sb.Append('(');
                _ = sb.Append(County);
                _ = sb.Append(" County)");
                _ = sb.Append(' ');
            }
            _ = sb.Append(State);
            _ = sb.Append(' ');
            if ((Country != null) && (Country.Length > 0))
            {
                _ = sb.Append(Country);
                _ = sb.Append(' ');
            }
            _ = sb.Append(PostalCode);
            return sb.ToString();
        }

        #region ICloneable Members

        /// <summary>
        /// returns clone of this address, which could be 
        /// considered deep since only strings and numbers
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }

    /// <summary>
    /// data to validate
    /// requests for password changes
    /// </summary>
    [Serializable]
    public class PasswordResetRequest
    {
        /// <summary>
        /// this is the same as its MiscellaneousItemID for the data stored
        /// in database in MiscellaneousItems table
        /// </summary>
        public int PasswordResetRequestID = int.MinValue;
        /// <summary>
        /// id of the web user requesting to change pwd
        /// </summary>
        public int WebUserID = int.MinValue;
        /// <summary>
        /// an ID generated to save in database which much match
        /// that given to the password Update web page
        /// </summary>
        public int RandomID = int.MinValue;
        /// <summary>
        /// when the request was requested, used to determine
        /// whether the request has timed out (typically after an hour)
        /// </summary>
        public DateTime WhenRequested = DateTime.MinValue;
        /// <summary>
        /// the address this password request was sent to, from 
        /// which user can link to the passwordupdate web page
        /// </summary>
        public string AddressRequestSentTo = string.Empty;
        private static readonly string[] separator = ["|"];

        /// <summary>
        /// e.g. 20091109_115959
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToCompactString(DateTime dt)
        {
            StringBuilder result = new();
            _ = result.Append(string.Format("{0:0000}", dt.Year));
            _ = result.Append(string.Format("{0:00}", dt.Month));
            _ = result.Append(string.Format("{0:00}", dt.Day));
            _ = result.Append('_');
            _ = result.Append(string.Format("{0:00}", dt.Hour));
            _ = result.Append(string.Format("{0:00}", dt.Minute));
            _ = result.Append(string.Format("{0:00}", dt.Second));
            return result.ToString();
        }

        /// <summary>
        /// from string generated by ToCompactString(),
        /// e.g. 20091109_115959
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime FromCompactString(string s)
        {
            return new DateTime(
                int.Parse(s[..4]), //.. is range indicator
                int.Parse(s.Substring(4, 2)),
                int.Parse(s.Substring(6, 2)),
                int.Parse(s.Substring(9, 2)),
                int.Parse(s.Substring(11, 2)),
                int.Parse(s.Substring(13, 2)));
        }
        /// <summary>
        /// this object serialized to a string with vertical bars
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            _ = sb.Append(WebUserID); //0          
            _ = sb.Append('|');
            if (PasswordResetRequestID != int.MinValue)
                _ = sb.Append(PasswordResetRequestID); //1
            _ = sb.Append('|');
            _ = sb.Append(RandomID); //2
            _ = sb.Append('|');
            _ = sb.Append(PasswordResetRequest.ToCompactString(WhenRequested)); //3
            _ = sb.Append('|');
            _ = sb.Append(AddressRequestSentTo); //4
            return sb.ToString();
        }
        /// <summary>
        /// regenerate object from its ToString() serialization
        /// </summary>
        /// <param name="serializedString"></param>
        /// <returns></returns>
        public static PasswordResetRequest FromString(string serializedString)
        {
            PasswordResetRequest result = new();
            string[] parts = serializedString.Split(separator,
                StringSplitOptions.None);
            result.WebUserID = int.Parse(parts[0]);
            if (parts[1].Length > 0)
                result.PasswordResetRequestID = int.Parse(parts[1]);
            result.RandomID = int.Parse(parts[2]);
            result.WhenRequested = PasswordResetRequest.FromCompactString(parts[3]);
            result.AddressRequestSentTo = parts[4];
            return result;
        }
    }

    /// <summary>
    /// verification of an address as valid and belonging to purported owner
    /// </summary>
    [Serializable]
    public class AddressVerification
    {
        /// <summary>
        /// id of this verification entry in database
        /// </summary>
        public int AddressVerificationID = int.MinValue;
        /// <summary>
        /// e.g. patient 
        /// </summary>
        public AddressVerificationCat Category = AddressVerificationCat.Unassigned;
        /// <summary>
        /// id of the addresss being verified, e.g. PatientCommunicationAddressID.
        /// </summary>
        public int CommunicationAddressID = int.MinValue;
        /// <summary>
        /// true if is verified 
        /// </summary>
        public bool IsVerified = false;
        /// <summary>
        /// when verified (generated by database on saving)
        /// </summary>
        public DateTime WhenVerified;
        /// <summary>
        /// id of user who entered this as verified
        /// </summary>
        public int WhoEnteredID;
        /// <summary>
        /// the text value of the communicationsAddress when verified
        /// </summary>
        public string AddressAsVerified = string.Empty;
        /// <summary>
        /// optiona details like how it was verified 
        /// e.g. verbal, eMailed
        /// </summary>
        public string Details = string.Empty;
        /// <summary>
        /// the patientID or userID or other person ID of whose address this is
        /// </summary>
        public int PersonID = int.MinValue;
        private static readonly string[] separator = ["|"];

        /// <summary>
        /// create vertical bar separated string of this AddressVerification
        /// Category, CommunicationAddressLinkID, AddressAsVerified,
        /// PatientID
        /// </summary>
        /// <returns></returns>
        public string ToShortString()
        {
            StringBuilder sb = new();
            _ = sb.Append((int)Category); //0
            _ = sb.Append('|');
            _ = sb.Append(CommunicationAddressID); //1
            _ = sb.Append('|');
            _ = sb.Append(AddressAsVerified); //2
            _ = sb.Append('|');
            _ = sb.Append(PersonID); //3
            return sb.ToString();
        }



        /// <summary>
        /// create AddressVerification object from information saved previously by ToShortString() method
        /// or return null if errors
        /// </summary>
        /// <param name="shortString"></param>
        /// <returns></returns>
        public static AddressVerification FromShortString(string shortString)
        {
            AddressVerification? _1;
            try
            {
                _1 = new AddressVerification();
                string[] results = shortString.Split(separator, StringSplitOptions.None);
                _1.Category = (AddressVerificationCat)(int.Parse(results[0]));
                _1.CommunicationAddressID = int.Parse(results[1]);
                _1.AddressAsVerified = results[2];
                _1.PersonID = int.Parse(results[3]);
            }
            catch (Exception er)
            {
                //return null
                _1 = null;
                throw new Exception("Sorry, could not interpret the string " +
                    "representation of this communication address verification.", er);
            }
            return _1;
        }
    }
    /// <summary>
    /// category of communication Address being verified
    /// </summary>
    public enum AddressVerificationCat : int
    {
        /// <summary>
        /// DO NOT LEAVE UNASSIGNED
        /// </summary>
        Unassigned = 0,
        /// <summary>
        /// patient communication address
        /// </summary>
        PatientCommunicationAddress = 1,
        /// <summary>
        /// user communication address
        /// </summary>
        UserCommunicationAddress = 2
    }

    /// <summary>
    /// non-postal addresses, e.g. phone, e-mail, url
    /// </summary>
    [Serializable]
    public partial class CommunicationAddress : ICloneable
    {
        /// <summary>
        /// id
        /// </summary>
        public int CommunicationAddressID = int.MinValue;
        /// <summary>
        /// link to whose address this is
        /// </summary>
        public int LinkID = int.MinValue;
        /// <summary>
        /// kind of address eg email, telephone
        /// </summary>
        public CommunicationMethod Method = CommunicationMethod.Other;
        /// <summary>
        /// location, eg home phone, cell phone, email
        /// </summary>
        public CommunicationsSubType SubType = CommunicationsSubType.Other;
        /// <summary>
        /// the coummunication address
        /// </summary>
        public string Value = "";
        /// <summary>
        /// eg primary, secondary, least preferred
        /// </summary>
        public AddressPriority Priority = AddressPriority.Primary;
        /// <summary>
        /// is it active, current?
        /// </summary>
        public bool IsActive = true;
        /// <summary>
        /// is it unlisted
        /// </summary>
        public bool IsUnlisted = false;

        /// <summary>
        /// comment
        /// </summary>
        public string Comment = "";
        /// <summary>
        /// the value or text of the address
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            _ = sb.Append(Method.ToString());
            _ = sb.Append(": ");
            _ = sb.Append(FormatPhoneNumber(Value));
            _ = sb.Append(IsUnlisted ? " *UNLISTED! " : " ");
            _ = sb.Append(SubType.ToString());
            _ = sb.Append(" (");
            _ = sb.Append(Priority.ToString());
            _ = sb.Append(") ");
            _ = sb.Append(Comment);
            return sb.ToString();
        }

        //////////private AddressVerification[] _verifications = null;
        ///////////// <summary>
        ///////////// Verifications that the address belongs to purported user, if verification exists
        ///////////// Returns an empty AddressVerification array if 
        ///////////// no verification is found, and should be only one AddressVerification
        ///////////// if any exist
        ///////////// </summary>
        ///////////// <param name="cat">category of Comm Address: patient or user</param>
        ///////////// <param name="data"></param>
        //////////public AddressVerification[] Verifications(IMM4Data data, AddressVerificationCat cat)
        //////////{
        //////////    //if we haven't checked yet...
        //////////    if (_verifications == null)
        //////////    {
        //////////        //check to see if any verifications exist
        //////////        _verifications = data.ActorsData.SelectAddressVerifications(
        //////////            cat, this.CommunicationAddressID);
        //////////    }
        //////////    return _verifications;
        //////////}

        /// <summary>
        /// formats a 7 and 10 digit number into phone number
        /// format, or else returns unchanged string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FormatPhoneNumber(string input)
        {
            StringBuilder sb;
            string inputTrimmed = input.Trim();
            if (MyRegex().IsMatch(inputTrimmed.Trim()))
            {
                inputTrimmed = inputTrimmed.Trim();
                sb = new StringBuilder();
                _ = sb.Append(inputTrimmed[..3]);
                _ = sb.Append('-');
                _ = sb.Append(inputTrimmed.AsSpan(3, 4));
                return sb.ToString();
            }
            else if (MyRegex1().IsMatch(inputTrimmed.Trim()))
            {
                inputTrimmed = inputTrimmed.Trim();
                sb = new StringBuilder();
                _ = sb.Append('(');
                _ = sb.Append(inputTrimmed[..3]);
                _ = sb.Append(") ");
                _ = sb.Append(inputTrimmed.AsSpan(3, 3));
                _ = sb.Append('-');
                _ = sb.Append(inputTrimmed.AsSpan(6, 4));
                return sb.ToString();
            }
            else
            {
                return inputTrimmed;
            }
        }

        #region ICloneable Members

        /// <summary>
        /// returns clone of this address, which is considered
        /// deep because only has strings and numbers
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        [GeneratedRegex(@"^\d\d\d\d\d\d\d$")]
        private static partial Regex MyRegex();
        [GeneratedRegex(@"^\d\d\d\d\d\d\d\d\d\d$")]
        private static partial Regex MyRegex1();

        #endregion
    }


    /// <summary>
    /// Provides textual description of privacy categories
    /// in its ToString() method
    /// </summary>
    /// <remarks>
    /// Provides textual descriptionn of privacy categories
    /// a little more detailed than the PrivacyFlags enumaration's 
    /// ToString() method does.
    /// </remarks>
    /// <param name="flags"></param>
    [Serializable]
    public class PrivacyDescription(PrivacyFlags flags)
    {
        private readonly PrivacyFlags flagsValue = flags;
        /// <summary>
        /// the privacy flags enumeration
        /// </summary>
        public PrivacyFlags FlagsValue
        {
            get { return flagsValue; }
        }
        /// <summary>
        /// description of flags setting
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            bool anyFlagSet = false; //unless one is set
            StringBuilder sb = new();
            if ((flagsValue & PrivacyFlags.DataSensitive) ==
                PrivacyFlags.DataSensitive)
            {
                _ = sb.Append("Sensitive ");
                anyFlagSet = true;
            }
            if ((flagsValue & PrivacyFlags.DataNotForPatient) ==
                PrivacyFlags.DataNotForPatient)
            {
                if (anyFlagSet)
                    _ = sb.Append("& ");
                _ = sb.Append("Not for pt view ");
                anyFlagSet = true;
            }
            //were any flags set?
            if (!anyFlagSet)
            {
                //if not, say so
                _ = sb.Append("No restrictions.");
            }
            return sb.ToString();
        }

        /// <summary>
        /// array of all possible combinations of privacy flags
        /// </summary>
        /// <returns></returns>
        public static PrivacyDescription[] CombinedDescriptions
        {
            get
            {
                PrivacyDescription[] result =
                [
                    new PrivacyDescription(PrivacyFlags.None),
                    new PrivacyDescription(PrivacyFlags.DataSensitive),
                    new PrivacyDescription(PrivacyFlags.DataNotForPatient),
                    new PrivacyDescription(PrivacyFlags.DataSensitive |
                        PrivacyFlags.DataNotForPatient),
                ];
                return result;
            }
        }

        /// <summary>
        /// array of names of individual flags of Privacy restrictions
        /// </summary>
        public static string[] IndividualDescriptions
        {
            get
            {
                String[] result = ["Unrestricted", "Sensitive: not for routine printouts", "Not for patient's view."];
                return result;
            }
        }
    }



    /// <summary>
    /// one patient may have multiple AdvanceDirective entries
    /// </summary>
    [Serializable]
    public class AdvanceDirective
    {
        /// <summary>
        /// id in table
        /// </summary>
        public int AdvanceDirectiveEntryID = int.MinValue;
        /// <summary>
        /// patient id
        /// </summary>
        public int PatientID = int.MinValue;
        /// <summary>
        /// such as resussitation status, tube feedings, etc
        /// </summary>
        public AdvDirType Type;
        /// <summary>
        /// enumerated directives like "no code"
        /// </summary>
        public AdvDirDescription Description;
        /// <summary>
        /// Free text description
        /// </summary>
        public string? Comment;
        /// <summary>
        /// date verified
        /// </summary>
        public DateTime DateVerified;
        /// <summary>
        /// applies to DateVerified, so avoid
        /// stop date and end date which are below
        /// </summary>
        public AdvDirDateType TypeOfDateVerified;
        /// <summary>
        /// date when this directive takes effect
        /// </summary>
        public DateTime StartDate;
        /// <summary>
        /// date when this directive expired
        /// </summary>
        public DateTime StopDate;
        /// <summary>
        /// status of this directive, such as current and verified
        /// </summary>
        public AdvDirStatus Status;
        /// <summary>
        /// the person or institution that is the source of this data
        /// such as patient, parent, physician, poa
        /// </summary>
        public string? Source;
        /// <summary>
        /// user who entered this data
        /// </summary>
        public int WhoEnteredID = int.MinValue;
        /// <summary>
        /// derived from Users
        /// </summary>
        public string? WhoEntered;
        /// <summary>
        /// when data was entered into database
        /// </summary>
        public DateTime WhenEntered;
        /// <summary>
        /// link if crossed out as an error
        /// or int.MinValue if not
        /// </summary>
        public int XOutID = int.MinValue;
        /// <summary>
        /// derived from XOuts
        /// </summary>
        public string? WhoXdOut;
        /// <summary>
        /// derived
        /// </summary>
        public DateTime WhenXdOut;
        /// <summary>
        /// derived
        /// </summary>
        public string? WhyXdOut;
    }

    /// <summary>
    /// stuff related to Accountable Care Organizations
    /// </summary>
    [Serializable]
    public class ACOStuff
    {
        /// <summary>
        /// classificationof ACO Eligibility status
        /// </summary>
        public enum ACOElig : int
        {
            /// <summary>
            /// not eligible
            /// </summary>
            Not = 0,
            /// <summary>
            /// eligible
            /// </summary>
            Is = 1
        }

        /// <summary>
        /// status of patient's option re: ACO
        /// </summary>
        public enum OptionStats : int
        {
            /// <summary>
            /// actively opted out
            /// </summary>
            OptedOut = -1,
            /// <summary>
            /// didn't take any action
            /// </summary>
            NoAction = 0,
            /// <summary>
            /// actively opted (back) in
            /// </summary>
            OptedIn = 1
        }
        /// <summary>
        /// id in database
        /// </summary>
        public int ACOStuffID = int.MinValue;
        /// <summary>
        /// patient this applies to
        /// </summary>
        public int PatientID = int.MinValue;
        /// <summary>
        /// when notification papwerwork was given,
        /// or DateTime.MinValue if not applicable
        /// </summary>
        public DateTime WhenPaperworkGiven = DateTime.MinValue;
        /// <summary>
        /// opted out or not, etc
        /// </summary>
        public OptionStats OptionStatus = OptionStats.NoAction;
        /// <summary>
        /// when last opted in or out, or DateTime.MinValue
        /// if not applicable
        /// </summary>
        public DateTime WhenOpted = DateTime.MinValue;
        /// <summary>
        /// optional extra information, in xml format
        /// </summary>
        public string Tag = string.Empty;
        /// <summary>
        /// if patient is (eligible for or ) in the ACO
        /// </summary>
        public ACOElig ACOEligibility = ACOElig.Not;
        /// <summary>
        /// optional flags for subdivisions of data mining
        /// </summary>
        public int Flags = 0;
    }

    /// <summary>
    /// Problem in the patient's problem list
    /// </summary>
    [Serializable]
    public class Problem
    {
        /// <summary>
        /// id in database
        /// </summary>
        public int ProblemID = int.MinValue;
        /// <summary>
        /// link to patient
        /// </summary>
        public int PatientID = int.MinValue;
        /// <summary>
        /// ccr calls this Description
        /// </summary>
        public string Text = "";
        /// <summary>
        /// optional accessory description
        /// </summary>
        public string Comment = "";
        /// <summary>
        /// when entered into database
        /// </summary>
        public DateTime DateEntered = DateTime.MinValue;
        /// <summary>
        /// optional  date of onset or start, or
        /// datetime.MinValue if not specified
        /// </summary>
        public DateTime StartDate = DateTime.MinValue;
        /// <summary>
        /// optional date resolved if applicable,
        /// or DateTime.MinValue if not specified
        /// </summary>
        public DateTime StopDate = DateTime.MinValue;
        /// <summary>
        /// individual episodes of problem if recurrent, 
        /// in xml
        /// </summary>
        public string Episodes = "";
        /// <summary>
        /// icd9 code, not to be confused with ICD10
        /// </summary>
        public string ICDx = "";
        /// <summary>
        /// icd10 code
        /// </summary>
        public string ICD10 = "";
        /// <summary>
        /// flags status of the problem: active, chronic etc,
        /// </summary>
        public ProblemStatus Status = ProblemStatus.Active;
        /// <summary>
        /// privacy status
        /// </summary>
        public PrivacyFlags PrivacyStatus = PrivacyFlags.None;
        /// <summary>
        /// link to ExternalActorID, or 1 if from this program, 
        /// int.MinValue if unspecified
        /// </summary>
        public int SourceID = 1;
        /// <summary>
        /// id of user who entered the problem
        /// </summary>
        public int WhoEnteredID = int.MinValue;
        /// <summary>
        /// derived from Users.ShortName
        /// </summary>
        public string WhoEntered = "";
        /// <summary>
        /// link to xOut or int.MinValue if none
        /// </summary>
        public int XOutID = int.MinValue;
        /// <summary>
        /// derived from XOuts
        /// </summary>
        public string WhoXdOut = "";
        /// <summary>
        /// derived
        /// </summary>
        public string WhyXdOut = "";
        /// <summary>
        /// derived
        /// </summary>
        public DateTime WhenXdOut = DateTime.MinValue;
        /// <summary>
        /// from database
        /// </summary>
        public long Timestamp = long.MinValue;
        /// <summary>
        /// how precisely date is specified, eg year, month, or day
        /// </summary>
        public DateTimePrecision StartDatePrecision = DateTimePrecision.Unspecified;
        /// <summary>
        /// how precisely date is specified, eg year, month, or day
        /// </summary>
        public DateTimePrecision StopDatePrecision = DateTimePrecision.Unspecified;
        /// <summary>
        /// ID used in DrFirst Rcopia database
        /// </summary>
        public long RcopiaID = long.MinValue;
        /// <summary>
        /// auxiliary id we can send to Rcopia or other database
        /// as an external (i.e. external to their database ) ID
        /// </summary>
        public int ExternalID = int.MinValue;
        /// <summary>
        /// optional flag that could be true if is codified in a standard codeset,
        /// false if just free text
        /// </summary>
        public bool IsCodified = false;
        /// <summary>
        /// code in the SNOMED CT coding system
        /// </summary>
        public string SNOMED = string.Empty;


        #region Constructors
        /// <summary>
        /// create a new problem without specifying patient id
        /// </summary>
        public Problem()
        {
            ;//nothing to do
        }
        /// <summary>
        /// create a new problem to attach to given patient id
        /// </summary>
        /// <param name="patientID"></param>
        public Problem(int patientID)
        {
            this.PatientID = patientID;
        }
        #endregion //Constructors


        /// <summary>
        /// show text and icd code for the problem
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new(Text);
            if ((ICD10 != null) &&
                (ICD10 != string.Empty))
            {
                _ = sb.Append(" (");
                _ = sb.Append(ICD10.Trim());
                _ = sb.Append(')');
            }
            if ((ICDx != null) &&
            (ICDx != string.Empty))
            {
                _ = sb.Append(" (");
                _ = sb.Append(ICDx);
                _ = sb.Append(')');
            }
            if ((SNOMED != null) &&
                (SNOMED.Trim() != string.Empty))
            {
                _ = sb.Append(" (");
                _ = sb.Append(SNOMED.Trim());
                _ = sb.Append(')');
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// ICD 10 diagnosis code
    /// </summary>
    public class ICD10
    {
        /// <summary>
        /// id in database
        /// </summary>
        public int ICD10ID = int.MinValue;
        /// <summary>
        /// order number for showing in logical order
        /// </summary>
        public int OrderNumber = int.MinValue;
        /// <summary>
        /// the code without periods
        /// </summary>
        public string ICD10Code = string.Empty;
        /// <summary>
        /// THe ICD10 code with periods
        /// </summary>
        public string ICD10CodeFormatted
        {
            get
            {
                StringBuilder sb = new();
                if (ICD10Code.Length > 2) //which it should always be
                {
                    _ = sb.Append(ICD10Code[..3]);
                }
                _ = sb.Append('.');
                if (ICD10Code.Length > 3)
                {
                    _ = sb.Append(ICD10Code[3..]);
                }
                return sb.ToString();
            }
        }
        /// <summary>
        /// is a useable icd10 code as opposed to just 
        /// a header entry for organizing codes
        /// </summary>
        public bool IsValidNotHeader = false;
        /// <summary>
        /// the not so short briefer version of its name
        /// </summary>
        public string ShortName = string.Empty;
        /// <summary>
        /// the longer version of the name
        /// </summary>
        public string LongName = string.Empty;
        /// <summary>
        /// empty string unless value is imported from a matching
        /// Icd10AltText table entry.
        /// </summary>
        public string AlternateText = string.Empty;
        /// <summary>
        /// the code and short name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            if (ICD10Code.Length > 2) //which it should always be
            {
                _ = sb.Append(ICD10Code[..3]);
            }
            _ = sb.Append('.');
            if (ICD10Code.Length > 3)
            {
                _ = sb.Append(ICD10Code[3..]);
            }
            _ = sb.Append(' ');
            _ = sb.Append(ShortName);
            return sb.ToString();
        }
    }

    /// <summary>
    /// a diagnosis code by the International Classification of Diseases
    /// </summary>
    public class ICDCode
    {
        /// <summary>
        /// the ICD code
        /// </summary>
        public string? Icd9Code;
        /// <summary>
        /// text label 
        /// </summary>
        public string? Name;
        /// <summary>
        /// other text it can be looked up by
        /// </summary>
        public string? AltName;
        /// <summary>
        /// description
        /// </summary>
        public string? Comment;
        /// <summary>
        /// to keep track of updates of codes
        /// </summary>
        public string? Version;
        /// <summary>
        /// primary key for table
        /// </summary>
        public int? ICDCodeID;
        /// <summary>
        /// show code and name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            _ = sb.Append(Icd9Code);
            _ = sb.Append(' ');
            _ = sb.Append(Name);
            return sb.ToString();
        }
    }

    /// <summary>
    /// abstract class that FamilyHistoryItem, etc inherit from
    /// </summary>
    [Serializable]
    public abstract class HistoryItem
    {
        /// <summary>
        /// id of patient
        /// </summary>
        public int PatientID = int.MinValue;
        /// <summary>
        /// the family history item description, up to 2000 char
        /// </summary>
        public string Text = "";
        /// <summary>
        /// can hold supplemental info such as source of info, dx codes etc. 
        /// up to 500 char
        /// </summary>
        public string Comment = "";
        /// <summary>
        /// source of info. (up to 50 char)
        /// </summary>
        public string Source = "";
        /// <summary>
        /// date of onset or start or DateTime.MinValue if unspecified
        /// </summary>
        public DateTime StartDate = DateTime.MinValue;
        /// <summary>
        /// date resolved or stopped if applicable, or DateTime.MinValue
        /// </summary>
        public DateTime StopDate = DateTime.MinValue;
        /// <summary>
        /// such as active, inactive, chronic, intermittent, 
        /// recurrrent, r/o, ruled out, resolved
        /// </summary>
        public ProblemStatus Status = ProblemStatus.Other;
        /// <summary>
        /// who entered this data
        /// </summary>
        public int WhoEnteredID = int.MinValue;
        /// <summary>
        /// derived from Users
        /// </summary>
        public string WhoEntered = "";
        /// <summary>
        /// when entered into database
        /// </summary>
        public DateTime WhenEntered = DateTime.MinValue;
        /// <summary>
        /// int.MinValue if not xdOut
        /// </summary>
        public int XOutID = int.MinValue;
        /// <summary>
        /// privacy status
        /// </summary>
        public PrivacyFlags PrivacyExclusions = PrivacyFlags.None;
        /// <summary>
        /// database generated timestamp
        /// </summary>
        public long TimeStamp = long.MinValue;
        /// <summary>
        /// international classification of diseases number
        /// up to 10 characters
        /// </summary>
        public string ICD = string.Empty;
        /// <summary>
        /// only used when editing history items
        /// </summary>
        public HxItemDisposition Disposition = HxItemDisposition.NotSpecified;
        /// <summary>
        /// snomed code
        /// </summary>
        public string SNOMED = string.Empty;
        /// <summary>
        /// single status line details
        /// </summary>
        /// <returns></returns>
        public virtual string ShowDetailsShort(IProgramPiece pieceRef)
        {
            StringBuilder sb = new();
            _ = sb.Append("Entered ");
            _ = sb.Append(WhenEntered.ToShortDateString());
            _ = sb.Append(" by ");
            _ = sb.Append(WhoEntered);

            _ = sb.Append(" {ts=");
            _ = sb.Append(TimeStamp);
            _ = sb.Append("} ");
            _ = sb.Append(Disposition.ToString());

            return sb.ToString();
        }
        /// <summary>
        /// returns the text of the item, 
        /// notated as x'd out if so
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            if (XOutID != int.MinValue)
            {
                _ = sb.Append("<<X'd Out-> ");
            }
            _ = sb.Append(Text);
            if (XOutID != int.MinValue)
            {
                _ = sb.Append(" >>");
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// past medical or surgical history
    /// </summary>
    [Serializable]
    public class PastMedSurgHistoryItem : HistoryItem
    {
        /// <summary>
        /// id in table
        /// </summary>
        public int PastMedSurgHistoryItemID = int.MinValue;
        /// <summary>
        /// true if this is a past surgical history item
        /// </summary>
        public bool IsSurgical = false;
        ///////////// <summary>
        ///////////// single line details
        ///////////// </summary>
        ///////////// <param name="pieceRef"></param>
        ///////////// <returns></returns>
        //////////public override string ShowDetailsShort(IProgramPiece pieceRef)
        //////////{
        //////////    StringBuilder sb = new StringBuilder();
        //////////    if (PastMedSurgHistoryItemID != int.MinValue)
        //////////    {
        //////////        sb.Append(base.ShowDetailsShort(pieceRef));
        //////////    }
        //////////    else
        //////////    {
        //////////        sb.Append("not saved yet");
        //////////    }
        //////////    //show xout details
        //////////    if (XOutID != int.MinValue)
        //////////    {
        //////////        //get xout info
        //////////        XOutInfo xi = pieceRef.MM3Data.ProgramData.SelectXOutInfo(XOutID);
        //////////        if (xi != null)
        //////////        {
        //////////            sb.Append(" ");
        //////////            sb.Append(xi.ShowDetails(PastMedSurgHistoryItemID,
        //////////                int.MinValue, true));
        //////////        }
        //////////        else
        //////////        {
        //////////            sb.Append(" X'd out but couldn't find xout info");
        //////////        }
        //////////    }//from if xd out
        //////////    sb.Append(" {id=");
        //////////    sb.Append(PastMedSurgHistoryItemID.ToString());
        //////////    sb.Append("} ");
        //////////    sb.Append(IsSurgical ? "(surg hx)" : "(p med hx)");
        //////////    return sb.ToString();
        //////////}
        /// <summary>
        /// shallow clone
        /// </summary>
        /// <returns></returns>
        public PastMedSurgHistoryItem Clone()
        {
            return (PastMedSurgHistoryItem)this.MemberwiseClone();
        }
    }

    /// <summary>
    /// family history
    /// </summary>
    [Serializable]
    public class FamilyHistoryItem : HistoryItem
    {
        /// <summary>
        /// id in table
        /// </summary>
        public int FamilyHistoryItemID = int.MinValue;
        /// <summary>
        /// relationship to patient
        /// </summary>
        public FamilyRelationship Relationship = FamilyRelationship.NotSpecified;
        ///////////// <summary>
        ///////////// single line details
        ///////////// </summary>
        ///////////// <param name="pieceRef"></param>
        ///////////// <returns></returns>
        //////////public override string ShowDetailsShort(IProgramPiece pieceRef)
        //////////{
        //////////    StringBuilder sb = new StringBuilder();
        //////////    if (FamilyHistoryItemID != int.MinValue)
        //////////    {
        //////////        sb.Append(base.ShowDetailsShort(pieceRef));
        //////////    }
        //////////    else
        //////////    {
        //////////        sb.Append("not saved yet");
        //////////    }
        //////////    //show xout details
        //////////    if (XOutID != int.MinValue)
        //////////    {
        //////////        //get xout info
        //////////        XOutInfo xi = pieceRef.MM3Data.ProgramData.SelectXOutInfo(XOutID);
        //////////        if (xi != null)
        //////////        {
        //////////            sb.Append(" ");
        //////////            sb.Append(xi.ShowDetails(FamilyHistoryItemID,
        //////////                int.MinValue, true));
        //////////        }
        //////////        else
        //////////        {
        //////////            sb.Append(" X'd out but couldn't find xout info");
        //////////        }
        //////////    }//from if xd out
        //////////    sb.Append(" {id=");
        //////////    sb.Append(FamilyHistoryItemID.ToString());
        //////////    sb.Append("} ");
        //////////    return sb.ToString();
        //////////}
        /// <summary>
        /// shallow clone
        /// </summary>
        /// <returns></returns>
        public FamilyHistoryItem Clone()
        {
            return (FamilyHistoryItem)this.MemberwiseClone();
        }
    }

    /// <summary>
    /// social history item
    /// </summary>
    [Serializable]
    public class SocialHistoryItem : HistoryItem
    {
        /// <summary>
        /// id in table
        /// </summary>
        public int SocialHistoryItemID = int.MinValue;
        /// <summary>
        /// e.g. MaritalStatus,Religion,SmokingHx etc
        /// </summary>
        public SocialHistoryType Type = SocialHistoryType.NotSpecified;
        /// <summary>
        /// DEPRECIATED.  Now see PatientProperties....was: eg. SmokesNow, SmokedButQuit, etc
        /// </summary>
        public SocialHistoryDefinedItem DefinedItem = SocialHistoryDefinedItem.None;
        /// <summary>
        /// individual episodes if recurrent, in xml
        /// </summary>
        public string Episodes = "";
        ///////////// history item already has SNOMED
        ///////////// <summary>
        ///////////// snomed code
        ///////////// </summary>
        //////////public string SNOMED = string.Empty;
        ///////////// <summary>
        ///////////// single line details
        ///////////// </summary>
        ///////////// <param name="pieceRef"></param>
        ///////////// <returns></returns>
        //////////public override string ShowDetailsShort(IProgramPiece pieceRef)
        //////////{
        //////////    StringBuilder sb = new StringBuilder();
        //////////    if (SocialHistoryItemID != int.MinValue)
        //////////    {
        //////////        sb.Append(base.ShowDetailsShort(pieceRef));
        //////////    }
        //////////    else
        //////////    {
        //////////        sb.Append("not saved yet");
        //////////    }
        //////////    //show xout details
        //////////    if (XOutID != int.MinValue)
        //////////    {
        //////////        //get xout info
        //////////        XOutInfo xi = pieceRef.MM3Data.ProgramData.SelectXOutInfo(XOutID);
        //////////        if (xi != null)
        //////////        {
        //////////            sb.Append(" ");
        //////////            sb.Append(xi.ShowDetails(SocialHistoryItemID,
        //////////                int.MinValue, true));
        //////////        }
        //////////        else
        //////////        {
        //////////            sb.Append(" X'd out but couldn't find xout info");
        //////////        }
        //////////    }//from if xd out
        //////////    sb.Append(" {id=");
        //////////    sb.Append(SocialHistoryItemID.ToString());
        //////////    sb.Append("} ");
        //////////    return sb.ToString();
        //////////}
        /// <summary>
        /// shallow clone
        /// </summary>
        /// <returns></returns>
        public SocialHistoryItem Clone()
        {
            return (SocialHistoryItem)this.MemberwiseClone();
        }
    }

    /// <summary>
    /// alerts user should know about patient such as drug allergies, latex allergies
    /// </summary>
    [Serializable]
    public class Alerts
    {
        /// <summary>
        /// patient id
        /// </summary>
        public int PatientID = int.MinValue;
        /// <summary>
        /// when alert began or DateTime.MinValue if not specified
        /// </summary>
        public DateTime StartDate = DateTime.MinValue;
        /// <summary>
        /// when alert became inactive or DAteTime.MinValue if not specified
        /// </summary>
        public DateTime StopDate = DateTime.MinValue;
        /// <summary>
        /// eg drug allergy, nondrug allergy
        /// </summary>
        public AlertType Type;
    }

    /// <summary>
    /// memos for users
    /// </summary>
    [Serializable]
    public class Memo
    {
        /// <summary>
        /// id
        /// </summary>
        public int MemoID = int.MinValue;
        /// <summary>
        /// patient memo is attached to if any
        /// or int.minvalue if none
        /// </summary>
        public int PatientID = int.MinValue;
        /// <summary>
        /// text of memo
        /// </summary>
        public string Text = "";
        /// <summary>
        /// comment about memo if any or empty string if none
        /// </summary>
        public string Comment = "";
        /// <summary>
        /// when memo due
        /// </summary>
        public DateTime WhenToRemind = DateTime.MinValue;
        /// <summary>
        /// id of who memo is for
        /// </summary>
        public int WhoToRemindID = int.MinValue;
        /// <summary>
        /// derived from Users
        /// </summary>
        public string WhoToRemind = "";
        /// <summary>
        /// what sort of memo
        /// </summary>
        public MemoType Type = MemoType.General;
        /// <summary>
        /// show emphasis if true
        /// </summary>
        public bool IsPriority = false;
        /// <summary>
        /// when memo was first saved
        /// </summary>
        public DateTime WhenEntered = DateTime.MinValue;
        /// <summary>
        /// id of creator
        /// </summary>
        public int WhoEnteredID = int.MinValue;
        /// <summary>
        /// derived
        /// </summary>
        public string WhoEntered = "";
        /// <summary>
        /// when last edited or MinValue if not yet
        /// </summary>
        public DateTime WhenLastEdited = DateTime.MinValue;
        /// <summary>
        /// id of who last edited or int.MinValue if none
        /// </summary>
        public int WhoLastEditedID = int.MinValue;
        /// <summary>
        /// derived
        /// </summary>
        public string WhoLastEdited = "";
        /// <summary>
        /// inactive if true
        /// </summary>
        public bool IsResolved = false;
        /// <summary>
        /// when made resolved, should be MinValue if IsResolved is false
        /// </summary>
        public DateTime WhenResolved = DateTime.MinValue;
        /// <summary>
        /// id of who resolved and made it inactive
        /// </summary>
        public int WhoResolvedID = int.MinValue;
        /// <summary>
        /// derived
        /// </summary>
        public string WhoResolved = "";
        /// <summary>
        /// privacy settings for memo
        /// </summary>
        public PrivacyFlags PrivacyStatus;
        /// <summary>
        /// link to xout info if has been x'd out as error
        /// </summary>
        public int XOutID = int.MinValue;
        /// <summary>
        /// derived from XOuts
        /// </summary>
        public int WhoXdOutID = int.MinValue;
        /// <summary>
        /// who if any
        /// </summary>
        public string WhoXdOut = "";
        /// <summary>
        /// why if x'd out
        /// </summary>
        public string WhyXdOut = "";
        /// <summary>
        /// when or MinValue if not x'd out as error
        /// </summary>
        public DateTime WhenXdOut = DateTime.MinValue;
    }

    /// <summary>
    /// information regarding an x-d out record
    /// </summary>
    public class XOutInfo
    {
        /// <summary>
        /// id of the xout record in XOuts table
        /// </summary>
        public int XOutID = int.MinValue;
        /// <summary>
        /// identity key of the x'd out record in another table
        /// </summary>
        public int LinkID = int.MinValue;
        /// <summary>
        /// when data was x'd out
        /// </summary>
        public DateTime WhenXdOut = DateTime.MinValue;
        /// <summary>
        /// id of user who x'd out record
        /// </summary>
        public int WhoXdOutID = int.MinValue;
        /// <summary>
        /// derived from Users table
        /// </summary>
        public string WhoXdOut = string.Empty;
        /// <summary>
        /// why record was x'd out
        /// </summary>
        public string WhyXdOut = string.Empty;
        /// <summary>
        /// long number representation of the database timestamp 
        /// of the xout entry
        /// </summary>
        public long Timestamp = long.MinValue;
        /// <summary>
        /// LEGACY FIELD to accomodate old MM2 data.
        /// Now we use TableNameID instead, and this is ignored
        /// </summary>
        public string TableName = string.Empty;
        /// <summary>
        /// the id of the name of the table the x'd out data resides
        /// in.  Comes from TableNames table
        /// </summary>
        public int TableNumber = int.MinValue;
        /// <summary>
        /// the timestamp, if any, that the x'd out data had before
        /// it was changed by setting its XOutID value
        /// </summary>
        public long DataTimestamp = long.MinValue;
        /// <summary>
        /// return true if XOuts.LinkID = MyStuff.StuffID and, optionally,
        /// if the data table name is referenced by the XOut record.  Modifies
        /// the ToString() message accordingly
        /// </summary>
        /// <param name="dataLinkID">primary key of the x'd out record that claims
        /// to reference this XOut entry</param>
        /// <param name="tableNumber">Optionally, the number of the table 
        /// the x'd out record resides in
        /// that claims to reference this XOut entry</param>
        /// <param name="ignoreTableNameMatch">If true, just check the
        /// primary key link but ignore the table number, which might
        /// be unknown</param>
        /// <returns></returns>
        public bool IntegrityCheck(int dataLinkID,
            int tableNumber,
            bool ignoreTableNameMatch)
        {
            StringBuilder sb = new();
            bool integrityCheckPasses;
            if ((dataLinkID == LinkID) &&
                (ignoreTableNameMatch ||
                (tableNumber == TableNumber)
                ))
            {
                //integrity match passes
                integrityCheckPasses = true;
                _ = sb.Append("X'd out on ");
                _ = sb.Append(WhenXdOut.ToShortDateString());
                _ = sb.Append(" by ");
                _ = sb.Append(WhoXdOut);
                if ((WhyXdOut != null) &&
                    (WhyXdOut.Trim() != string.Empty))
                {
                    _ = sb.Append(" for ");
                    _ = sb.Append(WhyXdOut);
                }
            }
            else
            {
                _ = sb.Append("X'd out but uncertain details of x-out. ");
                integrityCheckPasses = false;
            }
            _ = sb.Append(' ');
            xOutInformation = sb.ToString();
            return integrityCheckPasses;
        }

        /// <summary>
        /// this string is modified with details by IntegrityCheck()
        /// </summary>
        private string xOutInformation = "Data crossed-out as invalid.";

        /// <summary>
        /// informational message states information was crossed out and,
        /// IF IntegrityCheck() has been run, then it also contains
        /// details of who/when/why
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return xOutInformation;
        }

        /// <summary>
        /// runs integrity check then returns results
        /// </summary>
        /// <param name="dataLinkID">primary key of the x'd out record that claims
        /// to reference this XOut entry</param>
        /// <param name="tableNumber">Optionally, the number of the table 
        /// the x'd out record resides in
        /// that claims to reference this XOut entry</param>
        /// <param name="ignoreTableNameMatch">If true, just check the
        /// primary key link but ignore the table number, which might
        /// be unknown</param>
        /// <returns></returns>
        public string ShowDetails(int dataLinkID,
            int tableNumber,
            bool ignoreTableNameMatch)
        {
            _ = IntegrityCheck(dataLinkID, tableNumber, ignoreTableNameMatch);
            return xOutInformation;
        }
        /// <summary>
        /// shows details of x-out without checking integrity
        /// of references to table and id
        /// </summary>
        /// <returns></returns>
        public string ShowDetailsWithoutIntegrityCheck()
        {
            StringBuilder sb = new();
            _ = sb.Append("X'd out on ");
            _ = sb.Append(WhenXdOut.ToShortDateString());
            _ = sb.Append(" by ");
            _ = sb.Append(WhoXdOut);
            if ((WhyXdOut != null) &&
                (WhyXdOut.Trim() != string.Empty))
            {
                _ = sb.Append(" for ");
                _ = sb.Append(WhyXdOut);
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// sources for tests, e.g. Laboratory names,etc.
    /// addressed in TestsDataClass
    /// </summary>
    public class TestSource
    {
        /// <summary>
        /// id in database;  1 is reserved for local
        /// installation
        /// </summary>
        public int TestSourceID = int.MinValue;
        /// <summary>
        /// up to 15 chars
        /// </summary>
        public string ShortName = string.Empty;
        /// <summary>
        /// future use could include xml data here
        /// </summary>
        public string Info = string.Empty;

        /// <summary>
        /// blank constructor
        /// </summary>
        public TestSource()
        {
        }

        /// <summary>
        /// structure for identifying sources of test data etc
        /// </summary>
        /// <param name="testSourceID">note this will be 
        /// generated by database at the time of saving, 
        /// regardless of initial value</param>
        /// <param name="shortName"></param>
        /// <param name="info"></param>
        public TestSource(int testSourceID,
            string shortName,
            string info)
        {
            this.TestSourceID = testSourceID;
            this.ShortName = shortName;
            this.Info = info;
        }

        private static readonly TestSource testSourceDefault = new(6,
                    "Default",
                    "Default");

        /// <summary>
        /// the default test source for instance if we normally 
        /// import from Greenway...  can be changed if desired
        /// </summary>
        public static readonly TestSource Default = testSourceDefault;
        private static readonly TestSource testSourceGWPS = new(6,
                    "MRFM_GW",
                    "MRFM Greenway PrimeSuite");

        /// <summary>
        /// the test souce for instance from Greenway PrimeSuite
        /// </summary>
        public static readonly TestSource PrimeSuite = testSourceGWPS;
    }

    /// <summary>
    /// identifies tests in a user's inbox
    /// </summary>
    public struct TestInbox
    {
        /// <summary>
        /// id of user
        /// </summary>
        public int WhoseInboxID;
        /// <summary>
        /// id of test result entry in Tests table
        /// </summary>
        public int TestID;
        /// <summary>
        /// category of misc test, should be 0 or less for named tests
        /// </summary>
        public int MiscTestCatID;
        /// <summary>
        /// id of patient
        /// </summary>
        public int PatientID;
    }

    /// <summary>
    /// base class for LabTests and MiscTests
    /// </summary>
    public abstract class Test
    {
        /// <summary>
        /// id of patient
        /// </summary>
        public int PatientID = int.MinValue;
        /// <summary>
        /// when test done
        /// </summary>
        public DateTime DateOfTest = DateTime.MinValue;
        /// <summary>
        /// when entered into database
        /// </summary>
        public DateTime WhenEntered = DateTime.MinValue;
        /// <summary>
        /// id of user entering the data
        /// </summary>
        public int WhoEnteredID = int.MinValue;
        /// <summary>
        /// derived from Users table
        /// </summary>
        public string WhoEntered = string.Empty;
        /// <summary>
        /// link to XOuts table if data is crossed out, or 
        /// int.MinValue if not
        /// </summary>
        public int XOutID = int.MinValue;
        /// <summary>
        /// the text report of the test, up to 7000 chars
        /// </summary>
        public string TextValue = string.Empty;
        /// <summary>
        /// lower limit of normal, or double.MinValue if undefined
        /// </summary>
        public double LowLimit = double.MinValue;
        /// <summary>
        /// upper limit of normal, or double.MinValue (yes MinValue)
        /// if undefined
        /// </summary>
        public double HiLimit = double.MinValue;
        /// <summary>
        /// flag to show if result is abnormal
        /// </summary>
        public bool IsAbnormal = false;
        /// <summary>
        /// the numeric (SQL float is like .NET double) 
        /// value of the result
        /// </summary>
        public double FloatValue = double.MinValue;

        /// <summary>
        /// id of user who initialed that they saw the result,
        /// or int.MinValue if none
        /// </summary>
        public int WhoInitialedID = int.MinValue;
        /// <summary>
        /// derived from Users table
        /// </summary>
        public string WhoInitialed = string.Empty;
        /// <summary>
        /// privacy exclusions
        /// </summary>
        public PrivacyFlags PrivacyExclusions = PrivacyFlags.None;
        /// <summary>
        /// id of clinician who ordered or is responsible for 
        /// or is assigned to see the test, or
        /// int.MinValue if none
        /// </summary>
        public int ClinicianID = int.MinValue;
        /// <summary>
        /// derived from users
        /// </summary>
        public string Clinician = string.Empty;
        /// <summary>
        /// database timestamp of last alteration of table row
        /// </summary>
        public long Timestamp = long.MinValue;
        /// <summary>
        /// optional comment, up to 500 chars
        /// </summary>
        public string Comment = string.Empty;
        /// <summary>
        /// when user initialed that they saw the test result
        /// </summary>
        public DateTime WhenInitialed = DateTime.MinValue;
        /// <summary>
        /// id of user whose inbox this test is in, derived
        /// from TestsInboxes table (but only first one if multiple)
        /// </summary>
        public int WhoseInboxID = int.MinValue;
        /// <summary>
        /// derived from users
        /// </summary>
        public string WhoseInbox = string.Empty;
        /// <summary>
        /// id of the source of this data or int.MinValue if
        /// undefined; this program is 1;  Others are listed 
        /// in TestSources Table
        /// </summary>
        public int SourceID = int.MinValue;
        /// <summary>
        /// optional ID of this test's name used by source lab
        /// </summary>
        public string TestNameIDPerSource = string.Empty;
        /// <summary>
        /// universal LOINC code for this test name
        /// </summary>
        public string LOINC = string.Empty;
        /// <summary>
        /// flag (e.g. H or * or CRITICAL) up to 10 char
        /// </summary>
        public string Flag = string.Empty;



        // these are appended from the old LabTest class
        /// <summary>
        /// primary key in Tests table
        /// </summary>
        public int TestID = int.MinValue;
        /// <summary>
        /// link to TestNames table
        /// </summary>
        public int TestNameID = int.MinValue;
        /// <summary>
        /// indicates the panel this test is part of, if any.
        /// E.g. if seven labs are part of a basic metabolic profile,
        /// they share the same serial number and will be shown on the 
        /// same line on the grid showing results.
        /// </summary>
        public int PanelSerialNum = int.MinValue;
        /// <summary>
        /// DERIVED from test names if has TestNameID
        /// otherwise may define it here for misc tests
        /// </summary>
        public bool ValueIsFloat = false;

        //
        // //these are appended 6/21/2010, from old MiscTest...
        //
        /// <summary>
        /// Misc Test Category ID, only applicable to MiscTest
        /// objects, values in enum MiscTestCategory
        /// </summary>
        public int MiscTestCatID = int.MinValue;
        /// <summary>
        /// legacy value - the MiscTestID this entry had in Gate City before the 
        /// MiscTests and Tests tables were combined 6/21/2010
        /// </summary>
        public int OldMiscTestID = int.MinValue;
        /// <summary>
        /// up to 80 char; optional if defined in TestNames
        /// </summary>
        public string TestName = string.Empty;

        //

        /// <summary>
        /// shows up to the first 50 chars of text value of the test 
        /// and the flag
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            _ = sb.Append(TextValue.Length > 50 ?
                TextValue[..50] :
                TextValue);
            _ = sb.Append(' ');
            _ = sb.Append(Flag);
            return sb.ToString();
        }
    }


    /// <summary>
    /// A test that is named in the TestNames table.
    /// Use MiscTest for tests that are not named in that table
    /// </summary>
    public class LabTest : Test
    {
        /*
         * as of about 2010...
            // this is now just Test ; we no longer differentiate
         * MiscTest from LabTest
         
         
        /// <summary>
        /// primary key in Tests table
        /// </summary>
        public int TestID = int.MinValue;
        /// <summary>
        /// link to TestNames table
        /// </summary>
        public int TestNameID = int.MinValue;
        /// <summary>
        /// derived from TestNames
        /// </summary>
        public string TestName = string.Empty;
        /// <summary>
        /// indicates the panel this test is part of, if any.
        /// E.g. if seven labs are part of a basic metabolic profile,
        /// they share the same serial number and will be shown on the 
        /// same line on the grid showing results.
        /// </summary>
        public int PanelSerialNum = int.MinValue;
        /// <summary>
        /// DERIVED from test names; setting it here will not set it
        /// in the database; you must set TestNames.ValueIsFloat to 
        /// change it.
        /// </summary>
        public bool ValueIsFloat = false;
        */
    }

    /// <summary>
    /// a test with a miscellaneous name, not included
    /// in the TestNames table
    /// </summary>
    public class MiscTest : Test
    {
        // MiscTest and LabTest are both just Tests now, we no
        // longer differentiate them as separate classes

        /// <summary>
        /// id in the database
        /// </summary>
        public int MiscTestID
        {
            get
            {
                return this.TestID;
            }
            set
            {
                this.TestID = value;
            }
        }
        /*
        /// <summary>
        /// primary key in MiscTests table
        /// </summary>
        public int MiscTestID = int.MinValue;
        /// <summary>
        /// category of miscellaneous tests
        /// </summary>
        public int MiscTestCatID = (int)MiscTestCategory.Unspecified;
        /// <summary>
        /// name of test, up to 80 characters max
        /// </summary>
        public string TestName = string.Empty;
        /// <summary>
        /// set to true if value is numeric
        /// </summary>
        public bool ValueIsFloat = false;
         */
    }

    /// <summary>
    /// information on a named lab test
    /// </summary>
    public class TestName
    {
        /// <summary>
        /// id in TestNames data table
        /// </summary>
        public int TestNameID = int.MinValue;
        /// <summary>
        /// short name to show in grid, 10 char max
        /// </summary>
        public string ShortName = string.Empty;
        /// <summary>
        /// long name 50 char max
        /// </summary>
        public string LongName = string.Empty;
        /// <summary>
        /// id of test category e.g. hematology or chemistry
        /// </summary>
        public TestCategory TestCat;
        /// <summary>
        /// true if the value is numeric
        /// </summary>
        public bool ValueIsFloat = false;
        /// <summary>
        /// order in which to display among named tests in the same category
        /// </summary>
        public int PositionInCat = int.MinValue;
        /// <summary>
        /// number of chars to reserve for showing in grid
        /// </summary>
        public int DisplayWidth = 6;
        /// <summary>
        /// short name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ShortName;
        }
    }

    /// <summary>
    /// link of outside test sources ID's to our
    /// internal TestName objects
    /// </summary>
    public class TestNameIDLink
    {
        /// <summary>
        /// thie id of this record in the database
        /// </summary>
        public int TestNameIDLinkID = int.MinValue;
        /// <summary>
        /// id of source (laboratory) in TestSources table
        /// e.g. local = 1
        /// </summary>
        public int TestSourceID = int.MinValue;
        /// <summary>
        /// id of entry in our TestNames table
        /// </summary>
        public int TestNameID = int.MinValue;
        /// <summary>
        /// the id given this test by the source (lab),
        /// max 50 chars
        /// </summary>
        public string TestIDPerSource = string.Empty;
        /// <summary>
        /// loinc code assigned to this test by the source lab
        /// </summary>
        public string LOINCPerSource = string.Empty;
        /// <summary>
        /// name given this test by the source lab
        /// </summary>
        public string NamePerSource = string.Empty;
    }

    /// <summary>
    /// entry in protimes flowsheet
    /// </summary>
    public class ProtimeEntry
    {
        /// <summary>
        /// id of this entry in table
        /// </summary>
        public int ProtimeEntryID = int.MinValue;
        /// <summary>
        /// link to the protime test associated with this entry
        /// </summary>
        public int PTTestID = int.MinValue;
        /// <summary>
        /// link to the INR test associated with this entry
        /// </summary>
        public int INRTestID = int.MinValue;
        /// <summary>
        /// derived from Tests table
        /// </summary>
        public string INR = string.Empty;
        /// <summary>
        /// obsolete, legacy for when protimes entries also 
        /// generated chart notes
        /// </summary>
        public int ChartNoteID = int.MinValue;
        /// <summary>
        /// when this entry was entered into database
        /// </summary>
        public DateTime DateEntered = DateTime.MinValue;
        /// <summary>
        /// id of user entering this entry
        /// </summary>
        public int WhoEnteredID = int.MinValue;
        /// <summary>
        /// derived from Users table
        /// </summary>
        public string WhoEntered = string.Empty;
        /// <summary>
        /// id link to XOuts table or Int.MinValue if not x'd out
        /// </summary>
        public int XOutID = int.MinValue;
        /// <summary>
        /// link to patient
        /// </summary>
        public int PatientID = int.MinValue;
        /// <summary>
        /// id of tip printed with patient instructions
        /// </summary>
        public int TipID = int.MinValue;
        /// <summary>
        /// dose before this intervention, up to 200 char
        /// </summary>
        public string DoseBefore = string.Empty;
        /// <summary>
        /// id of clinician evaluating protime
        /// </summary>
        public int WhoEvaluatedID = int.MinValue;
        /// <summary>
        /// derived from Users table
        /// </summary>
        public string WhoEvaluated = string.Empty;
        /// <summary>
        /// instructions to print on handout
        /// </summary>
        public string Instructions = string.Empty;
        /// <summary>
        /// comments about this eval, such as "no bleeding"
        /// </summary>
        public string Comments = string.Empty;
        /// <summary>
        /// date this evaluation was made
        /// </summary>
        public DateTime DateOfEval = DateTime.MinValue;
        /// <summary>
        /// new dose after disposition
        /// </summary>
        public string DoseAfter = string.Empty;
        /// <summary>
        /// date to recheck protime next
        /// </summary>
        public DateTime DateRecheck = DateTime.MinValue;
        ///////////// <summary>
        ///////////// short version of entry details, in single line
        ///////////// </summary>
        ///////////// <param name="pieceRef">reference to a program
        ///////////// piece (needed in order to request x-out details
        ///////////// from database if needed</param>
        ///////////// <returns></returns>
        //////////public string ShortDetails(IProgramPiece pieceRef)
        //////////{
        //////////    StringBuilder sb = new StringBuilder();
        //////////    sb.Append("Entered by ");
        //////////    sb.Append(WhoEntered);
        //////////    sb.Append(" on ");
        //////////    sb.Append(DateEntered.ToShortDateString());
        //////////    if (XOutID != int.MinValue)
        //////////    {
        //////////        //get xout details
        //////////        //info of xout
        //////////        XOutInfo xi = null;
        //////////        try
        //////////        {
        //////////            if (pieceRef != null)
        //////////            {
        //////////                xi = pieceRef.MM3Data.ProgramData.SelectXOutInfo(
        //////////                    XOutID);
        //////////            }
        //////////        }
        //////////        catch
        //////////        {
        //////////            //go on, just cant give details of xout
        //////////        }
        //////////        if (xi == null)
        //////////        {
        //////////            sb.Append(" < X'd out, but couldn't find x-out details. >");
        //////////        }
        //////////        else
        //////////        {
        //////////            //check for integrity of xout info
        //////////            if (xi.LinkID != ProtimeEntryID)
        //////////            {
        //////////                sb.Append(" < X'd out but details of x-out corrupted. >");
        //////////            }
        //////////            else
        //////////            {
        //////////                sb.Append(" < X'd out by ");
        //////////                sb.Append(xi.WhoXdOut);
        //////////                sb.Append(" on ");
        //////////                sb.Append(xi.WhenXdOut.ToShortDateString());
        //////////                sb.Append(" for ");
        //////////                sb.Append(xi.WhyXdOut);
        //////////                sb.Append(" >");
        //////////            }
        //////////        }
        //////////    }//from if x'd out
        //////////    sb.Append(" {id=");
        //////////    sb.Append(ProtimeEntryID.ToString());
        //////////    sb.Append("}");
        //////////    return sb.ToString();
        //////////}
        ///////////// <summary>
        ///////////// long, multiline details of entry
        ///////////// </summary>
        ///////////// <param name="pieceRef">reference to a program piece
        ///////////// so can acccess database for x-out info if needed</param>
        ///////////// <returns></returns>
        //////////public string LongDetails(IProgramPiece pieceRef)
        //////////{
        //////////    StringBuilder sb = new StringBuilder();
        //////////    sb.Append("Entered by ");
        //////////    sb.Append(WhoEntered);
        //////////    sb.Append(" on ");
        //////////    sb.Append(DateEntered.ToShortDateString());

        //////////    if (XOutID != int.MinValue)
        //////////    {
        //////////        //get xout details
        //////////        //info of xout
        //////////        XOutInfo xi = null;
        //////////        try
        //////////        {
        //////////            if (pieceRef != null)
        //////////            {
        //////////                xi = pieceRef.MM3Data.ProgramData.SelectXOutInfo(
        //////////                    XOutID);
        //////////            }
        //////////        }
        //////////        catch
        //////////        {
        //////////            //go on, just cant give details of xout
        //////////        }
        //////////        sb.Append(Environment.NewLine);
        //////////        if (xi == null)
        //////////        {
        //////////            sb.Append(" < X'd out, but couldn't find x-out details. >");
        //////////        }
        //////////        else
        //////////        {
        //////////            //check for integrity of xout info
        //////////            if (xi.LinkID != ProtimeEntryID)
        //////////            {
        //////////                sb.Append(" < X'd out but details of x-out corrupted. >");
        //////////            }
        //////////            else
        //////////            {
        //////////                sb.Append(" < X'd out by ");
        //////////                sb.Append(xi.WhoXdOut);
        //////////                sb.Append(" on ");
        //////////                sb.Append(xi.WhenXdOut.ToShortDateString());
        //////////                sb.Append(" for ");
        //////////                sb.Append(xi.WhyXdOut);
        //////////                sb.Append(" >");
        //////////            }
        //////////        }
        //////////    }//from if x'd out
        //////////    sb.Append(Environment.NewLine);
        //////////    sb.Append("Dose before was: ");
        //////////    sb.Append(DoseBefore);
        //////////    sb.Append(Environment.NewLine);
        //////////    //try to locate the lab it refers to
        //////////    LabTest inrLabTest = pieceRef.MM3Data.TestsData.SelectTestByID(
        //////////        INRTestID);
        //////////    if (inrLabTest != null)
        //////////    {
        //////////        //show the test result
        //////////        sb.Append("INR lab test value of ");
        //////////        sb.Append(inrLabTest.TextValue);
        //////////        sb.Append(" on ");
        //////////        sb.Append(inrLabTest.DateOfTest.ToShortDateString());
        //////////        sb.Append(" {id=");
        //////////        sb.Append(INRTestID.ToString());
        //////////        sb.Append("}");
        //////////    }
        //////////    else
        //////////    {
        //////////        //just show the text value
        //////////        sb.Append("INR is ");
        //////////        sb.Append(INR);
        //////////    }
        //////////    sb.Append(Environment.NewLine);
        //////////    sb.Append("Dose advised is: ");
        //////////    sb.Append(DoseAfter);
        //////////    sb.Append(Environment.NewLine);
        //////////    sb.Append("Comments: ");
        //////////    sb.Append(this.Comments);
        //////////    sb.Append(Environment.NewLine);
        //////////    sb.Append("Instructions: ");
        //////////    sb.Append(this.Instructions);
        //////////    sb.Append(Environment.NewLine);
        //////////    sb.Append("Evaluated by ");
        //////////    sb.Append(this.WhoEvaluated);
        //////////    sb.Append(Environment.NewLine);
        //////////    sb.Append("Date of evaluation: ");
        //////////    sb.Append(this.DateOfEval.ToShortDateString());
        //////////    sb.Append(Environment.NewLine);
        //////////    sb.Append("Patient ID = ");
        //////////    sb.Append(this.PatientID.ToString());
        //////////    sb.Append(Environment.NewLine);
        //////////    sb.Append("Date to retest: ");
        //////////    sb.Append(this.DateRecheck.ToShortDateString());
        //////////    sb.Append(Environment.NewLine);
        //////////    sb.Append(" {id=");
        //////////    sb.Append(ProtimeEntryID.ToString());
        //////////    sb.Append("}");
        //////////    return sb.ToString();
        //////////}
        /// <summary>
        /// identifies object as anticoagulation eval of DateOfEval
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            _ = sb.Append("Protime eval of ");
            _ = sb.Append(DateOfEval.ToShortDateString());
            return sb.ToString();
        }
    }

    /// <summary>
    /// class of properties pertaining to a patient,
    /// saved in the PatientData table as xml
    /// </summary>
    public class PatientProperties
    {
        #region enums
        //enums
        /// <summary>
        /// categories for Smoking Status
        /// </summary>
        public enum SmokingStatusCat : int
        {
            /// <summary>
            /// unknown smoking status
            /// </summary>
            [Description("Unknown smoking hx ")]
            Unspecified = 0,
            /// <summary>
            /// currently smokes daily
            /// </summary>
            [Description("Smokes daily ")]
            SmokesDaily = 1,
            /// <summary>
            /// currently smokes but less than daily
            /// </summary>
            [Description("Smokes some days ")]
            SmokesSomeDays = 2,
            /// <summary>
            /// is smoker but current status unknown
            /// </summary>
            [Description("Smoker unkn. curr. stat. ")]
            SmokerUnkCurrent = 3,
            /// <summary>
            /// former smoker of 100 in life
            /// </summary>
            [Description("Former smoker ")]
            FormerSmoker = 4,
            /// <summary>
            /// never a smoker or less tha 100 per life
            /// </summary>
            [Description("Never a smoker ")]
            NeverSmoker = 5,
            /// <summary>
            /// ie more than 10 cigarettes per day
            /// </summary>
            [Description("Heavy smoker")]
            HeavySmoker = 6,
            /// <summary>
            /// ie less than 10 cigarettes per day
            /// </summary>
            [Description("Light smoker")]
            LightSmoker = 7
        }

        /// <summary>
        /// categories for smokeless tobacco  Status
        /// </summary>
        public enum SmokelessTobaccoStatusCat : int
        {
            /// <summary>
            /// unknown smokeless tobacco status
            /// </summary>
            [Description("Unspec. smokeless tob. hx ")]
            Unspecified = 0,
            /// <summary>
            /// currently uses smokeless tobacco daily
            /// </summary>
            [Description("Uses smokeless tob. daily ")]
            UsesDaily = 1,
            /// <summary>
            /// currently uses smokeless tobacco  but less than daily
            /// </summary>
            [Description("Uses smokeless tob. some days ")]
            UsesSomeDays = 2,
            /// <summary>
            /// uses smokeless tobacco  but current status unknown
            /// </summary>
            [Description("Smokeless tob, unspec. amt. ")]
            UsesUnspecAmt = 3,
            /// <summary>
            /// former smokeless tobacco user
            /// </summary>
            [Description("Formely used smokeless tob. ")]
            FormerlyUsed = 4,
            /// <summary>
            /// never used smokeless tobacco 
            /// </summary>
            [Description("Never used smokeless tobacco ")]
            NeverUsed = 5
        }

        //as for alcohol categories, SNOMED HAS:
        //Alcohol consumption unknown
        //Alcohol intake above recommended sensible limits
        //Alcohol intake exceeds recommended daily limit
        //Alcohol intake within recommended daily limit
        //Alcohol intake within recommended sensible limits
        //Denies alcohol abuse
        /// <summary>
        /// description of alcohol consumption
        /// </summary>
        public enum AlcoholStatusCat : int
        {
            /// <summary>
            /// unspecified alcohol consumption
            /// </summary>
            [Description("Unspecified alcohol use")]
            Unspecified = 0,
            /// <summary>
            /// does not drink alcohol
            /// </summary>
            [Description("No alcohol use")]
            NoAlcohol = 1,
            /// <summary>
            /// former use, not now
            /// </summary>
            [Description("No longer drinks alcohol")]
            NoMoreAlcohol = 2,
            /// <summary>
            /// up to 2 drinks alcohol per day average
            /// </summary>
            [Description("Less than 60 drinks alcohol monthly")]
            ModerateAlcohol = 3,
            /// <summary>
            /// more than 2 alcohol drinks per day
            /// </summary>
            [Description("More than 60 drinks alcohol monthly")]
            HeavyAlcohol = 4
        }
        /// <summary>
        /// description of mind altering substances
        /// </summary>
        [Flags]
        public enum SubstanceUseCat : int
        {
            /// <summary>
            /// not specified
            /// </summary>
            [Description("Not specified re: drug use.")]
            Unspecified = 0,
            /// <summary>
            /// none
            /// </summary>
            [Description("Not using illicit drugs")]
            None = 1,
            /// <summary>
            /// not any more
            /// </summary>
            [Description("No longer using illicit drugs")]
            NoLonger = 2,
            /// <summary>
            /// uses
            /// </summary>
            [Description("Uses illicit drugs")]
            Yes = 4,
            /// <summary>
            /// mj
            /// </summary>
            [Description("Uses marijuana")]
            Marijuana = 8,
            /// <summary>
            /// narcotics
            /// </summary>
            [Description("Uses opioids")]
            Narcotics = 16,
            /// <summary>
            /// trip out meds
            /// </summary>
            [Description("Uses hallucinogens")]
            Hallucinogens = 32,
            /// <summary>
            /// speed
            /// </summary>
            [Description("Uses stimulants")]
            Uppers = 64,
            /// <summary>
            /// calmers
            /// </summary>
            [Description("Uses sedatives")]
            Downers = 128
        }
        #endregion enums

        #region fields


        /// <summary>
        /// smoking status
        /// </summary>
        public SmokingStatusCat SmokingStatus = SmokingStatusCat.Unspecified;

        /// <summary>
        /// smokeless tobacco status
        /// </summary>
        public SmokelessTobaccoStatusCat SmokelessTobaccoStatus = SmokelessTobaccoStatusCat.Unspecified;

        /// <summary>
        /// alcohol use status
        /// </summary>
        public AlcoholStatusCat AlcoholStatus = AlcoholStatusCat.Unspecified;

        /// <summary>
        /// mind altering substance use status
        /// </summary>
        public SubstanceUseCat SubstanceUseStatus = SubstanceUseCat.Unspecified;


        /// <summary>
        /// when tobacco use status was last updated, or DateTime.MinValue if none
        /// </summary>
        public DateTime WhenTobaccoUseReviewed = DateTime.MinValue;

        #endregion fields

        #region constructors

        /// <summary>
        /// new blank PatientProperties object
        /// with default members
        /// </summary>
        public PatientProperties()
        {
        }

        /// <summary>
        /// new PatientProperties object created
        /// from xml serialization of previously existing 
        /// object
        /// </summary>
        /// <param name="xmlSerialized"></param>
        public PatientProperties(string xmlSerialized)
        {
            using StringReader sr = new(xmlSerialized);
            //{
            using XmlReader xr = XmlReader.Create(sr);
            //{
            _ = xr.ReadToFollowing("P"); //document element
                                     // not : 4xr.ReadStartElement("P"); //document element
            XmlLocation xlP = new(xr);
            while (xlP.ReadToNextChild())
            {
                switch (xr.Name)
                {
                    case "SS":
                        int smokingStatusValue;
                        if (int.TryParse(xr.ReadString(), out smokingStatusValue))
                        {
                            this.SmokingStatus = (SmokingStatusCat)smokingStatusValue;
                        }
                        break;
                    case "STS":
                        int smokelessTobaccoStatValue;
                        if (int.TryParse(xr.ReadString(), out smokelessTobaccoStatValue))
                        {
                            this.SmokelessTobaccoStatus =
                                (SmokelessTobaccoStatusCat)smokelessTobaccoStatValue;
                        }
                        break;
                    case "TUR":
                        DateTime whenRevd;
                        if (DateTime.TryParse(xr.ReadString(), out whenRevd))
                        {
                            this.WhenTobaccoUseReviewed = whenRevd;
                        }
                        break;
                    case "AU":
                        int alcoholUseValue;
                        if (int.TryParse(xr.ReadString(), out alcoholUseValue))
                        {
                            this.AlcoholStatus =
                                (AlcoholStatusCat)alcoholUseValue;
                        }
                        break;
                    case "SU":
                        int substanceUseValue;
                        if (int.TryParse(xr.ReadString(), out substanceUseValue))
                        {
                            this.SubstanceUseStatus =
                                (SubstanceUseCat)substanceUseValue;
                        }
                        break;
                }
            }
            //}//closes XmlReader
            //}//closes StringReader
        }

        #endregion //constructors

        #region methods

        /// <summary>
        /// serialize PatientProperties object into xml
        /// </summary>
        /// <returns></returns>
        public string ToXml()
        {
            StringBuilder sb = new();
            using (XmlWriter xw = XmlWriter.Create(sb))
            {
                xw.WriteStartElement("P");//document element
                //Smoking Status
                xw.WriteElementString("SS", ((int)SmokingStatus).ToString());
                //Smokeless Tobacco Status
                xw.WriteElementString("STS", ((int)SmokelessTobaccoStatus).ToString());
                //when tobacco use reviewed
                if (WhenTobaccoUseReviewed != DateTime.MinValue)
                {
                    xw.WriteElementString("TUR", WhenTobaccoUseReviewed.ToString("yyyy-MM-dd"));
                }
                //alcohol status
                xw.WriteElementString("AU", ((int)AlcoholStatus).ToString());
                //substance use
                xw.WriteElementString("SU", ((int)SubstanceUseStatus).ToString());
                //close
                xw.WriteEndElement(); //of document elemt P
            }//closes xw
            return sb.ToString();
        }

        ///////////// <summary>
        ///////////// save patient properties to database
        ///////////// </summary>
        //////////public static void Save(IMM4Data mm3Data, int patientID, PatientProperties props)
        //////////{
        //////////    PatientData pd = new PatientData();
        //////////    pd.DataText = props.ToXml();
        //////////    pd.DataType = Enum.GetName(typeof(PatientData.PatientDataKnownTypes),
        //////////        PatientData.PatientDataKnownTypes.PatProps);
        //////////    pd.PatientID = patientID;
        //////////    mm3Data.ProgramData.InsertPatientData(pd, true);
        //////////}
        #endregion methods
    }//class


    /// <summary>
    /// piece of data regarding a patient
    /// used for inr, DoctorFirst info,
    /// patient properties including smoking, etc
    /// and for external ID's like hospital numbers
    /// eg WelmID, MshaID
    /// </summary>
    public class PatientData
    {
        /// <summary>
        /// reserved values for DataType
        /// </summary>
        public enum PatientDataKnownTypes
        {
            /// <summary>
            /// anticoagulation goal for INR
            /// </summary>
            INRGoal,
            /// <summary>
            /// reason for anticoagulation
            /// </summary>
            WarfarinIndication,
            /// <summary>
            /// last update for eRx meds for DrFirst Rcopia
            /// </summary>
            Dr1Med,
            /// <summary>
            /// last update for eRx Allergies for DrFirst Rcopia
            /// </summary>
            Dr1Allergy,
            /// <summary>
            /// Properties of the patient
            /// </summary>
            PatProps,
            /// <summary>
            /// alternate ID's for this patient in other systems eg GW or Tiger
            /// </summary>
            ID,
            /// <summary>
            /// implantable device info
            /// </summary>
            ImplantableDevice
        }
        /// <summary>
        /// id of record in table
        /// </summary>
        public int PatientDatumID = int.MinValue;
        /// <summary>
        /// string identifying category of data, up to 50 char,
        /// see PatientDataKnownTypes
        /// </summary>
        public string DataType = string.Empty;
        /// <summary>
        /// optional key to subdivide DataType,
        /// eg. Msha, Wlmt, GW, Tiger etc - see PatientExpanded
        /// </summary>
        public string DataKey = string.Empty;
        /// <summary>
        /// text of the data
        /// </summary>
        public string DataText = string.Empty;
        /// <summary>
        /// numeric value of data
        /// </summary>
        public double DataFloat = double.MinValue;
        /// <summary>
        /// id of patient
        /// </summary>
        public int PatientID = int.MinValue;
        /// <summary>
        /// when this record last modified
        /// </summary>
        public DateTime WhenLastEntered = DateTime.MinValue;
        /// <summary>
        /// id of user who last modified it
        /// </summary>
        public int WhoEnteredID = int.MinValue;
        /// <summary>
        /// derived from Users
        /// </summary>
        public string WhoEntered = string.Empty;
        /// <summary>
        /// link to x-out entry, or int.MinValue if not x'd out
        /// </summary>
        public int XOutID = int.MinValue;
        /// <summary>
        /// privacy flags
        /// </summary>
        public int PrivacyExclusions = (int)PrivacyFlags.None;
        /// <summary>
        /// or datatype (key) = data
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new(DataType);
            if (!string.IsNullOrEmpty(DataKey))
            {
                _ = sb.Append(" (");
                _ = sb.Append(DataKey);
                _ = sb.Append(" )");
            }
            _ = sb.Append(" = ");
            _ = sb.Append(string.IsNullOrEmpty(DataText) ?
                DataFloat.ToString() :
                DataText);
            return sb.ToString();
        }
    }

    /// <summary>
    /// Vital sign item
    /// </summary>
    public class VitalSign : ICloneable
    {
        /// <summary>
        /// id in database
        /// </summary>
        public int VitalSignID = int.MinValue;
        /// <summary>
        /// id of patient
        /// </summary>
        public int PatientID = int.MinValue;
        /// <summary>
        /// when vital sign taken
        /// </summary>
        public DateTime DateOfVitals = DateTime.MinValue;
        /// <summary>
        /// when entered into database
        /// </summary>
        public DateTime DateEntered = DateTime.MinValue;
        /// <summary>
        /// id of user who entered data
        /// </summary>
        public int WhoEnteredID = int.MinValue;
        /// <summary>
        /// derived from Users
        /// </summary>
        public string WhoEntered = string.Empty;
        /// <summary>
        /// positive id of X-Out entry if x'd out, or int.MinValue if not
        /// </summary>
        public int XOutID = int.MinValue;
        /// <summary>
        /// identification of what vital sign this is
        /// </summary>
        public int VitalsNameID = int.MinValue;
        /// <summary>
        /// NOTICE: save numeric value in METRIC units even if text 
        /// value is in English units!!!  That way metric and english
        /// values can be compared.
        /// Float value in SQL Server is double in .NET
        /// Use double.MinValue for null
        /// </summary>
        public double FloatValue = double.MinValue;
        /// <summary>
        /// Text value should always be assigned - this is what 
        /// shows in displays; should include units if required
        /// as to differentiate inches from meters
        /// </summary>
        public string TextValue = string.Empty;
        /// <summary>
        /// optional comment
        /// </summary>
        public string Comment = string.Empty;
        /// <summary>
        /// serial number to associate it with other vitals in a single entry, 
        /// e.g. systolic bp and diastolic bp and pulse
        /// </summary>
        public int PanelSerialNum = int.MinValue;
        /// <summary>
        /// optionsl modifiers such as oral or rectal temp, standing or 
        /// sitting blood pressure etc
        /// </summary>
        public VitalsModifiers ModifierFlags = VitalsModifiers.None; //0=none
        /// <summary>
        /// database timestamp of last record modification
        /// </summary>
        public long TimeStamp = long.MinValue;
        /// <summary>
        /// vital sign paired with this one, as in systolic and
        /// diastolic blood pressures.  By convention we show the 
        /// systolic bp in visual controls but its ToString() method
        /// includes the associated diastolic vs's value
        /// </summary>
        public VitalSign? AssociatedVS = null;
        /// <summary>
        /// returns the text value and any modifiers
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            if (VitalsNameID == (int)VitalsNameIDs.BPSystolic)
            {
                _ = sb.Append("");
            }
            //special case for blood pressure
            if ((VitalsNameID == (int)VitalsNameIDs.BPSystolic) &&
                (AssociatedVS != null) &&
                (AssociatedVS.VitalsNameID == (int)VitalsNameIDs.BPDiastolic))
            {
                _ = sb.Append(TextValue.Trim());
                _ = sb.Append('/');
                _ = sb.Append(AssociatedVS.TextValue.Trim());
            }
            //prefer NOT to call ToString() from diastolic - systolic preferred
            else if ((VitalsNameID == (int)VitalsNameIDs.BPDiastolic) &&
                (AssociatedVS != null) &&
                (AssociatedVS.VitalsNameID == (int)VitalsNameIDs.BPSystolic))
            {
                _ = sb.Append(AssociatedVS.TextValue.Trim());
                _ = sb.Append('/');
                _ = sb.Append(TextValue.Trim());
            }
            //otherwise just show text value
            else
            {
                _ = sb.Append(TextValue.Trim());

                /* redundant:
                if (VitalsNameID == (int)VitalsNameIDs.HeightInches)
                {
                    sb.Append(" in ");
                }
                else if (VitalsNameID == (int)VitalsNameIDs.HeightMeters)
                {
                    sb.Append(" M ");
                }
                else if (VitalsNameID == (int)VitalsNameIDs.WaistCm)
                {
                    sb.Append(" cm ");
                }
                else if (VitalsNameID == (int)VitalsNameIDs.WaistInches)
                {
                    sb.Append(" in ");
                }
                else if (VitalsNameID == (int)VitalsNameIDs.WeightKilograms)
                {
                    sb.Append(" kg ");
                }
                else if (VitalsNameID == (int)VitalsNameIDs.WeightLbs)
                {
                    sb.Append(" lb ");
                }
                 */
            }
            _ = sb.Append(' ');
            if (ModifierFlags != VitalsModifiers.None)
            {
                _ = sb.Append(ModifierFlags.ToString());
            }
            return sb.ToString();
        }

        ///////////// <summary>
        ///////////// show properties of vital sign in short one line format
        ///////////// </summary>
        ///////////// <param name="pieceRef">ref to program piece</param>
        ///////////// <returns></returns>

        //////////public string ShowDetailsShort(IProgramPiece pieceRef)
        //////////{
        //////////    StringBuilder sb = new StringBuilder();
        //////////    sb.Append("(");
        //////////    sb.Append(FloatValue.ToString());
        //////////    sb.Append(") entered by ");
        //////////    sb.Append(WhoEntered.Trim());
        //////////    sb.Append(" on ");
        //////////    sb.Append(DateEntered.ToString());
        //////////    //if x'd out....
        //////////    if (XOutID != int.MinValue)
        //////////    {
        //////////        //get xout details
        //////////        //info of xout
        //////////        XOutInfo xi = null;
        //////////        try
        //////////        {
        //////////            if (pieceRef != null)
        //////////            {
        //////////                xi = pieceRef.MM3Data.ProgramData.SelectXOutInfo(
        //////////                    XOutID);
        //////////            }
        //////////        }
        //////////        catch
        //////////        {
        //////////            //go on, just cant give details of xout
        //////////        }
        //////////        if (xi == null)
        //////////        {
        //////////            sb.Append(" < X'd out, but couldn't find x-out details. >");
        //////////        }
        //////////        else
        //////////        {
        //////////            //check for integrity of xout info
        //////////            if (xi.LinkID != VitalSignID)
        //////////            {
        //////////                sb.Append(" < X'd out but details of x-out corrupted. >");
        //////////            }
        //////////            else
        //////////            {
        //////////                sb.Append(" < X'd out by ");
        //////////                sb.Append(xi.WhoXdOut);
        //////////                sb.Append(" on ");
        //////////                sb.Append(xi.WhenXdOut.ToShortDateString());
        //////////                sb.Append(" for ");
        //////////                sb.Append(xi.WhyXdOut);
        //////////                sb.Append(" >");
        //////////            }
        //////////        }
        //////////    }//from if x'd out
        //////////    sb.Append("{pnl id=");
        //////////    sb.Append(PanelSerialNum.ToString());
        //////////    sb.Append("; id=");
        //////////    sb.Append(VitalSignID.ToString());
        //////////    sb.Append("; ts=");
        //////////    sb.Append(TimeStamp != long.MinValue ? TimeStamp.ToString() : "null");
        //////////    sb.Append("}");
        //////////    return sb.ToString();
        //////////}

        /// <summary>
        /// creates a (shallow) clone of the vital sign
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    /// <summary>
    /// a vital sign associated with its VitalsName
    /// </summary>
    public class VitalSignAndName
    {
        /// <summary>
        /// the vital sign
        /// </summary>
        public VitalSign? Value = null;
        /// <summary>
        /// index for linking vital sign in text with coded sections
        /// of data transfer such as HL7 CCDA documents
        /// </summary>
        public int Index = int.MinValue;
        /// <summary>
        /// name and information about the type of vital sign this is
        /// </summary>
        public VitalsName? AssociatedVitalsName = null;
        /// <summary>
        /// name of vital sign and its value
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            if(AssociatedVitalsName != null)
                _ = sb.Append(AssociatedVitalsName.ShortName);
            _ = sb.Append(": ");
            _ = sb.Append((Value == null) ? "" : Value.ToString());
            return sb.ToString();
        }
    }

    /// <summary>
    /// name of vital sign item
    /// This data table is locked:  
    /// It is read from database but should NEVER  be inserted
    /// or updated by the program.
    /// </summary>
    public class VitalsName
    {
        /// <summary>
        /// id of entry in table
        /// </summary>
        public int VitalsNameID = int.MinValue;
        /// <summary>
        /// brief name up to 10 char
        /// </summary>
        public string ShortName = string.Empty;
        /// <summary>
        /// longer name up to 50 char
        /// </summary>
        public string LongName = string.Empty;
        /// <summary>
        /// units string up to 10 char
        /// </summary>
        public string Units = string.Empty;
        /// <summary>
        /// relative position of this item in a panel of vital sign items
        /// </summary>
        public int PositionInPanel = int.MinValue;
        /// <summary>
        /// set to false if not to be used any more
        /// </summary>
        public bool IsActive = true;
        /// <summary>
        /// LOINC code or empty string if none
        /// </summary>
        public string LOINC = string.Empty;
    }

    /// <summary>
    /// patient's directives about disclosure of protected
    /// health information (PHI)
    /// </summary>
    public class PatientDirective
    {
        /// <summary>
        /// id
        /// </summary>
        public int PatientDirectiveID = int.MinValue;
        /// <summary>
        /// id of patient
        /// </summary>
        public int PatientID = int.MinValue;
        /// <summary>
        /// the patient's directives
        /// </summary>
        public string Text = string.Empty;
        /// <summary>
        /// when entered or dateTime.MinValue for not entered yet
        /// </summary>
        public DateTime WhenEntered = DateTime.MinValue;
        /// <summary>
        /// id of user who entered data
        /// </summary>
        public int WhoEnteredID = int.MinValue;
        /// <summary>
        /// deriveed from Users
        /// </summary>
        public string WhoEntered = string.Empty;
        /// <summary>
        /// id of XOut entry, or int.minValue if not x'd out
        /// </summary>
        public int XOutID = int.MinValue;
        ///////////// <summary>
        ///////////// show details about this directive in short
        ///////////// one line format
        ///////////// </summary>
        ///////////// <param name="piecRef">reference to program piece 
        ///////////// required to get xout info</param>
        ///////////// <returns></returns>
        //////////public string ShowDetailsShort(IProgramComponent piecRef)
        //////////{
        //////////    StringBuilder sb = new StringBuilder();
        //////////    sb.Append("Pt directive saved ");
        //////////    sb.Append(WhenEntered.ToShortDateString());
        //////////    sb.Append(" by ");
        //////////    sb.Append(WhoEntered);
        //////////    //show xout info if was x'd out
        //////////    if (XOutID > -1)
        //////////    {
        //////////        XOutInfo xi = piecRef.MM4Data.ProgramData.SelectXOutInfo(XOutID);
        //////////        if (xi != null)
        //////////        {
        //////////            sb.Append(" ");
        //////////            sb.Append(xi.ShowDetails(PatientDirectiveID,
        //////////                int.MinValue, true));
        //////////        }
        //////////        else
        //////////        {
        //////////            sb.Append(" X'd out but couldn't find xout info");
        //////////        }
        //////////    }
        //////////    sb.Append(" {id=");
        //////////    sb.Append(PatientDirectiveID.ToString());
        //////////    sb.Append(", PatientID=");
        //////////    sb.Append(PatientID.ToString());
        //////////    sb.Append("}");
        //////////    return sb.ToString();
        //////////}
    }

    /// <summary>
    /// category of DISCLOSURE of protected health information (PHI)
    /// as defined by HIPAA (eg TPO, pat req, mandatory
    /// </summary>
    public class PHIDisclosureCat
    {
        /// <summary>
        /// category id
        /// </summary>
        public int PHIDisclosureCatID = int.MinValue;
        /// <summary>
        /// short name, up to 10 chars
        /// </summary>
        public string ShortName = "";
        /// <summary>
        /// long name, up to 50 chars
        /// </summary>
        public string LongName = "";
        /// <summary>
        /// returns long name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return LongName;
        }


        // static instances

        /// <summary>
        /// get disclosure cat with given id
        /// </summary>
        /// <param name="phiDisclosureCatID"></param>
        /// <returns></returns>
        public static PHIDisclosureCat Get(int phiDisclosureCatID)
        {
            if (phiDisclosureCatID == 1)
                return PHIDisclosureCat.TPO;
            else if (phiDisclosureCatID == 2)
                return PHIDisclosureCat.PatRequest;
            else if (phiDisclosureCatID == 3)
                return PHIDisclosureCat.Mandated;
            else
                return new PHIDisclosureCat();
        }

        /// <summary>
        /// array of all defined PHI Disclosure Categories
        /// </summary>
        /// <returns></returns>
        public static PHIDisclosureCat[] GetCategories()
        {
            return
            [
                PHIDisclosureCat.TPO,
                PHIDisclosureCat.PatRequest,
                PHIDisclosureCat.Mandated
            ];
        }

        /// <summary>
        /// for treatment, payment or operations
        /// </summary>
        /// <returns></returns>
        public static PHIDisclosureCat TPO
        {
            get
            {
                PHIDisclosureCat result = new()
                {
                    PHIDisclosureCatID = 1,
                    ShortName = "T.P.O.",
                    LongName = "Treatment, Payment or Operations"
                };
                return result;
            }
        }
        /// <summary>
        /// by patient's request
        /// </summary>
        /// <returns></returns>
        public static PHIDisclosureCat PatRequest
        {
            get
            {
                PHIDisclosureCat result = new()
                {
                    PHIDisclosureCatID = 2,
                    ShortName = "PatRequest",
                    LongName = "Patient's request"
                };
                return result;
            }
        }
        /// <summary>
        /// mandatory
        /// </summary>
        /// <returns></returns>
        public static PHIDisclosureCat Mandated
        {
            get
            {
                PHIDisclosureCat result = new()
                {
                    PHIDisclosureCatID = 3,
                    ShortName = "Mandated",
                    LongName = "Legally mandated"
                };
                return result;
            }
        }
    }

    /// <summary>
    /// log of an occasion of printing patient data
    /// </summary>
    public class LogOfReport
    {
        /// <summary>
        /// create empty log event
        /// </summary>
        public LogOfReport()
        {
            //nothing
        }
        /// <summary>
        /// log of an occasion of printing patient data
        /// </summary>
        /// <param name="patientID"></param>
        /// <param name="whoPrintedID"></param>
        /// <param name="comment"></param>
        /// <param name="disclosureCat"></param>
        /// <param name="privacyExclusions"></param>
        /// <param name="category"></param>
        public LogOfReport(
            int patientID,
            int whoPrintedID,
            string comment,
            PHIDisclosureCat disclosureCat,
            PrivacyFlags privacyExclusions,
            ReportCategory category)
        {
            //initialize
            this.PatientID = patientID;
            this.WhoPrintedID = whoPrintedID;
            this.Comment = comment;
            this.DisclosureCat = disclosureCat;
            this.PrivacyExclusions = privacyExclusions;
            this.Category = category;

        }
        /// <summary>
        /// id of this event in database table
        /// </summary>
        public int LogOfReportID = int.MinValue;
        /// <summary>
        /// id of patient whose data was reported
        /// </summary>
        public int PatientID = int.MinValue;
        /// <summary>
        /// derived from Patients table
        /// </summary>
        public string Patient = string.Empty;
        /// <summary>
        /// id of user who printed
        /// </summary>
        public int WhoPrintedID = int.MinValue;
        /// <summary>
        /// derived from Users table
        /// </summary>
        public string WhoPrinted = string.Empty;
        /// <summary>
        /// when was printed
        /// </summary>
        public DateTime WhenPrinted = DateTime.MinValue;
        /// <summary>
        /// why printed and other comments, up to 500 char
        /// </summary>
        public string Comment = string.Empty;
        /// <summary>
        /// category of protected health information disclosure,
        /// per HIPAA, eg mandated, pt request or TPO
        /// </summary>
        public PHIDisclosureCat? DisclosureCat = null;
        /// <summary>
        /// flags of privacy exclusions specified in report
        /// </summary>
        public PrivacyFlags PrivacyExclusions = PrivacyFlags.None;
        /// <summary>
        /// category, eg chart note, chart report, immunizations, etc
        /// </summary>
        public ReportCategory Category = ReportCategory.NotSpecified;
    }

    /// <summary>
    /// a saying that can be used for login messages
    /// </summary>
    public class Saying
    {
        /// <summary>
        /// id in database
        /// </summary>
        public int SayingID = int.MinValue;
        /// <summary>
        /// text of the saying
        /// </summary>
        public string Text = string.Empty;
        /// <summary>
        /// where saying was found or located
        /// </summary>
        public string Source = string.Empty;
        /// <summary>
        /// originator of saying
        /// </summary>
        public string Author = string.Empty;
        /// <summary>
        /// bit flags representing categories of sayings
        /// </summary>
        public SayingGroups GroupFlags = SayingGroups.unspecified;
        /// <summary>
        /// returns true if saying is in given group
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public bool IsInGroup(SayingGroups group)
        {
            return (group & GroupFlags) == group;
        }
        /// <summary>
        /// set SayingsGroup flag for given group and value
        /// </summary>
        /// <param name="group"></param>
        /// <param name="value"></param>
        public void SetInGroup(SayingGroups group, bool value)
        {
            //if setting true, set bit by bitwise or
            if (value)
            {
                GroupFlags |= group;
            }
            else
            //if false, check to see current value first
            {
                if (IsInGroup(group))
                {
                    //toggle it to off with exclusive or
                    GroupFlags ^= group;
                }
            }
        }
    }

    /// <summary>
    /// an item of data from the Miscellaneous table in database
    /// </summary>
    public class MiscellaneousItem
    {
        /// <summary>
        /// id for this row in database table
        /// </summary>
        public int MiscellaneousID = int.MinValue;
        /// <summary>
        /// a globally unique string identifying the company who defined
        /// this item.  see http://eastridges.com/m for reserved names,
        /// under the section  on Customizing
        /// (e.g. MM3 and WVE and SDE and  are reserved names)
        /// 50 character max length
        /// </summary>
        public string? CompanyUID = null;
        /// <summary>
        /// first of four integer ID's;  
        /// The table is indexed on CompanyUID, ID1, ID2
        /// default is -1
        /// Note choices are in MM3Common.MM3MiscItemID1 enum if CompanyUID = MM3
        /// (defined in MM3Enums)
        /// </summary>
        public int ID1 = -1;
        /// <summary>
        /// second of four ID's;
        /// default is -1
        /// </summary>
        public int ID2 = -1;
        /// <summary>
        /// third of four ID's; this one is not indexed
        /// default is -1
        /// </summary>
        public int ID3 = -1;
        /// <summary>
        /// fourth of four ID's; this one is not indexed
        /// default is -1
        /// </summary>
        public int ID4 = -1;
        /// <summary>
        /// string data of unlimited length
        /// </summary>
        public string Text = string.Empty;
    }


    /// <summary>
    /// list of DebugLogEvents for debugging timing etc
    /// </summary>
    public class DebugLogList
    {
        /// <summary>
        /// structures to aggregate in a DebugLogList to help debug with timings
        /// (see alternative object DebugLog to log to console or to Windows Event Log)
        /// </summary>
        public struct DebugLogEvent
        {
            /// <summary>
            /// name of event
            /// </summary>
            public string EventName;
            /// <summary>
            /// time of event start
            /// </summary>
            public DateTime EventStartTime;
            /// <summary>
            /// optional finish time
            /// </summary>
            public DateTime EventFinishTime;
            /// <summary>
            /// optional comment
            /// </summary>
            public string Comment;

            #region constructors
            /// <summary>
            /// debug structure defaulting to EventStartTime of DateTime now
            /// </summary>
            /// <param name="eventName"></param>
            public DebugLogEvent(string eventName)
            {
                EventName = eventName;
                EventStartTime = DateTime.Now;
                EventFinishTime = DateTime.MinValue;
                Comment = string.Empty;
            }
            /// <summary>
            /// debug structure with name and starttime only
            /// </summary>
            /// <param name="eventName"></param>
            /// <param name="eventStartTime"></param>
            public DebugLogEvent(string eventName, DateTime eventStartTime)
            {
                EventName = eventName;
                EventStartTime = eventStartTime;
                EventFinishTime = DateTime.MinValue;
                Comment = string.Empty;
            }
            /// <summary>
            /// debug structure with name and starttime,finishtime and comment
            /// </summary>
            /// <param name="eventName"></param>
            /// <param name="eventStartTime"></param>
            /// <param name="comment"></param>
            /// <param name="eventFinishTime"></param>
            public DebugLogEvent(string eventName,
                DateTime eventStartTime,
                DateTime eventFinishTime,
                string comment)
            {
                EventName = eventName;
                EventStartTime = eventStartTime;
                EventFinishTime = eventFinishTime;
                Comment = comment;
            }
            #endregion constructors
        }



        /// <summary>
        /// the list of events
        /// </summary>
        public List<DebugLogEvent> EventList;

        #region constructor
        /// <summary>
        /// start a new debug log list
        /// </summary>
        public DebugLogList()
        {
            EventList = [new DebugLogEvent("init", DateTime.Now)];
        }

        #endregion constructor
        /// <summary>
        /// print the list of events
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            DateTime lastEventTime = DateTime.MinValue;
            for (int i = 0; i < EventList.Count; i++)
            {
                _ = sb.Append(EventList[i].EventName);
                _ = sb.Append("@ ");
                _ = sb.Append(EventList[i].EventStartTime.ToLongTimeString());
                _ = sb.Append("   (");
                if (lastEventTime != DateTime.MinValue)
                {
                    _ = sb.AppendFormat("{0:0}", (EventList[i].EventStartTime - lastEventTime).TotalMilliseconds);
                }
                lastEventTime = EventList[i].EventStartTime; //reset
                _ = sb.Append(") ");
                if (EventList[i].EventFinishTime != DateTime.MinValue)
                {
                    _ = sb.Append(" to ");
                    _ = sb.Append(EventList[i].EventFinishTime.ToLongTimeString());
                    _ = sb.Append('(');
                    if (lastEventTime != DateTime.MinValue)
                    {
                        _ = sb.Append((EventList[i].EventFinishTime - lastEventTime).TotalMilliseconds);
                    }
                    lastEventTime = EventList[i].EventFinishTime; //reset
                    _ = sb.Append(") ");
                }
                if (!string.IsNullOrWhiteSpace(EventList[i].Comment))
                {
                    _ = sb.Append('[');
                    _ = sb.Append(EventList[i].Comment);
                    _ = sb.Append("] ");
                }
                _ = sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }
    }
    // not supposed to show public static members- might need to move it to the program that logs it
//////////    /// <summary>
//////////    /// a debugging tool to write timed notes to console
//////////    /// (see alternative DebugLogList that just holds list of events)
//////////    /// </summary>
//////////    public static class DebugLog
//////////    {
//////////        private const bool V_false = false;

//////////        /// <summary>
//////////        /// write debug log entries to console
//////////        /// </summary>
//////////        public static bool WriteToConsole = V_false;
//////////        /// <summary>
//////////        /// write debug log entries to debug file
//////////        /// </summary>
//////////        public static bool WriteToMM3DebugFile = true;
//////////        /// <summary>
//////////        /// write debug log entries to event log
//////////        /// </summary>
//////////        public static bool WriteToEventLog = V_false;


//////////        private static DateTime lastLogTime = DateTime.MinValue;
//////////        /// <summary>
//////////        /// log message to console
//////////        /// </summary>
//////////        /// <param name="message"></param>
//////////        public static void LogEvent(string message)
//////////        {
//////////            LogEvent(message, V_false);
//////////        }
//////////        /// <summary>
//////////        /// keep track if already advised of log error
//////////        /// </summary>
//////////        public static bool AlreadyAdvisedOfEventLogError = V_false;
//////////        /// <summary>
//////////        /// log message to console
//////////        /// </summary>
//////////        /// <param name="message"></param>
//////////        /// <param name="resetLastLogTime">if true, starts over with time 
//////////        /// interval of zero; otherwise reports time span since last time
//////////        /// LogEvent() was called.</param>
//////////        public static void LogEvent(string message, bool resetLastLogTime)
//////////        {
//////////            StringBuilder sb = new();
//////////            DateTime logTime = DateTime.Now;
//////////            //time since last log
//////////            TimeSpan ts;
//////////            //if directed to reset log time, OR
//////////            // if this is first event since class was created...
//////////            if ((resetLastLogTime) ||
//////////                (lastLogTime == DateTime.MinValue))
//////////            {
//////////                ts = TimeSpan.FromTicks(0);
//////////                //line break
//////////                sb.Append(Environment.NewLine);
//////////            }
//////////            else
//////////            {
//////////                ts = logTime - lastLogTime;
//////////            }
//////////            lastLogTime = logTime;
//////////            //create message
//////////            sb.Append('(');
//////////            sb.Append(ts.TotalMilliseconds);
//////////            sb.Append(" ms.) ");
//////////            sb.Append(message);
//////////            //write it
//////////            if (WriteToConsole)
//////////            {
//////////                Console.WriteLine(sb.ToString());
//////////            }
//////////            if (WriteToMM3DebugFile)
//////////            {
//////////                try
//////////                {
//////////                    // size  log can reach before being truncated
//////////                    int EventLogFileSize = 30000; //32767 capacity of text box 
//////////                    StringBuilder sbDebugFileName = new();
//////////                    sbDebugFileName.Append(System.Environment.GetFolderPath(
//////////                        Environment.SpecialFolder.LocalApplicationData));
//////////                    sbDebugFileName.Append(System.IO.Path.DirectorySeparatorChar);
//////////                    sbDebugFileName.Append(@"EastRidges\MM3\");
//////////                    sbDebugFileName.Append("MM3Debug.txt");
//////////                    //;System.Windows.Forms.MessageBox.Show(sbDebugFileName.ToString());
//////////                    //users/wesley/appdata/local/eastridges/mm3
//////////                    using (StreamWriter sw = new(sbDebugFileName.ToString(),
//////////                        true)) //true to append
//////////                    {
//////////                        sw.WriteLine();
//////////                        sw.WriteLine(DateTime.Now.ToString());
//////////                        sw.WriteLine(sb.ToString());
//////////                        sw.WriteLine();
//////////                        sw.WriteLine();
//////////                    }
//////////                    //now truncate if necessary
//////////                    // (reread file info)
//////////                    FileInfo fi = new(sbDebugFileName.ToString());
//////////                    int excessLength;
//////////                    char[] tempBuffer;
//////////                    if ((fi.Exists) &&
//////////                    (fi.Length > EventLogFileSize))
//////////                    {
//////////                        FileInfo fiNew = new(sbDebugFileName.ToString() + ".new");
//////////                        excessLength = (int)fi.Length - EventLogFileSize;
//////////                        //copy file to new file, skipping the excess
//////////                        using (StreamReader sr = new(fi.OpenRead()))
//////////                        {
//////////                            //set pointer in old log file
//////////                            tempBuffer = new char[excessLength];
//////////                            sr.Read(tempBuffer, 0, excessLength);
//////////                            //and read the rest of it into new file
//////////                            using (StreamWriter sw = fiNew.AppendText())
//////////                            {
//////////                                while (sr.Peek() != -1)
//////////                                {
//////////                                    sw.WriteLine(sr.ReadLine());
//////////                                }
//////////                            }
//////////                        }//from using stream reader 
//////////                        //rename new file to original name
//////////                        fi.Delete();
//////////                        fiNew.MoveTo(sbDebugFileName.ToString());
//////////                    }//from if file too big
//////////                }
//////////                catch (Exception er)
//////////                {
//////////                    System.Windows.MessageBox.Show(er.ToString() +
////////////                    System.Windows.Forms.MessageBox.Show(er.ToString() +
//////////                        Environment.NewLine +
//////////                        Environment.NewLine +
//////////                        "Message was:  " +
//////////                        Environment.NewLine +
//////////                        sb.ToString());
//////////                }
//////////            }//from if write to MM3Debug file
//////////            if (WriteToEventLog)
//////////            {
//////////                string sSource;
//////////                string sLog;
//////////                string sEvent;

//////////                sSource = "MountainMeadow3";
//////////                sLog = "Application";
//////////                sEvent = sb.ToString();

//////////                //this fails on some machines if program is not running as an administrator!
//////////                try
//////////                {
//////////                    if (!System.Diagnostics.EventLog.SourceExists(sSource))
//////////                        System.Diagnostics.EventLog.CreateEventSource(sSource, sLog);

//////////                    System.Diagnostics.EventLog.WriteEntry(sSource, sEvent);
//////////                    // or could say:
//////////                    //;System.Diagnostics.EventLog.WriteEntry(sSource, sEvent,
//////////                    //;    System.Diagnostics.EventLogEntryType.Warning, 234);
//////////                }
//////////                catch (Exception er)
//////////                {
//////////                    if (!AlreadyAdvisedOfEventLogError)
//////////                    {
//////////                        System.Windows.MessageBox.Show("For your information, " +
//////////                            "tried to log event and cannot.  The program will continue, " +
//////////                            "but if you are debugging and want to log events, you " +
//////////                            "might try running the program 'as an administrator'." +
//////////                            Environment.NewLine +
//////////                            Environment.NewLine +
//////////                            er.ToString());
//////////                    }
//////////                }
//////////            }//from if write to event log
//////////        }

//////////    }
    /// <summary>
    /// links to a family member's name or person object
    /// </summary>
    public class FamilyMember
    {
        /// <summary>
        /// id
        /// </summary>
        public int FamilyMemberID = int.MinValue;
        /// <summary>
        /// reference id
        /// </summary>
        public int ReferenceID = int.MinValue;
        /// <summary>
        /// relative pat id
        /// </summary>
        public int RelativePatientID = int.MinValue;
        /// <summary>
        /// relative misc name id
        /// </summary>
        public int RelativeMiscNameID = int.MinValue;
        /// <summary>
        /// is a patient
        /// </summary>
        public bool IsAPatient = false;
    }

    /// <summary>
    /// a record of a database update script
    /// </summary>
    public class DatabaseUpdate
    {
        /// <summary>
        /// id of this database record
        /// </summary>
        public int DatabaseUpdateID = int.MinValue;
        /// <summary>
        /// sequential number of scripts applied to database
        /// </summary>
        public int ScriptNumber = int.MinValue;
        /// <summary>
        /// script has been applied to database?
        /// </summary>
        public bool WasApplied = false;
        /// <summary>
        /// typically holds the query string
        /// </summary>
        public string Comments = string.Empty;
        /// <summary>
        /// when script was applied to database, or 
        /// datetime.minvalue if not applicable
        /// </summary>
        public DateTime WhenApplied = DateTime.MinValue;
    }

    /// <summary>
    /// holds information for the message of the day to 
    /// show on login form, etc
    /// </summary>
    public class MessageOfTheDay
    {
        /// <summary>
        /// message to be broadcast if UseSaying is false
        /// </summary>
        public string BroadcastMessage = string.Empty;
        /// <summary>
        /// selection from the Sayings table
        /// </summary>
        public string CurrentSaying = string.Empty;
        /// <summary>
        /// author for the saying
        /// </summary>
        public string CurrentSayingAuthor = string.Empty;
        /// <summary>
        /// if true, show the Current Saying instead of BroadcastMessage
        /// </summary>
        public bool UseSaying = true;
        /// <summary>
        /// groups to include
        /// </summary>
        public SayingGroups GroupsToInclude = SayingGroups.unspecified;
        /// <summary>
        /// groups to exclude 
        /// </summary>
        public SayingGroups GroupsToExclude = SayingGroups.Blackballed;
        /// <summary>
        /// date of the day of the message
        /// </summary>
        public DateTime DateOfTheDay = DateTime.MinValue;
        /// <summary>
        /// serialize to vertical-bar-delimited string
        /// </summary>
        /// <returns></returns>
        public string Serialize()
        {
            char delimiter = '|';
            StringBuilder sb = new();
            //can't include vertical bars
            _ = sb.Append(BroadcastMessage.Replace(delimiter, ':'));
            _ = sb.Append(delimiter);
            _ = sb.Append(CurrentSaying.Replace(delimiter, ':'));
            _ = sb.Append(delimiter);
            _ = sb.Append(UseSaying ? "true" : "false");
            _ = sb.Append(delimiter);
            _ = sb.Append((int)GroupsToInclude);
            _ = sb.Append(delimiter);
            _ = sb.Append((int)GroupsToExclude);
            _ = sb.Append(delimiter);
            _ = sb.Append(CurrentSayingAuthor.Replace(delimiter, ':'));
            _ = sb.Append(delimiter);
            _ = sb.Append(DateOfTheDay.ToShortDateString().Replace(delimiter, ':'));
            return sb.ToString();
        }
        /// <summary>
        /// deserialize from bar-delimited string or null if error
        /// </summary>
        /// <param name="serialized"></param>
        /// <returns></returns>
        public static MessageOfTheDay Deserialize(string serialized)
        {
            const int numParts = 7; //expected number of parts
            string[] parts = serialized.Split(['|']);
            MessageOfTheDay? result;
            if (parts.Length > numParts - 1)
            {
                result = new MessageOfTheDay
                {
                    BroadcastMessage = parts[0],
                    CurrentSaying = parts[1]
                };
                _ = bool.TryParse(parts[2], out result.UseSaying); //_ is temp variable that messages suggested
                if ((int.TryParse(parts[3], out int includeInt)) &&
                    (int.TryParse(parts[4], out int excludeInt)))
                {
                    result.GroupsToInclude = (SayingGroups)includeInt;
                    result.GroupsToExclude = (SayingGroups)excludeInt;
                }
                result.CurrentSayingAuthor = parts[5];
                _ = DateTime.TryParse(parts[6], out result.DateOfTheDay);
            }
            else
            {
                throw new Exception("Tried to read MessageOfTheDay information but " +
                    "it didn't have expected format.");
            }
            return result;
        }
    }

    /// <summary>
    /// database entry referencing documents such
    /// as scans, pictures, and other files
    /// </summary>
    public class DocumentReference
    {
        /// <summary>
        /// database id of this entry
        /// </summary>
        public int DocumentReferenceID = int.MinValue;
        /// <summary>
        /// patient whose chart this document goes in
        /// </summary>
        public int PatientID = int.MinValue;
        /// <summary>
        /// first category of documents, eg. consults, legal, hospital, 
        /// insurance, correspondance, results, pictures, handouts.
        /// if all CatFlags are 0 it's in no category
        /// </summary>
        public int CatFlag1 = 0;
        /// <summary>
        /// sub category of first category eg. cardiol, ophthalmology
        /// </summary>
        public int CatFlag2 = 0;
        /// <summary>
        /// subcategory of second category if needed
        /// </summary>
        public int CatFlag3 = 0;
        /// <summary>
        /// the folder where file is located, so fullname
        /// is DocumentsFolderName and DocuementFileName.
        /// Notice this should be derived from Global settings and is
        /// not saved in DocumentReferences table
        /// </summary>
        public string DocumentsFolderName = string.Empty;
        /// <summary>
        /// filename (without path) of the document in the Documents folder
        /// Use GetFullFileName() to get full path if DocumentsFolderName has
        /// been set.
        /// </summary>
        public string DocumentFileName = string.Empty;
        /// <summary>
        /// original filename of the document before import into
        /// the Documents folder
        /// </summary>
        public string OriginalFileName = string.Empty;
        /// <summary>
        /// date of the document i.e. letter
        /// </summary>
        public DateTime DocumentDate = DateTime.MinValue;
        /// <summary>
        /// title for user to see when browsing documents
        /// </summary>
        public string Title = string.Empty;
        /// <summary>
        /// optional comments about the document
        /// </summary>
        public string Comments = string.Empty;
        /// <summary>
        /// user who entered or last edited the entry
        /// </summary>
        public int WhoEnteredID = int.MinValue;
        /// <summary>
        /// id for user who entered or last edited entry
        /// </summary>
        public string WhoEntered = string.Empty;
        /// <summary>
        /// when entered or last edited
        /// </summary>
        public DateTime WhenEntered = DateTime.MinValue;
        /// <summary>
        /// link to xout information if x-d out or edited (i.e.
        /// replaced with new DocuementReference)
        /// </summary>
        public int XOutID = int.MinValue;
        /// <summary>
        /// the DocumentFileName, concatenated with the folder name 
        /// if the DocuementsFolderName has been specified
        /// </summary>
        /// <returns></returns>
        public string GetFullFileName()
        {
            string? result;
            if (DocumentsFolderName != string.Empty)
            {
                result = Path.Combine(DocumentsFolderName, DocumentFileName);
            }
            else
            {
                result = DocumentFileName;
            }
            return result;
        }
        /// <summary>
        /// show the title, date
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            _ = sb.Append(Title);
            _ = sb.Append("  ");
            _ = sb.Append(DocumentDate.ToShortDateString());
            return sb.ToString();
        }
        ///////////// <summary>
        ///////////// show details about this reference
        ///////////// </summary>
        ///////////// <param name="mm3Data">ref to data object for 
        ///////////// obtaining xout info if needed</param>
        ///////////// <returns></returns>
        //////////public string GetDetails(IMM4Data mm3Data)
        //////////{
        //////////    StringBuilder sb = new StringBuilder();
        //////////    sb.Append("Title: ");
        //////////    sb.Append(Title);
        //////////    sb.Append(Environment.NewLine);
        //////////    sb.Append("Date: ");
        //////////    sb.Append(DocumentDate.ToShortDateString());
        //////////    sb.Append(Environment.NewLine);
        //////////    sb.Append("Comments: ");
        //////////    sb.Append(Comments);
        //////////    sb.Append(Environment.NewLine);
        //////////    sb.Append("Categories: ");
        //////////    sb.Append(CatFlag1.ToString());
        //////////    sb.Append("/");
        //////////    sb.Append(CatFlag2.ToString());
        //////////    sb.Append("/");
        //////////    sb.Append(CatFlag3.ToString());
        //////////    sb.Append(Environment.NewLine);
        //////////    sb.Append("FileName: ");
        //////////    sb.Append(GetFullFileName());
        //////////    sb.Append(Environment.NewLine);
        //////////    sb.Append("Original Name: ");
        //////////    sb.Append(OriginalFileName);
        //////////    sb.Append(Environment.NewLine);
        //////////    sb.Append("Saved by: ");
        //////////    sb.Append(WhoEntered);
        //////////    sb.Append(" on ");
        //////////    sb.Append(WhenEntered.ToString());
        //////////    sb.Append(Environment.NewLine);
        //////////    if (XOutID > int.MinValue)
        //////////    {
        //////////        sb.Append("X'd out ");
        //////////        if (mm3Data != null)
        //////////        {
        //////////            XOutInfo xi = mm3Data.ProgramData.SelectXOutInfo(XOutID);
        //////////            if (xi == null)
        //////////            {
        //////////                sb.Append("id =");
        //////////                sb.Append(XOutID);
        //////////                sb.Append(" but couldn't retrieve details about the x-out.");
        //////////            }
        //////////            else
        //////////            {
        //////////                sb.Append("by ");
        //////////                sb.Append(xi.WhoXdOut == string.Empty ?
        //////////                    xi.WhoXdOutID.ToString() :
        //////////                    xi.WhoXdOut);
        //////////                sb.Append(" on ");
        //////////                sb.Append(xi.WhenXdOut.ToString());
        //////////                sb.Append(" for ");
        //////////                sb.Append(xi.WhyXdOut);
        //////////            }
        //////////        }
        //////////        else
        //////////        {
        //////////            sb.Append("id=");
        //////////            sb.Append(XOutID.ToString());
        //////////        }
        //////////        sb.Append(Environment.NewLine);
        //////////    }//from if xoutID not null
        //////////    //
        //////////    return sb.ToString();
        //////////}
    }

    /// <summary>
    /// category of documents in the annex.  The tree of 
    /// categories is built in the DPPDocuments program piece,
    /// and category 0,0,0 is not in any category
    /// but just shows if user views "all"
    /// </summary>
    /// <remarks>
    /// make category for documents annex
    /// (categories are defined in DocumentsProgramPiece)
    /// </remarks>
    /// <param name="catLevel"></param>
    /// <param name="shortName"></param>
    /// <param name="longName"></param>
    /// <param name="catFlag1"></param>
    /// <param name="catFlag2"></param>
    /// <param name="catFlag3"></param>
    public class DocumentsAnnexCat(int catLevel,
        string shortName,
        string longName,
        int catFlag1,
        int catFlag2,
        int catFlag3)
    {
        /// <summary>
        /// must be 0-3
        /// 0=trunk(all docucuments), 1=first level(eg Consults, Results)
        /// 2= second level (eg. Ophhalmolgy); 3= third level (eg EyeExam)
        /// </summary>
        public int CategoryLevel = catLevel;
        /// <summary>
        /// name to show on category tree
        /// </summary>
        public string ShortName = shortName;
        /// <summary>
        /// more descriptive name
        /// </summary>
        public string LongName = longName;
        /// <summary>
        /// first order category flag i.e. binary flag of 
        /// value 1,2,4,8,16 etc
        /// </summary>
        public int CatFlag1 = catFlag1;
        /// <summary>
        /// second order category flag
        /// </summary>
        public int CatFlag2 = catFlag2;
        /// <summary>
        /// third order cagegory flag
        /// </summary>
        public int CatFlag3 = catFlag3;
        /// <summary>
        /// list of child categories which should be empty
        /// if this is a third order category
        /// </summary>
        public List<DocumentsAnnexCat> Children = [];
        /// <summary>
        /// parent category if any
        /// </summary>
        public DocumentsAnnexCat? Parent = null;

        /// <summary>
        /// add new child category and return the new child, or
        /// null if error.
        /// </summary>
        /// <param name="shortName"></param>
        /// <param name="longName"></param>
        /// <param name="newLevelCatFlag"></param>
        /// <returns></returns>
        public DocumentsAnnexCat AddChild(string shortName,
            string longName,
            int newLevelCatFlag)
        {
            DocumentsAnnexCat? result;
            if (this.CategoryLevel < 3)
            {

                result = new DocumentsAnnexCat(
                    this.CategoryLevel + 1,
                    shortName,
                    longName,
                    0, 0, 0)
                {
                    Parent = this
                };
                switch (this.CategoryLevel)
                {
                    case 0:
                        result.CatFlag1 = newLevelCatFlag;
                        break;
                    case 1:
                        result.CatFlag1 = this.CatFlag1;
                        result.CatFlag2 = newLevelCatFlag;
                        break;
                    case 2:
                        result.CatFlag1 = this.CatFlag1;
                        result.CatFlag2 = this.CatFlag2;
                        result.CatFlag3 = newLevelCatFlag;
                        break;
                    default:
                        //never gets here
                        throw new Exception("whoops, shouldn't ever get here.");
                }
                this.Children.Add(result);
            }//from if level < 3
            else
            {
                throw new Exception("Oops, programming error. Programmer tried to " +
                "enter more than three levels of categories for documents.");
            }
            return result;
        }
    }

    /// <summary>
    /// an order for a test
    /// </summary>
    public class TestOrder
    {
        /// <summary>
        /// id of database entry
        /// </summary>
        public int TestOrderID = int.MinValue;
        /// <summary>
        /// id of patient
        /// </summary>
        public int PatientID = int.MinValue;
        /// <summary>
        /// derived from Patients table
        /// </summary>
        public string Patient_DisplayName = string.Empty;
        /// <summary>
        /// when test ordered
        /// </summary>
        public DateTime WhenOrdered = DateTime.MinValue;
        /// <summary>
        /// id of ordering user
        /// </summary>
        public int WhoOrderedID = int.MinValue;
        /// <summary>
        /// derived from Users table
        /// </summary>
        public string WhoOrdered = string.Empty;
        /// <summary>
        /// id of XOuts table entry or int.MinValue if none
        /// </summary>
        public int XOutID = int.MinValue;
        /// <summary>
        /// ID of named test
        /// </summary>
        public int TestNameID = int.MinValue;
        /// <summary>
        /// derived from TestNames table
        /// </summary>
        public string NamedTestShortName = string.Empty;
        /// <summary>
        /// panel number if this test is part of a panel
        /// </summary>
        public int PanelSerialNum = int.MinValue;
        /// <summary>
        /// from database
        /// </summary>
        public long Timestamp = long.MinValue;
        /// <summary>
        /// optional comment
        /// </summary>
        public string Comment = string.Empty;
        /// <summary>
        /// id of source, where 1 is typically local 
        /// (this program, this installation)
        /// </summary>
        public int SourceID = int.MinValue;
        /// <summary>
        /// Derived from TestNames...
        /// category of named test or int.MinValue if none
        /// </summary>
        public int TestCatID = int.MinValue;
        /// <summary>
        /// category of misc test or int.MinValue if none
        /// </summary>
        public int MiscTestCatID = int.MinValue;
        /// <summary>
        /// descriptive name of this test
        /// </summary>
        public string TestName = string.Empty;
        /// <summary>
        /// LOINC code if any
        /// </summary>
        public string LOINC = string.Empty;
        /// <summary>
        /// SNOMED code if any
        /// </summary>
        public string SNOMED = string.Empty;
        /// <summary>
        /// CPT code if any
        /// </summary>
        public string CPT = string.Empty;
        /// <summary>
        /// ID used by the external source if any
        /// </summary>
        public string TestNameIDPerSource = string.Empty;
        /// <summary>
        /// status of order
        /// </summary>
        public TestOrderStatus Status = TestOrderStatus.Unspecified;
        /// <summary>
        /// id of user who last changed status
        /// </summary>
        public int WhoChangedStatusID = int.MinValue;
        /// <summary>
        /// derived from Users table
        /// </summary>
        public string WhoChangedStatusDisplayName = string.Empty;
        /// <summary>
        /// when status last changed
        /// </summary>
        public DateTime WhenStatusChanged = DateTime.MinValue;
        /// <summary>
        /// link to the Tests table containing result when and if
        /// this order is completed.
        /// </summary>
        public int TestIDOfResult = int.MinValue;
    }

    /// <summary>
    /// status of order for a test
    /// </summary>
    public enum TestOrderStatus : int
    {
        /// <summary>
        /// not specified
        /// </summary>
        Unspecified = 0,
        /// <summary>
        /// in process
        /// </summary>
        Pending,
        /// <summary>
        /// done
        /// </summary>
        Completed,
        /// <summary>
        /// was cancelled
        /// </summary>
        Cancelled,
        /// <summary>
        /// error condition
        /// </summary>
        Error
    }

    // not using Win forms cursor now;  Instead, use Mouse.OverrideCursor=Cursors.Wait, 
    // and then set OverrideCursor to null when finished (e.g use finally clause)
    ///////////// <summary>
    ///////////// simple object to save current cursor, show WaitCursor,
    ///////////// and restore prior cursor when disposed. 
    ///////////// By Francesco Balena 
    ///////////// </summary>
    //////////public class HourglassCursor : IDisposable
    //////////{
    //////////    //published in Practical Guidelines and Best Practices
    //////////    //Microsoft Press 2005
    //////////    private System.Windows.Forms.Cursor savedCursor;
    //////////    /// <summary>
    //////////    /// simple object to save current cursor, show WaitCursor,
    //////////    /// and restore prior cursor when disposed. 
    //////////    /// adapted from Francesco Balena 
    //////////    /// </summary>
    //////////    public HourglassCursor()
    //////////    {
    //////////        savedCursor = System.Windows.Forms.Cursor.Current;
    //////////        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
    //////////    }
    //////////    /// <summary>
    //////////    /// resets cursor to prior value
    //////////    /// </summary>
    //////////    public void Dispose()
    //////////    {
    //////////        System.Windows.Forms.Cursor.Current = savedCursor;
    //////////    }
    //////////}
    /// <summary>
    /// implantable device such as pacemaker
    /// </summary>
    public class ImplantableDevice
    {
        /// <summary>
        /// Unique Device Identifier, includes 
        /// Device Identifier (manufacturer, model)
        /// and Production Identifier (lot, serial num, 
        /// exp date, date manuf 
        /// and distinct id code if any)
        /// </summary>
        public string UDI = string.Empty;
        /// <summary>
        /// name or description of device
        /// </summary>
        public string Description = string.Empty;
        /// <summary>
        /// device identifier which maps to make and model
        /// </summary>
        public string DI = string.Empty;
        /// <summary>
        /// description per SNOMED-CT
        /// </summary>
        public string Snomed_CT_Description = string.Empty;
        /// <summary>
        /// name per GMDN PT
        /// </summary>
        public string GMDN_PT_Name = string.Empty;
        /// <summary>
        /// optional comments
        /// </summary>
        public string Comments = string.Empty;
        /// <summary>
        /// object serialized with vertical bars
        /// </summary>
        /// <returns></returns>
        public string ToVBar()
        {
            StringBuilder sb = new();
            if (UDI.Contains('|'))
            {
                throw new Exception("Warning, the Unique " +
                    "Device Identifier (UDI) " +
                    UDI +
                    " has a corrupt format containing a " +
                    "vertical bar.  Please  correct the " +
                    "UDI if this was in error.  Otherwise " +
                    "this program developer needs to " +
                    "update the program if a UDI is found " +
                    "that  really has the | character in it.");
            }
            _ = sb.Append(UDI);
            _ = sb.Append('|');
            _ = sb.Append(GMDN_PT_Name.Replace('|', '#'));
            _ = sb.Append('|');
            _ = sb.Append(Snomed_CT_Description.Replace('|', '#'));
            _ = sb.Append('|');
            _ = sb.Append(Description.Replace('|', '#'));
            _ = sb.Append('|');
            _ = sb.Append(Comments.Replace('|', '#'));
            return sb.ToString();
        }
        /// <summary>
        /// create object from serialization wit
        /// vertical bars, or null if can't
        /// </summary>
        /// <param name="vbar"></param>
        /// <returns></returns>
        public static ImplantableDevice? FromVBar(string vbar)
        {
            ImplantableDevice? result = null;
            string[] parts = vbar.Split('|');
            if (parts.Length > 4)
            {
                result = new ImplantableDevice
                {
                    UDI = parts[0],
                    GMDN_PT_Name = parts[1],
                    Snomed_CT_Description = parts[2],
                    Description = parts[3],
                    Comments = parts[4]
                };
            }
            return result;
        }
    }



    #endregion classes

    #region attributes
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //Section three: Compiler Attributes
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    /// <summary>
    /// This attribute is for marking ProgramPiece classes
    /// with recommended settings for the admin console
    /// to suggest to admin when plugging them in
    /// NOTICE:  the assembly using this attribute must reference
    /// System.Drawing even if it is a class library without
    /// visible forms and controls, becasue that's where
    /// System.Drawing.KnownColor lives!
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class PluginSuggestionsAttribute : Attribute
    {
        /// <summary>
        /// user permissions to require befor a user can access this piece
        /// </summary>
        public UserPermissions PermissionsRequiredSuggested = UserPermissions.None;
        /// <summary>
        /// description to show what this piece is for
        /// </summary>
        public string DescriptionSuggested = string.Empty;
        /// <summary>
        /// label to show in nav bar if any
        /// </summary>
        public string LabelSuggested = string.Empty;
        /// <summary>
        /// true if should show in the nav bar; otherwise will still
        /// be available under Windows menu item
        /// </summary>
        public bool ShowInNavBarSuggested = false;
        /// <summary>
        /// enum System.Drawing.KnownColor for the label in the nav bar
        /// </summary>
        public System.Drawing.KnownColor LabelColorSuggested = System.Drawing.KnownColor.Black;
        /// <summary>
        /// character which launches the piece if pressed while Ctrl is down
        /// Watch out for reserved values
        /// </summary>
        public char ShortcutLetterSuggested = char.MinValue;
        /// <summary>
        /// suggested settings for admin to use in plugging in
        /// program piece objects
        /// </summary>
        public PluginSuggestionsAttribute()
        {
            //nothing
        }
    }

    #endregion attributes
}

