using eBroker.API.Controllers;
using eBroker.BLL;
using eBroker.Shared.DTO;
using Moq;
using System;
using Xunit;

namespace eBroker.Tests.Controllers
{
    public class FundsControllerTest
    {
        private readonly Mock<IFundsService> _fundsService;

        public FundsControllerTest()
        {
            _fundsService = new Mock<IFundsService>();
        }

        private FundsController GetInstance()
        {
            return new FundsController(_fundsService.Object);
        }

        [Fact]
        public void AddFunds_Successful()
        {
            _fundsService.Setup(x => x.AddFunds(It.IsAny<int>(), It.IsAny<double>())).Returns(true);
            var controller = GetInstance();

            var result = controller.AddFunds(new AddFunds() { UserId = 1, Amount = 100 });

            Assert.True(result);
        }

        [Fact]
        public void AddFunds_Unsuccessful()
        {
            _fundsService.Setup(x => x.AddFunds(It.IsAny<int>(), It.IsAny<double>())).Returns(false);
            var controller = GetInstance();

            var result = controller.AddFunds(new AddFunds() {UserId =1 , Amount =100});

            Assert.False(result);
        }

        [Fact]
        public void AddFunds_ReturnsException()
        {
            _fundsService.Setup(x => x.AddFunds(It.IsAny<int>(), It.IsAny<double>())).Throws(new Exception("Some Exception"));
            var controller = GetInstance();

            var result = controller.AddFunds(new AddFunds() { UserId = 1, Amount = 100 });

            Assert.False(result);
        }

    }
}
