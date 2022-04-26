namespace FarmVilleClone.Textures
{
    public class ModelTexture
    {
        private readonly int _textureId;
        private float _shineDamper;
        private float _reflectivity;

        public ModelTexture(int id)
        {
            _textureId = id;
            _shineDamper = 1;
            _reflectivity = 0;
        }

        public int GetId()
        {
            return _textureId;
        }

        public float GetShineDamper()
        {
            return _shineDamper;
        }

        public void SetShineDamper(float shineDamper)
        {
            _shineDamper = shineDamper;
        }

        public float GetReflectivity()
        {
            return _reflectivity;
        }

        public void SetReflectivity(float reflectivity)
        {
            _reflectivity = reflectivity;
        }
    }
}
