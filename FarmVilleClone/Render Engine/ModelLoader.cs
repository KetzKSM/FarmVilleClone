using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace FarmVilleClone.RenderEngine
{
    public class ModelLoader
    {
        // A 'VAO' is a Vertex Array Object
        // 'VBO' = Virtual Buffer Object

        private List<int> vaos = new List<int>();
        private List<int> vbos = new List<int>();
        private List<int> textures = new List<int>();

        public RawModel LoadToVAO(Vector3[] positions, Vector2[] textureCoords, int[] indices)
        {
            int vaoID = CreateVAO();
            BindIndicesBuffer(indices);
            CreateModelBuffer(0, positions);
            CreateTextureBuffer(1, textureCoords);
            UnbindVAO();
            return new RawModel(vaoID, indices.Length);
        }

        public int LoadTexture(string file)
        {
            if (!File.Exists(file)) throw new FileNotFoundException();

            int textureID = GL.GenTexture();
            textures.Add(textureID);

            GL.BindTexture(TextureTarget.Texture2D, textureID);
            Bitmap bmp = new Bitmap(file);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            return textureID;
        }

        public void CleanUp()
        {
            foreach(int vao in vaos) { GL.DeleteVertexArray(vao); }
            foreach(int vbo in vbos) { GL.DeleteBuffer(vbo); }
            foreach(int texture in textures) { GL.DeleteTexture(texture); }
        }

        private int CreateVAO()
        {
            int vaoID = GL.GenVertexArray();
            vaos.Add(vaoID);

            GL.BindVertexArray(vaoID);
            return vaoID;
        }

        private void CreateModelBuffer(int attNum, Vector3[] positions, int dimensions = 3)
        {
            int vboID = GL.GenBuffer();
            vbos.Add(vboID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboID);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * positions.Length, positions, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attNum, dimensions, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        private void CreateTextureBuffer(int attNum, Vector2[] positions, int dimensions = 2)
        {
            int vboID = GL.GenBuffer();
            vbos.Add(vboID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboID);
            GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, Vector3.SizeInBytes * positions.Length, positions, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attNum, dimensions, VertexAttribPointerType.Float, false, 0, 0);
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
