using eBroker.Shared.Enum;
using System.ComponentModel.DataAnnotations;

namespace eBroker.DAL.Models
{
    public class TransactionOrderDB
    {
        [Key]
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int EquityId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public OrderType Type { get; set; }
    }
}
