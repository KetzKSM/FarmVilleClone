using System.Collections.Generic;
using FarmVilleClone.Common;
using FarmVilleClone.Entities;
using FarmVilleClone.Models;
using FarmVilleClone.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace FarmVilleClone.Render_Engine
{
    public class EntityRenderer
    {
        private readonly StaticShader _shader;

        public EntityRenderer(StaticShader shader, Matrix4 projectionMatrix)
        {
            _shader = shader;
            _shader.Start();
            _shader.LoadProjectionMatrix(projectionMatrix);
            _shader.Stop();
        }

        public void Render(Dictionary<TexturedModel, List<Entity>> entities)
        {
            foreach (var model in entities)
            {
                var texturedModel = model.Key;
                PrepareTexturedModel(texturedModel);
                var batch = entities[texturedModel];

                foreach (var entity in batch)
                {
                    PrepareEntity(entity);
                    GL.DrawElements(PrimitiveType.Triangles, texturedModel.GetRawModel().GetVertexCount(), DrawElementsType.UnsignedInt, 0);
                }
                UnbindTexturedModel();
            }
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
            _shader.LoadFakeLighting(texture.GetUsesFakeLighting());
            _shader.LoadShine(texture.GetShineDamper(), texture.GetReflectivity());
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, model.GetModelTexture().GetId());
        }

        private static void UnbindTexturedModel()
        {
            MasterRenderer.EnableCulling();
            GL.DisableVertexAttribArray(2);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(0);
            GL.BindVertexArray(0);
        }

        private void PrepareEntity(Entity entity)
        {
            var transformationMatrix = LinearAlgebra.CreateTransformationMatrix(
                entity.GetPosition(), entity.GetRotationX(), entity.GetRotationY(), entity.GetRotationZ(), entity.GetScale());
            _shader.LoadTransformationMatrix(transformationMatrix);
        }
    }
}
