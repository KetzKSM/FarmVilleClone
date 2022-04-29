using System.Collections.Generic;
using FarmVilleClone.Entities;
using FarmVilleClone.Models;
using FarmVilleClone.Shaders;
using FarmVilleClone.Terrains;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace FarmVilleClone.Render_Engine
{
    public class MasterRenderer
    {
        private readonly StaticShader _entityShader;
        private readonly EntityRenderer _entityRenderer;
        private readonly TerrainShader _terrainShader;
        private readonly TerrainRenderer _terrainRenderer;
        private readonly Dictionary<TexturedModel, List<Entity>> _entityDictionary;
        private List<Entity> _entities;
        private readonly List<Terrain> _terrains;
        private readonly Matrix4 _projectionMatrix;

        private const float Fov = 70.0f;
        private const float NearPlane = .1f;
        private const float FarPlane = 1000f;

        public MasterRenderer()
        {
            EnableCulling();
            
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Fov), 1280f / 720f, NearPlane, FarPlane);

            _entityShader = new StaticShader();
            _entityRenderer = new EntityRenderer(_entityShader, _projectionMatrix);
            
            _terrainShader = new TerrainShader();
            _terrainRenderer = new TerrainRenderer(_terrainShader, _projectionMatrix);
            
            _entityDictionary = new Dictionary<TexturedModel, List<Entity>>();
            _entities = new List<Entity>();
            _terrains = new List<Terrain>();
        }

        public Matrix4 GetProjectionMatrix()
        {
            return _projectionMatrix;
        }

        public static void EnableCulling()
        {
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
        }

        public static void DisableCulling()
        {
            GL.Disable(EnableCap.CullFace);
        }

        public void Render(Light light, Camera camera)
        {
            Prepare();
            _entityShader.Start();
            _entityShader.LoadLight(light);
            _entityShader.LoadViewMatrix(camera);
            _entityRenderer.Render(_entityDictionary);
            _entityShader.Stop();
            
            _terrainShader.Start();
            _terrainShader.LoadLight(light);
            _terrainShader.LoadViewMatrix(camera);
            _terrainRenderer.RenderTerrain(_terrains);
            _terrainShader.Stop();
            
            _terrains.Clear();
            _entityDictionary.Clear();
        }

        public void ProcessTerrain(Terrain terrain)
        {
            _terrains.Add(terrain);
        }

        public void ProcessEntity(Entity entity)
        {
            var texturedModel = entity.GetModel();
            if (_entityDictionary.TryGetValue(texturedModel, out var batch))
            {
                batch.Add(entity);
            }
            else
            {
                var newBatch = new List<Entity> {entity};
                _entityDictionary[texturedModel] = newBatch;
            }
            
            _entities.Add(entity);
        }

        public void CleanUp()
        {
            _entityShader.CleanUp();
            _terrainShader.CleanUp();
        }

        public List<Entity> GetEntities()
        {
            return _entities;
        }

        private static void Prepare()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);
        }
        
        
    }
}