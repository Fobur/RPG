using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RPG
{
    public partial class Model
    {
        public Map Map;
        public Player Player;
        public List<Monster> Monsters;
        public List<Monster> AliveMonsters;

        public Model()
        {
            Map = new Map(41);
            Player = new Player(Map);
            Monsters = CreateMonsters();
            AliveMonsters = new List<Monster>();
            AliveMonsters.AddRange(Monsters);
            foreach (var monster in Monsters)
            {
                monster.MonsterAttack += (s, a) => CheckAttackZone(a, false, monster);
                monster.GetMap = () => Map;
                monster.GetVisibleEntities = (monster) => Map.GetAllEntityInView(monster);
                monster.GetAliveMonsters = () => AliveMonsters;
            }
        }

        private List<Monster> CreateMonsters()
        {

            var monsters = new List<Monster>();
            for (var i = 0; i < 4; i++)
            {
                CreateAllMonsterTypes(1, monsters);
            }
            CreateAllMonsterTypes(2, monsters);
            CreateAllMonsterTypes(3, monsters);
            return monsters.OrderByDescending(x => x.Stats.Agility).ToList();
        }

        private void CreateAllMonsterTypes(int dangerousLevel, List<Monster> monsters)
        {
            var random = new Random();
            var point = Map.GetRandomPoint((Zones)dangerousLevel, random);
            var beginIndex = (dangerousLevel - 1) * 3;
            var endIndex = dangerousLevel * 3;
            for (var i = beginIndex; i < endIndex; i++)
            {
                while (Map[point].Content.Entity != null)
                    point = Map.GetRandomPoint((Zones)dangerousLevel, random);
                var monster = new Monster((MonsterTypes)i, point, Map);
                Map[point].Content.Entity = monster;
                monsters.Add(monster);
            }
        }

        public void CheckAttackZone(Attack attack, bool playerAttacked, Entity attacker)
        {
            foreach (var tile in attack.Zone)
            {
                var underAttack = AliveMonsters
                    .Select(x => (Entity)x)
                    .Concat(new List<Entity> { Player })
                    .Where(x => x.Position == tile)
                    .FirstOrDefault();
                if (underAttack != null)
                {
                    underAttack.HP = underAttack.HP >= attack.Damage
                        ? underAttack.HP - attack.Damage
                        : 0;
                    if (!underAttack.IsPlayer)
                    {
                        ((Monster)underAttack).MakeHungry();
                        if (playerAttacked)
                            GameForm.AddIntoLog("Player deal " + attack.Damage.ToString() + "DMG to " +((Monster)underAttack).Type.ToString(), Color.Green);
                    }
                    else
                        GameForm.AddIntoLog("Player get " + attack.Damage.ToString() + "DMG from " + ((Monster)attacker).Type.ToString(), Color.Red);
                    if (underAttack.IsDead)
                    {
                        if (playerAttacked)
                        {
                            Player.Experience += ((Monster)underAttack).ExpGain;
                            GameForm.AddIntoLog("Player killed " + ((Monster)underAttack).Type.ToString(), Color.Green);
                            GameForm.AddIntoLog("Player gain " + ((Monster)underAttack).ExpGain.ToString()+"EXP", Color.Green);
                        }
                        else
                            GameForm.AddIntoLog("Something killed " + ((Monster)underAttack).Type.ToString(), Color.Red);
                        if (!underAttack.IsPlayer)
                            AliveMonsters.Remove((Monster)underAttack);
                        else
                            GameForm.AddIntoLog("Player was killed by  " + ((Monster)attacker).Type.ToString(), Color.Red);
                        Map[underAttack.Position].Content.Entity = null;
                    }
                }
            }
        }

        public void CheckMonsters()
        {
            Monsters = new List<Monster>();
            Monsters.AddRange(AliveMonsters.OrderByDescending(x => x.Stats.Agility));
        }
    }
}
