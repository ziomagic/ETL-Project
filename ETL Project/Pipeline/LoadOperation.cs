using ETL_Project.Models;
using ServiceStack.OrmLite;
using System.Windows;
using ETL_Project.Utils;

namespace ETL_Project.Pipeline
{
    /// <summary>
    /// Klasa odpowiedzialna za zapisanie danych do bazy
    /// </summary>
    public class LoadOperation : IPipelineOperation
    {
        /// <summary>
        /// Metoda odpowiedzialna za zapisanie danych do bazy
        /// </summary>
        /// <param name="input">Dane przetransformowane w kroku transformacji</param>
        /// <returns></returns>
        public object HandleData(object input)
        {
            Logger.Log("Starting loading operation.");

            var data = input as TransformedData;
            var dbFactory = DbManager.GetDbFactory();

            using (var db = dbFactory.Open())
            {
                // Remove data if it was inserted before
                db.Delete<Product>((p) => p.Code == data.Product.Code);
                db.Delete<Review>((r) => r.ProductCode == data.Product.Code);

                db.Insert(data.Product);
                db.InsertAll(data.Reviews);
            }

            MessageBox.Show($"{data.Reviews.Count} reviews added to database.");
            return null;
        }

    }
}
