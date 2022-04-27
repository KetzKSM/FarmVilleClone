using System.Collections;
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
        private StaticShader _entityShader;
        private EntityRenderer _entityRenderer;
        
        private TerrainShader _terrainShader;
        private TerrainRenderer _terrainRenderer;
        
        private Dictionary<TexturedModel, List<Entity>> _entities;
        private List<Terrain> _terrains;

        private const float Fov = 70.0f;
        private const float NearPlane = .1f;
        private const float FarPlane = 1000f;

        public MasterRenderer()
        {
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            
            var projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Fov), 1280f / 720f, NearPlane, FarPlane);

            _entityShader = new StaticShader();
            _entityRenderer = new EntityRenderer(_entityShader, projectionMatrix);
            
            _terrainShader = new TerrainShader();
            _terrainRenderer = new TerrainRenderer(_terrainShader, projectionMatrix);
            
            _entities = new Dictionary<TexturedModel, List<Entity>>();
            _terrains = new List<Terrain>();
        }

        public void Render(Light light, Camera camera)
        {
            Prepare();
            _entityShader.Start();
            _entityShader.LoadLight(light);
            _entityShader.LoadViewMatrix(camera);
            _entityRenderer.Render(_entities);
            _entityShader.Stop();
            
            _terrainShader.Start();
            _terrainShader.LoadLight(light);
            _terrainShader.LoadViewMatrix(camera);
            _terrainRenderer.RenderTerrain(_terrains);
            _terrainShader.Stop();
            
            _terrains.Clear();
            _entities.Clear();
        }

        public void ProcessTerrain(Terrain terrain)
        {
            _terrains.Add(terrain);
        }

        public void ProcessEntity(Entity entity)
        {
            var texturedModel = entity.GetModel();
            if (_entities.TryGetValue(texturedModel, out var batch))
            {
                batch.Add(entity);
            }
            else
            {
                var newBatch = new List<Entity> {entity};
                _entities[texturedModel] = newBatch;
            }
        }

        public void CleanUp()
        {
            _entityShader.CleanUp();
            _terrainShader.CleanUp();
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