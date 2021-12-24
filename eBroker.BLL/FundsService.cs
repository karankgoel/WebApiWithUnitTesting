using eBroker.DAL;
using System.Linq;

namespace eBroker.BLL
{
    public class FundsService : IFundsService
    {
        private readonly IUnitOfWork _uow;

        public FundsService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public bool AddFunds(int userId, double amount)
        {
            var wallet = _uow.WalletRepository.Get(x => x.UserId == userId).FirstOrDefault();

            if (wallet != null)
            {
                if(amount > 100000)
                {
                    amount = 0.9995 * amount;
                }

                wallet.Amount += amount;

                _uow.WalletRepository.Update(wallet);
                _uow.SaveChanges();

                return true;
            }
            return false;
        }
    }
}
