using eBroker.BLL;
using eBroker.DAL;
using eBroker.DAL.Models;
using eBroker.DAL.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace eBroker.Tests.Services
{
    public class FundsServiceTest
    {
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IWalletRepository> _walletRepository;

        public FundsServiceTest()
        {
            _uow = new Mock<IUnitOfWork>();
            _walletRepository = new Mock<IWalletRepository>();
        }

        private FundsService GetServiceInstance()
        {
            return new FundsService(_uow.Object);
        }

        [Fact]
        public void AddFunds_WalletDoesNotExst()
        {
            _uow.Setup(x => x.WalletRepository).Returns(_walletRepository.Object);

            var service = GetServiceInstance();

            var result = service.AddFunds(1, 100.00);

            Assert.False(result);
        }

        [Fact]
        public void AddFunds_AmountGreaterThan100000()
        {
            _uow.Setup(x => x.WalletRepository).Returns(_walletRepository.Object);
            _walletRepository.Setup(x => x.Get(It.IsAny<Expression<Func<WalletDB, bool>>>(), It.IsAny<string>()))
                .Returns(new List<WalletDB>() { new WalletDB() });

            var service = GetServiceInstance();

            var result = service.AddFunds(1, 1000000.00);

            Assert.True(result);
            _walletRepository.Verify(x => x.Update(It.Is<WalletDB>(y => y.Amount == 999500)), Times.Once, "Update should be called once.");
            _uow.Verify(x => x.SaveChanges(), Times.Once, "SaveChanges should be called once.");
        }

        [Fact]
        public void AddFunds_AmountLessThan100000()
        {
            _uow.Setup(x => x.WalletRepository).Returns(_walletRepository.Object);
            _walletRepository.Setup(x => x.Get(It.IsAny<Expression<Func<WalletDB, bool>>>(), It.IsAny<string>()))
                .Returns(new List<WalletDB>() { new WalletDB() });

            var service = GetServiceInstance();

            var result = service.AddFunds(1, 10000.00);

            Assert.True(result);
            _walletRepository.Verify(x => x.Update(It.Is<WalletDB>(y => y.Amount == 10000)), Times.Once, "Update should be called once.");
            _uow.Verify(x => x.SaveChanges(), Times.Once, "SaveChanges should be called once.");
        }
    }
}
