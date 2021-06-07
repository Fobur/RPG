using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
    public class Treasure : GameObject
    {
        public readonly string Name;
        public Treasure(TreasureNames name, Point position)
        {
            Position = position;
            Passable = true;
            switch(name)
            {
                case TreasureNames.BadDragon:
                    Skin = Properties.Resources.dragonwand;
                    Name = "Bad Dragon";
                    break;
                case TreasureNames.ShitImposedByDeveloper:
                    Skin = Properties.Resources.ShitImposedByDeveloper;
                    Name = "Shit Imposed By Developer";
                    break;
                case TreasureNames.TerraToilet:
                    Skin = Properties.Resources.terratoilet;
                    Name = "Terra Toilet";
                    break;
            }
        }
    }

    public enum TreasureNames
    {
        BadDragon,
        ShitImposedByDeveloper,
        TerraToilet
    }
}
