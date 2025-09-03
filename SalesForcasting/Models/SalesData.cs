using Microsoft.ML.Data;

namespace SalesForcasting.Models
{
    public class SalesData
    {
        [LoadColumn(2)]
        public DateTime Date { get; set; }

        [LoadColumn(3)]
        public float Sales { get; set; }
    }
}
