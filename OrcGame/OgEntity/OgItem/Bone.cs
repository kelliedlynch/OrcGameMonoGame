using System;
using MonoGame.Extended;

namespace OrcGame.OgEntity.OgItem;

public class Bone : Item
{
    public Bone()
    {
        Material = MaterialType.Bone;
        var rand = new Random();
        var r = rand.Next() % 40;
        Weight = 0.1f * (float)r;
        EntityName = "Bone";
        InstanceName = "Bone";
    }
}
