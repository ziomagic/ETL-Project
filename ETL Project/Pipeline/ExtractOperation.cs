using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using HtmlAgilityPack;
using System.Diagnostics;
using ETL_Project.Utils;
using System.Windows;

namespace ETL_Project.Pipeline
{
    public class ExtractOperation : IPipelineOperation
    {
        public const int AsyncRequestCount = 5;
        public const string CeneoUrl = "https://www.ceneo.pl";

        public object HandleData(object input)
        {
            var productId = input as string;

            var reviewPage = GetHtmlDocumentById(productId);
            var specPage = GetHtmlDocumentById(productId, "#tab=spec");

            var reviewCount = GetReviewCount(reviewPage.DocumentNode);
            if(reviewCount == 0)
            {
                throw new KeyNotFoundException();
            }
            var reviewsPerPage = GetReviewsPerPage(reviewPage.DocumentNode);

            var output = new List<HtmlDocument>();

            output.Add(specPage);
            output.Add(reviewPage);

            if (reviewCount > reviewsPerPage)
            {
                var pagesCount = (reviewCount / reviewsPerPage) + 1;

                for (int i = 2; i <= pagesCount; i++)
                {
                    output.Add(GetHtmlDocumentById(productId, $"/opinie-{i}"));
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

        private HtmlDocument GetHtmlDocumentById(string productId, string tab = "#tab=reviews")
        {
            var urlStr = $"{CeneoUrl}/{productId}{tab}";
            Logger.Log($"Parsing url: {urlStr}");
            
            var doc = new HtmlWeb();
            return doc.Load(urlStr);
        }
    }
}
