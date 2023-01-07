namespace OrcGame.Entity.Item;
public class BaseItem : Entity
{
	public float Weight;
	public Material Material;

}
public enum Material
{
	None,
	Bone,
	Wood,
	Stone
}


