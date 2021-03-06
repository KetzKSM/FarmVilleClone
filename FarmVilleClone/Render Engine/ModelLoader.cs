using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using FarmVilleClone.Models;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace FarmVilleClone.Render_Engine
{
    public class ModelLoader
    {
        // A 'VAO' is a Vertex Array Object
        // 'VBO' = Virtual Buffer Object

        private readonly List<int> _vaos = new List<int>();
        private readonly List<int> _vbos = new List<int>();
        private readonly List<int> _textures = new List<int>();

        public RawModel LoadToVao(Vector3[] positions, Vector2[] textureCoords, Vector3[] normals, int[] indices)
        {
            var vaoId = CreateVao();
            BindIndicesBuffer(indices);
            StoreVec3DataInAttributeList(0, positions);
            StoreVec2DataInAttributeList(1, textureCoords);
            StoreVec3DataInAttributeList(2, normals);
            UnbindVao();
            return new RawModel(vaoId, indices.Length);
        }

        public int LoadTexture(string file)
        {
            if (!File.Exists(file)) throw new FileNotFoundException();

            var textureId = GL.GenTexture();
            _textures.Add(textureId);

            GL.BindTexture(TextureTarget.Texture2D, textureId);
            var bmp = new Bitmap(file);
            var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            return textureId;
        }

        public void CleanUp()
        {
            foreach(var vao in _vaos) { GL.DeleteVertexArray(vao); }
            foreach(var vbo in _vbos) { GL.DeleteBuffer(vbo); }
            foreach(var texture in _textures) { GL.DeleteTexture(texture); }
        }

        private int CreateVao()
        {
            var vaoId = GL.GenVertexArray();
            _vaos.Add(vaoId);

            GL.BindVertexArray(vaoId);
            return vaoId;
        }

        private void StoreVec3DataInAttributeList(int attIndex, Vector3[] data, int dimensions = 3)
        {
            var vboId = GL.GenBuffer();
            _vbos.Add(vboId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboId);
            GL.BufferData<Vector3>(
                BufferTarget.ArrayBuffer, Vector3.SizeInBytes * data.Length, data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attIndex, dimensions, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        private void StoreVec2DataInAttributeList(int attIndex, Vector2[] data, int dimensions = 2)
        {
            var vboId = GL.GenBuffer();
            _vbos.Add(vboId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboId);
            GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, Vector2.SizeInBytes * data.Length, data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attIndex, dimensions, VertexAttribPointerType.Float, false,  Vector2.SizeInBytes, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        private void UnbindVao()
        {
            GL.BindVertexArray(0);
        }

        private void BindIndicesBuffer(int[] indices)
        {
            var eboId = GL.GenBuffer();
            _vbos.Add(eboId);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, eboId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * indices.Length, indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
    }
}
