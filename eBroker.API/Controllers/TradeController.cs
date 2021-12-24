using eBroker.BLL;
using eBroker.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using System;

namespace eBroker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TradeController : ControllerBase
    {
        public readonly ITradeService _tradeService;
        public TradeController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }

        [HttpPost]
        [Route("[action]")]
        [Route("api/trade/buyequity")]
        public bool BuyEquity(TransactionOrder order)
        {
            try
            {
                return _tradeService.BuyEquity(order);
            }
            catch(Exception e)
            {
                return false;
            }
        }

        [HttpPost]
        [Route("[action]")]
        [Route("api/trade/sellequity")]
        public bool SellEquity(TransactionOrder order)
        {
            try
            {
                return _tradeService.SellEquity(order);
            }
            catch
            {
                return false;
            }
        }
    }
}
