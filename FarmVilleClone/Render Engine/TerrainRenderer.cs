using System.Collections.Generic;
using FarmVilleClone.Common;
using FarmVilleClone.Entities;
using FarmVilleClone.Models;
using FarmVilleClone.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace FarmVilleClone.Render_Engine
{
    public class TerrainRenderer
    {
        private readonly TerrainShader _shader;

        public TerrainRenderer(TerrainShader shader, Matrix4 projectionMatrix)
        {
            _shader = shader;
            _shader.Start();
            _shader.LoadProjectionMatrix(projectionMatrix);
            _shader.Stop();
        }

        public void RenderTerrain(Dictionary<TexturedModel, List<Terrain>> terrainDictionary)
        {
            foreach (var model in terrainDictionary)
            {
                var texturedModel = model.Key;
                PrepareTexturedModel(texturedModel);
                var batch = terrainDictionary[texturedModel];

                foreach (var terrain in batch)
                {
                    PrepareTerrain(terrain);
                    GL.DrawElements(PrimitiveType.Triangles, texturedModel.GetRawModel().GetVertexCount(), DrawElementsType.UnsignedInt, 0);
                }
                UnbindTerrain();
            }
        }

        private static void UnbindTerrain()
        {
            MasterRenderer.EnableCulling();
            
            GL.DisableVertexAttribArray(2);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(0);
            GL.BindVertexArray(0);
        }

        private void PrepareTexturedModel(TexturedModel model)
        {
            var rawModel = model.GetRawModel();

            GL.BindVertexArray(rawModel.GetVaoId());
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);
            
            var texture = model.GetModelTexture();

            if (texture.IsTransparent())
            {
                MasterRenderer.DisableCulling();
            }
            
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, model.GetModelTexture().GetId());
        }
        
        private void PrepareTerrain(Terrain terrain)
        {
            var transformationMatrix = LinearAlgebra.CreateTransformationMatrix(
                terrain.GetPosition(), 0, 0, 0, terrain.GetScale());
            _shader.LoadTransformationMatrix(transformationMatrix);
        }
    }
}