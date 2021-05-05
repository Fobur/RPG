using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
    public class Tile
    {
        public readonly Point Coord;
        public GameObject Content;
        public readonly int Size = 30;
        public bool IsVisited;

        public Tile(Point coord, GameObject obj)
        {
            Coord = coord;
            Content = obj;
        }
    }
}
