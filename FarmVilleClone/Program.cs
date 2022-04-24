using FarmVilleClone.Render_Engine;

namespace FarmVilleClone
{
    internal static class MainClass
    {
        public static void Main(string[] args)
        {
            using (Game game = new Game (1280, 720, "FarmVilleClone"))
            {
                game.Run(60.0);
            }
        }
    }
}
