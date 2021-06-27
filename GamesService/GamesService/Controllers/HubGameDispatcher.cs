using GamesService.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesService.Controllers
{
    public class HubGameDispatcher : IHubGameDispatcher
    {
        private readonly IHubContext<GamesHub> _hubContext;
        public HubGameDispatcher(IHubContext<GamesHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task ChangeGame(Games game)
        {
            await this._hubContext.Clients.All.SendAsync("ChangeGame", game);
        }
    }
}
