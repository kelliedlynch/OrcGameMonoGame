using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using OrcGame.OgEntity.OgCreature;
using OrcGame.UserInterface;
using OrcGame.Utility;
using Serilog;
using Serilog.Core;

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
        Services.AddService(_spriteBatch);
        myTex = Content.Load<Texture2D>("Graphics/dirt-tiles");

        _atlas = new Rectangle[1];
        _atlas[0] = new Rectangle(32, 16, 16, 16);
        gameFont = Content.Load<SpriteFont>("NK57Condensed");
        Services.AddService(gameFont);
        
        var orc = new Orc();
        var man = CreatureManager.GetCreatureManager();
        man.AddCreatureToWorld(orc);
    }

    protected override void BeginRun()
    {
        var map = new Map(this, 10, 10, Rectangle.Empty);
        map.GenerateMap();
        Components.Add(map);
        base.BeginRun();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        

        base.Update(gameTime);
    }

    protected override bool BeginDraw()
    {
        _spriteBatch.Begin();
        return base.BeginDraw();
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.DarkOliveGreen);


        var orc = CreatureManager.GetCreatureManager().WorldCreatures.First();
        // TODO: Add your drawing code here
        
        _spriteBatch.Draw(Content.Load<Texture2D>(orc.SpriteSheet), Vector2.Zero, orc.Rectangle, Color.White);
        
        // var label = new TextLabel(this, "Orc Game");
        // Components.Add(label);
        var panel = new DebugInfoPanel(this);
        Components.Add(panel);
        

        //_textBatch.Begin();
        //_textBatch.DrawString(gameFont, "Hello world", new Vector2(100, 100), Color.Black);
        //_textBatch.End();

        base.Draw(gameTime);
    }

    protected override void EndDraw()
    {
        _spriteBatch.End();
        base.EndDraw();
    }
}

