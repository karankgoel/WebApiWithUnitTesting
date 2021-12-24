using eBroker.DAL.Models;

namespace eBroker.DAL.Repositories
{
    public class EquityHoldingRepository: BaseRepository<EquityHoldingDB>, IEquityHoldingRepository
    {
        public EquityHoldingRepository(EBrokerContext dbContext): base(dbContext)
        {
        }
    }
}
