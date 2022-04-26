using OpenTK;

namespace FarmVilleClone.Common
{
    public static class LinearAlgebra
    {

        public static Matrix4 CreateTransformationMatrix(
            Vector3 translationVector,      // Translation Vector
            float rx, float ry, float rz,   // Rotation Values
            float scale)                    // Scale Value
        {
            var translationMatrix = Matrix4.CreateTranslation(translationVector);
            
            var rotationX = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rx));
            var rotationY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(ry));
            var rotationZ = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rz));
            var scaleMatrix = Matrix4.CreateScale(scale);
            
            var matrix = scaleMatrix * rotationX * rotationY * rotationZ * translationMatrix;

            return matrix;
        }

        public static Matrix4 CreateViewMatrix(Vector3 position, Vector3 target, Vector3 up)
        {
            var view = Matrix4.LookAt(position, target, up);
            return view;
        }
    }
}
