using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
    public class Map
    {
        public readonly int Size;
        public Tile[] Grid;

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
            var ret = new Tile[size];
            for (var i = 0; i < size; i++)
            {
                ret[i] = new Tile(i, i, new GameObject());
            }
            return ret;
        }
    }
}
