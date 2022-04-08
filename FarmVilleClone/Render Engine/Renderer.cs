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
        private static readonly float FOV = 70.0f;
        private static readonly float NEAR_PLANE = 0.1f;
        private static readonly float FAR_PLANE = 100f;

        public Renderer(StaticShader shader)
        {
            var projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FOV), 1280 / 720, NEAR_PLANE, FAR_PLANE);
            shader.Start();
            shader.LoadProjectionMatrix(projectionMatrix);
            shader.Stop();
        }

        public void Prepare()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);
        }

        public void Render(Entity entity, StaticShader shader, Camera camera)
        {
            TexturedModel model = entity.GetModel();
            RawModel rawModel = model.getRawModel();

            GL.BindVertexArray(rawModel.getVaoID());
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            Matrix4 transformationMatrix = LinearAlgebra.CreateTransformationMatrix(
                entity.GetPosition(), entity.GetRotationX(), entity.GetRotationY(), entity.GetRotationZ(), entity.GetScale());

            Matrix4 viewMatrix = LinearAlgebra.CreateViewMatrix(camera.getPosition(), camera.getTarget(), camera.getCameraUp());

            shader.LoadTransformationMatrix(transformationMatrix);
            shader.LoadViewMatrix(viewMatrix);

            GL.DrawElements(PrimitiveType.Triangles, rawModel.getVertexCount(), DrawElementsType.UnsignedInt, 0);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(0);
            GL.BindVertexArray(0);
        }
    }
}
