using Microsoft.Xna.Framework;
using OrcGame.Utility;

namespace OrcGame.UserInterface;

public class DebugInfoPanel : SpritePanel
{
    public DebugInfoPanel(Game game) : base(game)
    {
        Position = new IntVector2(game.GraphicsDevice.Viewport.Width - 200, 20);
        AssignedSize = new IntVector2(200, 300);
    }
}