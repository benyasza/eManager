using System.Collections.Generic;

namespace Fodraszat.Models.ResponseObjects
{
    public class StatisticsResponse
    {
        public List<StatisticsDetails> StatisticsDetails { get; set; }

        public int TotalPrice { get; set; }

        public int TotalOverhead { get; set; }

        public int TotalSalary { get; set; }
    }
}