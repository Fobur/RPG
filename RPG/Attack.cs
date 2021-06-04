using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RPG
{
    public class Attack
    {
        public readonly List<Point> Zone;
        public readonly int Damage;
        public readonly int Cost;

        public Attack(List<Point> zone, int damage, int cost)
        {
            Zone = zone;
            Damage = damage;
            Cost = cost;
        }
    }

    public static class AttackMethods
    {
        public static Attack AttackConstructor(AttacksName name, Player attacker)
        {
            var zone = Zones[name].Select(x => attacker.Position + x).ToList();
            var cost = 0;
            var damage = 0;
            switch (name)
            {
                case AttacksName.Round:
                    damage = attacker.Stats.Strength * 2;
                    cost = attacker.MaxEnergy / 10 > 0
                        ? attacker.MaxEnergy / 10
                        : 1;
                    break;
                case AttacksName.Range:
                    damage = attacker.Stats.Strength + attacker.Stats.Agility;
                    cost = attacker.MaxEnergy / 8 > 0
                        ? attacker.MaxEnergy / 8
                        : 1;
                    break;
                case AttacksName.Heavy:
                    damage = attacker.Stats.Strength * 3;
                    cost = attacker.MaxEnergy / 3 > 0
                        ? attacker.MaxEnergy / 3
                        : 1;
                    break;
            }
            return new Attack(zone, damage, cost);
        }

        public static Attack AttackConstructor(MonsterTypes type, Entity attaker, List<Point> zone)
        {
            var multiplier = 1.0;
            switch (type)
            {
                case MonsterTypes.Dragon:
                    multiplier = 2;
                    break;
                case MonsterTypes.Hydra:
                    multiplier = 2;
                    break;
                case MonsterTypes.Wyvern:
                    multiplier = 2;
                    break;
                case MonsterTypes.Troll:
                    multiplier = 2;
                    break;
                case MonsterTypes.Orc:
                    multiplier = 2;
                    break;
                case MonsterTypes.Spider:
                    multiplier = 1.5;
                    break;
            }
            return new Attack(zone, (int)(attaker.Stats.Strength * multiplier), attaker.Energy);
        }

        public static Dictionary<AttacksName, List<Size>> Zones = new Dictionary<AttacksName, List<Size>>
        {
            [AttacksName.Round] = Map.GetCircleZonePattern(1, 0),
            [AttacksName.Range] = Map.GetCircleZonePattern(2, 1),
            [AttacksName.Heavy] = Map.GetCircleZonePattern(2, 0)
        };

        public static AttacksName ConvertFromString(string name)
        {
            AttacksName res;
            switch (name)
            {
                case "Round":
                    res = AttacksName.Round;
                    break;
                case "Range":
                    res = AttacksName.Range;
                    break;
                case "Heavy":
                    res = AttacksName.Heavy;
                    break;
                default:
                    throw new Exception("Invalid attack name");
            }
            return res;
        }
    }

    public enum AttacksName
    {
        Range,
        Round,
        Heavy
    }
}
