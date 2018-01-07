using ETL_Project.Models;
using ServiceStack.OrmLite;
using System.IO;

namespace ETL_Project.Utils
{
    public static class DbManager
    {
        public static OrmLiteConnectionFactory GetDbFactory()
        {
            var databasePath = $"Data Source={GetDbPath()};";

            var dbFactory = new OrmLiteConnectionFactory(databasePath, SqliteDialect.Provider);
            using (var db = dbFactory.Open())
            {
                db.CreateTableIfNotExists<Product>();
                db.CreateTableIfNotExists<Review>();
            }

            return dbFactory;
        }

        public static string GetDbPath()
        {
            return Directory.GetCurrentDirectory() + "\\etl.db";
        }
    }
}
