using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL_Project.Models
{
    /// <summary>
    /// Model recenzji
    /// </summary>
    public class Review
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public string Comment { get; set; }
        public string Author { get; set; }
        public bool Positive { get; set; }
        public DateTime Date { get; set; }
        public float Score { get; set; }
        public float MaxScore { get; set; }
        public int VotedUsefull { get; set; }
        public int VoteNotUsefull { get; set; }
        public List<string> Pros { get; set; }
        public List<string> Const { get; set; }
    }
}
