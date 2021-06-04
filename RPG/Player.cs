using System.Collections.Generic;
using System.Drawing;

namespace RPG
{
    public class Player : Entity
    {
        private int experience;
        private int level;

        public int Level
        {
            get => level;
            set
            {
                if (level < 20)
                {
                    level = value;
                    StatPoints += level % 5 == 0 ? 7 : 5;
                    RestoreHP();
                    RestoreEnergy();
                }
            }
        }
        public int Experience
        {
            get => experience;
            set
            {
                experience = value;
                while (experience >= Level * 10)
                {
                    experience -= Level * 10;
                    Level++;
                }
            }
        }
        public int StatPoints;
        public List<Treasure> Treasures;

        public Player(Map map)
        {
            Treasures = new List<Treasure>(3);
            IsPlayer = true;
            Stats = new StatList(3, 3, 3, 3);
            Position = map.Player;
            map[Position].Content.Entity = this;
            Skin = Properties.Resources.player;
            MaxEnergy = Stats.Agility * Stats.Stamina / 2;
            Energy = MaxEnergy;
            MaxHP = 10 + Stats.Stamina * 2;
            HP = MaxHP;
            ViewRadius = 3;
            level = 1;
            StatPoints = 4;
            Experience = 0;
        }

        public void LevelUp(int value)
        {
            for (var i = 0; i < value; i++)
                Level++;
        }

        public void IncreaseStat(Stats stat, int value)
        {
            if (StatPoints > 0)
            {
                switch (stat)
                {
                    case RPG.Stats.Strength:
                        Stats.Strength += value;
                        break;
                    case RPG.Stats.Agility:
                        Stats.Agility += value;
                        MaxEnergy = Stats.Agility * Stats.Stamina / 2;
                        break;
                    case RPG.Stats.Stamina:
                        Stats.Stamina += value;
                        MaxEnergy = Stats.Agility * Stats.Stamina / 2;
                        MaxHP = 10 + Stats.Stamina * 2;
                        break;
                    case RPG.Stats.Perception:
                        Stats.Perception += value;
                        ViewRadius = ViewRadius <= 5 ? 3 + (Stats.Perception - 3) / 3 : 5;
                        break;
                }
                StatPoints--;
            }
        }
    }
}
