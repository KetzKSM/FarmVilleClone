using FarmVilleClone.Textures;

namespace FarmVilleClone.Models
{
    public class TexturedModel
    {
        private readonly RawModel _rawModel;
        private readonly ModelTexture _modelTexture;

        public TexturedModel(RawModel rawModel, ModelTexture modelTexture)
        {
            _rawModel = rawModel;
            _modelTexture = modelTexture;
        }

        public RawModel GetRawModel()
        {
            return _rawModel;
        }

        public ModelTexture GetModelTexture()
        {
            return _modelTexture;
        }
    }
}
