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
        private static readonly Bitmap Grass = Properties.Resources.grass.Clone() as Bitmap;
        private static readonly Bitmap Hub =  Properties.Resources.hub.Clone() as Bitmap;
        private static readonly Bitmap Mountain = Properties.Resources.mountain.Clone() as Bitmap;
        private static readonly Bitmap Empty = Properties.Resources.empty.Clone() as Bitmap;
        private static readonly Bitmap River = Properties.Resources.river.Clone() as Bitmap;
        private static readonly Bitmap Desert = Properties.Resources.desert.Clone() as Bitmap;
        private static readonly Bitmap Forest = Properties.Resources.forest.Clone() as Bitmap;
        private static readonly Bitmap Swamp = Properties.Resources.swamp.Clone() as Bitmap;
        private static readonly Bitmap SnowForest = Properties.Resources.snowForest.Clone() as Bitmap;
        private static readonly Bitmap Snow = Properties.Resources.snow.Clone() as Bitmap;
        private static readonly Bitmap Jungle = Properties.Resources.jungle.Clone() as Bitmap;
        private static readonly Bitmap NoTexture = Properties.Resources.notexture.Clone() as Bitmap;
        
        
        public static Content GetGameObject(TileName name, Zones zone)
        {
            var ret = new Content();
            switch (name)
            {
                case TileName.Hub:
                    ret.Image = Hub;
                    ret.Background = Grass;
                    ret.Cost = 1;
                    break;
                case TileName.Mountain:
                    ret.Image = Mountain;
                    ret.Background = Grass;
                    ret.Cost = 5;
                    break;
                case TileName.River:
                    ret.Image = Empty;
                    ret.Background = River;
                    ret.Cost = 2;
                    break;
                case TileName.Meadow:
                    ret.Image = Empty;
                    ret.Background = Grass;
                    ret.Cost = 1;
                    break;
                case TileName.Desert:
                    ret.Image = Empty;
                    ret.Background = Desert;
                    ret.Cost = 2;
                    break;
                case TileName.Forest:
                    ret.Image = Forest;
                    ret.Background = Grass;
                    ret.Cost = 2;
                    break;
                case TileName.Swamp:
                    ret.Image = Empty;
                    ret.Background = Swamp;
                    ret.Cost = 3;
                    break;
                case TileName.Tundra:
                    ret.Image = SnowForest;
                    ret.Background = Snow;
                    ret.Cost = 2;
                    break;
                case TileName.TropicalForest:
                    ret.Image = Jungle;
                    ret.Background = Grass;
                    ret.Cost = 2;
                    break;
                default:
                    ret.Image = NoTexture;
                    ret.Background = Empty;
                    break;
            }

            ret.Difficulty = zone switch
            {
                Zones.HubZone => 0,
                Zones.GreenZone => 1,
                Zones.FrointerZone => 2,
                Zones.UnknownZone => 3,
                _ => ret.Difficulty
            };
            return ret;
        }

        public static Content GerRandomGameObject(Zones zone)
        {
            var random = new Random().Next(0,100);
            var name = zone switch
            {
                Zones.GreenZone => random <= 50 ? TileName.Meadow : random <= 90 ? TileName.Forest : TileName.River,
                Zones.FrointerZone => random <= 10 ? TileName.River :
                    random <= 30 ? TileName.Meadow :
                    random <= 50 ? TileName.TropicalForest :
                    random <= 80 ? TileName.Forest : TileName.Swamp,
                Zones.UnknownZone => random <= 10 ? TileName.Tundra :
                    random <= 30 ? TileName.Forest :
                    random <= 50 ? TileName.Swamp :
                    random <= 80 ? TileName.TropicalForest :
                    random <= 90 ? TileName.Desert : TileName.Mountain,
                _ => TileName.Meadow
            };
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
        Tundra,
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