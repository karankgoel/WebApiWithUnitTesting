using eBroker.DAL.Models;

namespace eBroker.DAL.Repositories
{
    public class EquityRepository : BaseRepository<EquityDB>, IEquityRepository
    {
        public EquityRepository(EBrokerContext dbContext) : base(dbContext)
        {
        }
    }
}
