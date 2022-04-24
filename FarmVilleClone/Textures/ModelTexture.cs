namespace FarmVilleClone.Textures
{
    public class ModelTexture
    {
        private readonly int _textureId;

        public ModelTexture(int id)
        {
            _textureId = id;
        }

        public int GetId()
        {
            return _textureId;
        }
    }
}
