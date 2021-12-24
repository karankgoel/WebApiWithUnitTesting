using eBroker.BLL;
using eBroker.DAL;
using eBroker.DAL.Models;
using eBroker.DAL.Repositories;
using eBroker.Shared.DTO;
using eBroker.Shared.Enum;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace eBroker.Tests.Services
{
    public class TradeServiceTest
    {
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IWalletRepository> _walletRepository;
        private readonly Mock<ITransactionOrderRepository> _transactionOrderRepository;
        private readonly Mock<IWrapper> _wrapper;

        public TradeServiceTest()
        {
            _uow = new Mock<IUnitOfWork>();
            _walletRepository = new Mock<IWalletRepository>();
            _transactionOrderRepository = new Mock<ITransactionOrderRepository>();
            _wrapper = new Mock<IWrapper>();
        }

        private TradeService GetServiceInstance()
        {
            return new TradeService(_uow.Object, _wrapper.Object);
        }

        [Fact]
        public void BuyEquity_MarketNotOpen()
        {
            _wrapper.Setup(x => x.IsMarketOpen()).Returns(false);

            var service = GetServiceInstance();

            var ex = Assert.Throws<Exception>(() => service.BuyEquity(new TransactionOrder()));
            Assert.Equal("Market not open", ex.Message);
        }

        [Fact]
        public void BuyEquity_WalletDoesNotExist()
        {
            _uow.Setup(x => x.WalletRepository).Returns(_walletRepository.Object);
            _wrapper.Setup(x => x.IsMarketOpen()).Returns(true);
            _walletRepository.Setup(x => x.Get(It.IsAny<Expression<Func<WalletDB, bool>>>(), It.IsAny<string>()))
                .Returns(new List<WalletDB>());

            var service = GetServiceInstance();

            var ex = Assert.Throws<Exception>(() => service.BuyEquity(new TransactionOrder()));
            Assert.Equal("Wallet does not exist", ex.Message);
        }

        [Fact]
        public void BuyEquity_InsufficientFunds()
        {
            _uow.Setup(x => x.WalletRepository).Returns(_walletRepository.Object);
            _wrapper.Setup(x => x.IsMarketOpen()).Returns(true);
            _walletRepository.Setup(x => x.Get(It.IsAny<Expression<Func<WalletDB, bool>>>(), It.IsAny<string>()))
                .Returns(new List<WalletDB>() { new WalletDB() });

            var service = GetServiceInstance();

            var ex = Assert.Throws<Exception>(() => service.BuyEquity(new TransactionOrder() { Quantity = 5, UnitPrice = 10 }));
            Assert.Equal("Insufficient balance in wallet", ex.Message);
        }

        [Fact]
        public void BuyEquity_AlreadyPossesSameEquity()
        {
            _uow.Setup(x => x.WalletRepository).Returns(_walletRepository.Object);
            _uow.Setup(x => x.TransactionOrderRepository).Returns(_transactionOrderRepository.Object);
            _wrapper.Setup(x => x.IsMarketOpen()).Returns(true);
            _walletRepository.Setup(x => x.Get(It.IsAny<Expression<Func<WalletDB, bool>>>(), It.IsAny<string>()))
                .Returns(new List<WalletDB>() { new WalletDB() {
                    UserId = 1,
                    WalletId = 1,
                    Amount = 100,
                    EquityHoldings = new List<EquityHoldingDB>()
                    {
                        new EquityHoldingDB()
                        {
                            UserId = 1,
                            EquityId = 1,
                            EquityHoldingId = 1,
                            Quantity = 10
                        }
                    }
                } });

            var service = GetServiceInstance();

            var result = service.BuyEquity(new TransactionOrder()
            {
                Quantity = 5,
                EquityId = 1,
                UnitPrice = 15,
                Type = OrderType.Buy,
                UserId = 1
            });

            Assert.True(result);
            _walletRepository.Verify(x => x.Update(It.Is<WalletDB>(y => y.Amount == 25 && y.EquityHoldings
            .Any(z => z.EquityId == 1 && z.UserId == 1 && z.Quantity == 15))), Times.Once, "Update should be called once.");
            _transactionOrderRepository.Verify(x => x.Insert(It.Is<TransactionOrderDB>(z => z.EquityId == 1 && z.UserId == 1 && z.Quantity == 5)), 
                Times.Once, "Insert should be called once.");
            _uow.Verify(x => x.SaveChanges(), Times.Once, "SaveChanges should be called once.");
        }

        [Fact]
        public void BuyEquity_NewEquity()
        {
            _uow.Setup(x => x.WalletRepository).Returns(_walletRepository.Object);
            _uow.Setup(x => x.TransactionOrderRepository).Returns(_transactionOrderRepository.Object);
            _wrapper.Setup(x => x.IsMarketOpen()).Returns(true);
            _walletRepository.Setup(x => x.Get(It.IsAny<Expression<Func<WalletDB, bool>>>(), It.IsAny<string>()))
                .Returns(new List<WalletDB>() { new WalletDB() {
                    UserId = 1,
                    WalletId = 1,
                    Amount = 1000,
                    EquityHoldings = new List<EquityHoldingDB>()
                    {
                        new EquityHoldingDB()
                        {
                            UserId = 1,
                            EquityId = 1,
                            EquityHoldingId = 1,
                            Quantity = 10
                        }
                    }
                } });

            var service = GetServiceInstance();

            var result = service.BuyEquity(new TransactionOrder()
            {
                Quantity = 10,
                EquityId = 2,
                UnitPrice = 50,
                Type = OrderType.Buy,
                UserId = 1
            });

            Assert.True(result);
            _walletRepository.Verify(x => x.Update(It.Is<WalletDB>(y => y.EquityHoldings.Count == 2 && y.Amount == 500 && y.EquityHoldings
            .Any(z => z.EquityId == 2 && z.UserId == 1))), Times.Once, "Update should be called once.");
            _transactionOrderRepository.Verify(x => x.Insert(It.Is<TransactionOrderDB>(z => z.EquityId == 2 && z.UserId == 1 && z.Quantity == 10)),
                    Times.Once, "Insert should be called once.");
            _uow.Verify(x => x.SaveChanges(), Times.Once, "SaveChanges should be called once.");
        }

        [Fact]
        public void SellEquity_MarketNotOpen()
        {
            _wrapper.Setup(x => x.IsMarketOpen()).Returns(false);

            var service = GetServiceInstance();

            var ex = Assert.Throws<Exception>(() => service.SellEquity(new TransactionOrder()));
            Assert.Equal("Market not open", ex.Message);
        }

        [Fact]
        public void SellEquity_WalletDoesNotExist()
        {
            _uow.Setup(x => x.WalletRepository).Returns(_walletRepository.Object);
            _wrapper.Setup(x => x.IsMarketOpen()).Returns(true);
            _walletRepository.Setup(x => x.Get(It.IsAny<Expression<Func<WalletDB, bool>>>(), It.IsAny<string>()))
                .Returns(new List<WalletDB>());

            var service = GetServiceInstance();

            var ex = Assert.Throws<Exception>(() => service.SellEquity(new TransactionOrder()));
            Assert.Equal("Wallet does not exist", ex.Message);
        }

        [Fact]
        public void SellEquity_InsufficientEquity()
        {
            _uow.Setup(x => x.WalletRepository).Returns(_walletRepository.Object);
            _wrapper.Setup(x => x.IsMarketOpen()).Returns(true);
            _walletRepository.Setup(x => x.Get(It.IsAny<Expression<Func<WalletDB, bool>>>(), It.IsAny<string>()))
                .Returns(new List<WalletDB>() { new WalletDB() });

            var service = GetServiceInstance();

            var ex = Assert.Throws<Exception>(() => service.SellEquity(new TransactionOrder() { Quantity = 5, UnitPrice = 10, EquityId = 1 }));
            Assert.Equal("Does not possess enough equity", ex.Message);
        }

        [Fact]
        public void SellEquity_WrongEquity()
        {
            _uow.Setup(x => x.WalletRepository).Returns(_walletRepository.Object);
            _wrapper.Setup(x => x.IsMarketOpen()).Returns(true);
            _walletRepository.Setup(x => x.Get(It.IsAny<Expression<Func<WalletDB, bool>>>(), It.IsAny<string>()))
                .Returns(new List<WalletDB>() { new WalletDB() {
                    UserId = 1,
                    WalletId = 1,
                    Amount = 100,
                    EquityHoldings = new List<EquityHoldingDB>()
                    {
                        new EquityHoldingDB()
                        {
                            UserId = 2,
                            EquityId = 1,
                            EquityHoldingId = 1,
                            Quantity = 100
                        }
                    }
                } });

            var service = GetServiceInstance();

            var ex = Assert.Throws<Exception>(() => service.SellEquity(new TransactionOrder() { Quantity = 5, UnitPrice = 10, EquityId = 1 }));
            Assert.Equal("Does not possess enough equity", ex.Message);
        }

        [Fact]
        public void SellEquity_MinBrokerage()
        {
            _uow.Setup(x => x.WalletRepository).Returns(_walletRepository.Object);
            _uow.Setup(x => x.TransactionOrderRepository).Returns(_transactionOrderRepository.Object);
            _wrapper.Setup(x => x.IsMarketOpen()).Returns(true);
            _walletRepository.Setup(x => x.Get(It.IsAny<Expression<Func<WalletDB, bool>>>(), It.IsAny<string>()))
                .Returns(new List<WalletDB>() { new WalletDB() {
                    UserId = 1,
                    WalletId = 1,
                    Amount = 100,
                    EquityHoldings = new List<EquityHoldingDB>()
                    {
                        new EquityHoldingDB()
                        {
                            UserId = 1,
                            EquityId = 1,
                            EquityHoldingId = 1,
                            Quantity = 100
                        }
                    }
                } });

            var service = GetServiceInstance();

            var result = service.SellEquity(new TransactionOrder()
            {
                Quantity = 10,
                EquityId = 1,
                UnitPrice = 15,
                Type = OrderType.Sell,
                UserId = 1
            });

            Assert.True(result);
            _walletRepository.Verify(x => x.Update(It.Is<WalletDB>(y => y.Amount == 230 && y.EquityHoldings
            .Any(z => z.EquityId == 1 && z.Quantity == 90 && z.UserId == 1))), Times.Once, 
            "Update should be called once.");
            _transactionOrderRepository.Verify(x => x.Insert(It.Is<TransactionOrderDB>(z => z.EquityId == 1 && z.UserId == 1 && z.Quantity == 10)),
                Times.Once, "Insert should be called once.");
            _uow.Verify(x => x.SaveChanges(), Times.Once, "SaveChanges should be called once.");
        }

        [Fact]
        public void SellEquity_BrokerageGreaterThanMin()
        {
            _uow.Setup(x => x.WalletRepository).Returns(_walletRepository.Object);
            _uow.Setup(x => x.TransactionOrderRepository).Returns(_transactionOrderRepository.Object);
            _wrapper.Setup(x => x.IsMarketOpen()).Returns(true);
            _walletRepository.Setup(x => x.Get(It.IsAny<Expression<Func<WalletDB, bool>>>(), It.IsAny<string>()))
                .Returns(new List<WalletDB>() { new WalletDB() {
                    UserId = 1,
                    WalletId = 1,
                    Amount = 100,
                    EquityHoldings = new List<EquityHoldingDB>()
                    {
                        new EquityHoldingDB()
                        {
                            UserId = 1,
                            EquityId = 1,
                            EquityHoldingId = 1,
                            Quantity = 1000
                        }
                    }
                } });

            var service = GetServiceInstance();

            var result = service.SellEquity(new TransactionOrder()
            {
                Quantity = 100,
                EquityId = 1,
                UnitPrice = 500,
                Type = OrderType.Sell,
                UserId = 1
            });

            Assert.True(result);
            _walletRepository.Verify(x => x.Update(It.Is<WalletDB>(y => y.Amount == 50075 && y.EquityHoldings
            .Any(z => z.EquityId == 1 && z.Quantity == 900 && z.UserId == 1))), Times.Once, 
            "Update should be called once.");
            _transactionOrderRepository.Verify(x => x.Insert(It.Is<TransactionOrderDB>(z => z.EquityId == 1 && z.UserId == 1 && z.Quantity == 100)),
                Times.Once, "Insert should be called once.");
            _uow.Verify(x => x.SaveChanges(), Times.Once, "SaveChanges should be called once.");
        }
    }
}
