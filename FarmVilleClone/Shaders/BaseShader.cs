using System;
using System.IO;
using System.Text;
using OpenTK;
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
            vertexShaderID = LoadShader(vertexFile, ShaderType.VertexShader);
            fragmentShaderID = LoadShader(fragmentFile, ShaderType.FragmentShader);
            programID = GL.CreateProgram();
            GL.AttachShader(programID, vertexShaderID);
            GL.AttachShader(programID, fragmentShaderID);
            BindAttributes();
            GL.LinkProgram(programID);
            GL.ValidateProgram(programID);
            GetAllUniformLocations();
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

        protected abstract void GetAllUniformLocations();

        protected int GetUniformLocation(string uniformName)
        {
            return GL.GetUniformLocation(programID, uniformName);
        }

        protected void BindAttribute(int attribute, string name)
        {
            GL.BindAttribLocation(programID, attribute, name);
        }

        protected void LoadFloat(int uniformLocation, float value)
        {
            GL.Uniform1(uniformLocation, value);
        }

        protected void LoadVector(int uniformLocation, Vector3 vector)
        {
            GL.Uniform3(uniformLocation, vector);
        }

        protected void LoadBoolean(int uniformLocation, bool value)
        {
            float toLoad = 0;
            if (value) toLoad = 1;
            GL.Uniform1(uniformLocation, toLoad);
        }

        protected void LoadMatrix(int uniformLocation, Matrix4 matrix)
        {
            GL.UniformMatrix4(uniformLocation, false, ref matrix);
        }

        private static int LoadShader(string file, ShaderType type)
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
