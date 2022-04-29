using System.Collections.Generic;
using FarmVilleClone.Models;
using FarmVilleClone.Render_Engine;
using FarmVilleClone.Textures;
using OpenTK;

namespace FarmVilleClone.Terrains
{
    public class Terrain
    {
        private static readonly float Size = 800;
        private static readonly int VertexCount = 128;

        private readonly float _x;
        private readonly float _z;
        private readonly RawModel _rawModel;
        private readonly ModelTexture _modelTexture;

        public Terrain(int gridX, int gridZ, ModelLoader loader, ModelTexture modelTexture)
        {
            _modelTexture = modelTexture;
            _x = gridX * Size;
            _z = gridZ * Size;
            _rawModel = GenerateTerrain(loader);
        }

        public RawModel GetModel()
        {
            return _rawModel;
        }

        public ModelTexture GetTexture()
        {
            return _modelTexture;
        }

        public float GetX()
        {
            return _x;
        }

        public float GetZ()
        {
            return _z;
        }

        private RawModel GenerateTerrain(ModelLoader loader)
        {
            var count = VertexCount * VertexCount;
            var vertices = new Vector3[count];
            var normals = new Vector3[count];
            var textureCoords = new Vector2[count];
            var indices = new List<int>();

            var vertexPointer = 0;
            for (var i = 0; i < VertexCount; i++)
            {
                for (var j = 0; j < VertexCount; j++)
                {
                    vertices[vertexPointer] = new Vector3((j / (VertexCount - 1f ) * Size) * -1, 0,(i / (VertexCount - 1f ) * Size) * -1);
                    normals[vertexPointer] = new Vector3(0, 1, 0);
                    textureCoords[vertexPointer] = new Vector2((j / (VertexCount - 1f)) * -1, (i / (VertexCount - 1f)) * -1);
                    vertexPointer++;
                }
            }
            
            for (var gridZ = 0; gridZ < VertexCount; gridZ++)
            {
                for (var gridX = 0; gridX < VertexCount; gridX++)
                {
                    var topLeft = gridZ * VertexCount + gridX;
                    var topRight = topLeft + 1;
                    var bottomLeft = (gridZ + 1) * VertexCount + gridX;
                    var bottomRight = bottomLeft + 1;
                    
                    indices.Add(topLeft);
                    indices.Add(bottomLeft);
                    indices.Add(topRight);
                    indices.Add(topRight);
                    indices.Add(bottomLeft);
                    indices.Add(bottomRight);
                }
            }
            return loader.LoadToVao(vertices, textureCoords, normals, indices.ToArray());
        }
    }
}