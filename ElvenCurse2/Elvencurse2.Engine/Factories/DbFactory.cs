using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace Elvencurse2.Engine.Factories
{
    public static class DbFactory
    {
        public static IDbConnection GetConnection(string connectionString = "")
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            }

            return new MySqlConnection(connectionString);
        }
    }
}
