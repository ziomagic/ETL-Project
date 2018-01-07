using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using HtmlAgilityPack;
using ETL_Project.Models;
using System.Diagnostics;
using ETL_Project.Utils;

namespace ETL_Project.Pipeline
{
    public class TransformOperation : IPipelineOperation
    {
        public object HandleData(object input)
        {
            Logger.Log("Starting transform operation.");
            var documents = input as List<HtmlDocument>;
            var output = new TransformedData()
            {
                Reviews = new List<Review>()
            };
            
            output.Product = GetProduct(documents[0]);

            documents.RemoveAt(0);

            foreach (var document in documents)
            {
                var reviewNodes = document.DocumentNode.SelectNodes("//*[@class='review-box js_product-review']");
                if (reviewNodes == null)
                {
                    Debug.WriteLine($"Url has no reviews.");
                    return output;
                }

                foreach (var node in reviewNodes)
                {
                    var review = GetReviewFromNode(node);
                    review.ProductCode = output.Product.Code;
                    output.Reviews.Add(review);
                }
            }

            return output;
        }

        private Review GetReviewFromNode(HtmlNode node)
        {
            var review = new Review();
            ParseBaseInfo(review, node);
            ParseProsCons(review, node);
            ParseUsefulness(review, node);
            return review;
        }

        private void ParseBaseInfo(Review review, HtmlNode node)
        {
            review.Author = node.SelectSingleNode(".//*[@class='reviewer-name-line']")?.InnerText.Trim();
            review.Comment = node.SelectSingleNode(".//*[@class='product-review-body']")?.InnerText.Trim();

            var dateTime = node.SelectSingleNode(".//*[@class='review-time']/time");
            if (dateTime != null)
            {
                review.Date = DateTime.Parse(dateTime
                    .GetAttributeValue("datetime", DateTime.Now.ToShortDateString()));
            }

            var score = node.SelectSingleNode(".//*[@class='review-score-count']")?.InnerText.Trim();
            if(score != null)
            {
                var scoreSplit = score.Split('/');
                review.Score = float.Parse(scoreSplit[0]);
                review.MaxScore = float.Parse(scoreSplit[1]);
            }
        }

        private void ParseProsCons(Review review, HtmlNode node)
        {
            var nodeProsCons = node.SelectSingleNode(".//*[@class='product-review-pros-cons']");

            var prosNodes = node.SelectNodes(".//*[@class='pros-cell']/ul/li");
            if (prosNodes != null)
            {
                var pros = new List<string>();
                foreach (var li in prosNodes)
                {
                    pros.Add(li.InnerText.Trim());
                }
            }

            var consNodes = node.SelectNodes(".//*[@class='cons-cell']/ul/li");
            if (consNodes != null)
            {
                var cons = new List<string>();
                foreach (var li in consNodes)
                {
                    cons.Add(li.InnerText.Trim());
                }
            }
        }

        private void ParseUsefulness(Review review , HtmlNode node)
        {
            var votedYes = node.SelectSingleNode(".//*[contains(@class, 'vote-yes')]");
            review.VotedUsefull = votedYes.GetAttributeValue("data-total-vote", 0);

            var votedNo = node.SelectSingleNode(".//*[contains(@class, 'vote-no')]");
            review.VoteNotUsefull = votedNo.GetAttributeValue("data-total-vote", 0);
        }

        private Product GetProduct(HtmlDocument doc)
        {
            var node = doc.DocumentNode;
            var productName = node.SelectSingleNode("//*[@class='product-content']/h1").InnerHtml.Trim();

            var code = productName.Substring(productName.LastIndexOf(' '));

            var specs = doc.DocumentNode.SelectNodes("//*[@id='productTechSpecs']/div/table/tbody/tr/td/ul/li");

            return new Product
            {
                Code = code,
                Name = productName.Replace(code, ""),
                Producer = specs[0].InnerText.Trim(),
                Type = specs[1].InnerText.Trim(),
            };
        }
    }
}
