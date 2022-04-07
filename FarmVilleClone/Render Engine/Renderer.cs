using System;
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

        public void Render(RawModel model)
        {
            GL.Color3(1, 1, 1);
            GL.BindVertexArray(model.getVaoID());
            GL.EnableVertexAttribArray(0);
            GL.DrawArrays(PrimitiveType.Triangles, 0, model.getVertexCount());
            GL.DisableVertexAttribArray(0);
            GL.BindVertexArray(0);
        }
    }
}
