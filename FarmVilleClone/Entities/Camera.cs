using OpenTK;
using OpenTK.Input;

namespace FarmVilleClone.Entities
{
    public class Camera
    {
        private Vector3 _position = new Vector3(0, 0, 3f);
        private Vector3 _target = new Vector3(0, 0, 0);
        readonly Vector3 _direction;
        readonly Vector3 _cameraRight;
        readonly Vector3 _cameraUp;

        public Camera()
        {
            _direction = Vector3.Normalize(_position - _target); //Camera Z-Axis
            _cameraRight = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, _direction)); //Camera X-Axis
            _cameraUp = Vector3.Cross(_direction, _cameraRight); //Camera Y-Axis
        }

        public void Move()
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Key.W)) _position.Z -= 0.02f;
            if (keyboardState.IsKeyDown(Key.D)) { _position.X += 0.02f; _target.X += 0.02f; }
            if (keyboardState.IsKeyDown(Key.A)) { _position.X -= 0.02f; _target.X -= 0.02f; }
            if (keyboardState.IsKeyDown(Key.S)) _position.Z += 0.02f;
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
