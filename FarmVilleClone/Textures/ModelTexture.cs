namespace FarmVilleClone.Textures
{
    public class ModelTexture
    {
        private readonly int _textureId;
        private float _shineDamper;
        private float _reflectivity;
        private bool _hasTransparency;
        private bool _useFakeLighting;

        public ModelTexture(int id)
        {
            _textureId = id;
            _shineDamper = 1;
            _reflectivity = 0;
            _hasTransparency = false;
            _useFakeLighting = false;
        }

        public int GetId()
        {
            return _textureId;
        }

        public float GetShineDamper()
        {
            return _shineDamper;
        }

        public bool IsTransparent()
        {
            return _hasTransparency;
        }

        public bool GetUsesFakeLighting()
        {
            return _useFakeLighting;
        }

        public void SetUsesFakeLighting(bool usesFakeLighting)
        {
            _useFakeLighting = usesFakeLighting;
        }

        public void SetTransparent(bool hasTransparency)
        {
            _hasTransparency = hasTransparency;
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
