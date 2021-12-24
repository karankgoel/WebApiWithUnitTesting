using System.Diagnostics.CodeAnalysis;

namespace eBroker.Shared.DTO
{
    [ExcludeFromCodeCoverage]
    public class Equity
    {
        public int EquityId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
