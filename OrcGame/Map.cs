using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrcGame.Utility;

namespace OrcGame;

public class Map : DrawableGameComponent
{
    private readonly int _width;
    private readonly int _height;
    private readonly MapTile[,] _tiles;
    private readonly Rectangle _bounds;
    private readonly IntVector2 _tileSize = new IntVector2(16, 16);

    public Map(Game game, int width, int height, Rectangle rect) : base(game)
    {
        _width = width;
        _height = height;
        _bounds = rect;
        _tiles = new MapTile[_width, _height];
    }

    public void GenerateMap()
    {
        
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                _tiles[i, j] = new MapTile(Game, new IntVector2(i, j));
            }
        }
    }

    public MapTile TileAtPoint(Point point)
    {
        var adjustedPoint = point - _bounds.Location;
        var mapLocation = (adjustedPoint / _tileSize).ToIntVector2();
        return TileAtLocation(mapLocation);
    }

    public MapTile TileAtLocation(IntVector2 location)
    {
        return _tiles[location.X, location.Y];
    }

    public override void Draw(GameTime gameTime)
    {
        var batch = Game.Services.GetService<SpriteBatch>();
        foreach (var tile in _tiles)
        {
            var tileBounds = new Rectangle(_bounds.Location + tile.Location * _tileSize, _tileSize);
            batch.Draw(tile.Texture, tileBounds, tile.TextureRect, tile.Color);
        }
        
        base.Draw(gameTime);
    }
}