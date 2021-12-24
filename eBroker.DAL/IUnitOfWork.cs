using eBroker.DAL.Repositories;

namespace eBroker.DAL
{
    public interface IUnitOfWork
    {
        IEquityRepository EquityRepository { get; }
        IEquityHoldingRepository EquityHoldingRepository { get; }
        ITransactionOrderRepository TransactionOrderRepository { get; }
        IWalletRepository WalletRepository { get; }
        void SaveChanges();
    }
}
