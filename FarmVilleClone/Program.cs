using System;
using FarmVilleClone.RenderEngine;

namespace FarmVilleClone
{
    class MainClass
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
