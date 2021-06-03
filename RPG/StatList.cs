namespace RPG
{
    public class StatList
    {
        public int Strength;
        public int Agility;
        public int Stamina;
        public int Perception;

        public StatList(int str, int ag, int sta, int per)
        {
            Strength = str;
            Agility = ag;
            Stamina = sta;
            Perception = per;
        }

        public int Average() => (Strength + Agility + Stamina + Perception) / 2;
    }
    public enum Stats
    {
        Strength,
        Agility,
        Stamina,
        Perception,
        Level,
        StatPoints
    }
}
