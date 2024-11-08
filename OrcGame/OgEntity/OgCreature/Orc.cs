using System.Collections.Generic;
using OrcGame.Utility;


namespace OrcGame.OgEntity.OgCreature;

public class Orc : Creature
{
    public Orc()
    {
        SpriteLocation = new IntVector2(25, 2);
        CreatureType = CreatureType.Humanoid;
        CreatureSubtype = CreatureSubtype.Orc;
    }
}