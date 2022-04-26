using FarmVilleClone.Common;
using FarmVilleClone.Entities;
using FarmVilleClone.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace FarmVilleClone.Render_Engine
{
    public class Renderer
    {
        private const float Fov = 70.0f;
        private const float NearPlane = .1f;
        private const float FarPlane = 100f;

        public Renderer(StaticShader shader)
        {
            var projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Fov), 1280f / 720f, NearPlane, FarPlane);
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
            var model = entity.GetModel();
            var rawModel = model.GetRawModel();

            GL.BindVertexArray(rawModel.GetVaoId());
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);

            var transformationMatrix = LinearAlgebra.CreateTransformationMatrix(
                entity.GetPosition(), entity.GetRotationX(), entity.GetRotationY(), entity.GetRotationZ(), entity.GetScale());

            var viewMatrix = LinearAlgebra.CreateViewMatrix(camera.GetPosition(), camera.GetTarget(), camera.GetCameraUp());

            shader.LoadTransformationMatrix(transformationMatrix);
            shader.LoadViewMatrix(viewMatrix);

            GL.DrawElements(PrimitiveType.Triangles, rawModel.GetVertexCount(), DrawElementsType.UnsignedInt, 0);
            
            GL.DisableVertexAttribArray(2);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(0);
            GL.BindVertexArray(0);
        }
    }
}
