using System;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace FarmVilleClone.Shaders
{
    public abstract class BaseShader
    {
        private int programID;
        private int vertexShaderID;
        private int fragmentShaderID;

        public BaseShader(string vertexFile, string fragmentFile)
        {
            vertexShaderID = loadShader(vertexFile, ShaderType.VertexShader);
            fragmentShaderID = loadShader(fragmentFile, ShaderType.FragmentShader);
            programID = GL.CreateProgram();
            GL.AttachShader(programID, vertexShaderID);
            GL.AttachShader(programID, fragmentShaderID);
            GL.LinkProgram(programID);
            GL.ValidateProgram(programID);
            BindAttributes();
        }

        public void Start()
        {
            GL.UseProgram(programID);
        }

        public void Stop()
        {
            GL.UseProgram(0);
        }

        public void CleanUp()
        {
            Stop();
            GL.DetachShader(programID, vertexShaderID);
            GL.DetachShader(programID, fragmentShaderID);
            GL.DeleteShader(vertexShaderID);
            GL.DeleteShader(fragmentShaderID);
            GL.DeleteProgram(programID);
        }

        protected abstract void BindAttributes();

        protected void BindAttribute(int attribute, string name)
        {
            GL.BindAttribLocation(programID, attribute, name);
        }

        private static int loadShader(string file, ShaderType type)
        {
            int shaderID = GL.CreateShader(type);
            GL.ShaderSource(shaderID, File.ReadAllText(file));
            GL.CompileShader(shaderID);

            string infoLog = GL.GetShaderInfoLog(shaderID);
            if (!string.IsNullOrEmpty(infoLog))
            {
                throw new Exception(infoLog);
            }

            return shaderID;
        }
    }
}
