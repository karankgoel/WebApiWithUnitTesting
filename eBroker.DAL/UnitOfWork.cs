using eBroker.DAL.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace eBroker.DAL
{
    [ExcludeFromCodeCoverage]
    public class UnitOfWork: IUnitOfWork
    {
        private EBrokerContext _context;
        private IEquityRepository equityRepository;
        private IEquityHoldingRepository equityHoldingRepository;
        private ITransactionOrderRepository transactionOrderRepository;
        private IWalletRepository walletRepository;

        public UnitOfWork(EBrokerContext context)
        {
            _context = context;
        }

        public IEquityRepository EquityRepository
        {
            get
            {
                if(this.equityRepository == null)
                {
                    this.equityRepository = new EquityRepository(_context);
                }
                return equityRepository;
            }
        }
        public IEquityHoldingRepository EquityHoldingRepository
        {
            get
            {
                if (this.equityHoldingRepository == null)
                {
                    this.equityHoldingRepository = new EquityHoldingRepository(_context);
                }
                return equityHoldingRepository;
            }
        }
        public ITransactionOrderRepository TransactionOrderRepository
        {
            get
            {
                if (this.transactionOrderRepository == null)
                {
                    this.transactionOrderRepository = new TransactionOrderRepository(_context);
                }
                return transactionOrderRepository;
            }
        }
        public IWalletRepository WalletRepository
        {
            get
            {
                if (this.walletRepository == null)
                {
                    this.walletRepository = new WalletRepository(_context);
                }
                return walletRepository;
            }
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
