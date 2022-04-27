using System.Collections.Generic;
using System.IO;
using FarmVilleClone.Models;
using OpenTK;

namespace FarmVilleClone.Render_Engine
{
    public static class ObjLoader
    {
        private static Vector2[] _textureArray;
        private static Vector3[] _normalsArray;
        private static readonly List<int> Indices = new List<int>();
        
        public static RawModel LoadModel(string file, ModelLoader loader)
        {
            string[] content;
            try
            {
                content = File.ReadAllLines(file);
            } catch
            {
                throw new FileNotFoundException("Could not find " + file);
            }

            var vertices = new List<Vector3>();
            var textures = new List<Vector2>();
            var normals = new List<Vector3>();
            
            foreach(var line in content)
            {
                var currentLine = line.Split(' ');

                if (line.StartsWith("v "))
                {
                    var vertex = new Vector3(float.Parse(currentLine[1]), float.Parse(currentLine[2]), float.Parse(currentLine[3]));
                    vertices.Add(vertex);
                } else if (line.StartsWith("vt "))
                {
                    var texture = new Vector2(float.Parse(currentLine[1]), float.Parse(currentLine[2]));
                    textures.Add(texture);
                } else if (line.StartsWith("vn "))
                {
                    var normal = new Vector3(float.Parse(currentLine[1]), float.Parse(currentLine[2]), float.Parse(currentLine[3]));
                    normals.Add(normal);
                } else if (line.StartsWith("f "))
                {
                    _textureArray = new Vector2[vertices.ToArray().Length];
                    _normalsArray = new Vector3[vertices.ToArray().Length];
                    break;
                }
            }
            
            foreach (var line in content)
            {
                if (!line.StartsWith("f ")) continue;
                var currentLine = line.Split(' ');
                var vertex1 = currentLine[1].Split('/');
                var vertex2 = currentLine[2].Split('/');
                var vertex3 = currentLine[3].Split('/');

                ProcessVertex(vertex1, textures, normals);
                ProcessVertex(vertex2, textures, normals);
                ProcessVertex(vertex3, textures, normals);
            }

            var verticesArray = vertices.ToArray();
            var indicesArray = Indices.ToArray();

            return loader.LoadToVao(verticesArray, _textureArray, _normalsArray, indicesArray);
        }

        private static void ProcessVertex(IReadOnlyList<string> vertex, IReadOnlyList<Vector2> textures, IReadOnlyList<Vector3> normals)
        {
            var vertexPointer = int.Parse(vertex[0]) - 1;
            Indices.Add(vertexPointer);
            
            var currentTexture = textures[int.Parse(vertex[1])-1];
            _textureArray[vertexPointer].X = currentTexture.X;
            _textureArray[vertexPointer].Y = 1 - currentTexture.Y;

            var currentNormal = normals[int.Parse(vertex[2]) - 1];
            _normalsArray[vertexPointer] = currentNormal;
        }
    }
}
