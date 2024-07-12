using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Data;
using MM4Common;
using System.Data;

namespace MM4DataSql
{
    internal class DataClass
    {
        protected ConnectionProvider _cnProvider;
        /// <summary>
        /// actors constuctor
        /// </summary>
        public DataClass()
        {
            _cnProvider = new ConnectionProvider();
        }

        /// <summary>
        /// selects users in brief format, allows including guests optionally
        /// </summary>
        /// <param name="onlyIfActive">if true returns only users for which 
        /// IsActive = true;
        /// if false returns all users</param>
        /// <param name="minimumPermissions">return all users who have at least these permissions.
        /// I.e. UserPermissions.None returns users regardless of UserPermission state</param>
        /// <param name="onlyIfNotGuest">if true it omits users where IsGuest is true.</param>
        /// <returns></returns>
        public UserBrief[] SelectUserBriefs(bool onlyIfActive,
            UserPermissions minimumPermissions,
            bool onlyIfNotGuest)
        {
            //request data from database and package as array
            int startSize = 50; //start size of array
            UserBrief[] results = new UserBrief[startSize]; //array of results
            UserBrief[] tempArray; //holds temporary array when redimensioning
            UserBrief result; //to be assigned each record
            int count = 0; //keep count of records returned
            using (SqlConnection cn = new SqlConnection(_cnProvider.MM4ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Select_Users", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //specify paramaters
                    cmd.Parameters.AddWithValue("@OnlyIfActive", onlyIfActive);
                    if (minimumPermissions != UserPermissions.None)
                        cmd.Parameters.AddWithValue("@PermissionsMask", (int)minimumPermissions);
                    cmd.Parameters.AddWithValue("@OnlyIfNotGuest", onlyIfNotGuest);

                    //read data
                    cn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //increment counter
                            count++; //check to see if need to redimension array
                            if (count > results.Length)
                            {
                                tempArray = results;
                                results = new UserBrief[results.Length + startSize];
                                tempArray.CopyTo(results, 0);
                            }

                            //read new result
                            result = new UserBrief();
                            readUserBrief(reader, result);
                            results[count - 1] = result;
                        }
                    } //from using reader
                } //from using cmd
            } //from using cn
            //redimension array
            tempArray = results;
            results = new UserBrief[count]; Array.Copy(tempArray, 0, results, 0, count);
            //return
            return results;
        }

        /// <summary>
        /// assign members to UserBrief - this is used by every method reading User from 
        /// database EXCEPT the Login() method in MM3DataClass!!
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="result"></param>
        private void readUserBrief(SqlDataReader reader, UserBrief result)
        {
            //read new result
            if (reader["UserID"] != DBNull.Value)
                result.UserID = (System.Int32)reader["UserID"];
            if (reader["ShortName"] != DBNull.Value)
                result.ShortName = ((System.String)reader["ShortName"]).Trim();
            if (reader["Login"] != DBNull.Value)
                result.Login = ((System.String)reader["Login"]).Trim();
            if (reader["Permissions"] != DBNull.Value)
                result.Permissions = (MM3Common.UserPermissions)reader["Permissions"];
            if (reader["PrivacyExclusions"] != DBNull.Value)
                result.PrivacyExclusions = (MM3Common.PrivacyFlags)reader["PrivacyExclusions"];
            if (reader["DateLastUpdatedPwd"] != DBNull.Value)
                result.DateLastUpdatedPwd = (System.DateTime)reader["DateLastUpdatedPwd"];
            if (reader["Comment"] != DBNull.Value)
                result.Comment = ((System.String)reader["Comment"]).Trim();
            if (reader["NPI"] != DBNull.Value)
                result.NPI = (int)reader["NPI"];
            if (reader["OtherID"] != DBNull.Value)
                result.SetOtherIDs(((System.String)reader["OtherID"]).Trim());
            if (reader["DisplayName"] != DBNull.Value)
                result.DisplayName = ((System.String)reader["DisplayName"]).Trim();
            if (reader["Salutation"] != DBNull.Value)
                result.Salutation = ((System.String)reader["Salutation"]).Trim();
            if (reader["DateOfBirth"] != DBNull.Value)
                result.DateOfBirth = (System.DateTime)reader["DateOfBirth"];
            if (reader["Gender"] != DBNull.Value)
                result.Gender = (MM3Common.GenderType)reader["Gender"];
            if (reader["IsActive"] != DBNull.Value)
                result.IsActive = (bool)reader["IsActive"];
            if (reader["RcopiaID"] != DBNull.Value)
                if (MM3Common.Medications.Medication.Use64bitRcopiaID)
                {
                    result.RcopiaID = (long)reader["RcopiaID"];
                }
                else
                {
                    result.RcopiaID = (long)((int)reader["RcopiaID"]);
                }
            if (reader["IsGuest"] != DBNull.Value)
                result.IsGuest = ((System.Boolean)reader["IsGuest"]);
        }
    }
}
