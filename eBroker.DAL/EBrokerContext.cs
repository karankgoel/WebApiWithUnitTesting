using eBroker.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace eBroker.DAL
{
    public class EBrokerContext: DbContext
    {
        public EBrokerContext(DbContextOptions options): base(options)
        {
        }

        public DbSet<EquityDB> Equities { get; set; }
        public DbSet<EquityHoldingDB> EquityHoldings { get; set; }
        public DbSet<TransactionOrderDB> TransactionOrders { get; set; }
        public DbSet<WalletDB> Wallet { get; set; }

        public new int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
