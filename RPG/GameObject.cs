using System.Drawing;

namespace RPG
{
    public class GameObject
    {
        public Image Image = Properties.Resources.grass;
        public int Difficulty;
    }

    enum TileName
    {
        Mountain,
        River,
        Hub
    }
}