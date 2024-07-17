
using System.Security.Cryptography.X509Certificates;
using System;
using System.Windows;
using System.Runtime.CompilerServices;
using Microsoft.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MM4DataSql
{
    /// <summary>
    /// provides SqlServer connection
    /// </summary>
    public class ConnectionProvider
    {
        // notes:
        // requires NuGet Microsoft.Data.SqlClient, which replaced the older System.Data.SqlClient namespace
        // also requires Nuget Microsoft.Extension.Configuration.UserSecrets for temporary storage of database password until formal component is built.

        /// <summary>
        /// storage for connection string
        /// </summary>
        protected string? _mm4ConnectionString;
        internal string? GetMM4ConnectionString(bool forceReload)
        {
            if ((_mm4ConnectionString == null) ||
                forceReload)
                return BuildConnectionString();
            else
                return _mm4ConnectionString;
        }

        /// <summary>
        /// secret value, only used for testing, not needed by program...
        /// </summary>
        public static string MySecretValue
        {
            get
            {
                string result = "";
                var config = new Microsoft.Extensions.Configuration.ConfigurationBuilder().AddUserSecrets<ConnectionProvider>().Build();
                string? secretResult = null;
                if (config != null)
                {
                    secretResult = config["MySecret"];
                }
                if (secretResult != null)
                {
                    result = secretResult;
                }
                return result;
            }
        }

        /// <summary>
        /// connection string builder
        /// </summary>
        protected static string? BuildConnectionString()

        {
            string? result;
            SqlConnectionStringBuilder builder = [];
            var config = new ConfigurationBuilder().AddUserSecrets<ConnectionProvider>().Build();
            //access 
            builder.DataSource = "localhost,2008";
            builder.InitialCatalog = "MM4";
            builder.UserID = "MM4Login";
            builder.Password = "UnknownPassword";
            builder.Encrypt = true;
            //this is not safe!  Need to install a certificate to the server instead of making it trust any certificate...
            builder.TrustServerCertificate = true;
            if (config["MM4DataSqlPwd"] != null)
            {
                builder.Password = config["MM4DataSqlPwd"];
            }
            result = builder.ToString();
            return result;
        }
    }

}
