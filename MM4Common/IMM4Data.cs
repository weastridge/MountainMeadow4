using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM4Common
{
    /// <summary>
    /// interface for database calls
    /// </summary>
    public interface IMM4Data
    {



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
        UserBrief[] SelectUserBriefs(bool onlyIfActive,
            UserPermissions minimumPermissions,
            bool onlyIfNotGuest);
    }
}
