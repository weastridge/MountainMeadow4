
using MM4Common;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace MM4Common
{
    //Communications, exchange and EMR interoperability helper classes
    /// <summary>
    /// type of document
    /// </summary>
    public enum CCDAType
    {
        /// <summary>
        /// toc
        /// </summary>
        TransitionOfCare = 1,
        /// <summary>
        /// summary as for data export
        /// </summary>
        ExportSummary = 2,
        /// <summary>
        /// cllinical
        /// </summary>
        ClinicalSummary = 3,
        /// <summary>
        /// DirectProject
        /// </summary>
        ProjectDirect = 4
    }

    ///////////// <summary>
    ///////////// information needed when requesting a CCDA document to 
    ///////////// be created for a patient
    ///////////// </summary>
    //////////public class CCDAGenerationRequest
    //////////{
    //////////    /// <summary>
    //////////    /// the patient the ccda will be for
    //////////    /// </summary>
    //////////    public PatientExpanded Patient = null;
    //////////    /// <summary>
    //////////    /// the primary clinician who is the referring
    //////////    /// provider or author
    //////////    /// </summary>
    //////////    public UserExpanded Author = null;
    //////////    /// <summary>
    //////////    /// the office contact may be the clinician
    //////////    /// or another user
    //////////    /// </summary>
    //////////    public UserExpanded OfficeContactPerson = null;
    //////////    /// <summary>
    //////////    /// the chart note information will be drawn from
    //////////    /// </summary>
    //////////    public ChartNotes.ChartNoteExpanded ChartNoteOfInterest = null;
    //////////    /// <summary>
    //////////    /// starting date of information to be mined from database
    //////////    /// </summary>
    //////////    public DateTime StartDate = DateTime.MinValue;
    //////////    /// <summary>
    //////////    /// ending date of information to be mined from the database
    //////////    /// </summary>
    //////////    public DateTime StopDate = DateTime.MinValue;
    //////////    /// <summary>
    //////////    /// e.g. Transition
    //////////    /// </summary>
    //////////    public CCDAType TypeOfCCDARequested = CCDAType.ExportSummary;
    //////////    /// <summary>
    //////////    /// privacy flags of info to be excluded
    //////////    /// </summary>
    //////////    public PrivacyFlags PrivacyExclusions = PrivacyFlags.None;
    //////////    /// <summary>
    //////////    /// attach Global config which normally lives in MM3State
    //////////    /// </summary>
    //////////    public IConfigForGlobal MM3GlobalConfig = null;
    //////////    /// <summary>
    //////////    /// the CCDA document generated as reply from this request
    //////////    /// </summary>
    //////////    public string Result = string.Empty;
    //////////}
    /// <summary>
    /// useful for CCDA Codified Equivalents CE 
    /// </summary>
    /// <remarks>
    /// create entity with values
    /// </remarks>
    /// <param name="code"></param>
    /// <param name="codeSystem"></param>
    /// <param name="codeSystemName"></param>
    /// <param name="displayName"></param>
    /// <param name="nullFlavor"></param>
    /// <param name="comments"></param>
    [Serializable]

    public class CodifiedEntity(string? code,
        string? codeSystem,
        string? codeSystemName,
        string? displayName,
        string? nullFlavor,
        string? comments)
    {
        /// <summary>
        /// the code for this concept
        /// </summary>
        public string? Code = code;
        /// <summary>
        /// the identifier for the code system code is from
        /// </summary>
        public string? CodeSystem = codeSystem;
        /// <summary>
        /// the human readable name of the code system code is from
        /// </summary>
        public string? CodeSystemName = codeSystemName;
        /// <summary>
        /// human readable name of the concept
        /// </summary>
        public string? DisplayName = displayName;
        /// <summary>
        /// the flavor of null, required if Code is null
        /// </summary>
        public string? NullFlavor = nullFlavor;
        /// <summary>
        /// optional comments
        /// </summary>
        public string? Comments = comments;

        /// <summary>
        /// shows display name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return DisplayName ?? "undefined DisplayName";
        }
    }
    /// <summary>
    /// codification of the smoking status
    /// that is defined in PatientProperies
    /// </summary>
    [Serializable]
    public class CodifiedSmokingStatus
    {
        /// <summary>
        /// all smoking status selections
        /// </summary>
        public static readonly CodifiedEntity[] All = new CodifiedEntity[8];
        /// <summary>
        /// static constructor
        /// </summary>
        static CodifiedSmokingStatus()
        {
            All[0] = new CodifiedEntity("266927001",
                CommCodeSys.SnowmedCt.CodeSystem,
                CommCodeSys.SnowmedCt.CodeSystemName,
                MainCommonClass.GetEnumDescription(
                    PatientProperties.SmokingStatusCat.Unspecified),
                null, //nullflavor
                "Unknown if ever smoked."); //comments
            All[1] = new CodifiedEntity("449868002",
            CommCodeSys.SnowmedCt.CodeSystem,
                CommCodeSys.SnowmedCt.CodeSystemName,
                MainCommonClass.GetEnumDescription(
                    PatientProperties.SmokingStatusCat.SmokesDaily),
                null, //nullflavor
                "Current every day smoker"); //comments
            All[2] = new CodifiedEntity("428041000124106",
                CommCodeSys.SnowmedCt.CodeSystem,
                CommCodeSys.SnowmedCt.CodeSystemName,
                MainCommonClass.GetEnumDescription(
                    PatientProperties.SmokingStatusCat.SmokesSomeDays),
                null, //nullflavor
                "Current some days smoker."); //comments
            All[3] = new CodifiedEntity("77176002",
                CommCodeSys.SnowmedCt.CodeSystem,
                CommCodeSys.SnowmedCt.CodeSystemName,
                MainCommonClass.GetEnumDescription(
                    PatientProperties.SmokingStatusCat.SmokerUnkCurrent),
                null, //nullflavor
                "Smoker, current status unknown."); //comments
            All[4] = new CodifiedEntity("8517006",
                CommCodeSys.SnowmedCt.CodeSystem,
                CommCodeSys.SnowmedCt.CodeSystemName,
                MainCommonClass.GetEnumDescription(
                    PatientProperties.SmokingStatusCat.FormerSmoker),
                null, //nullflavor
                "Former smoker of at least 100 cigs."); //comments
            All[5] = new CodifiedEntity("266919005",
                CommCodeSys.SnowmedCt.CodeSystem,
                CommCodeSys.SnowmedCt.CodeSystemName,
                MainCommonClass.GetEnumDescription(
                    PatientProperties.SmokingStatusCat.NeverSmoker),
                null, //nullflavor
                "Never smoked 100 cigs."); //comments
            All[6] = new CodifiedEntity("428071000124103",
                CommCodeSys.SnowmedCt.CodeSystem,
                CommCodeSys.SnowmedCt.CodeSystemName,
                MainCommonClass.GetEnumDescription(
                    PatientProperties.SmokingStatusCat.HeavySmoker),
                null, //nullflavor
                "Heavy smoker > 10 cigs/day."); //comments
            All[7] = new CodifiedEntity("428061000124105",
                CommCodeSys.SnowmedCt.CodeSystem,
                CommCodeSys.SnowmedCt.CodeSystemName,
                MainCommonClass.GetEnumDescription(
                    PatientProperties.SmokingStatusCat.LightSmoker),
                null, //nullflavor
                "Light smoker <=10 cigs/day."); //comments
        }
        /// <summary>
        /// get codes for given category
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>

        public static CodifiedEntity FromSmokingStatusCat(
            PatientProperties.SmokingStatusCat cat)
        {
            return cat switch
            {
                PatientProperties.SmokingStatusCat.Unspecified => All[0],
                PatientProperties.SmokingStatusCat.SmokesDaily => All[1],
                PatientProperties.SmokingStatusCat.SmokesSomeDays => All[2],
                PatientProperties.SmokingStatusCat.SmokerUnkCurrent => All[3],
                PatientProperties.SmokingStatusCat.FormerSmoker => All[4],
                PatientProperties.SmokingStatusCat.NeverSmoker => All[5],
                PatientProperties.SmokingStatusCat.HeavySmoker => All[6],
                PatientProperties.SmokingStatusCat.LightSmoker => All[7],
                _ => All[0],
            };
        }
    }

    /// <summary>
    /// codification of the family relationships
    /// that is defined in MM3Enums
    /// </summary>
    [Serializable]
    public class CodifiedFamilyRelationship
    {
        ///// <summary>
        ///// selections
        ///// </summary>
        //public enum Selections : int
        //{
        //    /// <summary>
        //    /// unspecified or unknown
        //    /// </summary>
        //    Unspecified = 0,
        //    /// <summary>
        //    /// father
        //    /// </summary>
        //    Father = 1,
        //    /// <summary>
        //    /// mother
        //    /// </summary>
        //    Mother = 2,
        //    /// <summary>
        //    /// brother
        //    /// </summary>
        //    Brother = 3,
        //    /// <summary>
        //    /// sister
        //    /// </summary>
        //    Sister = 4,
        //    /// <summary>
        //    /// son
        //    /// </summary>
        //    Son = 5,
        //    /// <summary>
        //    /// daughter
        //    /// </summary>
        //    Daughter = 6
        //}
        /// <summary>
        /// all fam hx selections, or at least the ones I've coded so far.
        /// </summary>
        public static readonly CodifiedEntity[] All = new CodifiedEntity[7];
        /// <summary>
        /// static constructor
        /// </summary>
        static CodifiedFamilyRelationship()
        {
            //unspec
            All[0] = new CodifiedEntity("407559004",
                CommCodeSys.SnowmedCt.CodeSystem,
                CommCodeSys.SnowmedCt.CodeSystemName,
                MainCommonClass.GetEnumDescription(
                    FamilyRelationship.Unknown),
                null, //nullflavor
                ""); //comments
            All[1] = new CodifiedEntity("160433007",
            CommCodeSys.SnowmedCt.CodeSystem,
                CommCodeSys.SnowmedCt.CodeSystemName,
                MainCommonClass.GetEnumDescription(
                    FamilyRelationship.Father),
                null, //nullflavor
                ""); //comments
            All[2] = new CodifiedEntity("160427003",
                CommCodeSys.SnowmedCt.CodeSystem,
                CommCodeSys.SnowmedCt.CodeSystemName,
                MainCommonClass.GetEnumDescription(
                    FamilyRelationship.Mother),
                null, //nullflavor
                ""); //comments
            All[3] = new CodifiedEntity("160444004",
                CommCodeSys.SnowmedCt.CodeSystem,
                CommCodeSys.SnowmedCt.CodeSystemName,
                MainCommonClass.GetEnumDescription(
                    FamilyRelationship.Brother),
                null, //nullflavor
                ""); //comments
            All[4] = new CodifiedEntity("160439006",
                CommCodeSys.SnowmedCt.CodeSystem,
                CommCodeSys.SnowmedCt.CodeSystemName,
                MainCommonClass.GetEnumDescription(
                    FamilyRelationship.Sister),
                null, //nullflavor
                ""); //comments
            All[5] = new CodifiedEntity("160449009",
                CommCodeSys.SnowmedCt.CodeSystem,
                CommCodeSys.SnowmedCt.CodeSystemName,
                MainCommonClass.GetEnumDescription(
                    FamilyRelationship.Son),
                null, //nullflavor
                ""); //comments
            All[6] = new CodifiedEntity("160454000",
                CommCodeSys.SnowmedCt.CodeSystem,
                CommCodeSys.SnowmedCt.CodeSystemName,
                MainCommonClass.GetEnumDescription(
                    FamilyRelationship.Daughter),
                null, //nullflavor
                ""); //comments

        }
        /// <summary>
        /// get codes for given category
        /// </summary>
        /// <param name="selection"></param>
        /// <returns></returns>
        public static CodifiedEntity FromFamilyRelationship(
            MM4Common.FamilyRelationship selection)
        {
            return selection switch
            {
                FamilyRelationship.Unknown => All[0],
                FamilyRelationship.Father => All[1],
                FamilyRelationship.Mother => All[2],
                FamilyRelationship.Brother => All[3],
                FamilyRelationship.Sister => All[4],
                FamilyRelationship.Son => All[5],
                FamilyRelationship.Daughter => All[6],
                _ => All[0]
            };
        }
        //////////public static CodifiedEntity FromFamilyRelationship(
        //////////    MM4Common.FamilyRelationship selection)
        //////////{
        //////////    switch (selection)
        //////////    {
        //////////        case FamilyRelationship.Unknown:
        //////////            return All[0];
        //////////        case FamilyRelationship.Father:
        //////////            return All[1];
        //////////        case FamilyRelationship.Mother:
        //////////            return All[2];
        //////////        case FamilyRelationship.Brother:
        //////////            return All[3];
        //////////        case FamilyRelationship.Sister:
        //////////            return All[4];
        //////////        case FamilyRelationship.Son:
        //////////            return All[5];
        //////////        case FamilyRelationship.Daughter:
        //////////            return All[6];
        //////////        default:
        //////////            return All[0];
        //////////    }
        //////////}

        /// <summary>
        /// get enum of FamilyRelationship matching snomed
        /// or unknown if no match or if snomed actually
        /// means unknown
        /// </summary>
        /// <param name="snomed"></param>
        /// <returns></returns>
        public static FamilyRelationship GetFamilyRelationshipFromSNOMED(
            string snomed)
        {
            return snomed switch
            {

                "407559004" => FamilyRelationship.Unknown,
                "160433007" => FamilyRelationship.Father,
                "160427003" => FamilyRelationship.Mother,
                "160444004" => FamilyRelationship.Brother,
                "160439006" => FamilyRelationship.Sister,
                "160449009" => FamilyRelationship.Son,
                "160454000" => FamilyRelationship.Daughter,
                _ => FamilyRelationship.Unknown
            };
        }
    }

    /// <summary>
    /// person's gender identity enum (integer)
    /// defined for use in HL7 reporting
    /// WARNING: Unspecified isn't necessarily 
    /// in the value set of approved codes!
    /// </summary>
    /// <remarks>
    /// NOTICE:  Use static methods instead of 
    /// creating new objects for predefined objects
    /// The constructor requires initial
    /// values, which can be null
    /// </remarks>
    /// <param name="selection"></param>
    /// <param name="code"></param>
    /// <param name="codeSystem"></param>
    /// <param name="codeSystemName"></param>
    /// <param name="displayName"></param>
    /// <param name="nullFlavor"></param>
    [Serializable]
    public class CodifiedGenderIdentity(CodifiedGenderIdentity.Selections selection,
        string? code,
        string? codeSystem,
        string? codeSystemName,
        string displayName,
        string? nullFlavor)
    {
        /// <summary>
        /// selections
        /// </summary>
        public enum Selections : int
        {
            /// <summary>
            /// not specified, which
            /// doesn't really have an 
            /// official code!
            /// </summary>
            Unspecified = 0,
            /// <summary>
            /// male male
            /// </summary>
            Male = 1,
            /// <summary>
            /// female female
            /// </summary>
            Female = 2,
            /// <summary>
            /// trans man
            /// </summary>
            FemaleToMale = 3,
            /// <summary>
            /// trans female
            /// </summary>
            MaleToFemale = 4,
            /// <summary>
            /// neither exclusively male nor female
            /// </summary>
            QueerBoth = 5,
            /// <summary>
            /// other please specify
            /// </summary>
            Other = 6,
            /// <summary>
            /// chooses not to disclose
            /// </summary>
            Undisclosed = 7
        }
        /// <summary>
        /// index to be stored in database to refer to this concept,
        /// as int
        /// </summary>
        public Selections Selection = selection;
        /// <summary>
        /// the code for this concept
        /// </summary>
        public string? Code = code;
        /// <summary>
        /// the identifier for the code system code is from
        /// </summary>
        public string? CodeSystem = codeSystem;
        /// <summary>
        /// the human readable name of the code system code is from
        /// </summary>
        public string? CodeSystemName = codeSystemName;
        /// <summary>
        /// human readable name of the concept
        /// </summary>
        public string DisplayName = displayName;
        /// <summary>
        /// the flavor of null, required if Code is null
        /// </summary>
        public string? NullFlavor = nullFlavor;

        /// <summary>
        /// the list of all choices
        /// </summary>
        public readonly static List<CodifiedGenderIdentity> All;

        /// <summary>
        /// static constructor builds the static library of selections
        /// </summary>
        static CodifiedGenderIdentity()
        {
            All =
            [
                //unknown
                new CodifiedGenderIdentity(Selections.Unspecified,
                    null,
                    null,
                    null,
                    "Unknown",
                    "UNK"),
                //other
                new CodifiedGenderIdentity(Selections.Other,
                    null,
                    null,
                    null,
                    "Other",
                    "OTH"),
                //declined
                new CodifiedGenderIdentity(Selections.Undisclosed,
                    null,
                    null,
                    null,
                    "Declined to specify",
                    "ASKU"),
                //male
                new CodifiedGenderIdentity(Selections.Male,
                    "446151000124109",
                    CommCodeSys.Get(CommCodeSys.Selections.SnomedCt)!.CodeSystem,  //! forgive null
                    //CommCodeSys.Get(CommCodeSys.Selections.SnomedCt).CodeSystem,
                    CommCodeSys.Get(CommCodeSys.Selections.SnomedCt)!.CodeSystemName,
                    "Identifies as male",
                    null),
                //female
                new CodifiedGenderIdentity(Selections.Female,
                    "446141000124107",
                    CommCodeSys.Get(CommCodeSys.Selections.SnomedCt)!.CodeSystem,
                    CommCodeSys.Get(CommCodeSys.Selections.SnomedCt)!.CodeSystemName,
                    "Identifies as female",
                    null),
                //female to male
                new CodifiedGenderIdentity(Selections.FemaleToMale,
                    "407377005",
                    CommCodeSys.Get(CommCodeSys.Selections.SnomedCt)!.CodeSystem,
                    CommCodeSys.Get(CommCodeSys.Selections.SnomedCt)!.CodeSystemName,
                    "Female to male, transgender man",
                    null),
                new CodifiedGenderIdentity(Selections.MaleToFemale,
                    "407376001",
                    CommCodeSys.Get(CommCodeSys.Selections.SnomedCt)!.CodeSystem,
                    CommCodeSys.Get(CommCodeSys.Selections.SnomedCt)!.CodeSystemName,
                    "Male to female, transgender woman",
                    null),
                new CodifiedGenderIdentity(Selections.QueerBoth,
                    "446131000124102",
                    CommCodeSys.Get(CommCodeSys.Selections.SnomedCt)!.CodeSystem,
                    CommCodeSys.Get(CommCodeSys.Selections.SnomedCt)!.CodeSystemName,
                    "Genderqueer, neither exclusively male nor female",
                    null),
            ];
        }
        /// <summary>
        /// unknown
        /// </summary>
        public static CodifiedGenderIdentity Unspecified
        {
            get
            {
                if (All[0].Selection == Selections.Unspecified)
                    return All[0];
                else
                    throw new Exception("Oops, found a programming error " +
                        "(the programmer's fault, not yours):  " +
                        "The Codified Gender Identity object is corrupted.");
            }
        }

        /// <summary>
        /// other
        /// </summary>
        public static CodifiedGenderIdentity Other
        {
            get
            {
                if (All[0].Selection == Selections.Other)
                    return All[0];
                else
                    throw new Exception("Oops, found a programming error " +
                        "(the programmer's fault, not yours):  " +
                        "The Codified Gender Identity object is corrupted.");
            }
        }
        /// <summary>
        /// undisclosed by person
        /// </summary>
        public static CodifiedGenderIdentity Undisclosed
        {
            get
            {
                if (All[0].Selection == Selections.Undisclosed)
                    return All[0];
                else
                    throw new Exception("Oops, found a programming error " +
                        "(the programmer's fault, not yours):  " +
                        "The Codified Gender Identity object is corrupted.");
            }
        }
        /// <summary>
        /// male
        /// </summary>
        public static CodifiedGenderIdentity Male
        {
            get
            {
                if (All[0].Selection == Selections.Male)
                    return All[0];
                else
                    throw new Exception("Oops, found a programming error " +
                        "(the programmer's fault, not yours):  " +
                        "The Codified Gender Identity object is corrupted.");
            }
        }
        /// <summary>
        /// female
        /// </summary>
        public static CodifiedGenderIdentity Female
        {
            get
            {
                if (All[0].Selection == Selections.Female)
                    return All[0];
                else
                    throw new Exception("Oops, found a programming error " +
                        "(the programmer's fault, not yours):  " +
                        "The Codified Gender Identity object is corrupted.");
            }
        }
        /// <summary>
        /// female to male
        /// </summary>
        public static CodifiedGenderIdentity FemaleToMale
        {
            get
            {
                if (All[0].Selection == Selections.FemaleToMale)
                    return All[0];
                else
                    throw new Exception("Oops, found a programming error " +
                        "(the programmer's fault, not yours):  " +
                        "The Codified Gender Identity object is corrupted.");
            }
        }
        /// <summary>
        /// male to female
        /// </summary>
        public static CodifiedGenderIdentity MaleToFemale
        {
            get
            {
                if (All[0].Selection == Selections.MaleToFemale)
                    return All[0];
                else
                    throw new Exception("Oops, found a programming error " +
                        "(the programmer's fault, not yours):  " +
                        "The Codified Gender Identity object is corrupted.");
            }
        }
        /// <summary>
        /// gender queer, not always one or the other
        /// </summary>
        public static CodifiedGenderIdentity QueerBoth
        {
            get
            {
                if (All[0].Selection == Selections.QueerBoth)
                    return All[0];
                else
                    throw new Exception("Oops, found a programming error " +
                        "(the programmer's fault, not yours):  " +
                        "The Codified Gender Identity object is corrupted.");
            }
        }

        /// <summary>
        /// get the result with given selection,
        /// or null if no match
        /// </summary>
        /// <param name="selection"></param>
        /// <returns></returns>
        public static CodifiedGenderIdentity? Get(Selections selection)
        {
            CodifiedGenderIdentity? result = null;
            //iterate through all subclasses defined here
            for (int i = 0; i < All.Count; i++)
            {
                if (All[i].Selection == selection)
                {
                    result = All[i];
                    break;
                }
            }
            return result;
        }
        ///// <summary>
        ///// serialized in vbar format, normally not used, buut
        ///// could be if one made a custom value 
        ///// </summary>
        ///// <returns></returns>
        //public string ToVBar()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append((int)Selection).ToString();
        //    sb.Append('|');
        //    sb.Append(Code);
        //    sb.Append('|');
        //    sb.Append(CodeSystem);
        //    sb.Append('|');
        //    sb.Append(CodeSystemName);
        //    sb.Append('|');
        //    sb.Append(DisplayName);
        //    sb.Append('|');
        //    sb.Append(NullFlavor);
        //    return sb.ToString();
        //}

        ///// <summary>
        ///// returns the object deserialized from vbar, normally
        ///// not used, but could be if one made a custom value
        ///// </summary>
        ///// <param name="vbar"></param>
        ///// <returns></returns>
        //public static CodifiedGenderIdentity FromVbar(string vbar)
        //{
        //    string[] parts = vbar.Split('|');
        //    if (parts.Length > 5)
        //    {
        //        int selection;
        //        if (!int.TryParse(parts[0], out selection))
        //        {
        //            selection = int.MinValue; //null
        //        }
        //        return new CodifiedGenderIdentity((Selections)selection,
        //            parts[1],
        //            parts[2],
        //            parts[3],
        //            parts[4],
        //            parts[5]);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        /// <summary>
        /// DisplayName
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return DisplayName;
        }
    }


    /// <summary>
    /// person's sexual orientation, enum (integer)
    /// defined for use in HL7v3 reporting
    /// </summary>
    /// <remarks>
    /// NOTICE:  Use static methods instead of 
    /// creating new objects for predefined objects
    /// The constructor requires initial
    /// values, which can be null
    /// </remarks>
    /// <param name="selection"></param>
    /// <param name="code"></param>
    /// <param name="codeSystem"></param>
    /// <param name="codeSystemName"></param>
    /// <param name="displayName"></param>
    /// <param name="nullFlavor"></param>
    [Serializable]
    public class CodifiedSexualOrientation(CodifiedSexualOrientation.Selections selection,
        string? code,
        string? codeSystem,
        string? codeSystemName,
        string displayName,
        string? nullFlavor)
    {
        /// <summary>
        /// selections
        /// </summary>
        public enum Selections : int
        {
            /// <summary>
            /// unknown or unspecified
            /// </summary>
            Unspecified = 0,
            /// <summary>
            /// other (please describe)
            /// </summary>
            Other = 1,
            /// <summary>
            /// choose not to disclose
            /// </summary>
            DeclinedToSay = 2,
            /// <summary>
            /// lesbian, gay or homosexual
            /// </summary>
            Homosexual = 3,
            /// <summary>
            /// straight or heterosexual
            /// </summary>
            Straight = 4,
            /// <summary>
            /// bisexual
            /// </summary>
            Bisexual = 5
        }
        /// <summary>
        /// index to be stored in database to refer to this concept,
        /// as int
        /// </summary>
        public Selections Selection = selection;
        /// <summary>
        /// the code for this concept
        /// </summary>
        public string? Code = code;
        /// <summary>
        /// the identifier for the code system code is from
        /// </summary>
        public string? CodeSystem = codeSystem;
        /// <summary>
        /// the human readable name of the code system code is from
        /// </summary>
        public string? CodeSystemName = codeSystemName;
        /// <summary>
        /// human readable name of the concept
        /// </summary>
        public string DisplayName = displayName;
        /// <summary>
        /// the flavor of null, required if Code is null
        /// </summary>
        public string? NullFlavor = nullFlavor;

        /// <summary>
        /// the list of all choices
        /// </summary>
        public static readonly List<CodifiedSexualOrientation> All;

        /// <summary>
        /// static constructor builds the static library of selections
        /// </summary>
        static CodifiedSexualOrientation()
        {
            All =
            [
                //unknown is preferred hl7 term
                new CodifiedSexualOrientation(Selections.Unspecified,
                    null,
                    null,
                    null,
                    "Unknown",
                    "UNK"),
                //other
                new CodifiedSexualOrientation(Selections.Other,
                    null,
                    null,
                    null,
                    "Other",
                    "OTH"),
                //declined
                new CodifiedSexualOrientation(Selections.DeclinedToSay,
                    null,
                    null,
                    null,
                    "Declined to specify",
                    "ASKU"),
                //homosexual
                new CodifiedSexualOrientation(Selections.Homosexual,
                    "38628009",
                    CommCodeSys.Get(CommCodeSys.Selections.SnomedCt)!.CodeSystem,
                    CommCodeSys.Get(CommCodeSys.Selections.SnomedCt)!.CodeSystemName,
                    "Lesbian, gay or homosexual",
                    null),
                //straight
                new CodifiedSexualOrientation(Selections.Straight,
                    "20430005",
                    CommCodeSys.Get(CommCodeSys.Selections.SnomedCt)!.CodeSystem,
                    CommCodeSys.Get(CommCodeSys.Selections.SnomedCt)!.CodeSystemName,
                    "Straight or heterosexual",
                    null),
                //bisexual
                new CodifiedSexualOrientation(Selections.Bisexual,
                    "42035005",
                    CommCodeSys.Get(CommCodeSys.Selections.SnomedCt)!.CodeSystem,
                    CommCodeSys.Get(CommCodeSys.Selections.SnomedCt)!.CodeSystemName,
                    "Bisexual",
                    null),
            ];
        }

        /// <summary>
        /// unspecified
        /// </summary>
        public static CodifiedSexualOrientation Unspecified
        {
            get
            {
                if (All[0].Selection == Selections.Unspecified)
                    return All[0];
                else
                    throw new Exception("Oops, found a programming error " +
                        "(the programmer's fault, not yours):  " +
                        "The Codified Sexual Orientation object is corrupted.");
            }
        }
        /// <summary>
        /// other
        /// </summary>
        public static CodifiedSexualOrientation Other
        {
            get
            {
                if (All[0].Selection == Selections.Other)
                    return All[0];
                else
                    throw new Exception("Oops, found a programming error " +
                        "(the programmer's fault, not yours):  " +
                        "The Codified Sexual Orientation object is corrupted.");
            }
        }
        /// <summary>
        /// person declined to disclose
        /// </summary>
        public static CodifiedSexualOrientation Undisclosed
        {
            get
            {
                if (All[0].Selection == Selections.DeclinedToSay)
                    return All[0];
                else
                    throw new Exception("Oops, found a programming error " +
                        "(the programmer's fault, not yours):  " +
                        "The Codified Sexual Orientation object is corrupted.");
            }
        }
        /// <summary>
        /// homosexual
        /// </summary>
        public static CodifiedSexualOrientation Homosexual
        {
            get
            {
                if (All[0].Selection == Selections.Homosexual)
                    return All[0];
                else
                    throw new Exception("Oops, found a programming error " +
                        "(the programmer's fault, not yours):  " +
                        "The Codified Sexual Orientation object is corrupted.");
            }
        }
        /// <summary>
        /// straight
        /// </summary>
        public static CodifiedSexualOrientation Straight
        {
            get
            {
                if (All[0].Selection == Selections.Straight)
                    return All[0];
                else
                    throw new Exception("Oops, found a programming error " +
                        "(the programmer's fault, not yours):  " +
                        "The Codified Sexual Orientation object is corrupted.");
            }
        }
        /// <summary>
        /// bisexual
        /// </summary>
        public static CodifiedSexualOrientation Bisexual
        {
            get
            {
                if (All[0].Selection == Selections.Bisexual)
                    return All[0];
                else
                    throw new Exception("Oops, found a programming error " +
                        "(the programmer's fault, not yours):  " +
                        "The Codified Sexual Orientation object is corrupted.");
            }
        }

        /// <summary>
        /// get the result with given selection,
        /// or null if no match
        /// </summary>
        /// <param name="selection"></param>
        /// <returns></returns>
        public static CodifiedSexualOrientation? Get(Selections selection)
        {
            CodifiedSexualOrientation? result = null;
            //iterate through all subclasses defined here
            for (int i = 0; i < All.Count; i++)
            {
                if (All[i].Selection == selection)
                {
                    result = All[i];
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// serialized in vbar format
        /// </summary>
        /// <returns></returns>
        public string ToVBar()
        {
            StringBuilder sb = new ();
            sb.Append((int)Selection).ToString();
            sb.Append('|');
            sb.Append(Code);
            sb.Append('|');
            sb.Append(CodeSystem);
            sb.Append('|');
            sb.Append(CodeSystemName);
            sb.Append('|');
            sb.Append(DisplayName);
            sb.Append('|');
            sb.Append(NullFlavor);
            return sb.ToString();
        }

        /// <summary>
        /// returns the object serialized to vbar
        /// </summary>
        /// <param name="vbar"></param>
        /// <returns></returns>
        public static CodifiedSexualOrientation? FromVbar(string vbar)
        {
            string[] parts = vbar.Split('|');
            if (parts.Length > 5)
            {
                //int selection;
                if (!int.TryParse(parts[0], out int selection))
                {
                    selection = int.MinValue; //null
                }
                return new CodifiedSexualOrientation((Selections)selection,
                    parts[1],
                    parts[2],
                    parts[3],
                    parts[4],
                    parts[5]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// DisplayName
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return DisplayName;
        }
    }


    /// <summary>
    /// code system used in HL7 reporting
    /// defined for use in HL7v3 reporting
    /// </summary>
    /// <remarks>
    /// NOTICE:  Use static Get() method instead of 
    /// creating new objects for predefined objects
    /// The constructor requires initial
    /// values, which can be null
    /// </remarks>
    /// <param name="selection"></param>
    /// <param name="codeSystem"></param>
    /// <param name="codeSystemName"></param>
    [Serializable]
    public class CommCodeSys(CommCodeSys.Selections selection,
        string? codeSystem,
        string? codeSystemName)
    {
        /// <summary>
        /// selections enum (integer)
        /// </summary>
        public enum Selections : int
        {
            /// <summary>
            /// unknown or unspecified
            /// </summary>
            Unknown,
            /// <summary>
            /// snomed
            /// </summary>
            SnomedCt,
            /// <summary>
            /// rx norm
            /// </summary>
            RxNorm,
            /// <summary>
            /// loinc
            /// </summary>
            LOINC,
            /// <summary>
            /// ama cpt codes
            /// </summary>
            CPT,
            /// <summary>
            /// national drug code
            /// </summary>
            NDC,
            /// <summary>
            /// immunization codes
            /// </summary>
            CVX,
            /// <summary>
            /// icd10
            /// </summary>
            ICD10,
            /// <summary>
            /// CDC Race and Ethnicity
            /// </summary>
            CDC_RaceEthnicity

        }
        /// <summary>
        /// index to be stored in database to refer to this concept,
        /// as int
        /// </summary>
        public Selections? Selection = selection;
        /// <summary>
        /// the identifier for the code system 
        /// </summary>
        public string? CodeSystem = codeSystem;
        /// <summary>
        /// the human readable name of the code system 
        /// </summary>
        public string? CodeSystemName = codeSystemName;

        /// <summary>
        /// the list of all choices
        /// </summary>
        public static readonly CommCodeSys[] All;

        /// <summary>
        /// static constructor builds the static library of selections
        /// </summary>
        static CommCodeSys()
        {
            All = new CommCodeSys[9];
            //unknown
            All[0] = (new CommCodeSys(Selections.Unknown,
                null,
                null));
            //snomed
            All[1] = (new CommCodeSys(Selections.SnomedCt,
                "2.16.840.1.113883.6.96",
                "SNOMED CT"));
            //rxnorm
            All[2] = (new CommCodeSys(Selections.RxNorm,
                "2.16.840.1.113883.6.88",
                "RxNorm"));
            //loinc
            All[3] = (new CommCodeSys(Selections.LOINC,
                "2.16.840.1.113883.6.1",
                "LOINC"));
            //cpt
            All[4] = (new CommCodeSys(Selections.CPT,
                "2.16.840.1.113883.6.12",
                "CPT"));
            //ndc
            All[5] = (new CommCodeSys(Selections.NDC,
                "2.16.840.1.113883.6.69",
                null));
            //cvx
            All[6] = (new CommCodeSys(Selections.CVX,
                "2.16.840.1.113883.12.292",
                "CVX"));
            //icd10
            All[7] = (new CommCodeSys(Selections.ICD10,
                "2.16.840.1.113883.6.3",
                "ICD10"));
            //cdc race and ethnicity
            All[8] = (new CommCodeSys(Selections.CDC_RaceEthnicity,
                "2.16.840.1.113883.6.238",
                "CDC Race and Ethnicity"));
            //notice: if you add more please redimension the array!
        }

        /// <summary>
        /// get the result with given selection,
        /// or null if no match
        /// </summary>
        /// <param name="selection"></param>
        /// <returns></returns>
        public static CommCodeSys? Get(Selections selection)
        {
            CommCodeSys? result = null;
            //iterate through all subclasses defined here
            for (int i = 0; i < All.Length; i++)
            {
                if (All[i].Selection == selection)
                {
                    result = All[i];
                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// unknown
        /// </summary>
        public static CommCodeSys Unknown
        {
            get { return All[0]; }
        }
        /// <summary>
        /// snomed ct
        /// </summary>
        /// <returns></returns>
        public static CommCodeSys SnowmedCt
        {
            get { return All[1]; }
        }
        ///rxnorm
        public static CommCodeSys RxNorm
        {
            get { return All[2]; }
        }
        ///loinc
        public static CommCodeSys LOINC
        {
            get { return All[3]; }
        }
        ///cpt
        public static CommCodeSys CPT
        {
            get { return All[4]; }
        }
        ///ndc
        public static CommCodeSys NDC
        {
            get { return All[5]; }
        }
        ///cvx
        public static CommCodeSys CVX
        {
            get { return All[6]; }
        }
        ///icd10
        public static CommCodeSys ICD10
        {
            get { return All[7]; }
        }
        ///cdc race and eth
        public static CommCodeSys CDC_RaceEthnicity
        {
            get { return All[8]; }
        }

        /// <summary>
        /// Code System Name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return CodeSystemName ?? "undefined CodeSystemName";
        }
    }

    /// <summary>
    /// Language, codified by ISO 639-1 or 2
    /// </summary>
    [Serializable]
    public class CodifiedLanguage
    {
        /// <summary>
        /// readable name of the language
        /// </summary>
        public string Name = string.Empty;
        /// <summary>
        /// two letter code if exists in ISO 6391,
        /// otherwise 3 letter code of ISO 639-2
        /// </summary>
        public string ISOCode = string.Empty;
        /// <summary>
        /// Language codified by ISO 639-1 or 2
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isoCode"></param>
        public CodifiedLanguage(string name, string isoCode)
        {
            Name = name;
            ISOCode = isoCode;
        }
        /// <summary>
        /// Language codified by ISO 639-1 or 2
        /// </summary
        public CodifiedLanguage()
        {

        }
        /// <summary>
        /// name, then code in parentheses
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new ();
            sb.Append(Name.Trim());
            sb.Append(" (");
            sb.Append(ISOCode.Trim());
            sb.Append(')');
            return sb.ToString();
        }
        /// <summary>
        /// parses string which should be name followed
        /// by the code in parentheses, eg English (EN)
        /// but may be 
        /// empty values if invalid format
        /// </summary>
        /// <returns></returns>
        public static CodifiedLanguage FromString(string nameAndCode)
        {
            CodifiedLanguage result = new ();
            string[] parts = nameAndCode.Split('(');
            if (parts.Length > 1)
            {
                result.Name = parts[0].Trim();
                result.ISOCode = parts[1].Trim().Replace(")", string.Empty);
            }
            else if (parts.Length > 0)
            {
                result.Name = parts[0].Trim();
            }
            return result;
        }
        /// <summary>
        /// the default value for language if not specified.
        /// </summary>
        public static CodifiedLanguage Unspecified
        {
            get
            {
                return new CodifiedLanguage("Unspecified", "");
            }
        }
        /// <summary>
        /// person declined to specify preferred language
        /// </summary>
        public static CodifiedLanguage Declined
        {
            get
            {
                return new CodifiedLanguage("Declined", "");
            }
        }

        /// <summary>
        /// list of common languages
        /// </summary>
        public static List<CodifiedLanguage> CommonSet
        {
            get
            {
                List<CodifiedLanguage> result =
                    [
                        CodifiedLanguage.Unspecified,
                        new CodifiedLanguage("English", "EN"),
                        new CodifiedLanguage("Spanish", "ES"),
                        new CodifiedLanguage("French", "FR"),
                        new CodifiedLanguage("Portuguese", "PT"),
                        CodifiedLanguage.Declined,
                    ];
                return result;
            }
        }
    }

    ///// <summary>
    ///// race, codified by CDC Race and Ethnicity Code Set
    ///// </summary>
    //public class CodifiedEthnicity
    //{
    //    /// <summary>
    //    /// the ethnicity code
    //    /// </summary>
    //    public string Code = string.Empty;
    //    /// <summary>
    //    /// text  value
    //    /// </summary>
    //    public string DisplayName = string.Empty;
    //    /// <summary>
    //    /// ID of system code is defined in 
    //    /// </summary>
    //    public string CodeSystem = string.Empty;
    //    /// <summary>
    //    /// name of system code is defined in
    //    /// </summary>
    //    public string CodeSystemName = string.Empty;

    //    /// <summary>
    //    /// code details for enumerated ethnicity
    //    /// </summary>
    //    /// <param name="ethnicity"></param>
    //    /// <returns></returns>
    //    public static CodifiedEthnicity FromEthnicityEnum(EthnicityEnumDepreciated ethnicity)
    //    {
    //        CodifiedEthnicity result = new CodifiedEthnicity();
    //        switch (ethnicity)
    //        {
    //            case EthnicityEnumDepreciated.Hispanic:
    //                result.Code = "2135-2";
    //                result.CodeSystem = "2.16.840.1.113883.6.238";
    //                result.CodeSystemName = "Race & Ethnicity - CDC";
    //                result.DisplayName = "Hispanic or Latino";
    //                break;
    //            case EthnicityEnumDepreciated.NotHispanic:
    //                result.Code = "2186-5";
    //                result.CodeSystem = "2.16.840.1.113883.6.238";
    //                result.CodeSystemName = "Race & Ethnicity - CDC";
    //                result.DisplayName = "Not Hispanic or Latino";
    //                break;
    //            //warning: no CDC code for unspecified
    //            case EthnicityEnumDepreciated.Unspecified:
    //                result.Code = "";
    //                result.CodeSystem = "";
    //                result.CodeSystemName = "";
    //                result.DisplayName = "(unspecified)";
    //                break;
    //            default:
    //                throw new Exception("Programmer needs to specify codes for the ethnicity " +
    //                    Enum.GetName(typeof(EthnicityEnumDepreciated), ethnicity));
    //        }
    //        return result;
    //    }//fromethnicityenum
    //}//EthnicityCode

    /// <summary>
    /// race, codified by CDC Race and Ethnicity Code Set
    /// </summary>
    [Serializable]
    public class CodifiedRace : IComparable
    {
        /// <summary>
        /// database id which is changeable if data table
        /// is updated in future!
        /// </summary>
        public int RaceID = int.MinValue;
        /// <summary>
        /// Unique identifier in CDC codeset
        /// </summary>
        public string UniqueID = string.Empty;
        /// <summary>
        /// hierarchical code, useful for searching by 
        /// race categories as well as specific race
        /// </summary>
        public string HierarchicalCode = string.Empty;
        /// <summary>
        /// human readable name
        /// </summary>
        protected string? _name = null;
        /// <summary>
        /// human readable name
        /// </summary>
        public string Name
        {
            get
            {
                if (_name == null)
                {
                    if (_raceGroup != null)
                        _name = _raceGroup.DisplayName;
                }
                return _name ?? "Undefined";
            }
            set
            {
                _name = value;
            }
        }
        /// <summary>
        /// one of 5 defined categories for US, 
        /// or unknown or declined which don't have codes
        /// </summary>
        protected CodifiedRaceGroup? _raceGroup = null;
        /// <summary>
        /// one of the 5 defined categories for US , 
        /// or unknown or declined, which don't have codes
        /// (returns null if neither RaceGroup nor HierchicalCode
        /// have been set yet)
        /// </summary>
        public CodifiedRaceGroup? RaceGroup
        {
            get
            {
                if (_raceGroup == null)
                {
                    if (string.IsNullOrEmpty(HierarchicalCode))
                    {
                        _raceGroup = null; //can't tell
                    }
                    else if (HierarchicalCode.StartsWith(CodifiedRaceGroup.Other.DisplayName))
                    {
                        _raceGroup = CodifiedRaceGroup.Other;
                    }
                    else if (HierarchicalCode.StartsWith(CodifiedRaceGroup.Declined.DisplayName))
                    {
                        _raceGroup = CodifiedRaceGroup.Declined;
                    }
                    else if (HierarchicalCode.StartsWith(CodifiedRaceGroup.Unspecified.DisplayName))
                    {
                        _raceGroup = CodifiedRaceGroup.Unspecified;
                    }
                    else if (HierarchicalCode.StartsWith("R1"))
                    {
                        _raceGroup = CodifiedRaceGroup.NativeAmericanIndian;
                    }
                    else if (HierarchicalCode.StartsWith("R2"))
                    {
                        _raceGroup = CodifiedRaceGroup.Asian;
                    }
                    else if (HierarchicalCode.StartsWith("R3"))
                    {
                        _raceGroup = CodifiedRaceGroup.Black;
                    }
                    else if (HierarchicalCode.StartsWith("R4"))
                    {
                        _raceGroup = CodifiedRaceGroup.PacificIslander;
                    }
                    else if (HierarchicalCode.StartsWith("R5"))
                    {
                        _raceGroup = CodifiedRaceGroup.White;
                    }
                    else
                    {
                        //shouldn't ever get here
                        //_raceGroup = CodifiedRaceGroup.FromRaceEnum(RaceEnumDepreciated.Other);
                        _raceGroup = null;
                    }
                }
                return _raceGroup;
            }
            set
            {
                _raceGroup = value;
            }
        }
        /// <summary>
        /// the default for unspecified race
        /// </summary>
        public static CodifiedRace Unspecified
        {
            get
            {
                return CodifiedRace.FromRaceGroup(
                    CodifiedRaceGroup.Unspecified);
                //CodifiedRace result = new CodifiedRace();
                //result.RaceGroup = CodifiedRaceGroup.Unspecified;
                //return result;
            }
        }
        /// <summary>
        /// person declined to specify
        /// </summary>
        public static CodifiedRace Declined
        {
            get
            {
                return CodifiedRace.FromRaceGroup(
                    CodifiedRaceGroup.Declined);
                //CodifiedRace result = new CodifiedRace();
                //result.RaceGroup = CodifiedRaceGroup.Declined;
                //return result;
            }
        }
        /// <summary>
        /// other race - probably shouldn't ever use...
        /// </summary>
        public static CodifiedRace Other
        {
            get
            {
                return CodifiedRace.FromRaceGroup(
                    CodifiedRaceGroup.Other);
                //CodifiedRace result = new CodifiedRace();
                //result.RaceGroup = CodifiedRaceGroup.Other;
                //return result;
            }
        }
        /// <summary>
        /// returns the codified race for the special cases that the race
        /// is only specified to one of the 5 major race groups.
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public static CodifiedRace FromRaceGroup(CodifiedRaceGroup group)
        {
            CodifiedRace result = new()
            {
                HierarchicalCode = group.HeirarchicalCode,
                RaceGroup = group,
                Name = group.DisplayName,
                UniqueID = group.Code
            };
            return result;
        }

        /// <summary>
        /// id of the code system for CDA communications
        /// </summary>
        public static readonly string  CodeSystem = "2.16.840.1.113883.6.238";
        // but could also use 2.16.840.1.113883.5.104 which is hl7's 'v3 Code System Race'
        //at http://hl7.org/fhir/ValueSet/v3-Race
        /// <summary>
        /// name of code system for CDA communications
        /// </summary>
        public static readonly string CodeSystemName = "Race & Ethnicity - CDC";
        /// <summary>
        /// serialize the Heirarchical Codes of list of races each
        /// preceded by a vertical bar to facilitate queries,
        /// or empty string if none
        /// </summary>
        /// <param name="races"></param>
        /// <returns></returns>
        public static string ToVBar(List<CodifiedRace> races)
        {
            StringBuilder sb = new();
            if ((races != null) && (races.Count > 0))
            {
                for (int i = 0; i < races.Count; i++)
                {
                    sb.Append('|');
                    if (races[i].UniqueID == CodifiedRace.Declined.UniqueID)
                        sb.Append("(declined)");
                    else
                        sb.Append(races[i].HierarchicalCode);
                }
            }
            return sb.ToString();
        }
        ///////////// <summary>
        ///////////// return list of races from serialized heirarchical codes
        ///////////// each preceeded by vertical bars, or empty list if none
        ///////////// </summary>
        ///////////// <param name="serializedToVbar"></param>
        ///////////// <param name="mm3Data"></param>
        ///////////// <returns></returns>
        //////////public static List<CodifiedRace> FromVBar(string serializedToVbar, IMM4Data mm3Data)
        //////////{
        //////////    List<CodifiedRace> result = new List<CodifiedRace>();
        //////////    if (!string.IsNullOrEmpty(serializedToVbar))
        //////////    {
        //////////        string[] parts = serializedToVbar.Split('|');
        //////////        //each segment starts with bar so part 0 should be empty
        //////////        if (parts.Length > 1)
        //////////        {
        //////////            for (int i = 1; i < parts.Length; i++)
        //////////            {
        //////////                CodifiedRace cr = FromHeirarchicalCode(
        //////////                    parts[i],
        //////////                    mm3Data);
        //////////                if (cr != null)
        //////////                {
        //////////                    result.Add(cr);
        //////////                }
        //////////            }//from for each code
        //////////        }//from if any codes
        //////////    }
        //////////    return result;
        //////////}

        ///////////// <summary>
        ///////////// return the first (should only be one) race matching
        ///////////// given heirarchical code (can include specification that
        ///////////// person declined to say, or 'other' as well.)
        ///////////// </summary>
        ///////////// <param name="hCode"></param>
        ///////////// <param name="mm3Data"></param>
        ///////////// <returns></returns>
        //////////private static CodifiedRace FromHeirarchicalCode(string hCode,
        //////////    IMM4Data mm4Data)
        //////////{
        //////////    bool foundMatch = false; //unless found
        //////////    CodifiedRace result = null;
        //////////    if (!string.IsNullOrEmpty(hCode))
        //////////    {
        //////////        if (hCode.Trim().ToLower() == "(declined)")
        //////////        {
        //////////            result = Declined;
        //////////            foundMatch = true;
        //////////        }
        //////////        if (!foundMatch)
        //////////        {
        //////////            // see if is standard race group or category
        //////////            for (int i = 0; i < CodifiedRaceGroup.Selections.Length; i++)
        //////////            {
        //////////                if (CodifiedRaceGroup.Selections[i].HeirarchicalCode == hCode.Trim())
        //////////                {
        //////////                    result = FromRaceGroup(CodifiedRaceGroup.Selections[i]);
        //////////                    foundMatch = true;
        //////////                }
        //////////            }
        //////////        }
        //////////        //but if still unfound
        //////////        if (!foundMatch)
        //////////        {
        //////////            //then have to search database
        //////////            CodifiedRace[] matches = mm3Data.ActorsData.SelectRaces(hCode);
        //////////            if (matches.Length > 0)
        //////////            {
        //////////                result = matches[0]; // should only be one
        //////////            }
        //////////        }
        //////////    }//from if code exists
        //////////    return result;
        //////////}

        /// <summary>
        /// /just the name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// display name, code and standard race group
        /// </summary>
        /// <returns></returns>
        public string ToStringLong()
        {
            StringBuilder sb = new();
            sb.Append(Name);
            sb.Append(" (");
            sb.Append(UniqueID);
            sb.Append(") ");
            if (HierarchicalCode.Length > 2)
            {
                //then this is a specific race within a grop
                sb.Append('<');
                sb.Append(RaceGroup!.DisplayName);
                sb.Append(" (");
                sb.Append(RaceGroup.Code);
                sb.Append(")>");
            }
            return sb.ToString();
        }

        /// <summary>
        /// sort by name
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object? obj)
        {
            if (obj is CodifiedRace race)
                return this.Name.CompareTo(race.Name);
            else
                return 0;
        }
    }


    /// <summary>
    /// ethnicity, codified by CDC Race and Ethnicity Code Set
    /// </summary>
    [Serializable]
    public class CodifiedEthnicity : IComparable
    {
        /// <summary>
        /// database id which is changeable if data table
        /// is updated in future!
        /// </summary>
        public int EthnicityID = int.MinValue;
        /// <summary>
        /// Unique identifier in CDC codeset
        /// </summary>
        public string UniqueID = string.Empty;
        /// <summary>
        /// hierarchical code, useful for searching by 
        /// ethnicity categories as well as specific ethnicity
        /// </summary>
        public string HierarchicalCode = string.Empty;
        /// <summary>
        /// human readable name
        /// </summary>
        protected string? _name = null;
        /// <summary>
        /// human readable name
        /// </summary>
        public string? Name
        {
            get
            {
                if (_name == null)
                {
                    if (_ethnicityGroup != null)
                        _name = _ethnicityGroup.DisplayName;
                }
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        /// <summary>
        /// one of 2 defined categories for US, 
        /// or unknown or declined which don't have codes
        /// </summary>
        protected CodifiedEthnicityGroup? _ethnicityGroup = null;
        /// <summary>
        /// one of the 5 defined categories for US , 
        /// or unknown or declined, which don't have codes
        /// (returns null if neither EthnicityGroup nor HierchicalCode
        /// have been set yet)
        /// </summary>
        public CodifiedEthnicityGroup? EthnicityGroup
        {
            get
            {
                if (_ethnicityGroup == null)
                {
                    if (string.IsNullOrEmpty(HierarchicalCode))
                    {
                        _ethnicityGroup = null; //can't tell
                    }
                    //else if (HierarchicalCode.StartsWith(CodifiedEthnicityGroup.Other.DisplayName))
                    //{
                    //    _ethnicityGroup = CodifiedEthnicityGroup.Other;
                    //}
                    else if (HierarchicalCode.StartsWith(CodifiedEthnicityGroup.DeclinedToSay.DisplayName))
                    {
                        _ethnicityGroup = CodifiedEthnicityGroup.DeclinedToSay;
                    }
                    else if (HierarchicalCode.StartsWith(CodifiedEthnicityGroup.Unspecified.DisplayName))
                    {
                        _ethnicityGroup = CodifiedEthnicityGroup.Unspecified;
                    }
                    else if (HierarchicalCode.StartsWith("E1"))
                    {
                        _ethnicityGroup = CodifiedEthnicityGroup.Hispanic;
                    }
                    else if (HierarchicalCode.StartsWith("E2"))
                    {
                        _ethnicityGroup = CodifiedEthnicityGroup.NotHispanic;
                    }

                    else
                    {
                        //shouldn't ever get here
                        _ethnicityGroup = CodifiedEthnicityGroup.Unspecified;
                    }
                }
                return _ethnicityGroup;
            }
            set
            {
                _ethnicityGroup = value;
            }
        }
        /// <summary>
        /// the default for unspecified race
        /// </summary>
        public static CodifiedEthnicity Unspecified
        {
            get
            {
                return CodifiedEthnicity.FromEthnicityGroup(
                    CodifiedEthnicityGroup.Unspecified);
                //CodifiedEthnicity result = new CodifiedEthnicity();
                //result.EthnicityGroup = CodifiedEthnicityGroup.Unspecified;
                //return result;
            }
        }
        /// <summary>
        /// person declined to specify
        /// </summary>
        public static CodifiedEthnicity Declined
        {
            get
            {
                return CodifiedEthnicity.FromEthnicityGroup(
                    CodifiedEthnicityGroup.Declined);
                //CodifiedEthnicity result = new CodifiedEthnicity();
                //result.EthnicityGroup = CodifiedEthnicityGroup.DeclinedToSay;
                //return result;
            }
        }

        /// <summary>
        /// returns the codified ethnicity for the special cases that the ethnicity
        /// is only specified to one of the 2 major ethnicity groups.
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public static CodifiedEthnicity FromEthnicityGroup(CodifiedEthnicityGroup group)
        {
            CodifiedEthnicity result = new()
            {
                HierarchicalCode = group.HeirarchicalCode,
                EthnicityGroup = group,
                Name = group.DisplayName,
                UniqueID = group.Code
            };
            return result;
        }

        /// <summary>
        /// id of the code system for CDA communications
        /// </summary>
        public static readonly string CodeSystem = "2.16.840.1.113883.6.238";
        // but could also use 2.16.840.1.113883.5.50 which is hl7's 'v3 Code System Ethnicity'
        //at http://hl7.org/fhir/ValueSet/v3-Race
        /// <summary>
        /// name of code system for CDA communications
        /// </summary>
        public static readonly string CodeSystemName = "Race & Ethnicity - CDC";
        /// <summary>
        /// serialize the Heirarchical Codes of list of ethnicities each
        /// preceded by a vertical bar to facilitate queries,
        /// or empty string if none
        /// </summary>
        /// <param name="eths"></param>
        /// <returns></returns>
        public static string ToVBar(List<CodifiedEthnicity> eths)
        {
            StringBuilder sb = new();
            if ((eths != null) && (eths.Count > 0))
            {
                for (int i = 0; i < eths.Count; i++)
                {
                    sb.Append('|');
                    if (eths[i].UniqueID == CodifiedEthnicity.Declined.UniqueID)
                        sb.Append("(declined)");
                    else
                        sb.Append(eths[i].HierarchicalCode);
                }
            }
            return sb.ToString();
        }
        ///////////// <summary>
        ///////////// return list of ethnicities from serialized heirarchical codes
        ///////////// each preceeded by vertical bars, or empty list if none
        ///////////// </summary>
        ///////////// <param name="serializedToVbar"></param>
        ///////////// <param name="mm3Data"></param>
        ///////////// <returns></returns>
        //////////public static List<CodifiedEthnicity> FromVBar(string serializedToVbar, IMM3Data mm3Data)
        //////////{
        //////////    List<CodifiedEthnicity> result = new List<CodifiedEthnicity>();
        //////////    if (!string.IsNullOrEmpty(serializedToVbar))
        //////////    {
        //////////        string[] parts = serializedToVbar.Split('|');
        //////////        //each segment starts with bar so part 0 should be empty
        //////////        if (parts.Length > 1)
        //////////        {
        //////////            for (int i = 1; i < parts.Length; i++)
        //////////            {
        //////////                CodifiedEthnicity ce = FromHeirarchicalCode(
        //////////                    parts[i],
        //////////                    mm3Data);
        //////////                if (ce != null)
        //////////                {
        //////////                    result.Add(ce);
        //////////                }
        //////////            }//from for each code
        //////////        }//from if any codes
        //////////    }
        //////////    return result;
        //////////}

        ///////////// <summary>
        ///////////// return the first (should only be one) ethnicity matching
        ///////////// given heirarchical code (can include specification that
        ///////////// person declined to say, or 'other' as well.)
        ///////////// </summary>
        ///////////// <param name="hCode"></param>
        ///////////// <param name="mm3Data"></param>
        ///////////// <returns></returns>
        //////////private static CodifiedEthnicity FromHeirarchicalCode(string hCode,
        //////////    IMM3Data mm3Data)
        //////////{
        //////////    bool foundMatch = false; //unless found
        //////////    CodifiedEthnicity result = null;
        //////////    if (!string.IsNullOrEmpty(hCode))
        //////////    {
        //////////        if (hCode.Trim().ToLower() == "(declined)")
        //////////        {
        //////////            result = CodifiedEthnicity.Declined;
        //////////            foundMatch = true;
        //////////        }
        //////////        if (!foundMatch)
        //////////        {
        //////////            // see if is standard race group or category
        //////////            for (int i = 0; i < CodifiedEthnicityGroup.Selections.Length; i++)
        //////////            {
        //////////                if (CodifiedEthnicityGroup.Selections[i].HeirarchicalCode == hCode.Trim())
        //////////                {
        //////////                    result = FromEthnicityGroup(CodifiedEthnicityGroup.Selections[i]);
        //////////                    foundMatch = true;
        //////////                }
        //////////            }
        //////////        }
        //////////        //if still not found...
        //////////        if (!foundMatch)
        //////////        {
        //////////            //then have to search database
        //////////            CodifiedEthnicity[] matches = mm3Data.ActorsData.SelectEthnicities(hCode);
        //////////            if (matches.Length > 0)
        //////////            {
        //////////                result = matches[0]; // should only be one
        //////////            }
        //////////        }
        //////////    }//from if code exists
        //////////    return result;
        //////////}

        /// <summary>
        /// display name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name ?? "Undefined";
        }

        /// <summary>
        /// sort by name
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object? obj)
        {
            if (obj is CodifiedEthnicity ce)
                return this.Name!.CompareTo(ce.Name);
            else
                return 0;
        }
    }



    /// <summary>
    /// code values for RaceEnum
    /// with EnumFieldDescription from
    /// Code System(s):Race and Ethnicity - CDC 2.16.840.1.113883.6.238
    /// IF specified group (not valid if unspecified or other)
    /// </summary>
    /// <remarks>
    /// constructor for RaceGroupCode
    /// </remarks>
    /// <param name="code"></param>
    /// <param name="displayName"></param>
    /// <param name="codeSystem"></param>
    /// <param name="codeSystemName"></param>
    /// <param name="heirarchicalCode"></param>
    [Serializable]
    public class CodifiedRaceGroup(string code,
        string codeSystem,
        string codeSystemName,
        string displayName,
        string heirarchicalCode)
    {
        /// <summary>
        /// the race code
        /// </summary>
        public string Code = code;
        /// <summary>
        /// text value
        /// </summary>
        public string DisplayName = displayName;
        /// <summary>
        /// ID of system code is defined in 
        /// </summary>
        public string CodeSystem = codeSystem;
        /// <summary>
        /// name of system code is defined in
        /// </summary>
        public string CodeSystemName = codeSystemName;
        /// <summary>
        /// the heirarchical code relating it to Races data table
        /// </summary>
        public string HeirarchicalCode = heirarchicalCode;
        /// <summary>
        /// native american
        /// </summary>
        public static readonly CodifiedRaceGroup NativeAmericanIndian;
        /// <summary>
        /// asian
        /// </summary>
        public static readonly CodifiedRaceGroup Asian;
        /// <summary>
        /// black
        /// </summary>
        public static readonly CodifiedRaceGroup Black;
        /// <summary>
        /// pacific islander
        /// </summary>
        public static readonly CodifiedRaceGroup PacificIslander;
        /// <summary>
        /// white
        /// </summary>
        public static readonly CodifiedRaceGroup White;
        /// <summary>
        /// declined too specify is NOT a code
        /// </summary>
        public static readonly CodifiedRaceGroup Declined;
        /// <summary>
        /// other race
        /// </summary>
        public static readonly CodifiedRaceGroup Other;
        /// <summary>
        /// unspecified is NOT a code
        /// </summary>
        public static readonly CodifiedRaceGroup Unspecified;
        /// <summary>
        /// array of all the race groups
        /// </summary>
        public static readonly CodifiedRaceGroup[] Selections;

        /// <summary>
        /// static constructor
        /// </summary>
        static CodifiedRaceGroup()
        {
            Selections = new CodifiedRaceGroup[8];
            NativeAmericanIndian = new CodifiedRaceGroup("1002-5",
                "2.16.840.1.113883.6.238",
                "Race & Ethnicity - CDC",
                "American Indian or Alaska Native"
                , "R1");
            Selections[1] = NativeAmericanIndian;
            Asian = new CodifiedRaceGroup("2028-9",
                "2.16.840.1.113883.6.238",
                "Race & Ethnicity - CDC",
                "Asian",
                "R2");
            Selections[2] = Asian;
            Black = new CodifiedRaceGroup("2054-5",
                 "2.16.840.1.113883.6.238",
                 "Race & Ethnicity - CDC",
                 "Black or African American",
                 "R3");
            Selections[3] = Black;
            PacificIslander = new CodifiedRaceGroup("2076-8",
                "2.16.840.1.113883.6.238",
                "Race & Ethnicity - CDC",
                "Native Hawaiian or Other Pacific Islander",
                "R4");
            Selections[4] = PacificIslander;
            White = new CodifiedRaceGroup("2106-3",
                "2.16.840.1.113883.6.238",
                "Race & Ethnicity - CDC",
                "White",
                "R5");
            Selections[5] = White;
            Other = new CodifiedRaceGroup("2131-1",
                "2.16.840.1.113883.6.238",
                "Race & Ethnicity - CDC",
                "Other Race",
                "R9");
            Selections[6] = Other;
            Unspecified = new CodifiedRaceGroup("",
                "",
                "",
                "(unspecified)"
                , string.Empty);
            Selections[0] = Unspecified;
            Declined = new CodifiedRaceGroup("(declined)",
                "",
                "",
                "(Declined to specify)",
                "(declined)");
            Selections[7] = Declined;
        }

        /// <summary>
        /// code details for enumerated race, or the
        /// illegal value of 
        /// (unspecified) if unspecified
        /// </summary>
        /// <param name="race"></param>
        /// <returns></returns>
        public static CodifiedRaceGroup FromRaceEnum(RaceEnumDepreciated race)
        {
            return race switch
            {
                RaceEnumDepreciated.Asian => Asian,
                RaceEnumDepreciated.Black => Black,
                RaceEnumDepreciated.NativeAmericanIndian => NativeAmericanIndian,
                RaceEnumDepreciated.Other => Other,
                RaceEnumDepreciated.PacificIslander => PacificIslander,
                RaceEnumDepreciated.Unspecified => Unspecified,
                RaceEnumDepreciated.White => White,
                RaceEnumDepreciated.DeclinedToSpecify => Declined,
                _ => throw new Exception("Programmer needs to define details for this category, " +
                        Enum.GetName(typeof(RaceEnumDepreciated), race))
            };
        }
        /// <summary>
        /// display name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return DisplayName;
        }
    }

    /// <summary>
    /// code values for Ethnicity groups
    /// with EnumFieldDescription from
    /// Code System(s):Race and Ethnicity - CDC 2.16.840.1.113883.6.238
    /// IF specified ethnicity (not valid if unspecified or other)
    /// </summary>
    /// <remarks>
    /// constructor for CodifiedEthnicityGroup
    /// </remarks>
    /// <param name="code"></param>
    /// <param name="displayName"></param>
    /// <param name="codeSystem"></param>
    /// <param name="codeSystemName"></param>
    /// <param name="heirarchicalCode"></param>
    [Serializable]
    public class CodifiedEthnicityGroup(string code,
        string codeSystem,
        string codeSystemName,
        string displayName,
        string heirarchicalCode)
    {
        /// <summary>
        /// the  code
        /// </summary>
        public string Code = code;
        /// <summary>
        /// text value
        /// </summary>
        public string DisplayName = displayName;
        /// <summary>
        /// ID of system code is defined in 
        /// </summary>
        public string CodeSystem = codeSystem;
        /// <summary>
        /// name of system code is defined in
        /// </summary>
        public string CodeSystemName = codeSystemName;
        /// <summary>
        /// the heirarchical code relating it to Ethnicity data table
        /// </summary>
        public string HeirarchicalCode = heirarchicalCode;
        /// <summary>
        /// native american
        /// </summary>
        public static readonly CodifiedEthnicityGroup Unspecified;
        /// <summary>
        /// asian
        /// </summary>
        public static readonly CodifiedEthnicityGroup NotHispanic;
        /// <summary>
        /// black
        /// </summary>
        public static readonly CodifiedEthnicityGroup Hispanic;
        /// <summary>
        /// pacific islander
        /// </summary>
        public static readonly CodifiedEthnicityGroup DeclinedToSay;

        /// <summary>
        /// array of all the race groups
        /// </summary>
        public static readonly CodifiedEthnicityGroup[] Selections;
        /// <summary>
        /// static constructor
        /// </summary>
        static CodifiedEthnicityGroup()
        {
            Selections = new CodifiedEthnicityGroup[4];
            Unspecified = new CodifiedEthnicityGroup("",
                "",
                "",
                "(unspecified)"
                , "");
            Selections[0] = Unspecified;
            NotHispanic = new CodifiedEthnicityGroup("2186-5 ",
                "2.16.840.1.113883.6.238",
                "Race & Ethnicity - CDC",
                "Not Hispanic",
                "E2");
            Selections[1] = NotHispanic;
            Hispanic = new CodifiedEthnicityGroup("2135-2 ",
                 "2.16.840.1.113883.6.238",
                 "Race & Ethnicity - CDC",
                 "Hispanic or Latino",
                 "E1");
            Selections[2] = Hispanic;
            DeclinedToSay = new CodifiedEthnicityGroup("(declined)",
                "",
                "",
                "(Declined to specify)",
                "(declined)");
            Selections[3] = DeclinedToSay;
        }

        /// <summary>
        /// declined to specify
        /// </summary>
        public static CodifiedEthnicityGroup Declined
        {
            get
            {
                return Selections[3];
            }
        }

        /// <summary>
        /// display name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return DisplayName;
        }
    }


}



