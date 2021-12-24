using eBroker.Shared.Enum;
using System.Diagnostics.CodeAnalysis;

namespace eBroker.Shared.DTO
{
    [ExcludeFromCodeCoverage]
    public class TransactionOrder
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int EquityId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public OrderType Type {get;set;}
    }
}
