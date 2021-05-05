using System;
using System.Drawing;

namespace RPG
{
    public class Map
    {
        public readonly int Size;
        public Tile[,] Grid;

        public Map(int size)
        {
            Size = size;
            Grid = GenerateMap(size);
        }

        public Tile this[Point point]
        {
            get => Grid[point.X, point.Y];
        }

        private Tile[,] GenerateMap(int size)
        {
            var ret = new Tile[size, size];
            for (var r = 0; r <= size / 2; r++)
            {
                for (var x = -r; x <= r; x++)
                    for (var y = -r; y <= r; y++)
                    {
                        var dist = GetDistanceFromCenter(x, y);
                        if (dist == r)
                        {
                            var normalCoord = ConvertToNormalCoord(new Point(x, y), size/2);
                            if (x == 0 && y == 0)
                                ret[normalCoord.X, normalCoord.Y] = new Tile(normalCoord, new GameObject(TileName.Hub, Zones.HubZone));
                            else if (r == size / 2)
                                ret[normalCoord.X, normalCoord.Y] = new Tile(normalCoord, new GameObject(TileName.Mountain, Zones.UnknownZone));
                            else
                                ret[normalCoord.X, normalCoord.Y] = new Tile(normalCoord, new GameObject(TileName.Meadow, Zones.GreenZone));
                        }
                    }
            }
            return ret;
        }

        private Point ConvertToNormalCoord(Point coord, int size)
        {
            return new Point(coord.X + size, coord.Y + size);
        }

        private int GetDistanceFromCenter(int x, int y)
        {
            var xAbs = Math.Abs(x);
            var yAbs = Math.Abs(y);
            return xAbs > yAbs
                ? xAbs
                : yAbs;
        }
      
        public bool InBounds(Point point)
        {
            return point.X > -1 && point.X < Size
                && point.Y > -1 && point.Y < Size;
        }
    }
}
