using System.Drawing;

namespace RPG
{
    public class Entity
    {
        public Point Position;
        public Image skin;

        public void TakeStep(MoveDirections direction, Map map)
        {
            var nextPosition = DirectionToPoint(direction, Position);
            if (map.InBounds(nextPosition))
                Position = nextPosition;
        }

        public Point DirectionToPoint(MoveDirections direction, Point position)
        {
            switch(direction)
            {
                case MoveDirections.Up:
                    return new Point(position.X, position.Y - 1);
                case MoveDirections.Down:
                    return new Point(position.X, position.Y + 1);
                case MoveDirections.Left:
                    return new Point(position.X - 1, position.Y);
                case MoveDirections.Right:
                    return new Point(position.X + 1, position.Y);
                default:
                    return position;
            }
        }
    }

    public enum MoveDirections
    {
        Up,
        Down,
        Right,
        Left,
        None
    }
}