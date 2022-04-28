using FarmVilleClone.Common;
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
        private int _locationShineDamper;
        private int _locationReflectivity;
        private int _locationUsesFakeLighting;

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
            _locationShineDamper = GetUniformLocation("shineDamper");
            _locationReflectivity = GetUniformLocation("reflectivity");
            _locationReflectivity = GetUniformLocation("usesFakeLighting");
        }

        public void LoadFakeLighting(bool useFakeLighting)
        {
            LoadBoolean(_locationUsesFakeLighting, useFakeLighting);
        }

        public void LoadTransformationMatrix(Matrix4 matrix)
        {
            LoadMatrix(_locationTransformationMatrix, matrix);
        }

        public void LoadProjectionMatrix(Matrix4 projection)
        {
            LoadMatrix(_locationProjectionMatrix, projection);
        }

        public void LoadViewMatrix(Camera camera)
        {
            var view = LinearAlgebra.CreateViewMatrix(camera.GetPosition(), camera.GetTarget(), camera.GetCameraUp());
            LoadMatrix(_locationViewMatrix, view);
        }

        public void LoadLight(Light light)
        {
            LoadVector(_locationLightPosition, light.GetPosition());
            LoadVector(_locationLightColor, light.GetColor());
        }

        public void LoadShine(float damper, float reflectivity)
        {
            LoadFloat(_locationShineDamper, damper);
            LoadFloat(_locationReflectivity, reflectivity);
        }
    }
}
