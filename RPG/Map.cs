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
        public Tile[][] Grid;

        Map(int size)
        {
            Size = size;
            Grid = GenerateMap(size);
        }

        private Tile[][] GenerateMap(int size)
        {
            return new Tile[size][];
        }
    }
}
