using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrcGame.Utility;

namespace OrcGame;

public class SpriteRepresented : DrawableGameComponent
{
    private const string SpriteSheet = "Graphics/monochrome-transparent_packed";
    protected IntVector2 SpriteLocation = new(0, 0);
    private readonly IntVector2 _tileSize = new(16, 16);


    protected SpriteRepresented(Game game) : base(game)
    {
        
    }

    public Texture2D Texture => Game.Content.Load<Texture2D>(SpriteSheet);
    public Rectangle TextureRect => new Rectangle(SpriteLocation * _tileSize, _tileSize);

    public Color Color = Color.Beige;
}