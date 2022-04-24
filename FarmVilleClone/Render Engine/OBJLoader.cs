using System.Collections.Generic;
using System.IO;
using FarmVilleClone.Models;
using OpenTK;

namespace FarmVilleClone.Render_Engine
{
    public static class ObjLoader
    {
        static Vector2[] textureArray;
        static Vector3[] normalsArray;
        static List<int> indices = new List<int>();
        
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

            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> textures = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            
            foreach(string line in content)
            {
                string[] currentLine = line.Split(' ');

                if (line.StartsWith("v "))
                {
                    Vector3 vertex = new Vector3(float.Parse(currentLine[1]), float.Parse(currentLine[2]), float.Parse(currentLine[3]));
                    vertices.Add(vertex);
                } else if (line.StartsWith("vt "))
                {
                    Vector2 texture = new Vector2(float.Parse(currentLine[1]), float.Parse(currentLine[2]));
                    textures.Add(texture);
                } else if (line.StartsWith("vn "))
                {
                    Vector3 normal = new Vector3(float.Parse(currentLine[1]), float.Parse(currentLine[2]), float.Parse(currentLine[3]));
                    normals.Add(normal);
                } else if (line.StartsWith("f "))
                {
                    // textureArray = new Vector2[vertices.ToArray().Length * 2];
                    textureArray = new Vector2[vertices.ToArray().Length];
                    normalsArray = new Vector3[vertices.ToArray().Length * 3];
                    break;
                }
            }
            
            foreach (string line in content)
            {
                if (!line.StartsWith("f ")) continue;
                string[] currentLine = line.Split(' ');
                string[] vertex1 = currentLine[1].Split('/');
                string[] vertex2 = currentLine[2].Split('/');
                string[] vertex3 = currentLine[3].Split('/');

                ProcessVertex(vertex1, textures, normals);
                ProcessVertex(vertex2, textures, normals);
                ProcessVertex(vertex3, textures, normals);
            }

            Vector3[] verticesArray = vertices.ToArray();
            int[] indicesArray = indices.ToArray();

            return loader.LoadToVao(verticesArray, textureArray, indicesArray);
        }

        private static void ProcessVertex(string[] vertex, List<Vector2> textures, List<Vector3> normals)
        {
            int vertexPointer = int.Parse(vertex[0]) - 1;
            indices.Add(vertexPointer);
            
            Vector2 currentTexture = textures[int.Parse(vertex[1])-1];
            textureArray[vertexPointer].X = currentTexture.X;
            textureArray[vertexPointer].Y = 1 - currentTexture.Y;

            Vector3 currentNormal = normals[int.Parse(vertex[2]) - 1];
            normalsArray[vertexPointer * 3] = currentNormal;
        }
    }
}
