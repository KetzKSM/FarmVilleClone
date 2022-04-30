using System.Collections.Generic;
using FarmVilleClone.Entities;
using FarmVilleClone.Models;
using FarmVilleClone.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace FarmVilleClone.Render_Engine
{
    public class MasterRenderer
    {
        private readonly StaticShader _entityShader;
        private readonly EntityRenderer _entityRenderer;
        private readonly TerrainShader _terrainShader;
        private readonly Dictionary<TexturedModel, List<Entity>> _entityDictionary;
        private readonly Dictionary<TexturedModel, List<Entity>> _terrainDictionary;
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

            _entityDictionary = new Dictionary<TexturedModel, List<Entity>>();
            _terrainDictionary = new Dictionary<TexturedModel, List<Entity>>();
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
            _entityRenderer.Render(_terrainDictionary);
            _entityRenderer.Render(_entityDictionary);
            _entityShader.Stop();
            
            // _terrainShader.Start();
            // _terrainShader.LoadLight(light);
            // _terrainShader.LoadViewMatrix(camera);
            // _terrainRenderer.RenderTerrain(_terrainDictionary);
            // _terrainShader.Stop();
            
            _terrainDictionary.Clear();
            _entityDictionary.Clear();
            // _entities.Clear();
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
        }

        public void ProcessTerrain(List<Entity> terrainField)
        {
            foreach (var terrain in terrainField)
            {
                var texturedModel = terrain.GetModel();
                if (_terrainDictionary.TryGetValue(texturedModel, out var batch))
                {
                    batch.Add(terrain);
                }
                else
                {
                    var newBatch = new List<Entity> {terrain};
                    _terrainDictionary[texturedModel] = newBatch;
                }
            }
        }

        public void CleanUp()
        {
            _entityShader.CleanUp();
            _terrainShader.CleanUp();
        }

        public Dictionary<TexturedModel, List<Entity>> GetEntityDictionary()
        {
            return _entityDictionary;
        }

        public Dictionary<TexturedModel, List<Entity>> GetTerrainDictionary()
        {
            return _terrainDictionary;
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