using GamesService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        public static Dictionary<string, List<Games>> GamesDB = new();
        public static List<Games> Games = new();
        private static readonly string[] GameList = new[]
        {
            "ColdWar", "Returnal", "Village", "Sackboy", "Spiderman", "ValHalla", "Cyberpunk", "Horizon", "Control", "Fifa21"
        };

        IHubGameDispatcher _dispatcher;
        public GamesController(IHubGameDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
        [HttpGet("{connectionID}")]
        public List<Games> Get(string connectionID)
        {
            var rng = new Random();
            Games = Enumerable.Range(1, 10).Select(index => new Games
            {
                ID = index,
                Name = GameList[rng.Next(GameList.Length)],
                Price = rng.Next(1000) + 500,
                CreatedDate = DateTime.Now
            }).GroupBy(item => item.Name)
                   .Select(grp => grp.First()).Take(5)
                   .ToList();
            Games.ForEach(g => g.ImgPath = g.Name + ".jpg");
            GamesDB.Add(connectionID, Games);
            return Games;
        }
        [HttpPost("UpdateGame")]
        public async Task UpdateGame([FromBody] Games game)
        {
            bool isChange = false;
            foreach (var gamesList in GamesDB)
            {
                var updateGame = gamesList.Value.Where(g => g.Name == game.Name).FirstOrDefault();
                if (updateGame != null)
                {
                    isChange = true;
                    gamesList.Value.Remove(updateGame);
                    gamesList.Value.Add(game);
                    GamesDB[gamesList.Key] = gamesList.Value;                    
                }
            }
            if (isChange) await this._dispatcher.ChangeGame(game);
        }
    }

    public class GamesHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("GetConnectionId", this.Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            GamesController.GamesDB.Remove(this.Context.ConnectionId);
            Console.WriteLine("DisconnectID:" + this.Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task ClearProduct(Games game)
        {
            await Clients.All.SendAsync("ChangeGame", game);
        }
    }
}
