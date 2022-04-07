using System;
using System.Drawing.Drawing2D;
using OpenTK;

namespace FarmVilleClone.Common
{
    public class LinearAlgebra
    {
        public LinearAlgebra()
        {
        }

        public static Matrix4 CreateTransformationMatrix(
            Vector3 translationVector,      // Translation Vector
            float rx, float ry, float rz,   // Rotation Values
            float scale)                    // Scale Value
        {
            // Matrix4 matrix = Matrix4.Identity;

            //TODO: Does translation multiply rather than add???

            var translationMatrix = Matrix4.CreateTranslation(translationVector);
            var rotationX = Matrix4.CreateRotationX(rx);
            var rotationY = Matrix4.CreateRotationY(ry);
            var rotationZ = Matrix4.CreateRotationZ(rz);
            var scaleMatrix = Matrix4.CreateScale(scale);

            var matrix = translationMatrix * rotationX * rotationY * rotationZ * scaleMatrix;

            return matrix;
        }
    }
}
