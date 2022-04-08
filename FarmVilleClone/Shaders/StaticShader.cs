using System;
using OpenTK;

namespace FarmVilleClone.Shaders
{
    public class StaticShader : BaseShader
    {
        private static string VERTEX_FILE = "./../../Shaders/VertexShader.txt";
        private static string FRAGMENT_FILE = "./../../Shaders/FragmentShader.txt";
        private int location_TransformationMatrix;
        private int location_ProjectionMatrix;
        private int location_ViewMatrix;

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
            location_ProjectionMatrix = base.GetUniformLocation("projectionMatrix");
            location_ViewMatrix = base.GetUniformLocation("viewMatrix");
        }

        public void LoadTransformationMatrix(Matrix4 matrix)
        {
            base.LoadMatrix(location_TransformationMatrix, matrix);
        }

        public void LoadProjectionMatrix(Matrix4 projection)
        {
            base.LoadMatrix(location_ProjectionMatrix, projection);
        }

        public void LoadViewMatrix(Matrix4 view)
        {
            base.LoadMatrix(location_ViewMatrix, view);
        }
    }
}
