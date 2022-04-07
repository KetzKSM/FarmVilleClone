using System;
using OpenTK;

namespace FarmVilleClone.Shaders
{
    public class StaticShader : BaseShader
    {
        private static string VERTEX_FILE = "./../../Shaders/VertexShader.txt";
        private static string FRAGMENT_FILE = "./../../Shaders/FragmentShader.txt";
        private int location_TransformationMatrix;

        public StaticShader() : base(VERTEX_FILE, FRAGMENT_FILE)
        { }

        protected override void BindAttributes()
        {
            base.BindAttribute(0, "position");
            base.BindAttribute(1, "textureCoords");
        }

        protected override void GetAllUniformLocations()
        {
            location_TransformationMatrix = base.GetUniformLocation("transformationMatrix");
        }

        public void LoadTransformationMatrix(Matrix4 matrix)
        {
            base.LoadMatrix(location_TransformationMatrix, matrix);
        }
    }
}
