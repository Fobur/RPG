using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
    public class Tile
    {
        public readonly int XCord;
        public readonly int YCord;
        GameObject Content;

        Tile(int x, int y, GameObject obj)
        {
            XCord = x;
            YCord = y;
            Content = obj;
        }
    }
}
