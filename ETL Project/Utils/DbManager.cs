using ETL_Project.Models;
using ServiceStack.OrmLite;
using System.IO;

namespace ETL_Project.Utils
{
    /// <summary>
    /// Statyczna metoda odpowiedzialna za zarządzanie bazą dabnych
    /// </summary>
    public static class DbManager
    {
        /// <summary>
        /// Zwraca połączenie do bazy danych
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Metoda zwracająca ścieżkę do bazy danych
        /// </summary>
        /// <returns></returns>
        public static string GetDbPath()
        {
            return Directory.GetCurrentDirectory() + "\\etl.db";
        }
    }
}
