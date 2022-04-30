using FarmVilleClone.Models;
using OpenTK;

namespace FarmVilleClone.Entities
{
    public class Terrain
    {
        private readonly TexturedModel _model;
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