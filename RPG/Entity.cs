using System.Drawing;

namespace RPG
{
    public class Entity : GameObject
    {
        private int energy;
        private int maxEnergy;
        private int hP;
        private int maxHP;

        public int ViewRadius;
        public StatList Stats;
        public int Energy
        {
            get => energy;
            set => energy = value;
        }
        public int MaxEnergy
        {
            get => maxEnergy;
            set => maxEnergy = value;
        }
        public int HP
        {
            get => hP;
            set
            {
                hP = value;
                if (hP <= 0)
                    Dead();
            }
        }
        public int MaxHP
        {
            get => maxHP;
            set => maxHP = value;
        }
        public bool IsDead;
        public bool IsPlayer;

        public void RestoreEnergy()
        {
            Energy = MaxEnergy;
        }

        public void RestoreHP()
        {
            HP = MaxHP;
        }

        public bool IsTileVisible(Point tile)
        {
            return Map.GetDistanceToTile(tile, Position) <= ViewRadius;
        }

        public bool IsVisible(Entity opponent) => opponent.Stats.Perception >= Stats.Agility / 3
                ? true
                : false;

        virtual public void Dead()
        {
            IsDead = true;
        }
    }
}