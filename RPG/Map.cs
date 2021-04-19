using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private Tile[,] GenerateMap(int size)
        {
            var ret = new Tile[size, size];
            for (var i = 0; i < size; i++)
            {
                for(var j = 0; j < size; j++)
                    ret[i,j] = new Tile(i, i, new GameObject());
            }
            return ret;
        }

        public bool InBounds(Point point)
        {
            return point.X > -1 && point.X < Size
                && point.Y > -1 && point.Y < Size;
        }
    }
}
