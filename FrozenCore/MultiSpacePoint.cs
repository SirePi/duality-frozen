// This code is provided under the MIT license. Originally by Alessandro Pilati.

using OpenTK;

namespace FrozenCore
{
    /// <summary>
    /// Represents a Point in Scene, World and UV coordinates
    /// </summary>
    public struct MultiSpacePoint
    {
        public Vector3 SceneCoords;
        public Vector2 UVCoords;
        public Vector3 WorldCoords;
    }
}