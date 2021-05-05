using System.Drawing;

namespace RPG
{
    public class Player : Entity
    {
        public Player(Map map)
        {
            Position = new Point(map.Size / 2, map.Size / 2);
            skin = Properties.Resources.player;
            InitialEnergy = 3;
            Energy = InitialEnergy;
            ViewRadius = 3;
        }
    }
}
