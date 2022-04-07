using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Buffer = OpenTK.Graphics.OpenGL.Buffer;

namespace FarmVilleClone.RenderEngine
{
    public class ModelLoader
    {
        // A 'VAO' is a Vertex Array Object

        private List<int> vaos = new List<int>();
        private List<int> vbos = new List<int>();

        public RawModel LoadToVAO(Vector3[] positions)
        {
            int vaoID = CreateVAO();
            CreateVBO(0, positions);
            UnbindVAO();
            return new RawModel(vaoID, positions.Length);
        }

        public void CleanUp()
        {
            foreach(int vao in vaos) { GL.DeleteVertexArray(vao); }
            foreach(int vbo in vbos) { GL.DeleteBuffer(vbo); }
        }

        private int CreateVAO()
        {
            int vertexArrayObject = GL.GenVertexArray();
            vaos.Add(vertexArrayObject);

            GL.BindVertexArray(vertexArrayObject);
            return vertexArrayObject;
        }

        private void CreateVBO(int attNum, Vector3[] positions)
        {
            int vertexBufferObject = GL.GenBuffer();
            vbos.Add(vertexBufferObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * 6, positions, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attNum, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        private void UnbindVAO()
        {
            GL.BindVertexArray(0);
        }
    }
}
