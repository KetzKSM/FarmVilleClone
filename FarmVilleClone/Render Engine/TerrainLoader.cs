using System.Collections.Generic;
using FarmVilleClone.Entities;
using OpenTK;

namespace FarmVilleClone.Render_Engine
{
    public static class TerrainLoader
    {
        public static List<Terrain> InitializeTerrain(Terrain terrain)
        {
            var terrainField = new List<Terrain>();
            var initialPosition = terrain.GetPosition();
            for (var i = -10f; i < 50; i++)
            {
                for (var j = -5f; j < 50f; j++)
                {
                    var translateVector = new Vector3(-i*2.0f, 0, -j*2.0f);
                    var translatedTerrainTile = new Terrain(terrain.GetModel(), initialPosition + translateVector, 1);
                    terrainField.Add(translatedTerrainTile);
                }
            }
            
            return terrainField;
        }
    }
}