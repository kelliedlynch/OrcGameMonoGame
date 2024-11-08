using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using OrcGame.OgEntity.OgItem;
using OrcGame.Utility;
using Vector2 = System.Numerics.Vector2;

namespace OrcGame.OgEntity
{
	public class Entity 
	{
		public Vector2 Location { get; protected set; } = Vector2.Zero;
		public string EntityName { get; protected set; } = "Generic Entity";
		public string InstanceName { get; protected set; } = "Generic Entity Instance";
		

		public string SpriteSheet = "Graphics/monochrome-transparent_packed";
		public IntVector2 SpriteLocation = new(0, 0);
		public IntVector2 TileSize = new(16, 16);

		public Rectangle Rectangle =>
			new Rectangle(SpriteLocation.X * TileSize.X, SpriteLocation.Y * TileSize.Y, TileSize.X, TileSize.Y);

		public Color Color =Color.Beige;

		
		
		public HashSet<Item> Owned { get; protected set; } = new();
		public HashSet<Item> Carried { get; protected set; } = new();
		public HashSet<Item> Tagged { get; protected set; } = new();
		public void AddToOwned(Item item)
		{
			if(Owned.Contains(item)) throw new ArgumentException("Entity already owns that item");
			item.OwnedBy = this;
			Owned.Add(item);
		}
	
		public void RemoveFromOwned(Item item)
		{
			if(!Owned.Contains(item)) throw new ArgumentException("Entity does not own that item");
			item.OwnedBy = null;
			Owned.Add(item);
		}

		public void AddToTagged(Item item)
		{
			if(Tagged.Contains(item)) throw new ArgumentException("Entity already tagged that item");
			item.TaggedBy = this;
			Tagged.Add(item);
		}
		public void RemoveFromTagged(Item item)
		{
			if(!Tagged.Contains(item)) throw new ArgumentException("Entity has not tagged that item");
			item.TaggedBy = null;
			Tagged.Add(item);
		}
		public void AddToCarried(Item item)
		{
			if(Carried.Contains(item)) throw new ArgumentException("Entity already carries that item");
			item.CarriedBy = this;
			Carried.Add(item);
		}
		public void RemoveFromCarried(Item item)
		{
			if(!Carried.Contains(item)) throw new ArgumentException("Entity is not carrying that item");
			item.CarriedBy = null;
			Carried.Add(item);
		}

		public void WalkTo(Vector2 location)
		{
			Location = location;
		}

	}
	
}

