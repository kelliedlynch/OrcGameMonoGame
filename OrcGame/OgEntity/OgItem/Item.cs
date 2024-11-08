using OrcGame.OgEntity;

namespace OrcGame.OgEntity.OgItem;
public class Item : Entity
{
	public float Weight { get; protected set; } = 0;
	public MaterialType Material { get; protected set; } = MaterialType.None;
	
	public Entity TaggedBy { get; set; } = null;
	public bool IsTagged => TaggedBy != null;
	
	public Entity OwnedBy { get; set; } = null;
	public bool IsOwned => OwnedBy != null;
	
	public Entity CarriedBy { get; set; } = null;
	public bool IsCarried => CarriedBy != null;

}
public enum MaterialType
{
	None,
	Bone,
	Wood,
	Stone
}


