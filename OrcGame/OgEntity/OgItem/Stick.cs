using MonoGame.Extended;

namespace OrcGame.OgEntity.OgItem;

public class Stick : Item
{
    public Stick()
    {
        Material = MaterialType.Wood;
        var rand = new FastRandom(57465);
        var r = rand.Next() % 90;
        Weight = 0.1f * (float)r;
        EntityName = "Stick";
        InstanceName = "Stick";
    }
}