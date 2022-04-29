using System;
using System.Collections.Generic;
using FarmVilleClone.Entities;
using OpenTK;
using OpenTK.Input;

namespace FarmVilleClone.Common
{
    public class MousePointer
    {
        private Vector3 _currentRay;
        private Matrix4 _projectionMatrix;
        private Matrix4 _viewMatrix;
        private readonly Camera _camera;
        private Vector3 _currentTerrainPoint;

        private const float RecursiveCount = 200;
        private const float RayRange = 500;

        public MousePointer(Camera camera, Matrix4 projectionMatrix)
        {
            _camera = camera;
            _projectionMatrix = projectionMatrix;
            _viewMatrix =
                LinearAlgebra.CreateViewMatrix(_camera.GetPosition(), _camera.GetTarget(), _camera.GetCameraUp());
        }

        public Vector3 GetCurrentTerrainPoint()
        {
            return _currentTerrainPoint;
        }

        public Vector3 GetCurrentRay()
        {
            return _currentRay;
        }

        public void Update()
        {
            _viewMatrix = LinearAlgebra.CreateViewMatrix(_camera.GetPosition(), _camera.GetTarget(), _camera.GetCameraUp());
            _currentRay = CalculateMouseRay();

            _currentTerrainPoint = RayIntersectsGround(0, RayRange, _currentRay) 
                ? GetTerrainVector(0, 0, RayRange, _currentRay) 
                : new Vector3(0, 0, 0);
        }

        private Vector3 CalculateMouseRay()
        {
            var mouse = Mouse.GetCursorState();
            var normalizedRay = GetNormalizedDeviceCoords(mouse.X, mouse.Y);
            var clipCoords = new Vector4(normalizedRay.X, normalizedRay.Y, -1f, 1f);
            var eyeCoords = ConvertToEyeSpace(clipCoords);
            var worldCoords = ConvertToWorldSpace(eyeCoords);
            return worldCoords;
        }
        
        private static Vector3 GetNormalizedDeviceCoords(float mouseX, float mouseY)
        {
            //// TODO: Implement functionality for Windowed mode
            var display = DisplayDevice.Default;
            var x = (2f * mouseX / display.Width - 1f);
            var y = 1f - (2f * mouseY / display.Height);
            return new Vector3(x, y, 1f);

        }
        
        private Vector4 ConvertToEyeSpace(Vector4 clipCoords)
        {
            var eyeCoords = Vector4.Transform(clipCoords,_projectionMatrix.Inverted());
            eyeCoords = new Vector4(eyeCoords.X, eyeCoords.Y, -1f, 0f);
            return eyeCoords;
        }

        private Vector3 ConvertToWorldSpace(Vector4 eyeCoords)
        {
            var rayWorldCoordinates = Vector4.Transform(eyeCoords, _viewMatrix.Inverted()).Xyz;
            rayWorldCoordinates.Normalize();
            return rayWorldCoordinates;
        }
        
        /****************************************************************************************************/

        private Vector3 GetPointOnRay(Vector3 ray, float distance)
        {
            var cameraPos = _camera.GetPosition();
            var scaledRay = Vector3.Multiply(ray, distance);
            return Vector3.Add(cameraPos, scaledRay);
        }

        private Vector3 GetTerrainVector(int count, float start, float finish, Vector3 ray)
        {
            while (true)
            {
                var half = start + ((finish - start) / 2f);
                if (count >= RecursiveCount)
                {
                    var endPoint = GetPointOnRay(ray, half);
                    return endPoint;
                }

                if (RayIntersectsGround(start, half, ray))
                {
                    count += 1;
                    finish = half;
                    continue;
                }

                count += 1;
                start = half;
            }
        }

        private bool RayIntersectsGround(float start, float finish, Vector3 ray)
        {
            var startPoint = GetPointOnRay(ray, start);
            var endPoint = GetPointOnRay(ray, finish);
            return !IsUnderground(startPoint) && IsUnderground(endPoint);
        }

        private static bool IsUnderground(Vector3 ray)
        {
            return ray.Y < 0;
        }

        public Entity FindClosestEntityByRay(List<Entity> entities)
        {
            const int tolerance = 3;
            Entity closestEntity = null;
            float closest = 100;
            
            foreach (var entity in entities)
            {
                //// TODO: Find more optimal algorithm, all I'm currently doing is comparing the length of the vectors
                var entityPos = entity.GetPosition();
                var closeness = (float)Math.Abs(Math.Ceiling(entityPos.Length) - Math.Ceiling(_currentTerrainPoint.Length));
                if (closeness > tolerance) continue;
                if (!(closest > closeness)) continue;
                
                closest = closeness;
                closestEntity = entity;
            }
            return closestEntity;
        }
    }
}