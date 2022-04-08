using System;
using OpenTK;
using OpenTK.Input;

namespace FarmVilleClone.Entities
{
    public class Camera
    {
        private Vector3 position = new Vector3(0, 0, 3f);
        private Vector3 target = new Vector3(0, 0, 0);
        Vector3 direction;
        Vector3 cameraRight;
        Vector3 cameraUp;

        private float pitch; // Top, Bottom Camera aims
        private float yaw; // left, right
        private float roll; // tilt

        public Camera()
        {
            direction = Vector3.Normalize(position - target); //Camera Z-Axis
            cameraRight = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, direction)); //Camera X-Axis
            cameraUp = Vector3.Cross(direction, cameraRight); //Camera Y-Axis
        }

        public void move()
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Key.W)) position.Z -= 0.02f;
            if (keyboardState.IsKeyDown(Key.D)) { position.X += 0.02f; target.X += 0.02f; }
            if (keyboardState.IsKeyDown(Key.A)) { position.X -= 0.02f; target.X -= 0.02f; }
            if (keyboardState.IsKeyDown(Key.S)) position.Z += 0.02f;
        }

        public Vector3 getPosition()
        {
            return position;
        }

        public Vector3 getTarget()
        {
            return target;
        }

        public Vector3 getCameraUp()
        {
            return cameraUp;
        }

        public float getPitch()
        {
            return pitch;
        }

        public float getYaw()
        {
            return yaw;
        }

        public float getRoll()
        {
            return roll;
        }
    }
}
