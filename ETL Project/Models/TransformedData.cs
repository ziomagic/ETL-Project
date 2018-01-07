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

namespace ETL_Project.Models
{
    public class TransformedData
    {
        public Product Product { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
