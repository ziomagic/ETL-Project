using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using HtmlAgilityPack;
using System.Diagnostics;

namespace ETL_Project.Pipeline
{
    public class ExtractOperation : IPipelineOperation
    {
        public const int AsyncRequestCount = 5;
        public const string CeneoUrl = "https://www.ceneo.pl";

        public object HandleData(object input)
        {
            var productId = input as string;
            var urlStr = $"{CeneoUrl}/{productId}#tab=reviews";

            var doc = new HtmlWeb();
            var mainPageDoc = doc.Load(urlStr);

            var reviewCount = GetReviewCount(mainPageDoc.DocumentNode);
            var reviewsPerPage = GetReviewsPerPage(mainPageDoc.DocumentNode);

            var output = new List<HtmlDocument>();
            output.Add(mainPageDoc);

            if (reviewCount > reviewsPerPage)
            {
                var pagesCount = (reviewCount / reviewsPerPage) + 1;

                for (int i = 2; i <= pagesCount; i++)
                {
                    var url = $"{CeneoUrl}/{productId}/opinie{i}";

                    doc = new HtmlWeb();
                    output.Add(doc.Load(url));
                    Debug.WriteLine($"Parsing url: {url}");
                }
            }

            return output;
        }

        private int GetReviewCount(HtmlNode node)
        {
            var childNode = node.SelectSingleNode("//span[@itemprop='reviewCount']");
            if (childNode == null)
            {
                return 0;
            }

            return int.Parse(childNode.InnerText.Trim());
        }

        private int GetReviewsPerPage(HtmlNode node)
        {
            var childNode = node.SelectNodes("//*[@class='review-box js_product-review']");
            return childNode.Count;
        }
    }
}
