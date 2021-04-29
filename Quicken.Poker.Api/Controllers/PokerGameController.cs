using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quicken.Poker.Core;
using Quicken.Poker.Core.Models;
using Quicken.Poker.Core.ViewModels;
using Quicken.Poker.Dal.Quicken.Poker.Models;
using Quicken.Poker.Api.Utilities;
using Quicken.Poker.Core.Services;

namespace Quicken.Poker.Api.Controllers
{


    [ApiController]
    [Route("[controller]")]
    public class PokerGameController : ApiControllerBase
    {
        private readonly ILogger<PokerGameController> _logger;
        ServiceBase _pokerService;

        public PokerGameController(ILogger<PokerGameController> logger, ServiceBase pokerService)
        {
            _pokerService = pokerService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<GetWinnerViewResult> Post([FromBody] PlayersViewModel request)
        {
            try
            {
                PokerService ps = (PokerService)this._pokerService;
                GetWinnerViewResult result = new GetWinnerViewResult()
                {
                    IsSuccess = true,
                   Winner = await ps.GetWinner(request)
                };
                return result;
            }
            catch (Exception ex)
            {
                return Fail<GetWinnerViewResult>(ex);
            }
        }


        [HttpGet]
        public async Task<GetPlayerViewResult> Get()
        {
            try
            {
                PokerService ps = (PokerService)this._pokerService;
                GetPlayerViewResult result = new GetPlayerViewResult()
                {
                    IsSuccess = true,
                    Players = await ps.GetPlayers()
                };
                return result;
            }
            catch (Exception ex)
            {
                return Fail<GetPlayerViewResult>(ex);
            }
        }

        public class GetWinnerViewResult : ActionResult
        {
            public GetWinnerViewResult()
            {
                
            }
            public  WinnerViewModel Winner { get; set; }
        }

        public class GetPlayerViewResult : ActionResult
        {
            public GetPlayerViewResult()
            {

            }
            public PlayersViewModel Players { get; set; }
        }

    }
}

