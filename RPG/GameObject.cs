using System.Drawing;

namespace RPG
{
    public class GameObject
    {
        public Image Image;
        public Image Background;
        public int Difficulty;
        public readonly int Cost;

        public GameObject()
        {
            Image = Properties.Resources.grass;
        }

        public GameObject(TileName name, Zones zone)
        {
            switch (name)
            {
                case TileName.Hub:
                    Image = Properties.Resources.hub;
                    Background = Properties.Resources.grass;
                    Cost = 1;
                    break;
                case TileName.Mountain:
                    Image = Properties.Resources.mountain;
                    Background = Properties.Resources.grass;
                    Cost = 3;
                    break;
                case TileName.River:
                    Image = Properties.Resources.empty;
                    Background = Properties.Resources.river;
                    Cost = 2;
                    break;
                case TileName.Meadow:
                    Image = Properties.Resources.empty;
                    Background = Properties.Resources.grass;
                    Cost = 1;
                    break;
                default:
                    Image = Properties.Resources.notexture;
                    Background = Properties.Resources.empty;
                    break;
            }
            switch (zone)
            {
                case Zones.HubZone:
                    Difficulty = 0;
                    break;
                case Zones.GreenZone:
                    Difficulty = 10;
                    break;
                case Zones.FrointerZone:
                    Difficulty = 15;
                    break;
                case Zones.UnknownZone:
                    Difficulty = 30;
                    break;
            }
        }
    }

    public enum TileName
    {
        Mountain,
        River,
        Meadow,
        Hub
    }

    public enum Zones
    {
        HubZone,
        GreenZone,
        FrointerZone,
        UnknownZone
    }
}