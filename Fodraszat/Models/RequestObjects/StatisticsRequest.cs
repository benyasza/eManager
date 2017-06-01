using System;

namespace Fodraszat.Models.RequestObjects
{
    public class StatisticsRequest
    {
        public int Hairdresser { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}