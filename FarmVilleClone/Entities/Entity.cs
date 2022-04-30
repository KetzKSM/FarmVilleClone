using FarmVilleClone.Models;
using FarmVilleClone.Render_Engine;
using FarmVilleClone.Textures;
using OpenTK;

namespace FarmVilleClone.Entities
{
    public class Entity
    {
        private TexturedModel _model;
        private Vector3 _position;
        private float _rx, _ry, _rz;
        private readonly float _scale;

        public Entity(TexturedModel texturedModel, Vector3 position, float rx, float ry, float rz, float scale)
        {
            _model = texturedModel;
            _position = position;
            _rx = rx;
            _ry = ry;
            _rz = rz;
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
        public float GetRotationX()
        {
            return _rx;
        }
        public float GetRotationY()
        {
            return _ry;
        }
        public float GetRotationZ()
        {
            return _rz;
        }
        public float GetScale()
        {
            return _scale;
        }
        
        public void SetTexture(string textureFile, ModelLoader loader)
        {
            var rawModel = _model.GetRawModel();
            var newTexture = new ModelTexture(loader.LoadTexture(textureFile));
            var newTexturedModel = new TexturedModel(rawModel, newTexture);
            _model = newTexturedModel;
        }

        public void Translate(float tx, float ty, float tz)
        {
            _position.X += tx;
            _position.Y += ty;
            _position.Z += tz;
        }

        public void Rotate(float rx, float ry, float rz)
        {
            _rx += rx;
            _ry += ry;
            _rz += rz;
        }
    }
}
