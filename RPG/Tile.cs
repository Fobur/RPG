using System.Drawing;

namespace RPG
{
    public class Tile
    {
        public readonly Point Coord;
        public Content Content;
        public readonly int Size = 30;
        public bool IsVisited;

        public Tile(Point coord, Content obj)
        {
            Coord = coord;
            Content = obj;
        }
    }
}
