using eBroker.DAL.Models;

namespace eBroker.DAL.Repositories
{
    public class TransactionOrderRepository: BaseRepository<TransactionOrderDB>, ITransactionOrderRepository
    {
        public TransactionOrderRepository(EBrokerContext dbContext): base(dbContext)
        {
        }
    }
}
