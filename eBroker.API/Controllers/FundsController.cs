using eBroker.BLL;
using eBroker.Shared.DTO;
using Microsoft.AspNetCore.Mvc;

namespace eBroker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FundsController : ControllerBase
    {
        private readonly IFundsService _fundsService;
        public FundsController(IFundsService fundsService)
        {
            _fundsService = fundsService;
        }

        [HttpPost]
        [Route("[action]")]
        [Route("api/funds/addfunds")]
        public bool AddFunds(AddFunds funds)
        {
            try
            {
                return _fundsService.AddFunds(funds.UserId, funds.Amount);
            }
            catch
            {
                return false;
            }
        }
    }
}
