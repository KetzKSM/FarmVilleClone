namespace FarmVilleClone.Models
{
    public class RawModel
    {
        private readonly int _vaoId;
        private readonly int _vertexCount;

        public RawModel(int vaoId, int vertexCount)
        {
            _vaoId = vaoId;
            _vertexCount = vertexCount;
        }

        public int GetVaoId()
        {
            return _vaoId;
        }

        public int GetVertexCount()
        {
            return _vertexCount;
        }
    }
}
