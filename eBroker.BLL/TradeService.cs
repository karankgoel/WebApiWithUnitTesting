using eBroker.DAL;
using eBroker.DAL.Models;
using eBroker.Shared.DTO;
using System;
using System.Linq;

namespace eBroker.BLL
{
    public class TradeService : ITradeService
    {
        private readonly IUnitOfWork _uow;
        private readonly IWrapper _wrapper;

        public TradeService(IUnitOfWork uow, IWrapper wrapper)
        {
            _uow = uow;
            _wrapper = wrapper;
        }

        public bool BuyEquity(TransactionOrder order)
        {
            if (_wrapper.IsMarketOpen())
            {
                var buyOrderValue = order.Quantity * order.UnitPrice;
                var wallet = _uow.WalletRepository.Get(x => x.UserId == order.UserId, includeProperties: "EquityHoldings").FirstOrDefault();

                if (wallet != null)
                {
                    var fundsAvailable = wallet.Amount;

                    if (fundsAvailable > buyOrderValue)
                    {
                        wallet.Amount = fundsAvailable - buyOrderValue;

                        if (wallet.EquityHoldings.Any(x => x.EquityId == order.EquityId && x.UserId == order.UserId))
                        {
                            wallet.EquityHoldings.FirstOrDefault(x => x.EquityId == order.EquityId && x.UserId == order.UserId).Quantity += order.Quantity;
                        }
                        else
                        {
                            wallet.EquityHoldings.Add(new DAL.Models.EquityHoldingDB()
                            {
                                EquityId = order.EquityId,
                                Quantity = order.Quantity,
                                UserId = order.UserId
                            });
                        }

                        _uow.WalletRepository.Update(wallet);
                        _uow.TransactionOrderRepository.Insert(new TransactionOrderDB()
                        {
                            UserId = order.UserId,
                            EquityId = order.EquityId,
                            Quantity = order.Quantity,
                            Type = order.Type,
                            UnitPrice = order.UnitPrice
                        });

                        _uow.SaveChanges();
                        return true;
                    }
                    else
                    {
                        throw new Exception("Insufficient balance in wallet");
                    }
                }
                else
                {
                    throw new Exception("Wallet does not exist");
                }
            }
            else
            {
                throw new Exception("Market not open");
            }

        }

        public bool SellEquity(TransactionOrder order)
        {
            if (_wrapper.IsMarketOpen())
            { 
                var wallet = _uow.WalletRepository.Get(x => x.UserId == order.UserId, "EquityHoldings").FirstOrDefault();

                if (wallet != null)
                {
                    if (wallet.EquityHoldings != null && wallet.EquityHoldings.Any(x => x.UserId == order.UserId && x.EquityId == order.EquityId && x.Quantity >= order.Quantity))
                    {
                        var sellOrderValue = order.Quantity * order.UnitPrice;

                        wallet.Amount += GetAmountAfterBrokerage(sellOrderValue);

                        wallet.EquityHoldings.FirstOrDefault(x => x.EquityId == order.EquityId && x.UserId == order.UserId).Quantity -= order.Quantity;

                        _uow.WalletRepository.Update(wallet);
                        _uow.TransactionOrderRepository.Insert(new TransactionOrderDB()
                        {
                            UserId = order.UserId,
                            EquityId = order.EquityId,
                            Quantity = order.Quantity,
                            Type = order.Type,
                            UnitPrice = order.UnitPrice
                        });

                        _uow.SaveChanges();
                        return true;
                    }
                    else
                    {
                        throw new Exception("Does not possess enough equity");
                    }
                }
                else
                {
                    throw new Exception("Wallet does not exist");
                }
            }
            else
            {
                throw new Exception("Market not open");
            }
        }

        private double GetAmountAfterBrokerage(double amount)
        {
            double minBrokerage = 20;

            if(0.0005* amount > minBrokerage)
            {
                return 0.9995 * amount;
            }
            else
            {
                return amount - minBrokerage;
            }
        }
    }
}
