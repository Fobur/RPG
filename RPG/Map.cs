using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RPG
{
    public class Map
    {
        public readonly int Size;
        public readonly Tile[,] Grid;
        public readonly Point Hub;
        public Point Player;
        public readonly Dictionary<Zones, List<Point>> ZonesZones;
        public Func<GameForm> GetGameForm;

        public Map(int size)
        {
            Size = size;
            Player = new Point(Size / 2, Size / 2);
            Hub = new Point(Size / 2, Size / 2);
            var zonesZones = new Dictionary<Zones, List<Point>>
            {
                [Zones.HubZone] = new List<Point> { Hub },
                [Zones.GreenZone] = GetCircleZone(Hub, Size / 6, 0),
                [Zones.FrointerZone] = GetCircleZone(Hub, Size / 3, Size / 6),
                [Zones.UnknownZone] = GetSqareZoneWithInnerCircle(Hub, Size, Size / 3)
            };
            ZonesZones = zonesZones;
            Grid = GenerateMap(size, zonesZones);
        }

        public Tile this[Point point]
        {
            get => InBounds(point)
                ? Grid[point.X, point.Y]
                : null;
        }

        public Tile this[Point? point]
        {
            get => InBounds(point)
                ? Grid[point.Value.X, point.Value.Y]
                : null;
        }

        private Tile[,] GenerateMap(int size, Dictionary<Zones, List<Point>> zonesZones)
        {
            var ret = new Tile[size, size];
            var random = new Random();
            ret[Hub.X,Hub.Y] = new Tile(Hub, ContentConstructor.GetGameObject(TileName.Hub, Zones.HubZone));
            foreach (var tile in zonesZones[Zones.GreenZone])
                ret[tile.X, tile.Y] = new Tile(tile, ContentConstructor.GerRandomGameObject(Zones.GreenZone));
            var randomPlaceForTreasure = GetRandomPoint(Zones.GreenZone, random);
            ret[randomPlaceForTreasure.X, randomPlaceForTreasure.Y].Content.Treasure = new Treasure(TreasureNames.TerraToilet, randomPlaceForTreasure);
            foreach (var tile in zonesZones[Zones.FrointerZone])
                ret[tile.X, tile.Y] = new Tile(tile, ContentConstructor.GerRandomGameObject(Zones.FrointerZone));
            randomPlaceForTreasure = GetRandomPoint(Zones.FrointerZone, random);
            ret[randomPlaceForTreasure.X, randomPlaceForTreasure.Y].Content.Treasure = new Treasure(TreasureNames.ShitImposedByDeveloper, randomPlaceForTreasure);
            foreach (var tile in zonesZones[Zones.UnknownZone])
                ret[tile.X, tile.Y] = new Tile(tile, ContentConstructor.GerRandomGameObject(Zones.UnknownZone));
            randomPlaceForTreasure = GetRandomPoint(Zones.UnknownZone, random);
            ret[randomPlaceForTreasure.X, randomPlaceForTreasure.Y].Content.Treasure = new Treasure(TreasureNames.BadDragon, randomPlaceForTreasure);
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

        public Point GetRandomPoint(Zones zone, Random random) => ZonesZones[zone][random.Next(0, ZonesZones[zone].Count - 1)];

        public bool IsLethalAttack(Attack attack, List<Point> target)
        {
            foreach (var point in target)
                if (InBounds(point) && this[point].Content.Entity != null
                    && attack.Damage >= this[point].Content.Entity.HP)
                    return true;
            return false;
        }

        public List<Entity> GetAllEntityInView(Entity entity)
        {
            return GetCircleZone(entity.Position, entity.ViewRadius, 0)
                .Where(x => InBounds(x) && this[x].Content.Entity != null && this[x].Content.Entity.IsVisible(entity))
                .Select(x => this[x].Content.Entity)
                .ToList();
        }

        public static int GetDistanceToTile(Point tile, Point position)
        {
            return (int)Math.Sqrt((tile.X - position.X) * (tile.X - position.X) + (tile.Y - position.Y) * (tile.Y - position.Y));
        }

        public static bool IsOpponentInViewVisible(Entity viewer, Entity opponent)
        {
            var isOpponentInViewRadius = GetCircleZonePattern(viewer.ViewRadius, 0)
                .Select(x => viewer.Position + x)
                .Where(x => x == opponent.Position)
                .ToList().Count != 0
                    ? true
                    : false;
            return isOpponentInViewRadius && viewer.Stats.Perception >= opponent.Stats.Agility / 2
                ? true
                : false;
        }

        public static List<Point> GetCircleZone(Point center, int radius, int innerRadius) =>
            GetCircleZonePattern(radius, innerRadius)
            .Select(x => center + x)
            .ToList();

        public static List<Size> GetCircleZonePattern(int radius, int innerRadius)
        {
            var res = new List<Size>();
            for (var i = radius; i >= -radius; i--)
                for (var j = radius; j >= -radius; j--)
                    if (GetDistanceToTile(new Point(i, j), new Point(0, 0)) <= radius
                        && GetDistanceToTile(new Point(i, j), new Point(0, 0)) > innerRadius)
                        res.Add(new Size(i, j));
            return res;
        }

        public static List<Point> GetSqareZoneWithInnerCircle(Point center, int size, int innerRadius)
        {
            var res = new List<Size>();
            for (var x = -size/2; x <= size/2; x++)
                for (var y = -size/2; y <= size/2; y++)
                    if (GetDistanceToTile(new Point(x, y), new Point(0, 0)) > innerRadius)
                        res.Add(new Size(x, y));
            return res
                .Select(x => center + x)
                .ToList();
        }

        public bool InBounds(Point point)
        {
            return point.X > -1 && point.X < Size
                && point.Y > -1 && point.Y < Size;
        }

        public bool InBounds(Point? point)
        {
            return point.HasValue
                ? point.Value.X > -1 && point.Value.X < Size
                && point.Value.Y > -1 && point.Value.Y < Size
                : false;
        }

        public void TakeMove(Directions direction, Entity entity)
        {
            var nextPosition = DirectionToPoint(direction, entity.Position);
            if (CanTakeMove(direction, entity))
            {
                this[entity.Position].Content.Entity = null;
                this[nextPosition].Content.Entity = entity;
                entity.Position = nextPosition;
                entity.Energy -= this[nextPosition].Content.Cost;
                if (entity.IsPlayer && this[entity.Position].Content.Treasure != null)
                {
                    ((Player)entity).Treasures.Add(this[entity.Position].Content.Treasure);
                    this[entity.Position].Content.Treasure = null;
                    GameForm.AddIntoLog(this[entity.Position].Content.Treasure.Name + " was found", Color.DarkBlue);
                    ((Player)entity).LevelUp(2);
                    if (((Player)entity).Treasures.Count == 3)
                        GameForm.AddIntoLog("Congtratulations! Game won", Color.DarkBlue);
                }
            }
        }

        public void TakeMove(Point nextPosition, Entity entity)
        {
            if (CanTakeMove(nextPosition, entity))
            {
                this[entity.Position].Content.Entity = null;
                this[nextPosition].Content.Entity = entity;
                entity.Position = nextPosition;
                entity.Energy -= this[nextPosition].Content.Cost;
                if (entity.IsPlayer && this[entity.Position].Content.Treasure != null)
                {
                    ((Player)entity).Treasures.Add(this[entity.Position].Content.Treasure);
                    this[entity.Position].Content.Treasure = null;
                    ((Player)entity).LevelUp(2);
                }
            }
        }

        public void TakeMove(List<Directions> path, Entity entity)
        {
            foreach (var direction in path)
                TakeMove(direction, entity);
        }

        public void TakeMove(List<Point> path, Entity entity)
        {
            var i = 0;
            var energyUsed = 0;
            while(path.Count - 1 > i && energyUsed <= entity.Energy)
            {
                energyUsed += this[path[i]].Content.Cost;
                this[entity.Position].Content.Entity = null;
                entity.Position = path[i];
                i++;
            }
            if(i > 0) TakeMove(path[i - 1], entity);
        }

        public bool CanTakeMove(Directions direction, Entity entity)
        {
            var nextPosition = DirectionToPoint(direction, entity.Position);
            return InBounds(nextPosition) && entity.Energy >= this[nextPosition].Content.Cost && this[nextPosition].Content.Entity == null;
        }

        public bool CanTakeMove(Point nextPosition, Entity entity)
        {
            return InBounds(nextPosition) && entity.Energy >= this[nextPosition].Content.Cost && this[nextPosition].Content.Entity == null;
        }

        public Point DirectionToPoint(Directions direction, Point position)
        {
            switch (direction)
            {
                case Directions.Up:
                    return new Point(position.X, position.Y - 1);
                case Directions.Down:
                    return new Point(position.X, position.Y + 1);
                case Directions.Left:
                    return new Point(position.X - 1, position.Y);
                case Directions.Right:
                    return new Point(position.X + 1, position.Y);
                default:
                    return position;
            }
        }
    }

    public enum Directions
    {
        Up,
        Down,
        Right,
        Left,
        None
    }
}
