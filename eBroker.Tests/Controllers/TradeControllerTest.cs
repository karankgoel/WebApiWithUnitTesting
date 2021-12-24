using eBroker.API.Controllers;
using eBroker.BLL;
using eBroker.Shared.DTO;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace eBroker.Tests.Controllers
{
    public class TradeControllerTest
    {
        private readonly Mock<ITradeService> _tradeService;

        public TradeControllerTest()
        {
            _tradeService = new Mock<ITradeService>();
        }

        private TradeController GetInstance()
        {
            return new TradeController(_tradeService.Object);
        }

        [Fact]
        public void BuyEquity_Successful()
        {
            _tradeService.Setup(x => x.BuyEquity(It.IsAny<TransactionOrder>())).Returns(true);
            var controller = GetInstance();

            var result = controller.BuyEquity(new TransactionOrder());

            Assert.True(result);
        }

        [Fact]
        public void BuyEquity_ReturnsFalse()
        {
            _tradeService.Setup(x => x.BuyEquity(It.IsAny<TransactionOrder>())).Returns(false);
            var controller = GetInstance();

            var result = controller.BuyEquity(new TransactionOrder());

            Assert.False(result);
        }

        [Fact]
        public void BuyEquity_ReturnsException()
        {
            _tradeService.Setup(x => x.BuyEquity(It.IsAny<TransactionOrder>())).Throws(new Exception("Does not possess enough equity"));
            var controller = GetInstance();

            var result = controller.BuyEquity(new TransactionOrder());

            Assert.False(result);
        }

        [Fact]
        public void SellEquity_Successful()
        {
            _tradeService.Setup(x => x.SellEquity(It.IsAny<TransactionOrder>())).Returns(true);
            var controller = GetInstance();

            var result = controller.SellEquity(new TransactionOrder());

            Assert.True(result);
        }

        [Fact]
        public void SellEquity_ReturnsFalse()
        {
            _tradeService.Setup(x => x.SellEquity(It.IsAny<TransactionOrder>())).Returns(false);
            var controller = GetInstance();

            var result = controller.SellEquity(new TransactionOrder());

            Assert.False(result);
        }

        [Fact]
        public void SellEquity_ReturnsException()
        {
            _tradeService.Setup(x => x.SellEquity(It.IsAny<TransactionOrder>())).Throws(new Exception("Does not possess enough equities"));
            var controller = GetInstance();

            var result = controller.SellEquity(new TransactionOrder());

            Assert.False(result);
        }
    }
}
