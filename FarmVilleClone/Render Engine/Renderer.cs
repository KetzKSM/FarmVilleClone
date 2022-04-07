using System;
using FarmVilleClone.Common;
using FarmVilleClone.Entities;
using FarmVilleClone.Models;
using FarmVilleClone.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace FarmVilleClone.RenderEngine
{
    public class Renderer
    {
        public Renderer()
        {
        }

        public void Prepare()
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public void Render(Entity entity, StaticShader shader)
        {
            TexturedModel model = entity.GetModel();
            RawModel rawModel = model.getRawModel();

            GL.Color3(1, 1, 1);

            GL.BindVertexArray(rawModel.getVaoID());
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            Matrix4 transformationMatrix = LinearAlgebra.CreateTransformationMatrix(
                entity.GetPosition(), entity.GetRotationX(), entity.GetRotationY(), entity.GetRotationZ(), entity.GetScale());
            shader.LoadTransformationMatrix(transformationMatrix);

            GL.DrawElements(PrimitiveType.Triangles, rawModel.getVertexCount(), DrawElementsType.UnsignedInt, 0);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(0);
            GL.BindVertexArray(0);
        }
    }
}
