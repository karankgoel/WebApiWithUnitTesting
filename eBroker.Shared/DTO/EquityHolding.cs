using System.Diagnostics.CodeAnalysis;

namespace eBroker.Shared.DTO
{
    [ExcludeFromCodeCoverage]
    public class EquityHolding
    {
        public int UserId { get; set; }
        public int EquityId { get; set; }
        public int Quantity { get; set; }
    }
}
