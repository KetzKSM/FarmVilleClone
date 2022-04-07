using System;
namespace FarmVilleClone.Shaders
{
    public class StaticShader : BaseShader
    {
        private static string VERTEX_FILE = "./../../Shaders/VertexShader.txt";
        private static string FRAGMENT_FILE = "./../../Shaders/FragmentShader.txt";

        public StaticShader() : base(VERTEX_FILE, FRAGMENT_FILE)
        { }

        protected override void BindAttributes()
        {
            base.BindAttribute(0, "position");
        }
    }
}
