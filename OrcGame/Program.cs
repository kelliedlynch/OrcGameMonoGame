using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

using var game = new OrcGame.Game1();

game.Run();

