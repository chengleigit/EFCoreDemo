using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Demo.Domain
{
    /// <summary>
    /// 比赛
    /// </summary>
    public  class Game
    {
        public Game()
        {
            GamePlayers = new List<GamePlayer>();
        }
        public int Id { get; set; }

        public int Round { get; set; }

        public DateTimeOffset? StartTime { get; set; }


        public List<GamePlayer> GamePlayers { get; set; }
    }
}
