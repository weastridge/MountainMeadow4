using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace MM4Common
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // Enumerations used by MM4Common
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    //////note:  to use [EnumFieldDescription]
    ////// attach [EnumFieldDescription("myname")] attribute to the enum value
    ////// retrieve w MM3Common.EnumFieldDescriptionAttribute.DescriptionFromEnumValue<T>()

    // or preffered, use [Description("myname")]
    // which requires using System.ComponentModel
    // and retrieve w with MM3Common.MM3CommonClass.GetEnumDescription(Enum e)
    // defined in MM4Types.cs





    /// <summary>
    /// User permissions, in 32 bit flags
    /// Set with OR and read with AND 
    /// </summary>
    /// <remarks>
    /// the bottom 16 bits are reserved for individual permissions,
    /// the top 16 are reserved for groups -- Chris
    /// </remarks>
    [Flags]
    public enum UserPermissions : int //32 bit integer
    {
        /// <summary>
        /// no permission
        /// </summary>
        None = 0x00000000,
        /// <summary>
        /// can log in to program (=1)
        /// </summary>
        [Description("Login")]
        CanLogin = 0x00000001,
        /// <summary>
        /// can enter data
        /// </summary>
        [Description("Write")]
        CanWrite = 0x00000002,
        //(Admin = 4 is below)
        /// <summary>
        /// can write on rx, typically a clinician
        /// or a clinical assistant
        /// </summary>
        [Description("Write on Rx")]
        CanWriteOnRx = 0x00000008,
        /// <summary>
        /// can sign rx, which is typically only
        /// for a clinician.  Thus anyone with this
        /// permission should be a clnician
        /// </summary>
        [Description("Sign Rx")]
        CanSignRx = 0x00000010,
        /// <summary>
        /// can review tests; again only typical for 
        /// a clinician
        /// </summary>
        [Description("Initial Tests")]
        CanInitialTests = 0x00000020,
        //(Numerically out of order so will show in control in this order...)
        /// <summary>
        /// can do administrator tasks
        /// </summary>
        [Description("Administrator")]
        CanAdmin = 0x00000004,
        /// <summary>
        /// Can be assigned as a patient's primary provider
        /// </summary>
        [Description("PrimProvider")]
        CanBePrimProv = 0x00000040,
        /// <summary>
        /// can change what patient they are viewing
        /// </summary>
        [Description("Lookup Pats")]
        CanLookupPats = 0x00000080,
        /// <summary>
        /// can only log in in emergency
        /// </summary>
        [Description("Emergency Only")]
        EmergencyOnly = 0x00000100,

        /// <summary>
        /// aux group 1
        /// </summary>
        Group1 = 0x00010000,
        /// <summary>
        /// aux group 2
        /// </summary>
        Group2 = 0x00020000,
        /// <summary>
        /// aux group 3
        /// </summary>
        Group3 = 0x00040000,
        /// <summary>
        /// aux group 4
        /// </summary>
        Group4 = 0x00080000,
        /// <summary>
        /// aux group 5
        /// </summary>
        Group5 = 0x00100000,
        /// <summary>
        /// aux group 6
        /// </summary>
        Group6 = 0x00200000,
        /// <summary>
        /// aux group 7
        /// </summary>
        Group7 = 0x00400000,
        //others reserved for future use
    }

    /// <summary>
    /// Privacy restrictions, in 32 bit flags
    /// To bitwise AND data flags with exclusion flags for reports
    /// i.e. data will be blocked if user and data both have a 
    /// common flag set.
    /// </summary>
    [Flags]
    public enum PrivacyFlags : int
    {
        /// <summary>
        /// unrestricted access
        /// </summary>
        None = 0,
        /// <summary>
        /// exclude sensitive material from standard reports
        /// </summary>
        DataSensitive = 1,
        /// <summary>
        /// exclude from reports given to patient
        /// </summary>
        DataNotForPatient = 2
    }

    /// <summary>
    /// useful for declaring intentions for a form being shown
    /// </summary>
    public enum EditingMode : int
    {
        /// <summary>
        /// allow new object to be created
        /// </summary>
        Add = 1,
        /// <summary>
        /// view only
        /// </summary>
        View = 2,
        /// <summary>
        /// view and allow editing
        /// </summary>
        Edit = 3,
        /// <summary>
        /// delete
        /// </summary>
        Delete = 4
    }


    /// <summary>
    /// a gender specification for a person or a rule,
    /// further described and displayed by the Gender object
    /// </summary>
    public enum GenderType : int
    {
        //for future changes, add new
        // numbers but don't change these
        // numbers as they are in database
        /// <summary>
        /// male
        /// </summary>
        Male = 1,
        /// <summary>
        /// female
        /// </summary>
        Female = 2,
        /// <summary>
        /// other (e.g. ambiguous)
        /// </summary>
        Other = 3,
        /// <summary>
        /// unknown or unspecified
        /// </summary>
        Unknown = 4,
        /// <summary>
        /// to signify a rule or category
        /// that applies to either sex
        /// </summary>
        Either = 5
    }


    /// <summary>
    /// eg current, birth, alias
    /// </summary>
    public enum PersonNameType : int
    {
        //for future changes, add new
        // numbers but don't change these
        // numbers as they are in database
        /// <summary>
        /// name currently using
        /// </summary>
        CurrentName = 1,
        /// <summary>
        /// eg maiden name
        /// </summary>
        BirthName = 2,
        /// <summary>
        /// other name person has gone by
        /// </summary>
        AdditionalName = 3
    }

    /// <summary>
    /// eg home phone, email etc
    /// </summary>
    public enum CommunicationsSubType : int
    {
        //for future changes, add new
        // numbers but don't change these
        // numbers as they are in database
        /// <summary>
        /// other or unspecified location
        /// </summary>
        Other = 0,
        /// <summary>
        /// home 
        /// </summary>
        Home = 1,
        /// <summary>
        /// work
        /// </summary>
        Work = 2,
        /// <summary>
        /// cell phone
        /// </summary>
        Cellular = 3
    }

    /// <summary>
    /// types of dateTime elements in the
    /// CCR definition of Advance Directives
    /// </summary>
    public enum AdvDirDateType : int
    {
        //for future changes, add new
        // numbers but don't change these
        // numbers as they are in database
        LastRecorded = 1,
        VerifiedWithPatient = 2,
        VerifiedWithParent = 3,
        VerifiedWithGuardian = 4,
        VerifiedWithFamily = 5,
        VerifiedWithPOA = 6,
        VerifiedWithPhysican = 7,
        StartDate = 8,
        EndDate = 9
    }

    /// <summary>
    /// eg recussitation status, intubation status
    /// </summary>
    public enum AdvDirType : int
    {
        //for future changes, add new
        // numbers but don't change these
        // numbers as they are in database
        Other = 0,
        RecussitationStatus = 1,
        IntubationStatus = 2,
        IVFAndSupportStatus = 3,
        CPRStatus = 4,
        AntibioticStatus = 5,
        LifeSupportStatus = 6,
        TubeFeedings = 7
    }

    /// <summary>
    /// eg NoCode, AntibioticsOnly
    /// </summary>
    public enum AdvDirDescription : int
    {
        //for future changes, add new
        // numbers but don't change these
        // numbers as they are in database
        Other = 0,
        FullCode = 1,
        NoCode = 2,
        NoCPR = 3,
        CardiovOnly = 4,
        CPRDrugsOnly = 5,
        NoIntubation = 6,
        IVFluidsOnly = 7,
        NoIVFluids = 8,
        AntibioticsOnly = 9,
        NoAntibiotics = 10,
        TubeFeedings = 11,
        NoFeedingTube = 12,
        NoProlongedLifeSupport = 13
    }

    /// <summary>
    /// eg current, verified
    /// </summary>
    public enum AdvDirStatus : int
    {
        //for future changes, add new
        // numbers but don't change these
        // numbers as they are in database
        CurrentAndVerified = 1,
        SupportedByHealthcareWill = 2,
        SupportedByDurablePowerOfAttorneyForHealthcare = 3,
        VerifiedWithFamilyOnly = 4,
        VerifiedByMedicalRecordOnly = 5
    }



    /// <summary>
    /// used for specifying how precisely
    /// a date or time is specified
    /// </summary>
    public enum DateTimePrecision : int
    {
        /// <summary>
        /// unknown or unspecified
        /// </summary>
        Unspecified = 0,
        /// <summary>
        /// specified to nearest year
        /// </summary>
        Year = 1,
        /// <summary>
        /// specified to nearest month
        /// </summary>
        Month = 2,
        /// <summary>
        /// specified to nearest day
        /// </summary>
        Day = 3,
        /// <summary>
        /// specified to nearest hour
        /// </summary>
        Hour = 4,
        /// <summary>
        /// specified to nearest minute
        /// </summary>
        Minute = 5,
        /// <summary>
        /// specified to nearest second
        /// </summary>
        Second = 6,
        /// <summary>
        /// specified to nearest millisecond
        /// </summary>
        Millisec = 7
    }

    /// <summary>
    /// Flags type enumeration for
    /// status of problem, eg active, chronic,
    /// intermittent
    /// </summary>
    [Flags]
    public enum ProblemStatus : int
    {
        //for future changes, add new
        // numbers but don't change these
        // numbers as they are in database
        /// <summary>
        /// unspecified or unknown
        /// </summary>
        Other = 0,
        /// <summary>
        /// problem in active list
        /// </summary>
        Active = 1,
        /// <summary>
        /// problem in inactive list
        /// </summary>
        Inactive = 2,
        /// <summary>
        /// a chronic problem
        /// </summary>
        Chronic = 4,
        /// <summary>
        /// an intermittent problem
        /// </summary>
        Intermittent = 16,
        /// <summary>
        /// a recurrent problem
        /// </summary>
        Recurrent = 32,
        /// <summary>
        /// a problem to be ruled out
        /// </summary>
        RuleOut = 64,
        /// <summary>
        /// the problem was ruled out
        /// </summary>
        RuledOut = 128,
        /// <summary>
        /// problem was resolved
        /// </summary>
        Resolved = 256
    }

    /// <summary>
    /// eg active, priorHx
    /// </summary>
    public enum HistoryItemStatus : int
    {
        //for future changes, add new
        // numbers but don't change these
        // numbers as they are in database
        /// <summary>
        /// not specified or not applicable
        /// </summary>
        NotSpecified = 0,
        /// <summary>
        /// active condition
        /// </summary>
        Active = 1,
        /// <summary>
        /// previously active or true but not now
        /// </summary>
        PriorHxButNoLonger = 2,
        /// <summary>
        /// never had or never was active or true
        /// </summary>
        NeverWas = 3,
        /// <summary>
        /// unknown
        /// </summary>
        Unknown = 4
    }

    /// <summary>
    /// eg marital status, etoh
    /// </summary>
    public enum SocialHistoryType : int
    {
        //for future changes, add new
        // numbers but don't change these
        // numbers as they are in database
        /// <summary>
        /// unspecified or none
        /// </summary>
        NotSpecified = 0,
        /// <summary>
        /// marital status
        /// </summary>
        MaritalStatus = 1,
        /// <summary>
        /// religion
        /// </summary>
        Religion = 2,
        /// <summary>
        /// ethnicity
        /// </summary>
        Ethnicity = 3,
        /// <summary>
        /// race
        /// </summary>
        Race = 4,
        /// <summary>
        /// language
        /// </summary>
        Language = 5,
        /// <summary>
        /// smoking history
        /// </summary>
        SmokingHx = 6,
        /// <summary>
        /// exercise history
        /// </summary>
        Exercise = 7,
        /// <summary>
        /// diet history
        /// </summary>
        Diet = 8,
        /// <summary>
        /// employment
        /// </summary>
        Employment = 9,
        /// <summary>
        /// toxic exposures
        /// </summary>
        ToxicExposure = 10,
        /// <summary>
        /// alcohol status
        /// </summary>
        ETOHUse = 11,
        /// <summary>
        /// illicit drug status
        /// </summary>
        DrugUse = 12,
        /// <summary>
        /// situation
        /// </summary>
        LivingSituation = 13,
        /// <summary>
        /// any treatment restrictions, like
        /// can't accept blood products for example
        /// </summary>
        TreatmentRestrictions = 14,
        /// <summary>
        /// unknown type
        /// </summary>
        Unknown = 15
    }

    /// <summary>
    /// some pre-defined social history items
    /// </summary>
    public enum SocialHistoryDefinedItem : int
    {
        /// <summary>
        /// none specified
        /// </summary>
        None = 0,
        /// <summary>
        /// currently smokes
        /// </summary>
        SmokesNow = 1,
        /// <summary>
        /// used to smoke but quit
        /// </summary>
        SmokedButQuit = 2,
        /// <summary>
        /// never smoked
        /// </summary>
        NeverSmoked = 3,
        /// <summary>
        /// never drinks etoh
        /// </summary>
        NeverDrinks = 4,
        /// <summary>
        /// drinks in moderation
        /// </summary>
        DrinksModerately = 5,
        /// <summary>
        /// drinks heavily
        /// </summary>
        DrinksHeavily = 6,
        /// <summary>
        /// used to drink etoh but quit
        /// </summary>
        QuitDrinking = 7
    }

    /// <summary>
    /// eg drug allergy, nondrug allergy
    /// </summary>
    public enum AlertType : int
    {
        //for future changes, add new
        // numbers but don't change these
        // numbers as they are in database
        /// <summary>
        /// other or not specified
        /// </summary>
        Other = 0,
        /// <summary>
        /// drug allergy
        /// </summary>
        DrugAllergy = 1,
        /// <summary>
        /// adverse drug reaction
        /// </summary>
        DrugAdverseReaction = 2,
        /// <summary>
        /// general alert
        /// </summary>
        Alert = 3,
        /// <summary>
        /// allergy other than to drugs
        /// </summary>
        NonDrugAllergy = 4
    }

    /// <summary>
    /// kind of non-postal address eg phone
    /// </summary>
    public enum CommunicationMethod : int
    {
        //for future changes, add new
        // numbers but don't change these
        // numbers as they are in database
        /// <summary>
        /// other than phone, email
        /// </summary>
        Other = 0,
        /// <summary>
        /// phone
        /// </summary>
        Telephone = 1,
        /// <summary>
        /// fax
        /// </summary>
        Fax = 2,
        /// <summary>
        /// e-mail
        /// </summary>
        EMail = 3,
        /// <summary>
        /// universal resource locator, like web address
        /// </summary>
        URL = 4,
        /// <summary>
        /// url for sending text messages
        /// </summary>
        TextMsgViaEmail = 5
    }

    /// <summary>
    /// the location eg home, work
    /// </summary>
    public enum PostalAddressType : int
    {
        //for future changes, add new
        // numbers but don't change these
        // numbers as they are in database
        /// <summary>
        /// other or not specified
        /// </summary>
        Other = 0,
        /// <summary>
        /// home address
        /// </summary>
        Home = 1,
        /// <summary>
        /// work address
        /// </summary>
        Work = 2
    }

    /// <summary>
    /// eg primary, secondary, leastPreferred
    /// </summary>
    public enum AddressPriority : int
    {
        //for future changes, add new
        // numbers but don't change these
        // numbers as they are in database
        /// <summary>
        /// not specified or other
        /// </summary>
        Other = 0,
        /// <summary>
        /// primary address
        /// </summary>
        Primary = 1,
        /// <summary>
        /// secondary address
        /// </summary>
        Secondary = 2,
        /// <summary>
        /// least preferred address
        /// </summary>
        LeastPreferred = 3
    }

    /// <summary>
    /// category of memo, eg personal, protime reminder...
    /// </summary>
    public enum MemoType : int
    {
        /// <summary>
        /// general memo
        /// </summary>
        General = 0,
        /// <summary>
        /// previously used for protime memos
        /// </summary>
        LegacyProtime = 1,
        /// <summary>
        /// memo for protime checks due
        /// </summary>
        Protime = 2,
        /// <summary>
        /// memo for recall office visit due
        /// </summary>
        Recall = 3
    }

    /// <summary>
    /// formatting options for a body of text
    /// </summary>
    public enum MM3TextFormat : int
    {
        /// <summary>
        /// unspecified or other
        /// </summary>
        Other = 0,
        /// <summary>
        /// plain text 
        /// </summary>
        PlainText = 1,
        /// <summary>
        /// rtf
        /// </summary>
        RichTextFormat = 2,
        /// <summary>
        /// html web page
        /// </summary>
        HTML = 3,
        /// <summary>
        /// extended markup language
        /// </summary>
        XML = 4
    }


    /// <summary>
    /// category of lab test 
    /// (elaborated in TestCats table)
    /// </summary>
    public enum TestCategory : int
    {
        /// <summary>
        /// unspecified or other
        /// </summary>
        Unspecified = 0,
        /// <summary>
        /// hematology
        /// </summary>
        CBC = 1,
        /// <summary>
        /// chemistry
        /// </summary>
        Chem = 2,
        /// <summary>
        /// urinalysis
        /// </summary>
        UA = 3,
        /// <summary>
        /// diabetes
        /// </summary>
        DM = 4,
        /// <summary>
        /// lipids
        /// </summary>
        Lipids = 5,
        /// <summary>
        /// anticoagulation
        /// </summary>
        Protimes = 6
    }

    /// <summary>
    /// category of misc tests 
    /// (elaborated in MiscTestCats table)
    /// </summary>
    public enum MiscTestCategory : int
    {
        /// <summary>
        /// unspecified or other
        /// </summary>
        Unspecified = 0,
        /// <summary>
        /// radiology, imaging
        /// </summary>
        XRays = 1,
        /// <summary>
        /// heart studies
        /// </summary>
        Cardiac = 2,
        /// <summary>
        /// procedures: pathology, eeg, emg...
        /// </summary>
        MiscProcs = 3,
        /// <summary>
        /// lab tests: ferritin, blood culture...
        /// </summary>
        MiscLabs = 4
    }

    /// <summary>
    /// Modifiers for vital signs, in flags format
    /// </summary>
    [Flags]
    public enum VitalsModifiers : int //32 bit integer
    {
        /// <summary>
        /// no modifier
        /// </summary>
        None = 0,
        /// <summary>
        /// oral temp
        /// </summary>
        Oral = 1,
        /// <summary>
        /// rectal temp
        /// </summary>
        Rectal = 2,
        /// <summary>
        /// axillary temp
        /// </summary>
        Axillary = 4,
        /// <summary>
        /// forehead temp
        /// </summary>
        Forehead = 8,
        /// <summary>
        /// tympanic membrane temp
        /// </summary>
        Tympanic = 16,
        /// <summary>
        /// future  type of temp
        /// </summary>
        UnusedTemp = 32,
        /// <summary>
        /// rt arm bp
        /// </summary>
        RtArm = 64,
        /// <summary>
        /// left arm bp
        /// </summary>
        LtArm = 128,
        /// <summary>
        /// rt leg bp
        /// </summary>
        RtLeg = 256,
        /// <summary>
        /// left leg bp
        /// </summary>
        LtLeg = 512,
        /// <summary>
        /// bp by palpation
        /// </summary>
        Palpation = 1024,
        /// <summary>
        /// bp lying down
        /// </summary>
        Lying = 2048,
        /// <summary>
        /// bp sitting
        /// </summary>
        Sitting = 4096,
        /// <summary>
        /// bp standing
        /// </summary>
        Standing = 8192,
        /// <summary>
        /// regular pulse
        /// </summary>
        Regular = 16384,
        /// <summary>
        /// irregular pulse
        /// </summary>
        Irregular = 32768,
        /// <summary>
        /// pulse with ectopics
        /// </summary>
        Ectopics = 65536
    }

    /// <summary>
    /// id of entry in VitalsNames table
    /// </summary>
    public enum VitalsNameIDs : int
    {
        /// <summary>
        /// unspecified
        /// </summary>
        Unspecified = 0,
        /// <summary>
        /// ht in inches
        /// </summary>
        HeightInches = 1,
        /// <summary>
        /// height in meters
        /// </summary>
        HeightMeters = 2,
        /// <summary>
        /// wt in pounds
        /// </summary>
        WeightLbs = 3,
        /// <summary>
        /// wt in grams
        /// </summary>
        WeightKilograms = 4,
        /// <summary>
        /// head circumference in cm
        /// </summary>
        HeadCirc = 5,
        /// <summary>
        /// temp in F
        /// </summary>
        TempFarenheit = 6,
        /// <summary>
        /// temp in C
        /// </summary>
        TempCelcius = 7,
        /// <summary>
        /// systolic blood pressure
        /// </summary>
        BPSystolic = 8,
        /// <summary>
        /// diastilic blood pressure
        /// </summary>
        BPDiastolic = 9,
        /// <summary>
        /// pulse rate per min
        /// </summary>
        Pulse = 10,
        /// <summary>
        /// respirations per minute
        /// </summary>
        Respirations = 11,
        /// <summary>
        /// waist circumference in cm
        /// </summary>
        WaistCm = 12,
        /// <summary>
        /// waist circumference in inches
        /// </summary>
        WaistInches = 13,
        /// <summary>
        /// pulse oximitry
        /// </summary>
        PulseOx = 14,
        /// <summary>
        /// normally a calculated, not stored value
        /// </summary>
        BodyMassIndex = 114,
        /// <summary>
        /// a calculated, not stored value
        /// </summary>
        WeightForHeight = 115,

    }

    /// <summary>
    /// category of history item
    /// </summary>
    public enum PMFSHxCategory : int
    {
        /// <summary>
        /// past medical history
        /// </summary>
        PastMedHx = 1,
        /// <summary>
        /// past surgical history
        /// </summary>
        SurgHx = 2,
        /// <summary>
        /// family history
        /// </summary>
        FamilyHx = 3,
        /// <summary>
        /// social history
        /// </summary>
        SocialHx = 4
    }

    /// <summary>
    /// disposition needed for history item
    /// being edited
    /// </summary>
    public enum HxItemDisposition : int
    {
        /// <summary>
        /// not applicable or not specified
        /// </summary>
        NotSpecified,
        /// <summary>
        /// newly created item to be saved
        /// </summary>
        NewToSave,
        /// <summary>
        /// newly created, then deleted so don't save
        /// </summary>
        NewButDeleted,
        /// <summary>
        /// existing item, unchanged so nothing to do
        /// </summary>
        ExistingUnedited,
        /// <summary>
        /// existing item changed so need to x-out old 
        /// and save new
        /// </summary>
        ExistingEdited,
        /// <summary>
        /// existing item deleted so need to x-out
        /// </summary>
        ExistingToDelete
    }

    /// <summary>
    /// family relationship
    /// </summary>
    public enum FamilyRelationship : int
    {
        //note: may append but never edit or delete
        // items because they are stored in database!
        /// <summary>
        /// not specified 
        /// </summary>
        NotSpecified = 0,
        /// <summary>
        /// other than choices given
        /// </summary>
        Other,
        /// <summary>
        /// unknown relationship
        /// </summary>
        Unknown,
        /// <summary>
        /// is adopted
        /// </summary>
        Adopted,
        /// <summary>
        /// mother
        /// </summary>
        Mother,
        /// <summary>
        /// father
        /// </summary>
        Father,
        /// <summary>
        /// parent
        /// </summary>
        Parent,
        /// <summary>
        /// sister
        /// </summary>
        Sister,
        /// <summary>
        /// brother
        /// </summary>
        Brother,
        /// <summary>
        /// sibling
        /// </summary>
        Sibling,
        /// <summary>
        /// twin
        /// </summary>
        Twin,
        /// <summary>
        /// daughter
        /// </summary>
        Daughter,
        /// <summary>
        /// son
        /// </summary>
        Son,
        /// <summary>
        /// child of 
        /// </summary>
        Child,
        /// <summary>
        /// grandchild
        /// </summary>
        Grandchild,
        /// <summary>
        /// maternal grandmother
        /// </summary>
        MatGM,
        /// <summary>
        /// maternal grandfather
        /// </summary>
        MatGF,
        /// <summary>
        /// paternalt grandmother
        /// </summary>
        PatGM,
        /// <summary>
        /// paternal grandfather
        /// </summary>
        PatGF,
        /// <summary>
        /// grandmother
        /// </summary>
        GM,
        /// <summary>
        /// grandfather
        /// </summary>
        GF,
        /// <summary>
        /// aunt
        /// </summary>
        Aunt,
        /// <summary>
        /// uncle
        /// </summary>
        Uncle,
        /// <summary>
        /// first cousin
        /// </summary>
        FirstCousin,
        /// <summary>
        /// second cousin
        /// </summary>
        SecondCousin,
        /// <summary>
        /// niece
        /// </summary>
        Niece,
        /// <summary>
        /// nephew
        /// </summary>
        Nephew
    }

    /// <summary>
    /// category of report for printing
    /// </summary>
    public enum ReportCategory : int
    {
        /// <summary>
        /// not specified
        /// </summary>
        NotSpecified = 0,
        /// <summary>
        /// printing of chart note(s) only
        /// </summary>
        ChartNote = 1,
        /// <summary>
        /// printing of any part(s) of the patient
        /// clinical record
        /// </summary>
        ChartReport = 2,
        /// <summary>
        /// printing of immunization record
        /// </summary>
        Immunizations = 3,
        /// <summary>
        /// printing of demographic information
        /// </summary>
        Demographics = 4,
        /// <summary>
        /// printout of a patient list
        /// </summary>
        PatientList = 5,
        /// <summary>
        /// personal health record
        /// </summary>
        PersonalHealthRecord = 6
        //note: never change the above numbers, but only add to the end
    }

    /// <summary>
    /// groupings for sayings
    /// </summary>
    [Flags]
    public enum SayingGroups : int
    {
        /// <summary>
        /// none specified
        /// </summary>
        unspecified = 0,
        /// <summary>
        /// general all purpose
        /// </summary>
        General = 1,
        /// <summary>
        /// funny
        /// </summary>
        Humorous = 2,
        /// <summary>
        /// wit and wisdom
        /// </summary>
        Wisdom = 4,
        /// <summary>
        /// Christian
        /// </summary>
        Christian = 8,
        /// <summary>
        /// sayings not to be shown
        /// </summary>
        Blackballed = 16
    }

    /// <summary>
    /// values for the first ID of MiscellaneousItem 
    /// objects whose CompanyUID is 'MM3'
    /// </summary>
    public enum MM3MiscItemID1 : int
    {
        /// <summary>
        /// item represents a PatientList object
        /// </summary>
        PatientList = 1,
        /// <summary>
        /// item represents a member of a PatientList
        /// </summary>
        PatientListItem = 2,
        /// <summary>
        /// most recently used VIIS Control ID for HL7
        /// messages to Virginia Immunization Information
        /// System
        /// </summary>
        VIISLastControlID = 3,
        /// <summary>
        /// file to include in Personal Health Record Report in
        /// the format (filename)data using brackets.  Data may be
        /// text or else Base64 representation of binary data.
        /// </summary>
        PHRReportFile = 4,
        /// <summary>
        /// associations between program's provider ID's and those of
        /// reference labs
        /// </summary>
        TestSourceProviderMaps = 5,
        /// <summary>
        /// information used by e-prescribing, but notice DrFirst info
        /// has its on table, not this Miscellaneous table
        /// </summary>
        ERxInfo = 6,
        /// <summary>
        /// 
        /// </summary>
        PQRI = 7,
        /// <summary>
        /// config for DatabaseStatusClass
        /// </summary>
        DatabaseStatusConfig = 8,
        /// <summary>
        /// Message of the day for login screen in the format of
        /// text message, vertical bar, date last updated
        /// (in ToShortDateString() format)
        /// </summary>
        MessageOfTheDay = 9,
        /// <summary>
        /// for radiology reports from Mtn Empire Rads
        /// </summary>
        MountainEmpireRadiology = 10,
        /// <summary>
        /// for PasswordUpdateRequest
        /// </summary>
        PasswordUpdateRequest = 11,
        /// <summary>
        /// patient depression score items
        /// </summary>
        DepressionScore = 12
    }

    /// <summary>
    /// result code from methods updating Chart Notes
    /// </summary>
    public enum NoteUpdateResult : int
    {
        /// <summary>
        /// updated successfully
        /// </summary>
        Success,
        /// <summary>
        /// failed for unspecified error
        /// </summary>
        UnspecifiedError,
        /// <summary>
        /// failed becuase note was locked and
        /// was not expected to be locked
        /// </summary>
        FoundLockedUnexpectedly,
        /// <summary>
        /// failed because note was locked with a LockID
        /// other than the one expected
        /// </summary>
        FoundOtherLock,
        /// <summary>
        /// no lock was found when one was expected
        /// </summary>
        MissingLockUnexpectedly,
        /// <summary>
        /// failed because timestamp of note in the database
        /// did not mach the exptected timestamp
        /// </summary>
        DataNotFresh,
        /// <summary>
        /// couldn't find note in the database, or db error
        /// </summary>
        NoteNotFound
    }

    /// <summary>
    /// meanings for CompanyUID in LogOfEvents table
    /// </summary>
    public enum LogOfEventsCompanyUID : int
    {
        /// <summary>
        /// unspecified
        /// </summary>
        Unspecified = 0,
        /// <summary>
        /// reserved for EastRidges (MM3)
        /// </summary>
        EastRidges = 1
    }

    /// <summary>
    /// categories used by CompanyUID Eastridges and optionally
    /// by anyone else for events, which are the values of ID1
    /// in the LogOfEvent objects
    /// </summary>
    public enum LogOfEventsID1 : int
    {
        /// <summary>
        /// unspecified
        /// </summary>
        Unspecified = 0,
        /// <summary>
        /// database backup-related events
        /// </summary>
        Backups = 1
    }

    /// <summary>
    /// meanings for ID2 in backup-related events
    /// as used by CompanyUID EastRidges and optionally by others
    /// </summary>
    public enum LogOfEventsBackupID2 : int
    {
        /// <summary>
        /// unspcified
        /// </summary>
        Unspecified = 0,
        /// <summary>
        /// full database backup
        /// </summary>
        FullBackup = 1,
        /// <summary>
        /// differential backup of changes since last full
        /// </summary>
        DifferentialBackup = 2,
        /// <summary>
        /// sent notification (email, etc) to admin regarding events
        /// </summary>
        SentNotification = 3,
        /// <summary>
        /// transfer full backup to another location
        /// </summary>
        TransferFullBackup = 4,
        /// <summary>
        /// transfere diff or incremental backup to another location
        /// </summary>
        TransferDiffOrIncrementalBackup = 5,
        /// <summary>
        /// restore database backup onto secondary server
        /// </summary>
        Restore = 6,
        /// <summary>
        /// encrypt database
        /// </summary>
        Encrypt = 7
    }

    /// <summary>
    /// category of program current size
    /// </summary>
    public enum MainFormSizeCat
    {
        /// <summary>
        /// or unknown
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// less than 800x600
        /// </summary>
        TooSmall = 1,
        /// <summary>
        /// legacy minimum size 800x600
        /// </summary>
        Small_800_600 = 2,
        /// <summary>
        /// current minimum size 1024x720
        /// </summary>
        Medium_1024_720 = 3
    }

    /*
     * 
     * 
     * 
     * 
     * These definitions are from CDC PHIN Vocabulary Access and Distribution system
     * which meet min standards from OMB
     * 
     * Value Set Name	Value Set Code	Value Set OID	Value Set Version	Value Set Definition	Value Set Status	VS Last Updated Date	VS Release Comments
Race Category	PHVS_RaceCategory_CDC	2.16.840.1.114222.4.11.836	1	General race category reported by the patient - subject may have more than one	Published	10/22/2008	

Concept Code	Concept Name	Preferred Concept Name	Preferred Alternate Code	Code System OID	Code System Name	Code System Code	Code System Version	HL7 Table 0396 Code
1002-5	American Indian or Alaska Native	American Indian or Alaska Native		2.16.840.1.113883.6.238	Race & Ethnicity - CDC	PH_RaceAndEthnicity_CDC	1.1	CDCREC
2028-9	Asian	Asian		2.16.840.1.113883.6.238	Race & Ethnicity - CDC	PH_RaceAndEthnicity_CDC	1.1	CDCREC
2054-5	Black or African American	Black or African American		2.16.840.1.113883.6.238	Race & Ethnicity - CDC	PH_RaceAndEthnicity_CDC	1.1	CDCREC
2076-8	Native Hawaiian or Other Pacific Islander	Native Hawaiian or Other Pacific Islander		2.16.840.1.113883.6.238	Race & Ethnicity - CDC	PH_RaceAndEthnicity_CDC	1.1	CDCREC
2131-1	Other Race	Other Race		2.16.840.1.113883.6.238	Race & Ethnicity - CDC	PH_RaceAndEthnicity_CDC	1.1	CDCREC
2106-3	White	White		2.16.840.1.113883.6.238	Race & Ethnicity - CDC	PH_RaceAndEthnicity_CDC	1.1	CDCREC

     * 
     * 
     * 
     * 
     * 
     * 
     */

    /// <summary>
    /// ethnicity - now depreciated because we use
    /// codifiedEthnicity instead
    /// </summary>
    public enum EthnicityEnumDepreciated : int
    {
        /// <summary>
        /// not specified
        /// </summary>
        Unspecified = 0,
        /// <summary>
        /// Hispanic or Latino
        /// </summary>
        Hispanic = 1,
        /// <summary>
        /// Not Hispanic or Latino
        /// </summary>
        NotHispanic = 2,
        /// <summary>
        /// Declined to specify
        /// </summary>
        Declined = 3
    }

    /// <summary>
    /// Depreciated.  We use RaceGroup
    /// The major categories of race
    /// with Description from
    /// Code System(s):Race and Ethnicity - CDC 2.16.840.1.113883.6.238
    /// IF specified race (not valid if unspecified or other)
    /// </summary>
    public enum RaceEnumDepreciated : int
    {
        /// <summary>
        /// unspecified
        /// </summary>
        [Description("unspecified")]
        Unspecified = 0,
        /// <summary>
        /// native American Indian or Alaska Native
        /// </summary>
        [Description("1002-5")]
        NativeAmericanIndian = 1,
        /// <summary>
        /// Asian, but not Pacific Islander
        /// </summary>
        [Description("2028-9")]
        Asian = 2,
        /// <summary>
        /// Black or African American
        /// </summary>
        [Description("2054-5")]
        Black = 3,
        /// <summary>
        /// Hawaiian or other Pacific Islander
        /// </summary>
        [Description("2076-8")]
        PacificIslander = 4,
        /// <summary>
        /// Caucasian
        /// </summary>
        [Description("2106-3")]
        White = 5,
        /// <summary>
        /// person declined o specify
        /// </summary>
        [Description("declined")]
        DeclinedToSpecify = 6,
        /// <summary>
        /// Other race
        /// </summary>
        [Description("other")]
        Other = 9
    }


}
