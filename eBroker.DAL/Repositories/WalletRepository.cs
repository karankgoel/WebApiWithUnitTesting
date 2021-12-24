using eBroker.DAL.Models;

namespace eBroker.DAL.Repositories
{
    public class WalletRepository: BaseRepository<WalletDB>, IWalletRepository
    {
        public WalletRepository(EBrokerContext dbContext): base(dbContext)
        {
        }
    }
}
