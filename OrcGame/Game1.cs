using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OrcGame.Entity;

namespace OrcGame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D myTex;
    private Rectangle[] _atlas;

    private SpriteFont gameFont;

    

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here


        base.Initialize();
    }

    protected override void LoadContent()
    {


        _spriteBatch = new SpriteBatch(GraphicsDevice);
        myTex = Content.Load<Texture2D>("Graphics/dirt-tiles");

        _atlas = new Rectangle[1];
        _atlas[0] = new Rectangle(32, 16, 16, 16);
        gameFont = Content.Load<SpriteFont>("NK57Condensed");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        _spriteBatch.Draw(myTex, Vector2.Zero, _atlas[0], Color.White);
        _spriteBatch.DrawString(gameFont, "Hello world", new Vector2(100, 100), Color.Black);
        _spriteBatch.End();

        //_textBatch.Begin();
        //_textBatch.DrawString(gameFont, "Hello world", new Vector2(100, 100), Color.Black);
        //_textBatch.End();

        base.Draw(gameTime);
    }
}

