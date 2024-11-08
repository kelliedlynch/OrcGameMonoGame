using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace OrcGame;

public class InputManager : GameComponent
{
    private Point? _mouseDownPosition;
    private bool _isMouseDown => _mouseDownPosition != null;
    
    
    public InputManager(Game game) : base(game)
    {
    }

    public override void Update(GameTime gameTime)
    {
        var mouse = Mouse.GetState();
        var mouseLeftDown = mouse.LeftButton == ButtonState.Pressed;
        if (!_isMouseDown && mouseLeftDown)
        {
            _mouseDownPosition = mouse.Position;
        } else if (_isMouseDown && !mouseLeftDown)
        {
            _mouseDownPosition = null;
        }
        
    }
}