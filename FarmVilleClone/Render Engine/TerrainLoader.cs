using System.Collections.Generic;
using FarmVilleClone.Entities;
using OpenTK;

namespace FarmVilleClone.Render_Engine
{
    public static class TerrainLoader
    {
        public static List<Entity> InitializeTerrain(Entity terrain)
        {
            const float scale = 3;
            var terrainField = new List<Entity>();
            var initialPosition = terrain.GetPosition();
            for (var i = -10f; i < 10; i++)
            {
                for (var j = -5f; j < 10; j++)
                {
                    var translateVector = new Vector3((-i * 2.0f) * scale, 0, (-j * 2.0f) * scale);
                    var translatedTerrainTile = new Entity(terrain.GetModel(), initialPosition + translateVector, 0, 0, 0, scale);
                    terrainField.Add(translatedTerrainTile);
                }
            }
            return terrainField;
        }
    }
}