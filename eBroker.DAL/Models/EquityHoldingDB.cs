using System.ComponentModel.DataAnnotations;

namespace eBroker.DAL.Models
{
    public class EquityHoldingDB
    {
        [Key]
        public int EquityHoldingId { get; set; }
        public int UserId { get; set; }
        public int EquityId { get; set; }
        public int Quantity { get; set; }
    }
}
