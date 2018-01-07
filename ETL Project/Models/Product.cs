using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL_Project.Models
{
    public class Product
    {
        [PrimaryKey]
        public string Code { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Producer { get; set; }
    }
}
