using GamesService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesService.Controllers
{
    public interface IHubGameDispatcher
    {
        Task ChangeGame(Games game);
    }
}
