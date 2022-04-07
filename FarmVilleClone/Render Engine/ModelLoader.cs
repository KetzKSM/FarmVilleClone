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
        // 'VBO' = Virtual Buffer Object

        private List<int> vaos = new List<int>();
        private List<int> vbos = new List<int>();

        public RawModel LoadToVAO(Vector3[] positions, int[] indices)
        {
            int vaoID = CreateVAO();
            BindIndicesBuffer(indices);
            CreateVBO(0, positions);
            UnbindVAO();
            return new RawModel(vaoID, indices.Length);
        }

        public void CleanUp()
        {
            foreach(int vao in vaos) { GL.DeleteVertexArray(vao); }
            foreach(int vbo in vbos) { GL.DeleteBuffer(vbo); }
        }

        private int CreateVAO()
        {
            int vaoID = GL.GenVertexArray();
            vaos.Add(vaoID);

            GL.BindVertexArray(vaoID);
            return vaoID;
        }

        private void CreateVBO(int attNum, Vector3[] positions)
        {
            int vboID = GL.GenBuffer();
            vbos.Add(vboID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboID);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * positions.Length, positions, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attNum, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        private void UnbindVAO()
        {
            GL.BindVertexArray(0);
        }

        private void BindIndicesBuffer(int[] indices)
        {
            int eboID = GL.GenBuffer();
            vbos.Add(eboID);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, eboID);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * indices.Length, indices, BufferUsageHint.StaticDraw);
            //GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
    }
}
