using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
    public class Model
    {
        public Map Map;
        public Player Player;
        public Entity[] Monsters;

        public Model()
        {
            Map = new Map(21);
            Player = new Player(Map);
            Monsters = CreateMonsters();
        }

        private Entity[] CreateMonsters()
        {
            return new Entity[0];
        }
    }
}
