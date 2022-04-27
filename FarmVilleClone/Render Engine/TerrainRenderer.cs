using System.Collections.Generic;
using FarmVilleClone.Common;
using FarmVilleClone.Entities;
using FarmVilleClone.Models;
using FarmVilleClone.Shaders;
using FarmVilleClone.Terrains;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace FarmVilleClone.Render_Engine
{
    public class TerrainRenderer
    {
        private TerrainShader _shader;

        public TerrainRenderer(TerrainShader shader, Matrix4 projectionMatrix)
        {
            _shader = shader;
            _shader.Start();
            _shader.LoadProjectionMatrix(projectionMatrix);
            _shader.Stop();
        }

        public void RenderTerrain(List<Terrain> terrains)
        {
            foreach (var terrain in terrains)
            {
                PrepareTerrain(terrain);
                LoadTerrainTransformationMatrix(terrain);
                GL.DrawElements(PrimitiveType.Triangles, terrain.GetModel().GetVertexCount(), DrawElementsType.UnsignedInt, 0);
                UnbindTerrain();
            }
        }
        
        private void PrepareTerrain(Terrain terrain)
        {
            var rawModel = terrain.GetModel();

            GL.BindVertexArray(rawModel.GetVaoId());
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);

            var texture = terrain.GetTexture();
            _shader.LoadShine(texture.GetShineDamper(), texture.GetReflectivity());
            
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture.GetId());
        }

        private static void UnbindTerrain()
        {
            GL.DisableVertexAttribArray(2);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(0);
            GL.BindVertexArray(0);
        }

        private void LoadTerrainTransformationMatrix(Terrain terrain)
        {
            var terrainPos = new Vector3(terrain.GetX(), 0, terrain.GetZ());
            var transformationMatrix = LinearAlgebra.CreateTransformationMatrix(
                terrainPos, 0, 0, 0, 1);
            _shader.LoadTransformationMatrix(transformationMatrix);
        }
    }
}