using System.Configuration;

namespace MZcms.Entity
{
    public static class Connection
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;

        public static string ConnectionString { get { return connectionString; } }
    }
}
