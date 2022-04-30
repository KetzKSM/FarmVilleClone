using System.Collections.Generic;
using FarmVilleClone.Entities;
using OpenTK;

namespace FarmVilleClone.Render_Engine
{
    public class TerrainLoader
    {
        private readonly MasterRenderer _masterRenderer;

        public TerrainLoader(MasterRenderer masterRenderer)
        {
            _masterRenderer = masterRenderer;
        }
        
        public List<Terrain> InitializeTerrain(Terrain terrain)
        {
            var terrainField = new List<Terrain>();
            var initialPosition = terrain.GetPosition();
            for (var i = 0f; i < 50f; i++)
            {
                for (var j = 0f; j < 50f; j++)
                {
                    var translateVector = new Vector3(-i, 0, -j);
                    var translatedTerrainTile = new Terrain(terrain.GetModel(), initialPosition + translateVector, 1);
                    // _masterRenderer.ProcessEntity(translatedTerrainTile);
                    terrainField.Add(translatedTerrainTile);
                }
            }
            
            return terrainField;
        }
    }
}