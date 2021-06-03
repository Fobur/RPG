using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RPG
{
    public class Monster : Entity
    {
        public event EventHandler<Attack> MonsterAttack;
        public readonly int ExpGain;
        public readonly MonsterTypes Type;
        private readonly int DangerousLevel;
        public readonly List<Size> AttackZone;
        public bool IsKilledByPlayer;
        public bool IsHungry;
        private int MovesAfterKill;
        private readonly int HungerRate;
        public Func<Monster, List<Entity>> GetVisibleEntities;
        public Func<Map> GetMap;
        public Func<List<Monster>> GetAliveMonsters;

        public Monster(MonsterTypes type, Point position, Map map)
        {
            Type = type;
            Position = position;
            map[Position].Content.Entity = this;
            switch (type)
            {
                case MonsterTypes.Slime:
                    Skin = Properties.Resources.slime;
                    Stats = new StatList(1, 1, 1, 1);
                    HungerRate = 0;
                    DangerousLevel = 1;
                    break;
                case MonsterTypes.Spider:
                    Skin = Properties.Resources.Spider;
                    Stats = new StatList(5, 5, 1, 4);
                    HungerRate = 1;
                    DangerousLevel = 1;
                    break;
                case MonsterTypes.Wolf:
                    Skin = Properties.Resources.wolf;
                    Stats = new StatList(5, 5, 10, 4);
                    HungerRate = 3;
                    DangerousLevel = 1;
                    break;
                case MonsterTypes.Goblin:
                    Skin = Properties.Resources.goblin;
                    Stats = new StatList(5, 5, 5, 7);
                    HungerRate = 2;
                    DangerousLevel = 2;
                    break;
                case MonsterTypes.Orc:
                    Skin = Properties.Resources.orc;
                    Stats = new StatList(10, 7, 10, 7);
                    HungerRate = 2;
                    DangerousLevel = 2;
                    break;
                case MonsterTypes.Troll:
                    Skin = Properties.Resources.troll;
                    Stats = new StatList(15, 5, 20, 5);
                    HungerRate = 1;
                    DangerousLevel = 2;
                    break;
                case MonsterTypes.Wyvern:
                    Skin = Properties.Resources.wyvern;
                    Stats = new StatList(15, 10, 20, 10);
                    HungerRate = 3;
                    DangerousLevel = 3;
                    break;
                case MonsterTypes.Hydra:
                    Skin = Properties.Resources.Hydra;
                    Stats = new StatList(20, 10, 50, 10);
                    HungerRate = 4;
                    DangerousLevel = 3;
                    break;
                case MonsterTypes.Dragon:
                    Skin = Properties.Resources.dragon;
                    Stats = new StatList(30, 10, 30, 15);
                    HungerRate = 5;
                    DangerousLevel = 3;
                    break;
            }
            if (monsterAttackZones.ContainsKey(type))
                AttackZone = monsterAttackZones[type];
            ViewRadius = Stats.Perception / 3 + 2;
            MaxEnergy = Stats.Stamina;
            Energy = MaxEnergy;
            MaxHP = Stats.Stamina * 2;
            HP = MaxHP;
            ExpGain = Stats.Average();
            IsHungry = true;
        }

        private void CheckHunger()
        {
            MovesAfterKill++;
            if (MovesAfterKill > HungerRate)
                IsHungry = true;
        }

        public void MakeHungry()
        {
            IsHungry = true;
            MovesAfterKill = 0;
        }

        public void TakeMove()
        {
            if (GetAliveMonsters().Contains(this))
            {
                var isKilledSomeone = false;
                if (IsHungry)
                {
                    var map = GetMap();
                    var visibleEntities = GetVisibleEntities(this);
                    var path = DijkstraPathFinder.GetPathsByDijkstra(GetMap(),
                        visibleEntities.Select(x => x.Position).ToList(), Position)
                        .OrderBy(x => x.Cost)
                        .FirstOrDefault();
                    if (GetVisibleEntities(this).Count != 0 && path != null)
                    {
                        isKilledSomeone = AttackVisibleEntity(path, visibleEntities);
                    }
                    else if (map[Position].Content.Difficulty == DangerousLevel)
                    {
                        var random = new Random();
                        var nextPoint = Position + DijkstraPathFinder.PossibleDirections[random.Next(0, 3)];
                        var randomPath = new List<Point>();
                        var costOfPath = 0;
                        while(map[nextPoint].Content.Difficulty != DangerousLevel)
                            nextPoint = Position + DijkstraPathFinder.PossibleDirections[random.Next(0, 3)];
                        while ((GetVisibleEntities(this).Count == 0 || DijkstraPathFinder.GetPathsByDijkstra(GetMap(),
                            visibleEntities.Select(x => x.Position).ToList(), Position) == null)
                            && Energy - costOfPath >= map[nextPoint].Content.Cost)     
                        {
                            randomPath.Add(nextPoint);
                            costOfPath += map[nextPoint].Content.Cost;
                            while (map[nextPoint].Content.Difficulty != DangerousLevel)
                                nextPoint = Position + DijkstraPathFinder.PossibleDirections[random.Next(0, 3)];
                        }
                        map.TakeMove(randomPath, this);
                        if (GetVisibleEntities(this).Count > 0)
                        {
                            path = DijkstraPathFinder.GetPathsByDijkstra(GetMap(),
                            visibleEntities.Select(x => x.Position).ToList(), Position)
                            .OrderBy(x => x.Cost)
                            .FirstOrDefault();
                            isKilledSomeone = AttackVisibleEntity(path, GetVisibleEntities(this));
                        }
                    }
                    else
                    {
                        var pathAway = GreedyPathFinder.FindPathToCompleteGoal(map,
                            map.ZonesZones[(Zones)DangerousLevel], this).Skip(1).ToList();
                        map.TakeMove(pathAway, this);
                    }
                }
                if (isKilledSomeone && HungerRate > 0)
                {
                    MovesAfterKill = 0;
                    IsHungry = false;
                }
                else if (!IsHungry)
                    CheckHunger();
                RestoreEnergy();
            }
        }

        private bool AttackVisibleEntity(PathWithCost path, List<Entity> visibleEntities)
        {
            var isKilled = false;
            var map = GetMap();
            map.TakeMove(path.Path, this);
            var possibleTarget = DijkstraPathFinder.PossibleDirections
                    .Select(x => Position + x)
                    .ToList();
            var target = possibleTarget
                    .Where(x => visibleEntities.Contains(map[x].Content.Entity)).Count() > 0
                    ? possibleTarget
                        .Where(x => visibleEntities.Contains(map[x].Content.Entity))
                        .First()
                    : possibleTarget
                        .First();
            if (monsterAttackZones.ContainsKey(Type))
                isKilled = Attack(null);
            else
            {
                isKilled = Attack(target);
            }
            return isKilled;
        }

        public bool Attack(Point? target)
        {
            var zone = target == null ? AttackZone.Select(x => Position + x).ToList() : new List<Point> { (Point)target };
            var attack = AttackMethods.AttackConstructor(Type, this, zone);
            var isLethal = GetMap().IsLethalAttack(attack, zone);
            MonsterAttack?.Invoke(this, attack);
            return isLethal;
        }

        private static Dictionary<MonsterTypes, List<Size>> monsterAttackZones = new Dictionary<MonsterTypes, List<Size>>
        {
            [MonsterTypes.Dragon] = Map.GetCircleZonePattern(2, 0),
            [MonsterTypes.Hydra] = Map.GetCircleZonePattern(2, 0),
            [MonsterTypes.Wyvern] = Map.GetCircleZonePattern(2, 0),
            [MonsterTypes.Troll] = Map.GetCircleZonePattern(2, 0)
        };
    }

    public enum MonsterTypes
    {
        Slime,
        Spider,
        Wolf,
        Goblin,
        Orc,
        Troll,
        Wyvern,
        Hydra,
        Dragon
    }
}
