using System;
using System.Drawing;

namespace RPG
{
    public class Content
    {
        public Image Image;
        public Image Background;
        public int Difficulty;
        public int Cost;
        public Entity Entity;
        public Treasure Treasure;

        public Content()
        {
            Image = Properties.Resources.grass;
        }
    }

    public static class ContentConstructor
    {
        public static Content GetGameObject(TileName name, Zones zone)
        {
            var ret = new Content();
            switch (name)
            {
                case TileName.Hub:
                    ret.Image = Properties.Resources.hub;
                    ret.Background = Properties.Resources.grass;
                    ret.Cost = 1;
                    break;
                case TileName.Mountain:
                    ret.Image = Properties.Resources.mountain;
                    ret.Background = Properties.Resources.grass;
                    ret.Cost = 5;
                    break;
                case TileName.River:
                    ret.Image = Properties.Resources.empty;
                    ret.Background = Properties.Resources.river;
                    ret.Cost = 2;
                    break;
                case TileName.Meadow:
                    ret.Image = Properties.Resources.empty;
                    ret.Background = Properties.Resources.grass;
                    ret.Cost = 1;
                    break;
                case TileName.Desert:
                    ret.Image = Properties.Resources.empty;
                    ret.Background = Properties.Resources.desert;
                    ret.Cost = 2;
                    break;
                case TileName.Forest:
                    ret.Image = Properties.Resources.forest;
                    ret.Background = Properties.Resources.grass;
                    ret.Cost = 2;
                    break;
                case TileName.Swamp:
                    ret.Image = Properties.Resources.empty;
                    ret.Background = Properties.Resources.swamp;
                    ret.Cost = 3;
                    break;
                case TileName.Thundra:
                    ret.Image = Properties.Resources.snowForest;
                    ret.Background = Properties.Resources.snow;
                    ret.Cost = 2;
                    break;
                case TileName.TropicalForest:
                    ret.Image = Properties.Resources.jungle;
                    ret.Background = Properties.Resources.grass;
                    ret.Cost = 2;
                    break;
                default:
                    ret.Image = Properties.Resources.notexture;
                    ret.Background = Properties.Resources.empty;
                    break;
            }
            switch (zone)
            {
                case Zones.HubZone:
                    ret.Difficulty = 0;
                    break;
                case Zones.GreenZone:
                    ret.Difficulty = 1;
                    break;
                case Zones.FrointerZone:
                    ret.Difficulty = 2;
                    break;
                case Zones.UnknownZone:
                    ret.Difficulty = 3;
                    break;
            }
            return ret;
        }

        public static Content GerRandomGameObject(Zones zone)
        {
            var random = new Random().Next(0,100);
            var name = TileName.Meadow;
            switch(zone)
            {
                case Zones.GreenZone:
                    name = random <= 50
                        ? TileName.Meadow
                        : random <= 90
                            ? TileName.Forest
                            : TileName.River;
                    break;
                case Zones.FrointerZone:
                    name = random <= 10
                        ? TileName.River
                        : random <= 30
                            ? TileName.Meadow
                            : random <= 50
                             ? TileName.TropicalForest
                             : random <= 80
                                ? TileName.Forest
                                : TileName.Swamp;
                    break;
                case Zones.UnknownZone:
                    name = random <= 10
                        ? TileName.Thundra
                        : random <= 30
                            ? TileName.Forest
                            : random <= 50
                             ? TileName.Swamp
                             : random <= 80
                                ? TileName.TropicalForest
                                : random <= 90
                                 ? TileName.Desert
                                 : TileName.Mountain;
                    break;
            }
            return GetGameObject(name, zone);
        }
    }

    public enum TileName
    {
        Hub,
        River,
        Meadow,
        Forest,
        Swamp,
        TropicalForest,
        Desert,
        Thundra,
        Mountain
    }

    public enum Zones
    {
        HubZone,
        GreenZone,
        FrointerZone,
        UnknownZone
    }
}