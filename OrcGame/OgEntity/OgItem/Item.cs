using OrcGame.OgEntity;

namespace OrcGame.OgEntity.OgItem;
public class Item : Entity
{
	public float Weight { get; protected set; } = 0;
	public MaterialType Material { get; protected set; } = MaterialType.None;

}
public enum MaterialType
{
	None,
	Bone,
	Wood,
	Stone
}


