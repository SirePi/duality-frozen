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
    /// <summary>
    /// Camera extensions
    /// </summary>
    public static class CameraExtensions
    {
        /// <summary>
        /// Moves the Camera in 3D in order to contain the entirety of the Target Area in the screen's
        /// viewport
        /// </summary>
        /// <param name="inCamera">The Camera</param>
        /// <param name="inTargetArea">A 2D Polygon representing the extent of the area to show on screen</param>
        /// <param name="inTargetZ">The reference Z coordinate of the area to show on screen</param>
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

        /// <summary>
        /// Moves the Camera on the XY plane, and alters the camera's FocusDist property in order to 
        /// contain the entirety of the Target Area in the screen's viewport
        /// </summary>
        /// <param name="inCamera">The Camera</param>
        /// <param name="inFocusArea">A 2D Polygon representing the extent of the area to show on screen</param>
        /// <param name="inFocusZ">The reference Z coordinate of the area to show on screen</param>
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

        /// <summary>
        /// Creates a GameObject covering the entirety of the Camera's viewport and applies a ColorizeSprite
        /// Command going from one Color to another
        /// </summary>
        /// <param name="inCamera">The Camera</param>
        /// <param name="inTime">The time, in milliseconds, that the transition will take</param>
        /// <param name="inStartColor">The starting color</param>
        /// <param name="inEndColor">The ending color</param>
        public static void Fade(this Camera inCamera, float inTime, ColorRgba inStartColor, ColorRgba inEndColor)
        {
            GameObject fader = new GameObject("Fader", inCamera.GameObj);

            Transform t = new Transform();
            fader.AddComponent<Transform>(t);

            SpriteRenderer sr = new SpriteRenderer();
            fader.AddComponent<SpriteRenderer>(sr);

            Commander cmd = new Commander();
            fader.AddComponent<Commander>(cmd);

            t.RelativePos = new Vector3(0, 0, inCamera.FocusDist);

            sr.Rect = Rect.AlignCenter(0, 0, DualityApp.TargetResolution.X, DualityApp.TargetResolution.Y);
            sr.SharedMaterial = new ContentRef<Material>(new Material(DrawTechnique.Alpha, Colors.White, Texture.White));
            sr.ColorTint = inStartColor;

            cmd.ColorizeSprite(inEndColor).Timed(inTime);
            cmd.Destroy();

            inCamera.GameObj.ParentScene.AddObject(fader);
        }
    }
}