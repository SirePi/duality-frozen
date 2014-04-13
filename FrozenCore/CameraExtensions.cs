﻿// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using Duality.Components;
using Duality;
using OpenTK;

namespace FrozenCore
{
    public static class CameraExtensions
    {
        public static void LookAt(this Camera inCamera, Polygon inTargetArea, float inTargetZ)
        {
            float angle = inCamera.GameObj.Transform.Angle;

            Vector2 centroid = inTargetArea.Centroid;
            inTargetArea.Offset(-centroid);

            Vector2 min = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
            Vector2 max = new Vector2(float.NegativeInfinity, float.NegativeInfinity);

            for (int i = 0; i < inTargetArea.Vertices.Length; i++)
            {
                Vector2 v = inTargetArea.Vertices[i];
                MathF.TransformCoord(ref v.X, ref v.Y, angle);

                min.X = Math.Min(min.X, v.X);
                min.Y = Math.Min(min.Y, v.Y);
                max.X = Math.Max(max.X, v.X);
                max.Y = Math.Max(max.Y, v.Y);
            }
            
            Vector2 areaOnScreen = max - min;
            float scaleX = DualityApp.TargetResolution.X / areaOnScreen.X;
            float scaleY = DualityApp.TargetResolution.Y / areaOnScreen.Y;

            float scale = Math.Min(scaleX, scaleY);

            float z = inCamera.GameObj.Transform.Pos.Z;

            if (inCamera.Perspective == Duality.Drawing.PerspectiveMode.Parallax)
            {
                /**
                 * scale = camera.focusDistance / Max(focusZ - camera.Z, camera.nearZ)
                 * camera.Z = - ((camera.focusDistance / scale) - focusZ)
                 **/

                z = -((inCamera.FocusDist / scale) - inTargetZ);
            }

            inCamera.GameObj.Transform.MoveToAbs(new Vector3(centroid, z));
        }

        public void FocusOn(this Camera inCamera, Polygon inFocusArea, float inFocusZ)
        {
            float angle = inCamera.GameObj.Transform.Angle;

            Vector2 centroid = inFocusArea.Centroid;
            inFocusArea.Offset(-centroid);

            Vector2 min = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
            Vector2 max = new Vector2(float.NegativeInfinity, float.NegativeInfinity);

            for (int i = 0; i < inFocusArea.Vertices.Length; i++)
            {
                Vector2 v = inFocusArea.Vertices[i];
                MathF.TransformCoord(ref v.X, ref v.Y, angle);

                min.X = Math.Min(min.X, v.X);
                min.Y = Math.Min(min.Y, v.Y);
                max.X = Math.Max(max.X, v.X);
                max.Y = Math.Max(max.Y, v.Y);
            }

            Vector2 areaOnScreen = max - min;
            float scaleX = DualityApp.TargetResolution.X / areaOnScreen.X;
            float scaleY = DualityApp.TargetResolution.Y / areaOnScreen.Y;

            float scale = Math.Min(scaleX, scaleY);

            float z = inCamera.GameObj.Transform.Pos.Z;

            if (inCamera.Perspective == Duality.Drawing.PerspectiveMode.Parallax)
            {
                /**
                 * scale = camera.focusDistance / Max(focusZ - camera.Z, camera.nearZ)
                 * camera.focusDistance = (focusZ - camera.Z) * scale
                 **/

                inCamera.FocusDist = (inFocusZ - inCamera.GameObj.Transform.Pos.Z) * scale;
            }

            inCamera.GameObj.Transform.MoveToAbs(new Vector3(centroid, inCamera.GameObj.Transform.Pos.Z));
        }
    }
}