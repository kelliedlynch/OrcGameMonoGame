using Microsoft.Xna.Framework;
using OrcGame.Utility;

namespace OrcGame;

public class MapTile : SpriteRepresented
{
    public IntVector2 Location;
    // public int X => Location.X;
    // public int Y => Location.Y;
    
    public MapTile(Game game, IntVector2 location) : base(game)
    {
        Location = location;
        SpriteLocation = new IntVector2(1, 0);
    }
}