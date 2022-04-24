using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace FarmVilleClone.Shaders
{
    public abstract class BaseShader
    {
        private readonly int _programId;
        private readonly int _vertexShaderId;
        private readonly int _fragmentShaderId;

        protected BaseShader(string vertexFile, string fragmentFile)
        {
            _vertexShaderId = LoadShader(vertexFile, ShaderType.VertexShader);
            _fragmentShaderId = LoadShader(fragmentFile, ShaderType.FragmentShader);
            _programId = GL.CreateProgram();
            GL.AttachShader(_programId, _vertexShaderId);
            GL.AttachShader(_programId, _fragmentShaderId);
            BindAttributes();
            GL.LinkProgram(_programId);
            GL.ValidateProgram(_programId);
            GetAllUniformLocations();
        }

        public void Start()
        {
            GL.UseProgram(_programId);
        }

        public void Stop()
        {
            GL.UseProgram(0);
        }

        public void CleanUp()
        {
            Stop();
            GL.DetachShader(_programId, _vertexShaderId);
            GL.DetachShader(_programId, _fragmentShaderId);
            GL.DeleteShader(_vertexShaderId);
            GL.DeleteShader(_fragmentShaderId);
            GL.DeleteProgram(_programId);
        }

        protected abstract void BindAttributes();

        protected abstract void GetAllUniformLocations();

        protected int GetUniformLocation(string uniformName)
        {
            return GL.GetUniformLocation(_programId, uniformName);
        }

        protected void BindAttribute(int attribute, string name)
        {
            GL.BindAttribLocation(_programId, attribute, name);
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
            int shaderId = GL.CreateShader(type);
            GL.ShaderSource(shaderId, File.ReadAllText(file));
            GL.CompileShader(shaderId);

            string infoLog = GL.GetShaderInfoLog(shaderId);
            if (!string.IsNullOrEmpty(infoLog))
            {
                throw new Exception(infoLog);
            }

            return shaderId;
        }
    }
}
