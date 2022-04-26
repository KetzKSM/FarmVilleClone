using FarmVilleClone.Entities;
using OpenTK;

namespace FarmVilleClone.Shaders
{
    public class StaticShader : BaseShader
    {
        private static readonly string VertexFile = "./../../Shaders/VertexShader.glsl";
        private static readonly string FragmentFile = "./../../Shaders/FragmentShader.glsl";
        
        private int _locationTransformationMatrix;
        private int _locationProjectionMatrix;
        private int _locationViewMatrix;
        private int _locationLightPosition;
        private int _locationLightColor;

        public StaticShader() : base(VertexFile, FragmentFile)
        { }

        protected override void BindAttributes()
        {
            BindAttribute(0, "position");
            BindAttribute(1, "textureCoords");
            BindAttribute(2, "normal");
        }

        protected override void GetAllUniformLocations()
        {
            _locationTransformationMatrix = GetUniformLocation("transformationMatrix");
            _locationProjectionMatrix = GetUniformLocation("projectionMatrix");
            _locationViewMatrix = GetUniformLocation("viewMatrix");
            _locationLightPosition = GetUniformLocation("lightPosition");
            _locationLightColor = GetUniformLocation("lightColor");
        }

        public void LoadTransformationMatrix(Matrix4 matrix)
        {
            LoadMatrix(_locationTransformationMatrix, matrix);
        }

        public void LoadProjectionMatrix(Matrix4 projection)
        {
            LoadMatrix(_locationProjectionMatrix, projection);
        }

        public void LoadViewMatrix(Matrix4 view)
        {
            LoadMatrix(_locationViewMatrix, view);
        }

        public void LoadLight(Light light)
        {
            LoadVector(_locationLightPosition, light.GetPosition());
            LoadVector(_locationLightColor, light.GetColor());
        }
    }
}
