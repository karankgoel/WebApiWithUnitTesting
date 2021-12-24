using eBroker.Shared.DTO;

namespace eBroker.BLL
{
    public interface ITradeService
    {
        bool BuyEquity(TransactionOrder order);
        bool SellEquity(TransactionOrder order);
    }
}
