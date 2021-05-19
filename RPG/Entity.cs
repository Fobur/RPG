using System;
using System.Drawing;

namespace RPG
{
    public class Entity
    {
        public Point Position;
        public Image skin;
        private int energy;
        private int maxEnergy;
        private int hP;
        private int maxHP;
        public int ViewRadius;
        public StatList Stats;

        public int Energy 
        {  
            get => energy;
            set
            {
                energy = value;
            }
        }
        public int MaxEnergy { get => maxEnergy; set => maxEnergy = value; }
        public int HP { get => hP; set => hP = value; }
        public int MaxHP { get => maxHP; set => maxHP = value; }

        public void RestoreEnergy()
        {
            Energy = MaxEnergy;
        }

        public void RestoreHP()
        {
            HP = MaxHP;
        }

        public bool IsVisible(Point tile)
        {
            return GetDistanceToTile(tile) <= ViewRadius;
        }

        public int GetDistanceToTile(Point tile)
        {
            return Math.Abs(tile.X - Position.X) + Math.Abs(tile.Y - Position.Y);
        }

        public void TakeStep(MoveDirections direction, Map map)
        {
            var nextPosition = DirectionToPoint(direction, Position);
            if (map.InBounds(nextPosition) && Energy >= map[nextPosition].Content.Cost)
            {
                Position = nextPosition;
                Energy -= map[nextPosition].Content.Cost;
            }
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