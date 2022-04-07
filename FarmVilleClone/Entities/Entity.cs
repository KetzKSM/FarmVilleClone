using System;
using FarmVilleClone.Models;
using OpenTK;

namespace FarmVilleClone.Entities
{
    public class Entity
    {
        private TexturedModel model;
        private Vector3 position;
        private float rx, ry, rz, scale;

        public Entity(TexturedModel texturedModel, Vector3 position, float rx, float ry, float rz, float scale)
        {
            this.model = texturedModel;
            this.position = position;
            this.rx = rx;
            this.ry = ry;
            this.rz = rz;
            this.scale = scale;
        }

        public TexturedModel GetModel()
        {
            return model;
        }
        public Vector3 GetPosition()
        {
            return position;
        }
        public float GetRotationX()
        {
            return rx;
        }
        public float GetRotationY()
        {
            return ry;
        }
        public float GetRotationZ()
        {
            return rz;
        }
        public float GetScale()
        {
            return scale;
        }

        public void Translate(float tx, float ty, float tz)
        {
            position.X += tx;
            position.Y += ty;
            position.Z += tz;
        }

        public void Rotate(float rx, float ry, float rz)
        {
            this.rx += rx;
            this.ry += ry;
            this.rz += rz;
        }
    }
}
