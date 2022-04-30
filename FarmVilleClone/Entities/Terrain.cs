using FarmVilleClone.Models;
using FarmVilleClone.Render_Engine;
using FarmVilleClone.Textures;
using OpenTK;

namespace FarmVilleClone.Entities
{
    public class Terrain
    {
        private TexturedModel _model;
        private Vector3 _position;
        private readonly float _scale;
        
        public Terrain(TexturedModel texturedModel, Vector3 position, float scale)
        {
            _model = texturedModel;
            _position = position;
            _scale = scale;
        }

        public TexturedModel GetModel()
        {
            return _model;
        }

        public void SetTexture(string textureFile, ModelLoader loader)
        {
            var rawModel = _model.GetRawModel();
            var newTexture = new ModelTexture(loader.LoadTexture(textureFile));
            var newTexturedModel = new TexturedModel(rawModel, newTexture);
            _model = newTexturedModel;
        }

        public void SetPosition(Vector3 position)
        {
            _position = position;
        }
        public Vector3 GetPosition()
        {
            return _position;
        }
        public float GetScale()
        {
            return _scale;
        }

        public void Translate(float tx, float ty, float tz)
        {
            _position.X += tx;
            _position.Y += ty;
            _position.Z += tz;
        }
    }
}