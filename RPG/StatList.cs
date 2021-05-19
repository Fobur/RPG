namespace RPG
{
    public class StatList
    {
        private int strength;
        private int agility;
        private int stamina;

        public int Strength { get => strength; }
        public int Agility { get => agility; }
        public int Stamina { get => stamina; }

        public StatList(int str, int ag, int sta)
        {
            strength = str;
            agility = ag;
            stamina = sta;
        }

        public void IncreaseStat(string stat, int value)
        {
            switch (stat)
            {
                case "strength":
                    strength += value;
                    break;
                case "agility":
                    agility += value;
                    break;
                case "stamina":
                    stamina += value;
                    break;
            }
        }
    }
}
