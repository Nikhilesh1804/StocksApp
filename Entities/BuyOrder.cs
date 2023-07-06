using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class BuyOrder
    {
        [Key]
        public Guid BuyOrderID { get; set; }
        [Required]
        public string StockSymbol { get; set; }
        [Required]
        public string StockName { get; set; }
        
        public DateTime DateAndTimeOfOrder { get; set; }

        [Range(minimum:1, maximum: 100000, ErrorMessage = "Quantity should be between 1 and 100000")]
        public uint Quantity { get; set; }

        [Range(minimum:1, maximum: 10000, ErrorMessage = "Price should be between 1 and 10000")]
        public double Price { get; set; }
    }
}