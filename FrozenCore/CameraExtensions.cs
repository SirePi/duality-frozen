// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using Duality.Components;
using Duality;
using OpenTK;
using Duality.Drawing;
using FrozenCore.Components;
using Duality.Components.Renderers;
using Duality.Resources;

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

        public static void FocusOn(this Camera inCamera, Polygon inFocusArea, float inFocusZ)
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

        public static void Fade(this Camera inCamera, float inTime, ColorRgba inStartColor, ColorRgba inEndColor)
        {
            GameObject fader = new GameObject("Fader", inCamera.GameObj);

            Transform t = new Transform();
            t.RelativePos = new Vector3(0, 0, inCamera.GameObj.Transform.Pos.Z - inCamera.FocusDist);

            SpriteRenderer sr = new SpriteRenderer();
            sr.Rect = Rect.AlignCenter(0, 0, DualityApp.TargetResolution.X, DualityApp.TargetResolution.Y);
            sr.SharedMaterial = Material.SolidWhite;
            sr.ColorTint = inStartColor;

            Commander cmd = new Commander();
            cmd.ColorizeSprite(inEndColor).Timed(inTime);
            cmd.Destroy();

            fader.AddComponent<Commander>(cmd);

            inCamera.GameObj.ParentScene.AddObject(fader);
        }
    }
}