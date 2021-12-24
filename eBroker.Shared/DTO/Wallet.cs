using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace eBroker.Shared.DTO
{
    [ExcludeFromCodeCoverage]
    public class Wallet
    {
        public int UserId { get; set; }
        public double Amount { get; set; }

        public IList<EquityHolding> EquityHoldings { get; set; }
    }
}
