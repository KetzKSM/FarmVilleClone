using OpenTK;
using OpenTK.Input;

namespace FarmVilleClone.Entities
{
    public class Camera
    {
        private Vector3 _position = new Vector3(0, -.5f, 3f);
        private Vector3 _target = new Vector3(0, 0, 0);
        private readonly Vector3 _cameraUp;

        public Camera()
        {
            var direction = Vector3.Normalize(_position - _target);
            var cameraRight = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, direction));
            _cameraUp = Vector3.Cross(direction, cameraRight); //Camera Y-Axis
        }

        public void Move()
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Key.W)) _position.Z -= 0.2f;
            if (keyboardState.IsKeyDown(Key.D)) { _position.X += .2f; _target.X += .2f; }
            if (keyboardState.IsKeyDown(Key.A)) { _position.X -= .2f; _target.X -= .2f; }
            if (keyboardState.IsKeyDown(Key.S)) _position.Z += 0.2f;
        }

        public Vector3 GetPosition()
        {
            return _position;
        }

        public Vector3 GetTarget()
        {
            return _target;
        }

        public Vector3 GetCameraUp()
        {
            return _cameraUp;
        }
    }
}
