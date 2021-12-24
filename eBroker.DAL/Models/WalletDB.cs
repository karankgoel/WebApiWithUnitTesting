using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eBroker.DAL.Models
{
    public class WalletDB
    {
        [Key]
        public int WalletId { get; set; }
        public int UserId { get; set; }
        public double Amount { get; set; }

        public IList<EquityHoldingDB> EquityHoldings { get; set; }
    }
}
