namespace OrcGame.Entity.Item;
public class BaseItem : Entity
{
	public float Weight { get; protected set; } = 0;
	public Material Material { get; protected set; } = Material.None;

}
public enum Material
{
	None,
	Bone,
	Wood,
	Stone
}


