namespace OrcGame.Entity.Item;

public class Bone : BaseItem
{
    public Bone()
    {
        Material = Material.Bone;
        Weight = 0.1f;
        EntityName = "Bone";
        InstanceName = "Bone";
    }
}
