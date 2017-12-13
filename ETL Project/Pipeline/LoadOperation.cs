using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using HtmlAgilityPack;
using ETL_Project.Models;
using System.IO;
using ServiceStack.OrmLite;
using System.Windows;

namespace ETL_Project.Pipeline
{
    public class LoadOperation : IPipelineOperation
    {
        public object HandleData(object input)
        {
            var reviews = input as List<Review>;
            var dbFactory = InitDatabase();
            using (var db = dbFactory.Open())
            {
                db.InsertAll(reviews);
            }

            return null;
        }

        private OrmLiteConnectionFactory InitDatabase()
        {
            var databasePath = $"Data Source={GetDbPath()};";

            var dbFactory = new OrmLiteConnectionFactory(databasePath, SqliteDialect.Provider);
            using (var db = dbFactory.Open())
            {
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
